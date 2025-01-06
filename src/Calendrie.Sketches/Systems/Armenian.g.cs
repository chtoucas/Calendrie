﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

#region ArmenianMonth

/// <summary>
/// Represents the Armenian month.
/// <para><i>All</i> months within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="ArmenianMonth"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct ArmenianMonth :
    ICalendarMonth<ArmenianMonth>,
    ICalendarBound<ArmenianCalendar>,
    // A month viewed as a finite sequence of days
    IDaySegment<ArmenianDate>,
    ISetMembership<ArmenianDate>,
    // Arithmetic
    ISubtractionOperators<ArmenianMonth, ArmenianMonth, int>
{ }

public partial struct ArmenianMonth // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_monthsSinceEpoch"/>.
    /// <para>This field is a constant equal to 119_987.</para></summary>
    private const int MaxMonthsSinceEpoch = 119_987;

    /// <summary>
    /// Represents the count of consecutive months since the epoch
    /// <see cref="DayZero.Armenian"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxMonthsSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly int _monthsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianMonth"/> struct to the
    /// specified month components.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid month or <paramref name="year"/> is outside the
    /// range of supported years.</exception>
    public ArmenianMonth(int year, int month)
    {
        ArmenianCalendar.Instance.Scope.ValidateYearMonth(year, month);

        _monthsSinceEpoch = CountMonthsSinceEpoch(year, month);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianMonth"/> struct.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal ArmenianMonth(int monthsSinceEpoch)
    {
        _monthsSinceEpoch = monthsSinceEpoch;
    }

    /// <summary>
    /// Gets the earliest possible value of a <see cref="ArmenianMonth"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // MinValue = new(0) = new() = default(ArmenianMonth)
    public static ArmenianMonth MinValue { get; }

    /// <summary>
    /// Gets the latest possible value of a <see cref="ArmenianMonth"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ArmenianMonth MaxValue { get; } = new(MaxMonthsSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current month type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ArmenianCalendar Calendar => ArmenianCalendar.Instance;

    /// <inheritdoc />
    public int MonthsSinceEpoch => _monthsSinceEpoch;

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

    /// <summary>
    /// Gets the year number.
    /// <para>This property represents the algebraic year, but since it's greater
    /// than 0, there is no difference between the algebraic year and the year
    /// of the era.</para>
    /// </summary>
    public int Year =>
        // NB: both dividend and divisor are >= 0.
        1 + _monthsSinceEpoch / ArmenianCalendar.MonthsInYear;

    /// <inheritdoc />
    public int Month
    {
        get
        {
            var (_, m) = this;
            return m;
        }
    }

    /// <inheritdoc />
    bool ICalendarMonth.IsIntercalary => false;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        var (y, m) = this;
        return FormattableString.Invariant($"{m:D2}/{y:D4} ({Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month)
    {
        // See RegularSchema.GetMonthParts().
        // NB: both dividend and divisor are >= 0.
        year = 1 + MathN.Divide(_monthsSinceEpoch, ArmenianCalendar.MonthsInYear, out int m0);
        month = 1 + m0;
    }
}

public partial struct ArmenianMonth // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="ArmenianMonth"/> struct from the
    /// specified <see cref="ArmenianDate"/> value.
    /// </summary>
    [Pure]
    public static ArmenianMonth Create(ArmenianDate date)
    {
        var (y, m, _) = date;
        return UnsafeCreate(y, m);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ArmenianMonth"/> struct
    /// from the specified month components.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ArmenianMonth UnsafeCreate(int year, int month)
    {
        int monthsSinceEpoch = CountMonthsSinceEpoch(year, month);
        return new ArmenianMonth(monthsSinceEpoch);
    }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int CountMonthsSinceEpoch(int y, int m) =>
        // See RegularSchema.CountMonthsSinceEpoch().
        ArmenianCalendar.MonthsInYear * (y - 1) + m - 1;
}

public partial struct ArmenianMonth // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedMonthsInYear() => Month - 1;

    /// <inheritdoc />
    [Pure]
    public int CountRemainingMonthsInYear() => ArmenianCalendar.MonthsInYear - 1;

#if false
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear()
    {
        var (y, m) = this;
        return Calendar.Schema.CountDaysInYearBeforeMonth(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear()
    {
        var (y, m) = this;
        return Calendar.Schema.CountDaysInYearAfterMonth(y, m);
    }
#endif
}

public partial struct ArmenianMonth // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public ArmenianMonth WithYear(int newYear)
    {
        int m = Month;
        // Even when "newYear" is valid, we must re-check "m".
        Calendar.Scope.ValidateYearMonth(newYear, m, nameof(newYear));
        return new ArmenianMonth(newYear, m);
    }

    /// <inheritdoc />
    [Pure]
    public ArmenianMonth WithMonth(int newMonth)
    {
        int y = Year;
        // We already know that "y" is valid, we only need to check "newMonth".
        Calendar.Scope.PreValidator.ValidateMonth(y, newMonth, nameof(newMonth));
        return new ArmenianMonth(y, newMonth);
    }
}

public partial struct ArmenianMonth // IDaySegment
{
    /// <inheritdoc />
    public ArmenianDate MinDay
    {
        get
        {
            var (y, m) = this;
            int daysSinceEpoch = Calendar.Schema.CountDaysSinceEpoch(y, m, 1);
            return new ArmenianDate(daysSinceEpoch);
        }
    }

    /// <inheritdoc />
    public ArmenianDate MaxDay
    {
        get
        {
            var (y, m) = this;
            var sch = Calendar.Schema;
            int d = sch.CountDaysInMonth(y, m);
            int daysSinceEpoch = sch.CountDaysSinceEpoch(y, m, d);
            return new ArmenianDate(daysSinceEpoch);
        }
    }

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.CountDaysInMonth(int, int)"/>.
    /// </remarks>
    [Pure]
    public int CountDays()
    {
        var (y, m) = this;
        return Calendar.Schema.CountDaysInMonth(y, m);
    }

    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    /// <remarks>See also <see cref="CalendarSystem{TDate}.GetDaysInMonth(int, int)"/>.
    /// </remarks>
    [Pure]
    public Range<ArmenianDate> ToRange() => Range.UnsafeCreate(MinDay, MaxDay);

    [Pure]
    Range<ArmenianDate> IDaySegment<ArmenianDate>.ToDayRange() => ToRange();

    /// <summary>
    /// Returns an enumerable collection of all days in this month instance.
    /// </summary>
    [Pure]
    public IEnumerable<ArmenianDate> ToEnumerable()
    {
        var (y, m) = this;
        var sch = Calendar.Schema;
        int startOfMonth = sch.CountDaysSinceEpoch(y, m, 1);
        int daysInMonth = sch.CountDaysInMonth(y, m);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select new ArmenianDate(daysSinceEpoch);
    }

    [Pure]
    IEnumerable<ArmenianDate> IDaySegment<ArmenianDate>.EnumerateDays() => ToEnumerable();

    /// <inheritdoc />
    [Pure]
    public bool Contains(ArmenianDate date)
    {
        var (y, m) = this;
        Calendar.Schema.GetDateParts(date.DaysSinceEpoch, out int y1, out int m1, out _);
        return y1 == y && m1 == m;
    }

    /// <summary>
    /// Obtains the date corresponding to the specified day of this month
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfMonth"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public ArmenianDate GetDayOfMonth(int dayOfMonth)
    {
        var (y, m) = this;
        Calendar.Scope.PreValidator.ValidateDayOfMonth(y, m, dayOfMonth);
        return new ArmenianDate(y, m, dayOfMonth);
    }
}

public partial struct ArmenianMonth // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(ArmenianMonth left, ArmenianMonth right) =>
        left._monthsSinceEpoch == right._monthsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(ArmenianMonth left, ArmenianMonth right) =>
        left._monthsSinceEpoch != right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(ArmenianMonth other) => _monthsSinceEpoch == other._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ArmenianMonth month && Equals(month);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _monthsSinceEpoch;
}

