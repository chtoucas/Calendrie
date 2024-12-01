﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Specialized;

using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Validation;
using Calendrie.Hemerology;

/// <summary>
/// Represents the Armenian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class Armenian13Calendar : SpecialCalendar<Armenian13Date>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Armenian13Calendar"/>
    /// class.
    /// </summary>
    public Armenian13Calendar() : this(new Egyptian13Schema()) { }

    internal Armenian13Calendar(Egyptian13Schema schema) : base("Armenian", GetScope(schema))
    {
        OnInitializing(schema);
    }

    [Pure]
    private static partial CalendarScope GetScope(Egyptian13Schema schema);

    partial void OnInitializing(Egyptian13Schema schema);

    [Pure]
    private protected sealed override Armenian13Date NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="Armenian13Date"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class Armenian13Adjuster : SpecialAdjuster<Armenian13Date>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Armenian13Adjuster"/>
    /// class.
    /// </summary>
    public Armenian13Adjuster() : base(Armenian13Date.Calendar.Scope) { }

    internal Armenian13Adjuster(CalendarScope scope) : base(scope) { }

    [Pure]
    private protected sealed override Armenian13Date NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the Armenian date.
/// <para><see cref="Armenian13Date"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct Armenian13Date :
    IDate<Armenian13Date, Armenian13Calendar>,
    IAdjustable<Armenian13Date>
{ }

public partial struct Armenian13Date // Preamble
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly Egyptian13Schema s_Schema = new();
    private static readonly Armenian13Calendar s_Calendar = new(s_Schema);
    private static readonly CalendarScope s_Scope = s_Calendar.Scope;
    private static readonly DayNumber s_Epoch = s_Calendar.Epoch;
    private static readonly Range<DayNumber> s_Domain = s_Calendar.Domain;
    private static readonly Armenian13Adjuster s_Adjuster = new(s_Scope);
    private static readonly Armenian13Date s_MinValue = new(s_Domain.Min - s_Epoch);
    private static readonly Armenian13Date s_MaxValue = new(s_Domain.Max - s_Epoch);

    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="Armenian13Date"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public Armenian13Date(int year, int month, int day)
    {
        s_Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Armenian13Date"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
    public Armenian13Date(int year, int dayOfYear)
    {
        s_Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = s_Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal Armenian13Date(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static Armenian13Date MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static Armenian13Date MaxValue => s_MaxValue;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Armenian13Adjuster Adjuster => s_Adjuster;

    /// <inheritdoc />
    public static Armenian13Calendar Calendar => s_Calendar;

    /// <inheritdoc />
    public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;

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

    /// <inheritdoc />
    public int Year => s_Schema.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = s_Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsIntercalaryDay(y, m, d);
        }
    }

    /// <inheritdoc />
    public bool IsSupplementary
    {
        get
        {
            s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return s_Schema.IsSupplementaryDay(y, m, d);
        }
    }

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        s_Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({s_Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        s_Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = s_Schema.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct Armenian13Date // Factories
{
    /// <inheritdoc />
    [Pure]
    public static Armenian13Date FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        return new Armenian13Date(dayNumber - s_Epoch);
    }
}

public partial struct Armenian13Date // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => s_Schema.CountDaysInYearBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => s_Schema.CountDaysInYearAfter(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => s_Schema.CountDaysInMonthBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => s_Schema.CountDaysInMonthAfter(_daysSinceEpoch);
}

public partial struct Armenian13Date // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public Armenian13Date Adjust(Func<Armenian13Date, Armenian13Date> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        if (!s_Domain.Contains(dayNumber)) ThrowHelpers.ThrowDateOverflow();
        return new Armenian13Date(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        if (!s_Domain.Contains(dayNumber)) ThrowHelpers.ThrowDateOverflow();
        return new Armenian13Date(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        if (!s_Domain.Contains(dayNumber)) ThrowHelpers.ThrowDateOverflow();
        return new Armenian13Date(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        if (!s_Domain.Contains(dayNumber)) ThrowHelpers.ThrowDateOverflow();
        return new Armenian13Date(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        if (!s_Domain.Contains(dayNumber)) ThrowHelpers.ThrowDateOverflow();
        return new Armenian13Date(dayNumber - s_Epoch);
    }
}

public partial struct Armenian13Date // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(Armenian13Date other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Armenian13Date date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct Armenian13Date // IComparable
{
    /// <inheritdoc />
    public static bool operator <(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(Armenian13Date left, Armenian13Date right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static Armenian13Date Min(Armenian13Date x, Armenian13Date y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static Armenian13Date Max(Armenian13Date x, Armenian13Date y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(Armenian13Date other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is Armenian13Date date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(Armenian13Date), obj);
}

public partial struct Armenian13Date // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(Armenian13Date left, Armenian13Date right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static Armenian13Date operator +(Armenian13Date value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static Armenian13Date operator -(Armenian13Date value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static Armenian13Date operator ++(Armenian13Date value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static Armenian13Date operator --(Armenian13Date value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(Armenian13Date other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public Armenian13Date AddDays(int days)
    {
        int daysSinceEpoch = checked(_daysSinceEpoch + days);
        // Don't write (the addition may also overflow...):
        // > s_Domain.CheckOverflow(s_Epoch + daysSinceEpoch);
        s_Scope.DaysValidator.CheckOverflow(daysSinceEpoch);
        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new Armenian13Date(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new Armenian13Date(_daysSinceEpoch - 1);
    }
}
