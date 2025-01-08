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
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

#region TabularIslamicCalendar

/// <summary>
/// Represents the Tabular Islamic calendar.
/// <para>This calendar is <i>retropolated</i>. It supports <i>all</i> dates
/// within the range [1..9999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class TabularIslamicCalendar : CalendarSystem<TabularIslamicDate>
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = TabularIslamicSchema.MonthsInYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="TabularIslamicCalendar"/> class.
    /// </summary>
    public TabularIslamicCalendar() : this(new TabularIslamicSchema()) { }

    private TabularIslamicCalendar(TabularIslamicSchema schema)
        : base("Tabular Islamic", new StandardScope(schema, DayZero.TabularIslamic))
    {
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="TabularIslamicCalendar"/> class.
    /// <para>See <see cref="TabularIslamicDate.Calendar"/>.</para>
    /// </summary>
    internal static TabularIslamicCalendar Instance { get; } = new();

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => StandardScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => StandardScope.MaxYear;

    /// <summary>
    /// Gets the schema.
    /// </summary>
    internal TabularIslamicSchema Schema { get; }
}

#endregion

#region TabularIslamicDate

/// <summary>
/// Represents the Tabular Islamic date.
/// <para><i>All</i> dates within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="TabularIslamicDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct TabularIslamicDate :
    IDate<TabularIslamicDate>,
    ICalendarBound<TabularIslamicCalendar>,
    IUnsafeFactory<TabularIslamicDate>,
    ISubtractionOperators<TabularIslamicDate, TabularIslamicDate, int>
{ }

public partial struct TabularIslamicDate // Preamble
{
    /// <summary>Represents the value of the property <see cref="DayNumber.DaysSinceZero"/>
    /// for the epoch <see cref="DayZero.TabularIslamic"/>.
    /// <para>This field is a constant equal to 227_014.</para></summary>
    private const int EpochDaysSinceZero = 227_014;

    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to 3_543_311.</para></summary>
    private const int MaxDaysSinceEpoch = 3_543_311;

    /// <summary>
    /// Represents the count of consecutive days since the epoch
    /// <see cref="DayZero.TabularIslamic"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxDaysSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="TabularIslamicDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public TabularIslamicDate(int year, int month, int day)
    {
        var chr = TabularIslamicCalendar.Instance;
        chr.Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TabularIslamicDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public TabularIslamicDate(int year, int dayOfYear)
    {
        var chr = TabularIslamicCalendar.Instance;
        chr.Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TabularIslamicDate"/> struct.
    /// <para>This constructor does NOT validate its parameter.</para>
    /// </summary>
    internal TabularIslamicDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <summary>
    /// Gets the smallest possible value of <see cref="TabularIslamicDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The earliest supported date.</returns>
    //
    // MinValue = new(0) = new() = default(TabularIslamicDate)
    public static TabularIslamicDate MinValue { get; }

    /// <summary>
    /// Gets the largest possible value of <see cref="TabularIslamicDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The latest supported date.</returns>
    public static TabularIslamicDate MaxValue { get; } = new(MaxDaysSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static TabularIslamicCalendar Calendar => TabularIslamicCalendar.Instance;

    /// <inheritdoc />
    //
    // We already know that the resulting day number is valid, so instead of
    // > public DayNumber DayNumber => Epoch + _daysSinceEpoch;
    // we can use an unchecked addition
    public DayNumber DayNumber => new(EpochDaysSinceZero + _daysSinceEpoch);

    /// <inheritdoc />
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

    /// <summary>
    /// Gets the year number.
    /// <para>Actually, this property returns the algebraic year, but since its
    /// value is greater than 0, one can ignore this subtlety.</para>
    /// </summary>
    public int Year => Calendar.Schema.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = Calendar.Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
            var sch = Calendar.Schema;
            sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return sch.IsIntercalaryDay(y, m, d);
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
        var chr = Calendar;
        chr.Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({chr})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Calendar.Schema.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct TabularIslamicDate // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static TabularIslamicDate Create(int year, int month, int day) => new(year, month, day);

    /// <inheritdoc />
    [Pure]
    public static TabularIslamicDate FromDayNumber(DayNumber dayNumber)
    {
        Calendar.Scope.Validate(dayNumber);

        // NB: the subtraction won't overflow.
        return new TabularIslamicDate(dayNumber.DaysSinceZero - EpochDaysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="TabularIslamicDate"/> struct
    /// from the specified date components.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static TabularIslamicDate UnsafeCreate(int year, int month, int day)
    {
        int daysSinceEpoch = Calendar.Schema.CountDaysSinceEpoch(year, month, day);
        return new TabularIslamicDate(daysSinceEpoch);
    }

    [Pure]
    static TabularIslamicDate IUnsafeFactory<TabularIslamicDate>.UnsafeCreate(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct TabularIslamicDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => Calendar.Schema.CountDaysInYearBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => Calendar.Schema.CountDaysInYearAfter(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Calendar.Schema.CountDaysInMonthBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => Calendar.Schema.CountDaysInMonthAfter(_daysSinceEpoch);
}

public partial struct TabularIslamicDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate WithYear(int newYear)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);

        // We MUST re-validate the entire date.
        chr.Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newYear, m, d);
        return new TabularIslamicDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate WithMonth(int newMonth)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceEpoch, out int y, out _, out int d);

        // We only need to validate "newMonth" and "d".
        chr.Scope.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newMonth, d);
        return new TabularIslamicDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate WithDay(int newDay)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out _);

        // We only need to validate "newDay".
        chr.Scope.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, m, newDay);
        return new TabularIslamicDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate WithDayOfYear(int newDayOfYear)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        int y = sch.GetYear(_daysSinceEpoch);

