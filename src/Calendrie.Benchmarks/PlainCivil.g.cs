﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable

namespace Benchmarks;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Numerics;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;
using Calendrie.Systems;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the Plain Civil calendar.
/// <para>This calendar is <i>retropolated</i>. It supports <i>all</i> dates
/// within the range [1..9999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class PlainCivilCalendar : CalendarSystem<PlainCivilDate>
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GregorianSchema.MonthsInYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainCivilCalendar"/> class.
    /// </summary>
    public PlainCivilCalendar() : this(new GregorianSchema()) { }

    private PlainCivilCalendar(GregorianSchema schema)
        : base("Plain Civil", new StandardScope(schema, DayZero.NewStyle))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="PlainCivilCalendar"/> class.
    /// <para>See <see cref="PlainCivilDate.Calendar"/>.</para>
    /// </summary>
    internal static PlainCivilCalendar Instance { get; } = new();

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
    internal GregorianSchema Schema { get; }
}

/// <summary>
/// Represents the Plain Civil date.
/// <para><i>All</i> dates within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="PlainCivilDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct PlainCivilDate :
    ICalendarDate<PlainCivilDate>,
    ICalendarBound<PlainCivilCalendar>,
    IUnsafeDateFactory<PlainCivilDate>,
    ISubtractionOperators<PlainCivilDate, PlainCivilDate, int>
{ }

public partial struct PlainCivilDate // Preamble
{
    /// <summary>Represents the maximum value of <see cref="_daysSinceZero"/>.
    /// <para>This field is a constant equal to 3_652_058.</para></summary>
    private const int MaxDaysSinceZero = 3_652_058;

    /// <summary>
    /// Represents the count of consecutive days since <see cref="DayZero.NewStyle"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxDaysSinceZero"/>.
    /// </para>
    /// </summary>
    private readonly int _daysSinceZero;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainCivilDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public PlainCivilDate(int year, int month, int day)
    {
        var chr = PlainCivilCalendar.Instance;
        chr.Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceZero = chr.Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainCivilDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public PlainCivilDate(int year, int dayOfYear)
    {
        var chr = PlainCivilCalendar.Instance;
        chr.Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceZero = chr.Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal PlainCivilDate(int daysSinceZero)
    {
        _daysSinceZero = daysSinceZero;
    }

    /// <summary>
    /// Gets the earliest possible value of a <see cref="PlainCivilDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // MinValue = new(0) = new() = default(PlainCivilDate)
    public static PlainCivilDate MinValue { get; }

    /// <summary>
    /// Gets the latest possible value of a <see cref="PlainCivilDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static PlainCivilDate MaxValue { get; } = new(MaxDaysSinceZero);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static PlainCivilCalendar Calendar => PlainCivilCalendar.Instance;

    /// <inheritdoc />
    public DayNumber DayNumber => new(_daysSinceZero);

    /// <summary>Gets the count of days since the Gregorian epoch.</summary>
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

    /// <summary>
    /// Gets the year number.
    /// <para>This property represents the algebraic year, but since it's greater
    /// than 0, there is no difference between the algebraic year and the year
    /// of the era.</para>
    /// </summary>
    public int Year => Calendar.Schema.GetYear(_daysSinceZero);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceZero, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = Calendar.Schema.GetYear(_daysSinceZero, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceZero, out _, out _, out int d);
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
            sch.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
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
        chr.Schema.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({chr})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        Calendar.Schema.GetDateParts(_daysSinceZero, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Calendar.Schema.GetYear(_daysSinceZero, out dayOfYear);
}

public partial struct PlainCivilDate // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static PlainCivilDate FromDayNumber(DayNumber dayNumber)
    {
        int daysSinceZero = dayNumber.DaysSinceZero;

        if (unchecked((uint)daysSinceZero) > MaxDaysSinceZero)
            throw new ArgumentOutOfRangeException(nameof(dayNumber));

        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    static PlainCivilDate IUnsafeDateFactory<PlainCivilDate>.UnsafeCreate(int daysSinceZero) =>
        new(daysSinceZero);
}

public partial struct PlainCivilDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => Calendar.Schema.CountDaysInYearBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => Calendar.Schema.CountDaysInYearAfter(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Calendar.Schema.CountDaysInMonthBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => Calendar.Schema.CountDaysInMonthAfter(_daysSinceZero);
}

public partial struct PlainCivilDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public PlainCivilDate WithYear(int newYear)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceZero, out _, out int m, out int d);

        // We MUST re-validate the entire date.
        chr.Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceZero = sch.CountDaysSinceEpoch(newYear, m, d);
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainCivilDate WithMonth(int newMonth)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceZero, out int y, out _, out int d);

        // We only need to validate "newMonth" and "d".
        chr.Scope.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceZero = sch.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainCivilDate WithDay(int newDay)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceZero, out int y, out int m, out _);

        // We only need to validate "newDay".
        chr.Scope.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceZero = sch.CountDaysSinceEpoch(y, m, newDay);
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainCivilDate WithDayOfYear(int newDayOfYear)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        int y = sch.GetYear(_daysSinceZero);

        // We only need to validate "newDayOfYear".
        chr.Scope.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceZero = sch.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceZero);
    }
}

