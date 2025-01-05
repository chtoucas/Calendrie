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
    ICalendarDate<GregorianDate>,
    ICalendarBound<GregorianCalendar>,
    IUnsafeDateFactory<GregorianDate>,
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
    /// Represents the count of consecutive days since the epoch
    /// <see cref="DayZero.NewStyle"/>.
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
    internal GregorianDate(int daysSinceZero)
    {
        _daysSinceZero = daysSinceZero;
    }

    /// <summary>
    /// Gets the earliest possible value of a <see cref="GregorianDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static GregorianDate MinValue { get; } = new(MinDaysSinceZero);

    /// <summary>
    /// Gets the latest possible value of a <see cref="GregorianDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static GregorianDate MaxValue { get; } = new(MaxDaysSinceZero);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static GregorianCalendar Calendar => GregorianCalendar.Instance;

    /// <inheritdoc />
    public DayNumber DayNumber => new(_daysSinceZero);

    /// <summary>
    /// Gets the count of days since the Gregorian epoch.
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
        return y > 0 ? FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({Calendar})")
            : FormattableString.Invariant($"{d:D2}/{m:D2}/{getBCEYear(y)} BCE ({Calendar})");

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

public partial struct GregorianDate // Factories & conversions
{
    /// <summary>
    /// Creates a new instance of the <see cref="GregorianDate"/> struct from
    /// the specified <see cref="CivilDate"/> value.
    /// <para>See also <see cref="CivilDate.ToGregorianDate()"/></para>
    /// </summary>
    [Pure]
    public static GregorianDate FromCivilDate(CivilDate date) => new(date.DaysSinceZero);

    /// <inheritdoc />
    [Pure]
    public static GregorianDate FromDayNumber(DayNumber dayNumber)
    {
        int daysSinceZero = dayNumber.DaysSinceZero;

        if (daysSinceZero < MinDaysSinceZero || daysSinceZero > MaxDaysSinceZero)
            throw new ArgumentOutOfRangeException(nameof(dayNumber));

        return new(daysSinceZero);
    }

    // REVIEW(code): inlining? Same for the other date types.

    /// <inheritdoc />
    [Pure]
    //[Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static GregorianDate IUnsafeDateFactory<GregorianDate>.UnsafeCreate(int daysSinceZero) =>
        new(daysSinceZero);
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
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate WithMonth(int newMonth)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out _, out int d);

        // We only need to validate "newMonth" and "d".
        Calendar.Scope.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate WithDay(int newDay)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out _);

        // We only need to validate "newDay".
        Calendar.Scope.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(y, m, newDay);
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate WithDayOfYear(int newDayOfYear)
    {
        int y = GregorianFormulae.GetYear(_daysSinceZero);

        // We only need to validate "newDayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceZero);
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
        return new(daysSinceZero);
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
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Nearest(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.Nearest(dayOfWeek).DaysSinceZero;
        if (daysSinceZero < MinDaysSinceZero || daysSinceZero > MaxDaysSinceZero)
            ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
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
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public GregorianDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
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
        return new(daysSinceZero);
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
        return new(_daysSinceZero + 1);
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
        return new(_daysSinceZero - 1);
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
    public GregorianDate AddWeeks(int weeks) => PlusDays(DaysInWeek * weeks);

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
    /// Adds a number of years to the year field of this date instance, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public GregorianDate PlusYears(int years)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return AddYears(y, m, d, years);
    }

    /// <summary>
    /// Adds a number of months to the month field of this date instance,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public GregorianDate PlusMonths(int months)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return AddMonths(y, m, d, months);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified date.
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
    /// Counts the number of months elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountMonthsSince(GregorianDate other)
    {
        GregorianFormulae.GetDateParts(_daysSinceZero, out int y, out int m, out _);
        GregorianFormulae.GetDateParts(other._daysSinceZero, out int y0, out int m0, out int d0);

        // Exact difference between two calendar months.
        int months = checked(GJSchema.MonthsInYear * (y - y0) + m - m0);

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
    /// Adds a number of years to the year field of the specified date, yielding
    /// a new date.
    /// </summary>
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
    /// Adds a number of months to the month field of the specified date,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static GregorianDate AddMonths(int y, int m, int d, int months)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(
            checked(m - 1 + months), GJSchema.MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < GregorianScope.MinYear || newY > GregorianScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, GregorianFormulae.CountDaysInMonth(newY, newM));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(newY, newM, newD);
        return new GregorianDate(daysSinceZero);
    }
}