        // We only need to validate "newDayOfYear".
        chr.Scope.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newDayOfYear);
        return new TabularIslamicDate(daysSinceEpoch);
    }
}

public partial struct TabularIslamicDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < 0) ThrowHelpers.ThrowDateOverflow();
        return new TabularIslamicDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ > 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < 0) ThrowHelpers.ThrowDateOverflow();
        return new TabularIslamicDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate Nearest(DayOfWeek dayOfWeek)
    {
        var nearest = DayNumber.Nearest(dayOfWeek);
        int daysSinceEpoch = nearest.DaysSinceZero - EpochDaysSinceZero;
        if (unchecked((uint)daysSinceEpoch) > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new TabularIslamicDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ < 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new TabularIslamicDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new TabularIslamicDate(daysSinceEpoch);
    }
}

public partial struct TabularIslamicDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(TabularIslamicDate left, TabularIslamicDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(TabularIslamicDate left, TabularIslamicDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(TabularIslamicDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is TabularIslamicDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct TabularIslamicDate // IComparable
{
    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(TabularIslamicDate left, TabularIslamicDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified dates to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(TabularIslamicDate left, TabularIslamicDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(TabularIslamicDate left, TabularIslamicDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified dates to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(TabularIslamicDate left, TabularIslamicDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static TabularIslamicDate Min(TabularIslamicDate x, TabularIslamicDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static TabularIslamicDate Max(TabularIslamicDate x, TabularIslamicDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(TabularIslamicDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is TabularIslamicDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(TabularIslamicDate), obj);
}

public partial struct TabularIslamicDate // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(TabularIslamicDate left, TabularIslamicDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static TabularIslamicDate operator +(TabularIslamicDate value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static TabularIslamicDate operator -(TabularIslamicDate value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static TabularIslamicDate operator ++(TabularIslamicDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static TabularIslamicDate operator --(TabularIslamicDate value) => value.PreviousDay();

    /// <summary>
    /// Counts the number of days elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountDaysSince(TabularIslamicDate other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to MaxDaysSinceEpoch.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <summary>
    /// Adds a number of days to the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public TabularIslamicDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        if (unchecked((uint)daysSinceEpoch) > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new TabularIslamicDate(daysSinceEpoch);
    }

    /// <summary>
    /// Obtains the date after the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [Pure]
    public TabularIslamicDate NextDay()
    {
        if (_daysSinceEpoch == MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new TabularIslamicDate(_daysSinceEpoch + 1);
    }

    /// <summary>
    /// Obtains the date before the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [Pure]
    public TabularIslamicDate PreviousDay()
    {
        if (_daysSinceEpoch == 0) ThrowHelpers.ThrowDateOverflow();
        return new TabularIslamicDate(_daysSinceEpoch - 1);
    }

    //
    // Math operations based on the week unit
    //

    /// <summary>
    /// Counts the number of weeks elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountWeeksSince(TabularIslamicDate other) => MathZ.Divide(CountDaysSince(other), DaysInWeek);

    /// <summary>
    /// Adds a number of weeks to the current instance, yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public TabularIslamicDate AddWeeks(int weeks) => PlusDays(DaysInWeek * weeks);

    /// <summary>
    /// Obtains the date after the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public TabularIslamicDate NextWeek() => PlusDays(DaysInWeek);

    /// <summary>
    /// Obtains the date before the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public TabularIslamicDate PreviousWeek() => PlusDays(-DaysInWeek);
}

public partial struct TabularIslamicDate // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of this date instance, yielding
    /// a new date.
    /// <para>This method may truncate the (naïve) result to ensure that it
    /// returns a valid date; see <see cref="AdditionRule.Truncate"/>.</para>
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public TabularIslamicDate PlusYears(int years)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return AddYears(sch, y, m, d, years);
    }

    /// <summary>
    /// Adds a number of months to the month field of this date instance,
    /// yielding a new date.
    /// <para>This method may truncate the (naïve) result to ensure that it
    /// returns a valid date; see <see cref="AdditionRule.Truncate"/>.</para>
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public TabularIslamicDate PlusMonths(int months)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return AddMonths(sch, y, m, d, months);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified date.
    /// <para>Beware, the result may not be exact. Behind the scene, it uses
    /// <see cref="PlusYears(int)"/> which may apply a kind of truncation.</para>
    /// </summary>
    [Pure]
    public int CountYearsSince(TabularIslamicDate other)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(other._daysSinceEpoch, out int y0, out int m0, out int d0);

        // Exact difference between two calendar years.
        int years = Year - y0;

        // To avoid extracting y0 twice, we inline:
        // > var newStart = other.PlusYears(years);
        var newStart = AddYears(sch, y0, m0, d0, years);
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
    /// <para>Beware, the result may not be exact. Behind the scene, it uses
    /// <see cref="PlusMonths(int)"/> which may apply a kind of truncation.</para>
    /// </summary>
    [Pure]
    public int CountMonthsSince(TabularIslamicDate other)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out _);
        sch.GetDateParts(other._daysSinceEpoch, out int y0, out int m0, out int d0);

        // Exact difference between two calendar months.
        int months = checked(TabularIslamicCalendar.MonthsInYear * (y - y0) + m - m0);

        // To avoid extracting (y0, m0, d0) twice, we inline:
        // > var newStart = other.PlusMonths(months);
        var newStart = AddMonths(sch, y0, m0, d0, months);

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
    /// <para>This method may truncate the (naïve) result to ensure that it
    /// returns a valid date; see <see cref="AdditionRule.Truncate"/>.</para>
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static TabularIslamicDate AddYears(TabularIslamicSchema sch, int y, int m, int d, int years)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, m));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, m, newD);
        return new TabularIslamicDate(daysSinceEpoch);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date,
    /// yielding a new date.
    /// <para>This method may truncate the (naïve) result to ensure that it
    /// returns a valid date; see <see cref="AdditionRule.Truncate"/>.</para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static TabularIslamicDate AddMonths(TabularIslamicSchema sch, int y, int m, int d, int months)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), TabularIslamicCalendar.MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, newM));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return new TabularIslamicDate(daysSinceEpoch);
    }
}

#endregion

