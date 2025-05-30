﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the Julian date.
/// <para><i>All</i> dates within the range [-999_998..999_999] of years are
/// supported.</para>
/// <para><see cref="JulianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct JulianDate :
    IDate<JulianDate>,
    IUnsafeFactory<JulianDate>,
    ISubtractionOperators<JulianDate, JulianDate, int>
{ }

public partial struct JulianDate // Preamble
{
    private const int EpochDaysSinceZero = -2;

    /// <summary>Represents the minimum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to -365_249_635.</para></summary>
    internal const int MinDaysSinceEpoch = -365_249_635;

    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to -365_249_633.</para></summary>
    internal const int MaxDaysSinceEpoch = 365_249_633;

    /// <summary>
    /// Represents the count of consecutive days since the Julian epoch.
    /// <para>This field is in the range from <see cref="MinDaysSinceEpoch"/>
    /// to <see cref="MaxDaysSinceEpoch"/>.</para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDate"/> struct to the
    /// specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public JulianDate(int year, int month, int day)
    {
        JulianScope.ValidateYearMonthDayImpl(year, month, day);

        _daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDate"/> struct to the
    /// specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public JulianDate(int year, int dayOfYear)
    {
        JulianScope.ValidateOrdinalImpl(year, dayOfYear);

        _daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDate"/> struct.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    private JulianDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <summary>
    /// Gets the earliest possible value of <see cref="JulianDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The earliest supported date.</returns>
    public static JulianDate MinValue { get; } = new(MinDaysSinceEpoch);

    /// <summary>
    /// Gets the latest possible value of <see cref="JulianDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The latest supported date.</returns>
    public static JulianDate MaxValue { get; } = new(MaxDaysSinceEpoch);

    /// <summary>
    /// Gets the companion calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static JulianCalendar Calendar => JulianCalendar.Instance;

    static Calendar IDate.Calendar => Calendar;

    /// <inheritdoc />
    public DayNumber DayNumber => new(EpochDaysSinceZero + _daysSinceEpoch);

    /// <summary>
    /// Gets the count of consecutive days since the Julian epoch.
    /// </summary>
    public int DaysSinceEpoch => _daysSinceEpoch;

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
    public int Year => JulianFormulae.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            JulianFormulae.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = JulianFormulae.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            JulianFormulae.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
            JulianFormulae.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
            return JulianFormulae.IsIntercalaryDay(m, d);
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
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return y > 0
            ? FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({JulianCalendar.DisplayName})")
            : FormattableString.Invariant($"{d:D2}/{m:D2}/{getBCEYear(y):D4} BCE ({JulianCalendar.DisplayName})");

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
        JulianFormulae.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = JulianFormulae.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct JulianDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static JulianDate Create(int year, int month, int day) => new(year, month, day);

    /// <inheritdoc />
    [Pure]
    public static JulianDate Create(int year, int dayOfYear) => new(year, dayOfYear);

    /// <summary>
    /// Attempts to create a new instance of the <see cref="JulianDate"/>
    /// struct from the specified date components.
    /// </summary>
    [Pure]
    public static JulianDate? TryCreate(int year, int month, int day)
    {
        if (!JulianScope.CheckYearMonthDayImpl(year, month, day)) return null;

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(year, month, day);
        return new JulianDate(daysSinceEpoch);
    }

    /// <summary>
    /// Attempts to create a new instance of the <see cref="JulianDate"/>
    /// struct from the specified ordinal components.
    /// </summary>
    [Pure]
    public static JulianDate? TryCreate(int year, int dayOfYear)
    {
        if (!JulianScope.CheckOrdinalImpl(year, dayOfYear)) return null;

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(year, dayOfYear);
        return new JulianDate(daysSinceEpoch);
    }

    // Explicit implementation: JulianDate being a value type, better to use the
    // others TryCreate().

    [Pure]
    static bool IDate<JulianDate>.TryCreate(int year, int month, int day, out JulianDate result)
    {
        var date = TryCreate(year, month, day);
        result = date ?? default;
        return date.HasValue;
    }

    [Pure]
    static bool IDate<JulianDate>.TryCreate(int year, int dayOfYear, out JulianDate result)
    {
        var date = TryCreate(year, dayOfYear);
        result = date ?? default;
        return date.HasValue;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="JulianDate"/> struct from the
    /// specified date components.
    /// <para>This method does NOT validate its parameters.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static JulianDate UnsafeCreate(int year, int month, int day)
    {
        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(year, month, day);
        return new JulianDate(daysSinceEpoch);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="JulianDate"/> struct from the
    /// specified ordinal components.
    /// <para>This method does NOT validate its parameters.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static JulianDate UnsafeCreate(int year, int dayOfYear)
    {
        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(year, dayOfYear);
        return new JulianDate(daysSinceEpoch);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="JulianDate"/> struct from the
    /// specified count of consecutive days since the epoch.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static JulianDate UnsafeCreate(int daysSinceEpoch) => new(daysSinceEpoch);

    [Pure]
    static JulianDate IUnsafeFactory<JulianDate>.UnsafeCreate(int daysSinceEpoch) =>
        UnsafeCreate(daysSinceEpoch);
}

public partial struct JulianDate // Conversions
{
    /// <summary>
    /// Defines an implicit conversion of a <see cref="JulianDate"/> value to a
    /// <see cref="Calendrie.DayNumber"/> value.
    /// <para>See also <seealso cref="DayNumber"/>.</para>
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See DayNumber")]
    public static implicit operator DayNumber(JulianDate date) => date.DayNumber;

    /// <summary>
    /// Defines an explicit conversion of a <see cref="JulianDate"/> value to a
    /// <see cref="GregorianDate"/> value.
    /// <para>See also <seealso cref="ToGregorianDate()"/>.</para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported <see cref="GregorianDate"/> values.</exception>
    public static explicit operator GregorianDate(JulianDate date) =>
        GregorianDate.FromAbsoluteDate(date);

    /// <summary>
    /// Creates a new instance of the <see cref="JulianDate"/> struct from the
    /// specified absolute value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayNumber"/>
    /// is outside the range of supported values.</exception>
    [Pure]
    public static JulianDate FromAbsoluteDate(DayNumber dayNumber)
    {
        Calendar.Scope.Validate(dayNumber);
        // NB: now that the day number is validated, we know for sure that the
        // subtraction won't overflow.
        return new JulianDate(dayNumber.DaysSinceZero - EpochDaysSinceZero);
    }

    [Pure]
    static JulianDate IAbsoluteDate<JulianDate>.FromDayNumber(DayNumber dayNumber) =>
        FromAbsoluteDate(dayNumber);

    /// <summary>
    /// Creates a new instance of the <see cref="JulianDate"/> struct from the
    /// specified <see cref="Calendrie.DayNumber"/> date.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    //
    // Truely UnsafeFromAbsoluteDate().
    // Used by the other date types to implement ToJulianDate().
    [Pure]
    internal static JulianDate UnsafeCreate(DayNumber dayNumber) =>
        // NB: in general, the subtraction may overflow, but it just happens that
        // this is not the case for date types in Calendrie.Systems,
        // GregorianDate being the sole exception.
        new(dayNumber.DaysSinceZero - EpochDaysSinceZero);

    /// <summary>
    /// Creates a new instance of the <see cref="JulianDate"/> struct from the
    /// specified <see cref="GregorianDate"/> value.
    /// <para>The operation NEVER overflows.</para>
    /// </summary>
    //
    // Used by GregorianDate to implement ToJulianDate().
    // NB: This operation does NOT overflow.
    [Pure]
    internal static JulianDate FromAbsoluteDate(GregorianDate date)
    {
        int daysSinceEpoch = date.DaysSinceZero - EpochDaysSinceZero;

        // GregorianDate.MinValue.DayNumber > JulianDate.MinValue.DayNumber
        Debug.Assert(daysSinceEpoch >= MinDaysSinceEpoch);
        // GregorianDate.MaxValue.DayNumber < JulianDate.MaxValue.DayNumber
        Debug.Assert(daysSinceEpoch <= MaxDaysSinceEpoch);

        return new JulianDate(daysSinceEpoch);
    }

    /// <summary>
    /// Converts the current instance to a <see cref="JulianDate"/> value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported <see cref="GregorianDate"/> values.</exception>
    [Pure]
    public GregorianDate ToGregorianDate() => GregorianDate.FromAbsoluteDate(this);
}

public partial struct JulianDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public JulianDate WithYear(int newYear)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);

        // We MUST re-validate the entire date.
        JulianScope.ValidateYearMonthDayImpl(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(newYear, m, d);
        return new JulianDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate WithMonth(int newMonth)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out _, out int d);

        // We only need to validate "newMonth" and "d".
        Calendar.Scope.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(y, newMonth, d);
        return new JulianDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate WithDay(int newDay)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out _);

        // We only need to validate "newDay".
        Calendar.Scope.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(y, m, newDay);
        return new JulianDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate WithDayOfYear(int newDayOfYear)
    {
        int y = JulianFormulae.GetYear(_daysSinceEpoch);

        // We only need to validate "newDayOfYear".
        Calendar.Scope.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(y, newDayOfYear);
        return new JulianDate(daysSinceEpoch);
    }
}

public partial struct JulianDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public JulianDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ >= 0 ? δ - DaysPerWeek : δ);
        if (daysSinceEpoch < MinDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new JulianDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ > 0 ? δ - DaysPerWeek : δ); ;
        if (daysSinceEpoch < MinDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new JulianDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate Nearest(DayOfWeek dayOfWeek)
    {
        var nearest = DayNumber.Nearest(dayOfWeek);
        int daysSinceEpoch = nearest.DaysSinceZero - EpochDaysSinceZero;
        if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();
        return new JulianDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ < 0 ? δ + DaysPerWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new JulianDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ <= 0 ? δ + DaysPerWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new JulianDate(daysSinceEpoch);
    }
}

public partial struct JulianDate // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(JulianDate left, JulianDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static JulianDate operator +(JulianDate value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static JulianDate operator -(JulianDate value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static JulianDate operator ++(JulianDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static JulianDate operator --(JulianDate value) => value.PreviousDay();

