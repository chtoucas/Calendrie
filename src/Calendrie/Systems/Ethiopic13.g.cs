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

#region Ethiopic13Calendar

/// <summary>
/// Represents the Ethiopic calendar.
/// <para>This calendar is <i>retropolated</i>. It supports <i>all</i> dates
/// within the range [1..9999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class Ethiopic13Calendar : CalendarSystem<Ethiopic13Date>
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    public const int MonthsInYear = Coptic13Schema.MonthsInYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Calendar"/> class.
    /// </summary>
    public Ethiopic13Calendar() : this(new Coptic13Schema()) { }

    private Ethiopic13Calendar(Coptic13Schema schema)
        : base("Ethiopic", new StandardScope(schema, DayZero.Ethiopic))
    {
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="Ethiopic13Calendar"/> class.
    /// <para>See <see cref="Ethiopic13Date.Calendar"/>.</para>
    /// </summary>
    internal static Ethiopic13Calendar Instance { get; } = new();

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
    internal Coptic13Schema Schema { get; }
}

#endregion

#region Ethiopic13Date

/// <summary>
/// Represents the Ethiopic date.
/// <para><i>All</i> dates within the range [1..9999] of years are supported.
/// </para>
/// <para><see cref="Ethiopic13Date"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Ethiopic13Date :
    IDate<Ethiopic13Date>,
    ICalendarBound<Ethiopic13Calendar>,
    IUnsafeFactory<Ethiopic13Date>,
    ISubtractionOperators<Ethiopic13Date, Ethiopic13Date, int>
{ }

public partial struct Ethiopic13Date // Preamble
{
    /// <summary>Represents the value of the property <see cref="DayNumber.DaysSinceZero"/>
    /// for the epoch <see cref="DayZero.Ethiopic"/>.
    /// <para>This field is a constant equal to 2795.</para></summary>
    private const int EpochDaysSinceZero = 2795;

    /// <summary>Represents the maximum value of <see cref="_daysSinceEpoch"/>.
    /// <para>This field is a constant equal to 3_652_134.</para></summary>
    private const int MaxDaysSinceEpoch = 3_652_134;

