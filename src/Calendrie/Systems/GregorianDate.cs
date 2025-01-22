// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the Gregorian date.
/// <para><i>All</i> dates within the range [-999_998..999_999] of years are
/// supported.</para>
/// <para><see cref="GregorianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct GregorianDate :
    IDate<GregorianDate>,
    IUnsafeFactory<GregorianDate>,
    ISubtractionOperators<GregorianDate, GregorianDate, int>
{ }

public partial struct GregorianDate // Preamble
{
    /// <summary>Represents the minimum value of <see cref="_daysSinceZero"/>.
    /// <para>This field is a constant equal to -365_242_135.</para></summary>
    private const int MinDaysSinceZero = -365_242_135;
    /// <summary>Represents the maximum value of <see cref="_daysSinceZero"/>.
    /// <para>This field is a constant equal to 365_242_133.</para></summary>
    private const int MaxDaysSinceZero = 365_242_133;

    /// <summary>
    /// Represents the count of consecutive days since the Gregorian epoch.
    /// <para>This field is in the range from <see cref="MinDaysSinceZero"/>
    /// to <see cref="MaxDaysSinceZero"/>.</para>
    /// </summary>
    private readonly int _daysSinceZero;

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianDate"/> struct to
    /// the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public GregorianDate(int year, int month, int day)
    {
        GregorianScope.ValidateYearMonthDayImpl(year, month, day);

        _daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianDate"/> struct to
    /// the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public GregorianDate(int year, int dayOfYear)
    {
        GregorianScope.ValidateOrdinalImpl(year, dayOfYear);

        _daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianDate"/> struct.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    private GregorianDate(int daysSinceZero)
    {
        _daysSinceZero = daysSinceZero;
    }

    /// <summary>
    /// Gets the earliest possible value of <see cref="GregorianDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The earliest supported date.</returns>
    public static GregorianDate MinValue { get; } = new(MinDaysSinceZero);

    /// <summary>
    /// Gets the latest possible value of <see cref="GregorianDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The latest supported date.</returns>
    public static GregorianDate MaxValue { get; } = new(MaxDaysSinceZero);

    /// <summary>
    /// Gets the companion calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static GregorianCalendar Calendar => GregorianCalendar.Instance;

    static Calendar IDate.Calendar => Calendar;

    /// <inheritdoc />
    public DayNumber DayNumber => new(_daysSinceZero);

    /// <summary>
    /// Gets the count of consecutive days since the Gregorian epoch.
    /// </summary>
    public int DaysSinceZero => _daysSinceZero;

    int IAbsoluteDate.DaysSinceEpoch => _daysSinceZero;

    /// <summary>
    /// Gets the century of the era.
    /// </summary>
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <summary>
    /// Gets the century number.
    /// </summary>
    public int Century => YearNumbering.GetCentury(Year);

    /// <summary>
    /// Gets the year of the era.
    /// </summary>
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <summary>
    /// Gets the year of the century.
    /// <para>The result is in the range from 1 to 100.</para>
    /// </summary>
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <inheritdoc />
    public int Year => GregorianFormulae.GetYear(_daysSinceZero);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            GregorianFormulae.GetDateParts(_daysSinceZero, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = GregorianFormulae.GetYear(_daysSinceZero, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            GregorianFormulae.GetDateParts(_daysSinceZero, out _, out _, out int d);
            return d;
        }
    }

    /// <inheritdoc />
    public DayOfWeek DayOfWeek => DayNumber.DayOfWeek;

    /// <inheritdoc />
    public bool IsIntercalary
    {
        get
        {
            GregorianFormulae.GetDateParts(_daysSinceZero, out _, out int m, out int d);
            return GregorianFormulae.IsIntercalaryDay(m, d);
        }
    }

    bool IDateable.IsSupplementary => false;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return y > 0
            ? FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({GregorianCalendar.DisplayName})")
            : FormattableString.Invariant($"{d:D2}/{m:D2}/{getBCEYear(y):D4} BCE ({GregorianCalendar.DisplayName})");

        [Pure]
        static int getBCEYear(int y)
        {
            Debug.Assert(y <= 0);
            var (pos, _) = Ord.FromInt32(y);
            return pos;
        }
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        GregorianFormulae.GetDateParts(_daysSinceZero, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = GregorianFormulae.GetYear(_daysSinceZero, out dayOfYear);
}

public partial struct GregorianDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static GregorianDate Create(int year, int month, int day) => new(year, month, day);

    /// <inheritdoc />
    [Pure]
    public static GregorianDate Create(int year, int dayOfYear) => new(year, dayOfYear);

    /// <summary>
    /// Attempts to create a new instance of the <see cref="GregorianDate"/>
    /// struct from the specified date components.
    /// </summary>
    [Pure]
    public static GregorianDate? TryCreate(int year, int month, int day)
    {
        if (!GregorianScope.CheckYearMonthDayImpl(year, month, day)) return null;

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, month, day);
        return new GregorianDate(daysSinceZero);
    }

    /// <summary>
    /// Attempts to create a new instance of the <see cref="GregorianDate"/>
    /// struct from the specified ordinal components.
    /// </summary>
    [Pure]
    public static GregorianDate? TryCreate(int year, int dayOfYear)
    {
        if (!GregorianScope.CheckOrdinalImpl(year, dayOfYear)) return null;

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, dayOfYear);
        return new GregorianDate(daysSinceZero);
    }