    /// <summary>
    /// Counts the number of days elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountDaysSince(JulianDate other) =>
        // No need to use a checked context here. Indeed, the result is at most
        // equal to:
        //   MaxDaysSinceEpoch - MinDaysSinceEpoch
        //     = 365_249_633 - (-365_249_635)
        //     = 730_499_268 <= int.MaxValue
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <summary>
    /// Adds a number of days to the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public JulianDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();
        return new JulianDate(daysSinceEpoch);
    }

    /// <summary>
    /// Obtains the date after the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [Pure]
    public JulianDate NextDay()
    {
        if (_daysSinceEpoch == MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new JulianDate(_daysSinceEpoch + 1);
    }

    /// <summary>
    /// Obtains the date before the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [Pure]
    public JulianDate PreviousDay()
    {
        if (_daysSinceEpoch == MinDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new JulianDate(_daysSinceEpoch - 1);
    }

    //
    // Math operations based on the week unit
    //

    /// <summary>
    /// Counts the number of weeks elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountWeeksSince(JulianDate other) => MathZ.Divide(CountDaysSince(other), DaysPerWeek);

    /// <summary>
    /// Adds a number of weeks to the current instance, yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public JulianDate PlusWeeks(int weeks) => PlusDays(DaysPerWeek * weeks);

    /// <summary>
    /// Obtains the date after the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public JulianDate NextWeek() => PlusDays(DaysPerWeek);

    /// <summary>
    /// Obtains the date before the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public JulianDate PreviousWeek() => PlusDays(-DaysPerWeek);
}

public partial struct JulianDate // Non-standard math ops
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
    public JulianDate PlusYears(int years)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return AddYears(y, m, d, years);
    }

    /// <summary>
    /// Adds the specified number of years to the year part of this date instance
    /// and also returns the roundoff in an output parameter, yielding a new date.
    /// </summary>
    /// <returns>The end of the target month when roundoff &gt; 0.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public JulianDate PlusYears(int years, out int roundoff)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return AddYears(y, m, d, years, out roundoff);
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
    public JulianDate PlusMonths(int months)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return AddMonths(y, m, d, months);
    }

    /// <summary>
    /// Adds the specified number of months to the month part of this date
    /// instance and also returns the roundoff in an output parameter, yielding
    /// a new date.
    /// </summary>
    /// <returns>The end of the target month when roundoff &gt; 0.</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public JulianDate PlusMonths(int months, out int roundoff)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return AddMonths(y, m, d, months, out roundoff);
    }

    /// <summary>
    /// Counts the number of whole years from <paramref name="other"/> to this
    /// date instance.
    /// <para>Beware, the result may not be exact. Behind the scene, it uses
    /// <see cref="PlusYears(int)"/> which may apply a kind of truncation.</para>
    /// </summary>
    [Pure]
    public int CountYearsSince(JulianDate other)
    {
        JulianFormulae.GetDateParts(other._daysSinceEpoch, out int y0, out int m0, out int d0);

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
    public int CountMonthsSince(JulianDate other)
    {
        JulianFormulae.GetDateParts(_daysSinceEpoch, out int y, out int m, out _);
        JulianFormulae.GetDateParts(other._daysSinceEpoch, out int y0, out int m0, out int d0);

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

    //
    // Helpers
    //

    /// <summary>
    /// Adds a number of years to the year part of the specified date, yielding
    /// a new date.
    /// <para>This method may truncate the result to ensure that it returns a
    /// valid date; see <see cref="AdditionRule.Truncate"/>.</para>
    /// </summary>
    /// <returns>The end of the target month when truncation happens.</returns>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static JulianDate AddYears(int y, int m, int d, int years)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < JulianScope.MinYear || newY > JulianScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, JulianFormulae.CountDaysInMonth(newY, m));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(newY, m, newD);
        return new JulianDate(daysSinceEpoch);
    }

    [Pure]
    private static JulianDate AddYears(int y, int m, int d, int years, out int roundoff)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < JulianScope.MinYear || newY > JulianScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = JulianFormulae.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(newY, m, newD);
        return new JulianDate(daysSinceEpoch);
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
    private static JulianDate AddMonths(int y, int m, int d, int months)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), GJSchema.MonthsPerYear, out int years);

        return AddYears(y, newM, d, years);
    }

    [Pure]
    private static JulianDate AddMonths(int y, int m, int d, int months, out int roundoff)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(
            checked(m - 1 + months), GJSchema.MonthsPerYear, out int years);

        return AddYears(y, newM, d, years, out roundoff);
    }
}