    /// <summary>
    /// Represents the count of consecutive days since the epoch
    /// <see cref="DayZero.Ethiopic"/>.
    /// <para>This field is in the range from 0 to <see cref="MaxDaysSinceEpoch"/>.
    /// </para>
    /// </summary>
    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Date"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the range
    /// of supported years.</exception>
    public Ethiopic13Date(int year, int month, int day)
    {
        var chr = Ethiopic13Calendar.Instance;
        chr.Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Date"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid ordinal date or <paramref name="year"/> is outside
    /// the range of supported years.</exception>
    public Ethiopic13Date(int year, int dayOfYear)
    {
        var chr = Ethiopic13Calendar.Instance;
        chr.Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = chr.Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ethiopic13Date"/> struct.
    /// <para>This constructor does NOT validate its parameter.</para>
    /// </summary>
    internal Ethiopic13Date(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <summary>
    /// Gets the smallest possible value of <see cref="Ethiopic13Date"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The earliest supported date.</returns>
    //
    // MinValue = new(0) = new() = default(Ethiopic13Date)
    public static Ethiopic13Date MinValue { get; }

    /// <summary>
    /// Gets the largest possible value of <see cref="Ethiopic13Date"/>.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    /// <returns>The latest supported date.</returns>
    public static Ethiopic13Date MaxValue { get; } = new(MaxDaysSinceEpoch);

    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Ethiopic13Calendar Calendar => Ethiopic13Calendar.Instance;

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

public partial struct Ethiopic13Date // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static Ethiopic13Date Create(int year, int month, int day) => new(year, month, day);

    /// <inheritdoc />
    [Pure]
    public static Ethiopic13Date FromDayNumber(DayNumber dayNumber)
    {
        Calendar.Scope.Validate(dayNumber);

        // NB: the subtraction won't overflow.
        return new Ethiopic13Date(dayNumber.DaysSinceZero - EpochDaysSinceZero);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Ethiopic13Date"/> struct
    /// from the specified date components.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Ethiopic13Date UnsafeCreate(int year, int month, int day)
    {
        int daysSinceEpoch = Calendar.Schema.CountDaysSinceEpoch(year, month, day);
        return new Ethiopic13Date(daysSinceEpoch);
    }

    [Pure]
    static Ethiopic13Date IUnsafeFactory<Ethiopic13Date>.UnsafeCreate(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct Ethiopic13Date // Counting
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

public partial struct Ethiopic13Date // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date WithYear(int newYear)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);

        // We MUST re-validate the entire date.
        chr.Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newYear, m, d);
        return new Ethiopic13Date(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date WithMonth(int newMonth)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceEpoch, out int y, out _, out int d);

        // We only need to validate "newMonth" and "d".
        chr.Scope.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newMonth, d);
        return new Ethiopic13Date(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date WithDay(int newDay)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out _);

        // We only need to validate "newDay".
        chr.Scope.PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, m, newDay);
        return new Ethiopic13Date(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date WithDayOfYear(int newDayOfYear)
    {
        var chr = Calendar;
        var sch = Calendar.Schema;

        int y = sch.GetYear(_daysSinceEpoch);

        // We only need to validate "newDayOfYear".
        chr.Scope.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, newDayOfYear);
        return new Ethiopic13Date(daysSinceEpoch);
    }
}

public partial struct Ethiopic13Date // Find close by day of the week
{
    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Previous(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ >= 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < 0) ThrowHelpers.ThrowDateOverflow();
        return new Ethiopic13Date(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date PreviousOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ > 0 ? δ - DaysInWeek : δ);
        if (daysSinceEpoch < 0) ThrowHelpers.ThrowDateOverflow();
        return new Ethiopic13Date(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Nearest(DayOfWeek dayOfWeek)
    {
        var nearest = DayNumber.Nearest(dayOfWeek);
        int daysSinceEpoch = nearest.DaysSinceZero - EpochDaysSinceZero;
        if (unchecked((uint)daysSinceEpoch) > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new Ethiopic13Date(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date NextOrSame(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        if (δ == 0) return this;
        int daysSinceEpoch = _daysSinceEpoch + (δ < 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new Ethiopic13Date(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Ethiopic13Date Next(DayOfWeek dayOfWeek)
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - DayOfWeek;
        int daysSinceEpoch = _daysSinceEpoch + (δ <= 0 ? δ + DaysInWeek : δ);
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new Ethiopic13Date(daysSinceEpoch);
    }
}

public partial struct Ethiopic13Date // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Ethiopic13Date other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Ethiopic13Date date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct Ethiopic13Date // IComparable
{
    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// earlier than the right one.
    /// </summary>
    public static bool operator <(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified dates to see if the left one is earlier
    /// than or equal to the right one.
    /// </summary>
    public static bool operator <=(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified dates to see if the left one is strictly
    /// later than the right one.
    /// </summary>
    public static bool operator >(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <summary>
    /// Compares the two specified dates to see if the left one is later than
    /// or equal to the right one.
    /// </summary>
    public static bool operator >=(Ethiopic13Date left, Ethiopic13Date right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static Ethiopic13Date Min(Ethiopic13Date x, Ethiopic13Date y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static Ethiopic13Date Max(Ethiopic13Date x, Ethiopic13Date y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Ethiopic13Date other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Ethiopic13Date date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(Ethiopic13Date), obj);
}

public partial struct Ethiopic13Date // Standard math ops
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    public static int operator -(Ethiopic13Date left, Ethiopic13Date right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static Ethiopic13Date operator +(Ethiopic13Date value, int days) => value.PlusDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PlusDays()")]
    public static Ethiopic13Date operator -(Ethiopic13Date value, int days) => value.PlusDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See NextDay()")]
    public static Ethiopic13Date operator ++(Ethiopic13Date value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See PreviousDay()")]
    public static Ethiopic13Date operator --(Ethiopic13Date value) => value.PreviousDay();

    /// <summary>
    /// Counts the number of days elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountDaysSince(Ethiopic13Date other) =>
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
    public Ethiopic13Date PlusDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        if (unchecked((uint)daysSinceEpoch) > MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new Ethiopic13Date(daysSinceEpoch);
    }

    /// <summary>
    /// Obtains the date after the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [Pure]
    public Ethiopic13Date NextDay()
    {
        if (_daysSinceEpoch == MaxDaysSinceEpoch) ThrowHelpers.ThrowDateOverflow();
        return new Ethiopic13Date(_daysSinceEpoch + 1);
    }

    /// <summary>
    /// Obtains the date before the current instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [Pure]
    public Ethiopic13Date PreviousDay()
    {
        if (_daysSinceEpoch == 0) ThrowHelpers.ThrowDateOverflow();
        return new Ethiopic13Date(_daysSinceEpoch - 1);
    }

    //
    // Math operations based on the week unit
    //

    /// <summary>
    /// Counts the number of weeks elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountWeeksSince(Ethiopic13Date other) => MathZ.Divide(CountDaysSince(other), DaysInWeek);

    /// <summary>
    /// Adds a number of weeks to the current instance, yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public Ethiopic13Date AddWeeks(int weeks) => PlusDays(DaysInWeek * weeks);

    /// <summary>
    /// Obtains the date after the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public Ethiopic13Date NextWeek() => PlusDays(DaysInWeek);

    /// <summary>
    /// Obtains the date before the current instance falling on the same day of
    /// the week, yielding a new date.
    /// </summary>
    [Pure]
    public Ethiopic13Date PreviousWeek() => PlusDays(-DaysInWeek);
}

public partial struct Ethiopic13Date // Non-standard math ops
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
    public Ethiopic13Date PlusYears(int years)
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
    public Ethiopic13Date PlusMonths(int months)
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
    public int CountYearsSince(Ethiopic13Date other)
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
    public int CountMonthsSince(Ethiopic13Date other)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out _);
        sch.GetDateParts(other._daysSinceEpoch, out int y0, out int m0, out int d0);

        // Exact difference between two calendar months.
        int months = checked(Ethiopic13Calendar.MonthsInYear * (y - y0) + m - m0);

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
    private static Ethiopic13Date AddYears(Coptic13Schema sch, int y, int m, int d, int years)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, m));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, m, newD);
        return new Ethiopic13Date(daysSinceEpoch);
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
    private static Ethiopic13Date AddMonths(Coptic13Schema sch, int y, int m, int d, int months)
    {
        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), Ethiopic13Calendar.MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, newM));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return new Ethiopic13Date(daysSinceEpoch);
    }
}

#endregion