    // Explicit implementation: GregorianDate being a value type, better to use
    // the others TryCreate().

    [Pure]
    static bool IDate<GregorianDate>.TryCreate(int year, int month, int day, out GregorianDate result)
    {
        var date = TryCreate(year, month, day);
        result = date ?? default;
        return date.HasValue;
    }

    [Pure]
    static bool IDate<GregorianDate>.TryCreate(int year, int dayOfYear, out GregorianDate result)
    {
        var date = TryCreate(year, dayOfYear);
        result = date ?? default;
        return date.HasValue;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianDate"/> struct from
    /// the specified date components.
    /// <para>This method does NOT validate its parameters.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static GregorianDate UnsafeCreate(int year, int month, int day)
    {
        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, month, day);
        return new GregorianDate(daysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianDate"/> struct from
    /// the specified ordinal components.
    /// <para>This method does NOT validate its parameters.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static GregorianDate UnsafeCreate(int year, int dayOfYear)
    {
        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(year, dayOfYear);
        return new GregorianDate(daysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianDate"/> struct from
    /// the specified count of consecutive days since the epoch.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static GregorianDate UnsafeCreate(int daysSinceZero) => new(daysSinceZero);

    [Pure]
    static GregorianDate IUnsafeFactory<GregorianDate>.UnsafeCreate(int daysSinceZero) =>
        UnsafeCreate(daysSinceZero);
}

public partial struct GregorianDate // Conversions
{
    /// <summary>
    /// Defines an implicit conversion of a <see cref="GregorianDate"/> value to
    /// a <see cref="Calendrie.DayNumber"/> value.
    /// <para>See also <seealso cref="DayNumber"/>.</para>
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See DayNumber")]
    public static implicit operator DayNumber(GregorianDate date) => date.DayNumber;

    /// <summary>
    /// Defines an explicit conversion of a <see cref="GregorianDate"/> value to
    /// a <see cref="JulianDate"/> value.
    /// <para>The conversion always succeeds.</para>
    /// <para>See also <seealso cref="ToJulianDate()"/>.</para>
    /// </summary>
    //
    // NB: This operation does NOT overflow.
    public static explicit operator JulianDate(GregorianDate date) =>
        JulianDate.FromAbsoluteDate(date);

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianDate"/> struct from
    /// the specified absolute value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayNumber"/>
    /// is outside the range of supported values.</exception>
    [Pure]
    public static GregorianDate FromAbsoluteDate(DayNumber dayNumber)
    {
        int daysSinceZero = dayNumber.DaysSinceZero;

        if (daysSinceZero < MinDaysSinceZero || daysSinceZero > MaxDaysSinceZero)
            ThrowHelpers.ThrowDayNumberOutOfRange(dayNumber);

        return new GregorianDate(daysSinceZero);
    }

    [Pure]
    static GregorianDate IAbsoluteDate<GregorianDate>.FromDayNumber(DayNumber dayNumber) =>
        FromAbsoluteDate(dayNumber);

    /// <summary>
    /// Creates a new instance of the <see cref="GregorianDate"/> struct from
    /// the specified <see cref="JulianDate"/> value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported <see cref="GregorianDate"/> values.</exception>
    //
    // Used by JulianDate to implement ToGregorianDate().
    [Pure]
    internal static GregorianDate FromAbsoluteDate(JulianDate date)
    {
        int daysSinceZero = date.DayNumber.DaysSinceZero;

        if (daysSinceZero < MinDaysSinceZero || daysSinceZero > MaxDaysSinceZero)
            ThrowHelpers.ThrowDateOverflow();

        return new GregorianDate(daysSinceZero);
    }

    /// <summary>
    /// Converts the current instance to a <see cref="JulianDate"/> value.
    /// <para>The conversion always succeeds.</para>
    /// </summary>
    //
    // NB: This operation does NOT overflow.
    [Pure]
    public JulianDate ToJulianDate() => JulianDate.FromAbsoluteDate(this);
}

public partial struct GregorianDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public GregorianDate WithYear(int newYear)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out _, out int m, out int d);

        // We MUST re-validate the entire date.
        GregorianScope.ValidateYearMonthDayImpl(newYear, m, d, nameof(newYear));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(newYear, m, d);
        return new GregorianDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate WithMonth(int newMonth)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out _, out int d);

        // We only need to validate "newMonth" and "d".
        Calendar.Scope.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(y, newMonth, d);
        return new GregorianDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate WithDay(int newDay)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out _);

