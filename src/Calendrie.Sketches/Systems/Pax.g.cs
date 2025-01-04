﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Systems;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Numerics;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the Pax calendar.
/// <para>This calendar is <i>retropolated</i>. It supports <i>all</i> dates
/// within the range [1..9999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class PaxCalendar : CalendarSystem<PaxDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaxCalendar"/> class.
    /// </summary>
    public PaxCalendar() : this(new PaxSchema()) { }

    private PaxCalendar(PaxSchema schema)
        : base("Pax", new StandardScope(schema, DayZero.SundayBeforeGregorian))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="PaxCalendar"/> class.
    /// <para>See <see cref="PaxDate.Calendar"/>.</para>
    /// </summary>
    internal static PaxCalendar Instance { get; } = new();

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
    internal PaxSchema Schema { get; }

    /// <summary>
    /// Obtains the number of months in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure]
    public int CountMonthsInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountMonthsInYear(year);
    }
}

/// <summary>
/// Represents the Pax date.
/// <para><i>All</i> dates within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="PaxDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct PaxDate :
    IDateable,
    IAbsoluteDate<PaxDate>,
    IAdjustableDate<PaxDate>,
    IDateFactory<PaxDate>,
    ISubtractionOperators<PaxDate, PaxDate, int>
{ }

public partial struct PaxDate // Preamble
{
    /// <summary>Represents the value of the property <see cref="DayNumber.DaysSinceZero"/>
    /// for the epoch <see cref="DayZero.SundayBeforeGregorian"/>.
    /// <para>This field is a constant equal to -1.</para></summary>
    private const int EpochDaysSinceZero = -1;

    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to 3_652_060.</para></summary>
    private const int MaxDaysSinceEpoch = 3_652_060;

    /// <summary>
    /// Represents the count of consecutive days since the epoch <see cref="DayZero.SundayBeforeGregorian"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxDaysSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaxDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public PaxDate(int year, int month, int day)
    {
        var chr = PaxCalendar.Instance;
        chr.Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaxDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public PaxDate(int year, int dayOfYear)
    {
        var chr = PaxCalendar.Instance;
        chr.Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal PaxDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <summary>
    /// Gets the earliest possible value of a <see cref="PaxDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    //
    // MinValue = new(0) = new() = default(PaxDate)
    public static PaxDate MinValue { get; }

    /// <summary>
    /// Gets the latest possible value of a <see cref="PaxDate"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static PaxDate MaxValue { get; } = new(MaxDaysSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static PaxCalendar Calendar => PaxCalendar.Instance;

    /// <inheritdoc />
    //
    // We already know that the resulting day number is valid so instead of
    // > public DayNumber DayNumber => Epoch + _daysSinceEpoch;
    // we can use an unchecked addition
    public DayNumber DayNumber => new(EpochDaysSinceZero + _daysSinceEpoch);

    /// <inheritdoc />
    public int DaysSinceEpoch => _daysSinceEpoch;

    /// <inheritdoc />
    public Ord CenturyOfEra => Ord.FromInt32(Century);

    /// <inheritdoc />
    public int Century => YearNumbering.GetCentury(Year);

    /// <inheritdoc />
    public Ord YearOfEra => Ord.FromInt32(Year);

    /// <inheritdoc />
    public int YearOfCentury => YearNumbering.GetYearOfCentury(Year);

    /// <summary>
    /// Gets the year number.
    /// <para>This property represents the algebraic year, but since it's greater
    /// than 0, there is no difference between the algebraic year and the year
    /// of the era.</para>
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

    /// <inheritdoc />
    public bool IsSupplementary
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return sch.IsSupplementaryDay(y, m, d);
        }
    }

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

public partial struct PaxDate // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static PaxDate FromDayNumber(DayNumber dayNumber)
    {
        Calendar.Scope.Validate(dayNumber);

        // NB: the subtraction won't overflow.
        return new(dayNumber.DaysSinceZero - EpochDaysSinceZero);
    }

    /// <inheritdoc />
    [Pure]
    static PaxDate IDateFactory<PaxDate>.UnsafeCreate(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct PaxDate // Counting
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

public partial struct PaxDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public PaxDate WithYear(int newYear)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);