public partial struct ArmenianMonth // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(ArmenianMonth left, ArmenianMonth right) =>
        left._monthsSinceEpoch < right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(ArmenianMonth left, ArmenianMonth right) =>
        left._monthsSinceEpoch <= right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(ArmenianMonth left, ArmenianMonth right) =>
        left._monthsSinceEpoch > right._monthsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(ArmenianMonth left, ArmenianMonth right) =>
        left._monthsSinceEpoch >= right._monthsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static ArmenianMonth Min(ArmenianMonth x, ArmenianMonth y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static ArmenianMonth Max(ArmenianMonth x, ArmenianMonth y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(ArmenianMonth other) => _monthsSinceEpoch.CompareTo(other._monthsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is ArmenianMonth month ? CompareTo(month)
        : ThrowHelpers.ThrowNonComparable(typeof(ArmenianMonth), obj);
}

public partial struct ArmenianMonth // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified months and returns the number of months
    /// between them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountMonthsSince()")]
    public static int operator -(ArmenianMonth left, ArmenianMonth right) => left.CountMonthsSince(right);

    /// <summary>
    /// Adds a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusMonths()")]
    public static ArmenianMonth operator +(ArmenianMonth value, int months) => value.PlusMonths(months);

    /// <summary>
    /// Subtracts a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusMonths()")]
    public static ArmenianMonth operator -(ArmenianMonth value, int months) => value.PlusMonths(-months);

    /// <summary>
    /// Adds one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextMonth()")]
    public static ArmenianMonth operator ++(ArmenianMonth value) => value.NextMonth();

    /// <summary>
    /// Subtracts one month to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousMonth()")]
    public static ArmenianMonth operator --(ArmenianMonth value) => value.PreviousMonth();

    /// <summary>
    /// Counts the number of months elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountMonthsSince(ArmenianMonth other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to MaxMonthsSinceEpoch.
        _monthsSinceEpoch - other._monthsSinceEpoch;

    /// <summary>
    /// Adds a number of months to the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [Pure]
    public ArmenianMonth PlusMonths(int months)
    {
        int monthsSinceEpoch = checked(_monthsSinceEpoch + months);
        if (unchecked((uint)monthsSinceEpoch) > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowMonthOverflow();
        return new ArmenianMonth(monthsSinceEpoch);
    }

    /// <summary>
    /// Obtains the month after the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [Pure]
    public ArmenianMonth NextMonth()
    {
        if (_monthsSinceEpoch == MaxMonthsSinceEpoch) ThrowHelpers.ThrowMonthOverflow();
        return new ArmenianMonth(_monthsSinceEpoch + 1);
    }

    /// <summary>
    /// Obtains the month before the current instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [Pure]
    public ArmenianMonth PreviousMonth()
    {
        if (_monthsSinceEpoch == 0) ThrowHelpers.ThrowMonthOverflow();
        return new ArmenianMonth(_monthsSinceEpoch - 1);
    }
}

public partial struct ArmenianMonth // Non-standard math ops
{
    // For regular calendars, the next operations are unambiguous.

    /// <summary>
    /// Adds a number of years to the year field of this month instance, yielding
    /// a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    [Pure]
    public ArmenianMonth PlusYears(int years)
    {
        var (y, m) = this;
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowMonthOverflow();

        int monthsSinceEpoch = CountMonthsSinceEpoch(newY, m);
        return new ArmenianMonth(monthsSinceEpoch);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified month.
    /// </summary>
    [Pure]
    public int CountYearsSince(ArmenianMonth other) =>
        // NB: this subtraction never overflows.
        Year - other.Year;
}

#endregion

#region ArmenianYear

/// <summary>
/// Represents the Armenian year.
/// <para><i>All</i> years within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="ArmenianYear"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct ArmenianYear :
    ICalendarYear<ArmenianYear>,
    ICalendarBound<ArmenianCalendar>,
    // A year viewed as a finite sequence of months
    IMonthSegment<ArmenianMonth>,
    ISetMembership<ArmenianMonth>,
    // A year viewed as a finite sequence of days
    IDaySegment<ArmenianDate>,
    ISetMembership<ArmenianDate>,
    // Arithmetic
    ISubtractionOperators<ArmenianYear, ArmenianYear, int>
{ }

public partial struct ArmenianYear // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_yearsSinceEpoch"/>.
    /// <para>This field is a constant equal to 9998.</para></summary>
    private const int MaxYearsSinceEpoch = StandardScope.MaxYear - 1;

    /// <summary>
    /// Represents the count of consecutive years since the epoch
    /// <see cref="DayZero.Armenian"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxYearsSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly ushort _yearsSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianYear"/> struct to the
    /// specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of years supported values.</exception>
    public ArmenianYear(int year)
    {
        if (year < StandardScope.MinYear || year > StandardScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);

        _yearsSinceEpoch = unchecked((ushort)(year - 1));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArmenianYear"/> struct to the
    /// specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    private ArmenianYear(ushort yearsSinceEpoch)
    {
        _yearsSinceEpoch = yearsSinceEpoch;
    }

    /// <summary>
    /// Gets the earliest possible value of a <see cref="ArmenianYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // MinValue = new(1) = new() = default(ArmenianYear)
    public static ArmenianYear MinValue { get; }

    /// <summary>
    /// Gets the latest possible value of a <see cref="ArmenianYear"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ArmenianYear MaxValue { get; } = new(StandardScope.MaxYear);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static ArmenianCalendar Calendar => ArmenianCalendar.Instance;

    /// <inheritdoc />
    public int YearsSinceEpoch => _yearsSinceEpoch;

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

    /// <summary>
    /// Gets the year number.
    /// <para>This property represents the algebraic year, but since it's greater
    /// than 0, there is no difference between the algebraic year and the year
    /// of the era.</para>
    /// </summary>
    public int Year => _yearsSinceEpoch + 1;

    /// <inheritdoc />
    public bool IsLeap => Calendar.Schema.IsLeapYear(Year);

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString() => FormattableString.Invariant($"{Year:D4} ({Calendar})");
}

public partial struct ArmenianYear // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="ArmenianYear"/> struct from the
    /// specified <see cref="ArmenianMonth"/> value.
    /// </summary>
    [Pure]
    public static ArmenianYear Create(ArmenianMonth month) => UnsafeCreate(month.Year);

    /// <summary>
    /// Creates a new instance of the <see cref="ArmenianYear"/> struct from the
    /// specified <see cref="ArmenianDate"/> value.
    /// </summary>
    [Pure]
    public static ArmenianYear Create(ArmenianDate date) => UnsafeCreate(date.Year);

    /// <summary>
    /// Creates a new instance of the <see cref="ArmenianYear"/> struct from the
    /// specified year.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ArmenianYear UnsafeCreate(int year) => new(unchecked((ushort)(year - 1)));
}

public partial struct ArmenianYear // IMonthSegment
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthCount = ArmenianCalendar.MonthsInYear;

    /// <inheritdoc />
    public ArmenianMonth MinMonth => ArmenianMonth.UnsafeCreate(Year, 1);

    /// <inheritdoc />
    public ArmenianMonth MaxMonth => ArmenianMonth.UnsafeCreate(Year, MonthCount);

    /// <inheritdoc />
    [Pure]
    int IMonthSegment<ArmenianMonth>.CountMonths() => MonthCount;

    /// <inheritdoc />
    [Pure]
    public Range<ArmenianMonth> ToMonthRange() => Range.UnsafeCreate(MinMonth, MaxMonth);

    /// <inheritdoc />
    [Pure]
    public IEnumerable<ArmenianMonth> EnumerateMonths()
    {
        int startOfYear = ArmenianMonth.CountMonthsSinceEpoch(Year, 1);

        return from monthsSinceEpoch
               in Enumerable.Range(startOfYear, MonthCount)
               select new ArmenianMonth(monthsSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(ArmenianMonth month) => month.Year == Year;

    /// <summary>
    /// Obtains the month corresponding to the specified month of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="month"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public ArmenianMonth GetMonthOfYear(int month)
    {
        // We already know that "y" is valid, we only need to check "month".
        int y = Year;
        Calendar.Scope.PreValidator.ValidateMonth(y, month);
        return ArmenianMonth.UnsafeCreate(y, month);
    }
}

public partial struct ArmenianYear // IDaySegment
{
    /// <inheritdoc />
    public ArmenianDate MinDay
    {
        get
        {
            int daysSinceEpoch = Calendar.Schema.CountDaysSinceEpoch(Year, 1);
            return new ArmenianDate(daysSinceEpoch);
        }
    }

    /// <inheritdoc />
    public ArmenianDate MaxDay
    {
        get
        {
            var sch = Calendar.Schema;
            int y = Year;
            int doy = sch.CountDaysInYear(y);
            int daysSinceEpoch = sch.CountDaysSinceEpoch(y, doy);
            return new ArmenianDate(daysSinceEpoch);
        }
    }

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.CountDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public int CountDays() => Calendar.Schema.CountDaysInYear(Year);

    /// <inheritdoc />
    /// <remarks>See also <see cref="CalendarSystem{TDate}.GetDaysInYear(int)"/>.
    /// </remarks>
    [Pure]
    public Range<ArmenianDate> ToDayRange() => Range.UnsafeCreate(MinDay, MaxDay);

    /// <inheritdoc />
    [Pure]
    public IEnumerable<ArmenianDate> EnumerateDays()
    {
        var sch = Calendar.Schema;
        int y = Year;
        int startOfYear = sch.CountDaysSinceEpoch(y, 1);
        int daysInYear = sch.CountDaysInYear(y);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select new ArmenianDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public bool Contains(ArmenianDate date) => date.Year == Year;

    /// <summary>
    /// Obtains the ordinal date corresponding to the specified day of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfYear"/>
    /// is outside the range of valid values.</exception>
    [Pure]
    public ArmenianDate GetDayOfYear(int dayOfYear)
    {
        var chr = Calendar;
        int y = Year;
        // We already know that "y" is valid, we only need to check "dayOfYear".
        chr.Scope.PreValidator.ValidateDayOfYear(y, dayOfYear);
        int daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(y, dayOfYear);
        return new ArmenianDate(daysSinceEpoch);
    }
}

public partial struct ArmenianYear // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(ArmenianYear left, ArmenianYear right) =>
        left._yearsSinceEpoch == right._yearsSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(ArmenianYear left, ArmenianYear right) =>
        left._yearsSinceEpoch != right._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(ArmenianYear other) => _yearsSinceEpoch == other._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is ArmenianYear year && Equals(year);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _yearsSinceEpoch;
}

public partial struct ArmenianYear // IComparable
{
    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(ArmenianYear left, ArmenianYear right) =>
        left._yearsSinceEpoch < right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(ArmenianYear left, ArmenianYear right) =>
        left._yearsSinceEpoch <= right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(ArmenianYear left, ArmenianYear right) =>
        left._yearsSinceEpoch > right._yearsSinceEpoch;

    /// <summary>
    /// Compares the two specified instances to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(ArmenianYear left, ArmenianYear right) =>
        left._yearsSinceEpoch >= right._yearsSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static ArmenianYear Min(ArmenianYear x, ArmenianYear y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static ArmenianYear Max(ArmenianYear x, ArmenianYear y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(ArmenianYear other) =>
        _yearsSinceEpoch.CompareTo(other._yearsSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is ArmenianYear year ? CompareTo(year)
        : ThrowHelpers.ThrowNonComparable(typeof(ArmenianYear), obj);
}

public partial struct ArmenianYear // Math ops
{
    /// <summary>
    /// Subtracts the two specified years and returns the number of years between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountYearsSince()")]
    public static int operator -(ArmenianYear left, ArmenianYear right) => left.CountYearsSince(right);

    /// <summary>
    /// Adds a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusYears()")]
    public static ArmenianYear operator +(ArmenianYear value, int years) => value.PlusYears(years);

    /// <summary>
    /// Subtracts a number of years to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the range
    /// of supported years.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusYears()")]
    public static ArmenianYear operator -(ArmenianYear value, int years) => value.PlusYears(-years);

    /// <summary>
    /// Adds one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextYear()")]
    public static ArmenianYear operator ++(ArmenianYear value) => value.NextYear();

    /// <summary>
    /// Subtracts one year to the specified year, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousYear()")]
    public static ArmenianYear operator --(ArmenianYear value) => value.PreviousYear();

    /// <summary>
    /// Counts the number of years elapsed since the specified year.
    /// </summary>
    [Pure]
    public int CountYearsSince(ArmenianYear other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to (MaxYear - 1).
        _yearsSinceEpoch - other._yearsSinceEpoch;

    /// <summary>
    /// Adds a number of years to the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported years.
    /// </exception>
    [Pure]
    public ArmenianYear PlusYears(int years)
    {
        int yearsSinceEpoch = checked(_yearsSinceEpoch + years);
        if (unchecked((uint)yearsSinceEpoch) > MaxYearsSinceEpoch) ThrowHelpers.ThrowYearOverflow();
        return new ArmenianYear(unchecked((ushort)yearsSinceEpoch));
    }

    /// <summary>
    /// Obtains the year after the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [Pure]
    public ArmenianYear NextYear()
    {
        if (_yearsSinceEpoch == MaxYearsSinceEpoch) ThrowHelpers.ThrowYearOverflow();
        return new ArmenianYear(unchecked((ushort)(_yearsSinceEpoch + 1)));
    }

    /// <summary>
    /// Obtains the year before the current instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [Pure]
    public ArmenianYear PreviousYear()
    {
        if (_yearsSinceEpoch == 0) ThrowHelpers.ThrowYearOverflow();
        return new ArmenianYear(unchecked((ushort)(_yearsSinceEpoch - 1)));
    }
}

#endregion