        // We only need to validate "newDay".
        Calendar.Scope.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(y, m, newDay);
        return new GregorianDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate WithDayOfYear(int newDayOfYear)
    {
        int y = GregorianFormulae.GetYear(_daysSinceZero);

        // We only need to validate "newDayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(y, newDayOfYear);
        return new GregorianDate(daysSinceZero);
    }
}

public partial struct GregorianDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public GregorianDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceZero < MinDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new GregorianDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceZero = _daysSinceZero + (δ > 0 ? δ - DaysInWeek : δ); ;
        if (daysSinceZero < MinDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new GregorianDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Nearest(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.Nearest(dayOfWeek).DaysSinceZero;
        if (daysSinceZero < MinDaysSinceZero || daysSinceZero > MaxDaysSinceZero)
            ThrowHelpers.ThrowDateOverflow();
        return new GregorianDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceZero = _daysSinceZero + (δ < 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new GregorianDate(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new GregorianDate(daysSinceZero);
    }
}

public partial struct GregorianDate // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(GregorianDate left, GregorianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static GregorianDate operator +(GregorianDate value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static GregorianDate operator -(GregorianDate value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static GregorianDate operator ++(GregorianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static GregorianDate operator --(GregorianDate value) => value.PreviousDay();

    /// <summary>
    /// Counts the number of days elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountDaysSince(GregorianDate other) =>
        // No need to use a checked context here. Indeed, the result is at most
        // equal to:
        //   MaxDaysSinceZero - MinDaysSinceZero
        //     = 365_242_133 - (-365_242_135)
        //     = 730_484_268 <= int.MaxValue
        _daysSinceZero - other._daysSinceZero;

    /// <summary>
    /// Adds a number of days to the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public GregorianDate PlusDays(int days)
    {
        int daysSinceZero = checked(_daysSinceZero + days);
        if (daysSinceZero < MinDaysSinceZero || daysSinceZero > MaxDaysSinceZero)
            ThrowHelpers.ThrowDateOverflow();
        return new GregorianDate(daysSinceZero);
    }

    /// <summary>
    /// Obtains the date after the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [Pure]
    public GregorianDate NextDay()
    {
        if (_daysSinceZero == MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new GregorianDate(_daysSinceZero + 1);
    }

    /// <summary>
    /// Obtains the date before the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [Pure]
    public GregorianDate PreviousDay()
    {
        if (_daysSinceZero == MinDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new GregorianDate(_daysSinceZero - 1);
    }

    //
    // Math operations based on the week unit
    //

    /// <summary>
    /// Counts the number of weeks elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountWeeksSince(GregorianDate other) => MathZ.Divide(CountDaysSince(other), DaysInWeek);

    /// <summary>
    /// Adds a number of weeks to the current instance, yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public GregorianDate PlusWeeks(int weeks) => PlusDays(DaysInWeek * weeks);

    /// <summary>
    /// Obtains the date after the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public GregorianDate NextWeek() => PlusDays(DaysInWeek);

    /// <summary>
    /// Obtains the date before the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public GregorianDate PreviousWeek() => PlusDays(-DaysInWeek);
}

public partial struct GregorianDate // Non-standard math ops
{
    /// <summary>
    /// Adds the specified number of years to the year part of this date instance,
    /// yielding a new date.
    /// <para>This method may truncate the result to ensure that it returns a
    /// valid date; see <see cref="AdditionRule.Truncate"/>.</para>
    /// </summary>
    /// <returns>The end of the target month when truncation happens.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public GregorianDate PlusYears(int years)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return AddYears(y, m, d, years);
    }

    /// <summary>
    /// Adds the specified number of months to the month part of this date
    /// instance, yielding a new date.
    /// <para>This method may truncate the result to ensure that it returns a
    /// valid date; see <see cref="AdditionRule.Truncate"/>.</para>
    /// </summary>
    /// <returns>The end of the target month when truncation happens.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public GregorianDate PlusMonths(int months)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return AddMonths(y, m, d, months);
    }

    /// <summary>
    /// Counts the number of whole years from <paramref name="other"/> to this
    /// date instance.
    /// <para>Beware, the result may not be exact. Behind the scene, it uses
    /// <see cref="PlusYears(int)"/> which may apply a kind of truncation.</para>
    /// </summary>
    [Pure]
    public int CountYearsSince(GregorianDate other)
    {
        GregorianFormulae.GetDateParts(other._daysSinceZero, out int y0, out int m0, out int d0);

        // Exact difference between two calendar years.
        int years = Year - y0;

        // To avoid extracting y0 twice, we inline:
        // > var newStart = other.PlusYears(years);
        var newStart = AddYears(y0, m0, d0, years);
        if (other < this)
        {
            if (newStart > this) years--;
        }
        else
        {
            if (newStart < this) years++;
        }

        return years;
    }

    /// <summary>
    /// Counts the number of whole months from <paramref name="other"/> to this
    /// date instance.
    /// <para>Beware, the result may not be exact. Behind the scene, it uses
    /// <see cref="PlusMonths(int)"/> which may apply a kind of truncation.</para>
    /// </summary>
    [Pure]
    public int CountMonthsSince(GregorianDate other)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out _);
        GregorianFormulae.GetDateParts(other._daysSinceZero, out int y0, out int m0, out int d0);

        // Exact difference between two calendar months.
        int months = checked(GJSchema.MonthsPerYear * (y - y0) + m - m0);

        // To avoid extracting (y0, m0, d0) twice, we inline:
        // > var newStart = other.PlusMonths(months);
        var newStart = AddMonths(y0, m0, d0, months);

        if (other < this)
        {
            if (newStart > this) months--;
        }
        else
        {
            if (newStart < this) months++;
        }

        return months;
    }

    /// <summary>
    /// Adds a number of years to the year part of the specified date, yielding
    /// a new date.
    /// <para>This method may truncate the result to ensure that it returns a
    /// valid date; see <see cref="AdditionRule.Truncate"/>.</para>
    /// </summary>
    /// <returns>The end of the target month when truncation happens.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static GregorianDate AddYears(int y, int m, int d, int years)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < GregorianScope.MinYear || newY > GregorianScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, GregorianFormulae.CountDaysInMonth(newY, m));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(newY, m, newD);
        return new GregorianDate(daysSinceZero);
    }

    /// <summary>
    /// Adds a number of months to the month part of the specified date,
    /// yielding a new date.
    /// <para>This method may truncate the result to ensure that it returns a
    /// valid date; see <see cref="AdditionRule.Truncate"/>.</para>
    /// </summary>
    /// <returns>The end of the target month when truncation happens.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static GregorianDate AddMonths(int y, int m, int d, int months)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(
            checked(m - 1 + months), GJSchema.MonthsPerYear, out int years);

        return AddYears(y, newM, d, years);
    }
}