        // We MUST re-validate the entire date.
        chr.Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newYear, m, d);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public PaxDate WithMonth(int newMonth)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceEpoch, out int y, out _, out int d);

        // We only need to validate "newMonth" and "d".
        chr.Scope.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public PaxDate WithDay(int newDay)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out _);

        // We only need to validate "newDay".
        chr.Scope.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, m, newDay);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public PaxDate WithDayOfYear(int newDayOfYear)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        int y = sch.GetYear(_daysSinceEpoch);

        // We only need to validate "newDayOfYear".
        chr.Scope.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceEpoch);
    }
}

public partial struct PaxDate // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public PaxDate Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public PaxDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ > 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < 0) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public PaxDate Nearest(DayOfWeek dayOfWeek)
    {
        var nearest = DayNumber.Nearest(dayOfWeek);
        int daysSinceEpoch = nearest.DaysSinceZero - EpochDaysSinceZero;
        if (unchecked((uint)daysSinceEpoch) > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public PaxDate NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ < 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public PaxDate Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }
}

public partial struct PaxDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(PaxDate left, PaxDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(PaxDate left, PaxDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(PaxDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is PaxDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct PaxDate // IComparable
{
    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(PaxDate left, PaxDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified dates to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(PaxDate left, PaxDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(PaxDate left, PaxDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified dates to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(PaxDate left, PaxDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <summary>
    /// Obtains the earliest date between the two specified dates.
    /// </summary>
    [Pure]
    public static PaxDate Min(PaxDate x, PaxDate y) => x < y ? x : y;

    /// <summary>
    /// Obtains the latest date between the two specified dates.
    /// </summary>
    [Pure]
    public static PaxDate Max(PaxDate x, PaxDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(PaxDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is PaxDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(PaxDate), obj);
}

public partial struct PaxDate // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(PaxDate left, PaxDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static PaxDate operator +(PaxDate value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static PaxDate operator -(PaxDate value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static PaxDate operator ++(PaxDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static PaxDate operator --(PaxDate value) => value.PreviousDay();

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(PaxDate other) =>
        // No need to use a checked context here. Indeed, the absolute value of
        // the result is at most equal to MaxDaysSinceEpoch.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public PaxDate PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        if (unchecked((uint)daysSinceEpoch) > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public PaxDate NextDay()
    {
        if (_daysSinceEpoch == MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public PaxDate PreviousDay()
    {
        if (_daysSinceEpoch == 0) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

public partial struct PaxDate // Non-standard math ops
{
    /// <summary>Represents the maximum value for the number of consecutive
    /// months from the epoch.
    /// <para>This field is a constant equal to 131_761.</para></summary>
    private const int MaxMonthsSinceEpoch = 131_761;

    /// <summary>
    /// Adds a number of years to the year field of this date instance, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public PaxDate PlusYears(int years)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return AddYears(sch, y, m, d, years);
    }

    /// <summary>
    /// Adds a number of months to the month field of this date instance,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public PaxDate PlusMonths(int months)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return AddMonths(sch, y, m, d, months);
    }
    /// <summary>
    /// Counts the number of years elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountYearsSince(PaxDate other)
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
    /// </summary>
    [Pure]
    public int CountMonthsSince(PaxDate other)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out _);
        sch.GetDateParts(other._daysSinceEpoch, out int y0, out int m0, out int d0);

        // Exact difference between two calendar months.
        int months = checked(sch.CountMonthsSinceEpoch(y, m) - sch.CountMonthsSinceEpoch(y0, m0));

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
    private static PaxDate AddYears(PaxSchema sch, int y, int m, int d, int years)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newM = Math.Min(m, sch.CountMonthsInYear(newY));
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, m));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return new PaxDate(daysSinceEpoch);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static PaxDate AddMonths(PaxSchema sch, int y, int m, int d, int months)
    {
        // Exact addition of months to a calendar month.
        int monthsSinceEpoch = checked(sch.CountMonthsSinceEpoch(y, m) + months);
        if (unchecked((uint)monthsSinceEpoch) > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();

        sch.GetMonthParts(monthsSinceEpoch, out int newY, out int newM);

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, newM));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return new PaxDate(daysSinceEpoch);
    }
}