public partial struct PlainCivilDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public PlainCivilDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceZero < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainCivilDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceZero = _daysSinceZero + (δ > 0 ? δ - DaysInWeek : δ);
        if (daysSinceZero < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainCivilDate Nearest(DayOfWeek dayOfWeek)
    {
        int daysSinceZero = DayNumber.Nearest(dayOfWeek).DaysSinceZero;
        if (unchecked((uint)daysSinceZero) > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    public PlainCivilDate NextOrSame(DayOfWeek dayOfWeek)
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
    public PlainCivilDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceZero = _daysSinceZero + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceZero > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }
}

public partial struct PlainCivilDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(PlainCivilDate left, PlainCivilDate right) =>
        left._daysSinceZero == right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator !=(PlainCivilDate left, PlainCivilDate right) =>
        left._daysSinceZero != right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public bool Equals(PlainCivilDate other) => _daysSinceZero == other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is PlainCivilDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceZero;
}

public partial struct PlainCivilDate // IComparable
{
    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(PlainCivilDate left, PlainCivilDate right) =>
        left._daysSinceZero < right._daysSinceZero;

    /// <summary>
    /// Compares the two specified dates to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(PlainCivilDate left, PlainCivilDate right) =>
        left._daysSinceZero <= right._daysSinceZero;

    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(PlainCivilDate left, PlainCivilDate right) =>
        left._daysSinceZero > right._daysSinceZero;

    /// <summary>
    /// Compares the two specified dates to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(PlainCivilDate left, PlainCivilDate right) =>
        left._daysSinceZero >= right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public static PlainCivilDate Min(PlainCivilDate x, PlainCivilDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static PlainCivilDate Max(PlainCivilDate x, PlainCivilDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(PlainCivilDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is PlainCivilDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(PlainCivilDate), obj);
}

public partial struct PlainCivilDate // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(PlainCivilDate left, PlainCivilDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static PlainCivilDate operator +(PlainCivilDate value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static PlainCivilDate operator -(PlainCivilDate value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static PlainCivilDate operator ++(PlainCivilDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static PlainCivilDate operator --(PlainCivilDate value) => value.PreviousDay();

    /// <summary>
    /// Counts the number of days elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountDaysSince(PlainCivilDate other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to MaxDaysSinceZero.
        _daysSinceZero - other._daysSinceZero;

    /// <summary>
    /// Adds a number of days to the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public PlainCivilDate PlusDays(int days)
    {
        int daysSinceZero = checked(_daysSinceZero + days);
        if (unchecked((uint)daysSinceZero) > MaxDaysSinceZero) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceZero);
    }

    /// <summary>
    /// Obtains the date after the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [Pure]
    public PlainCivilDate NextDay()
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
    public PlainCivilDate PreviousDay()
    {
        if (_daysSinceZero == 0) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceZero - 1);
    }

    //
    // Math operations based on the week unit
    //

    /// <inheritdoc />
    [Pure]
    public int CountWeeksSince(PlainCivilDate other) => MathZ.Divide(CountDaysSince(other), DaysInWeek);

    /// <inheritdoc />
    [Pure]
    public PlainCivilDate AddWeeks(int weeks) => PlusDays(DaysInWeek * weeks);

    /// <inheritdoc />
    [Pure]
    public PlainCivilDate NextWeek() => PlusDays(DaysInWeek);

    /// <inheritdoc />
    [Pure]
    public PlainCivilDate PreviousWeek() => PlusDays(-DaysInWeek);
}

public partial struct PlainCivilDate // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of this date instance, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public PlainCivilDate PlusYears(int years)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return AddYears(sch, y, m, d, years);
    }

    /// <summary>
    /// Adds a number of months to the month field of this date instance,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public PlainCivilDate PlusMonths(int months)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceZero, out int y, out int m, out int d);
        return AddMonths(sch, y, m, d, months);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountYearsSince(PlainCivilDate other)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(other._daysSinceZero, out int y0, out int m0, out int d0);

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
    /// </summary>
    [Pure]
    public int CountMonthsSince(PlainCivilDate other)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceZero, out int y, out int m, out _);
        sch.GetDateParts(other._daysSinceZero, out int y0, out int m0, out int d0);

        // Exact difference between two calendar months.
        int months = checked(PlainCivilCalendar.MonthsInYear * (y - y0) + m - m0);

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
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static PlainCivilDate AddYears(GregorianSchema sch, int y, int m, int d, int years)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, m));

        int daysSinceZero = sch.CountDaysSinceEpoch(newY, m, newD);
        return new PlainCivilDate(daysSinceZero);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static PlainCivilDate AddMonths(GregorianSchema sch, int y, int m, int d, int months)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), PlainCivilCalendar.MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, newM));

        int daysSinceZero = sch.CountDaysSinceEpoch(newY, newM, newD);
        return new PlainCivilDate(daysSinceZero);
    }
}

