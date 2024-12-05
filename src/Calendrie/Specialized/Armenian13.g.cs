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
/// Provides static methods related to the scope of application of
/// <see cref="Armenian13Calendar"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static partial class Armenian13Scope
{
    // WARNING: the order in which the static fields are written is __important__.

    public static partial DayNumber Epoch { get; }

    public static readonly Egyptian13Schema Schema = new();

    public static readonly StandardScope Instance = new(Schema, Epoch);
}

/// <summary>
/// Represents the Armenian calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class Armenian13Calendar : SpecialCalendar<Armenian13Date>
{
    internal static readonly Armenian13Calendar Instance = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Armenian13Calendar"/>
    /// class.
    /// <para>See also <seealso cref="Armenian13Date.Calendar"/>.</para>
    /// </summary>
    public Armenian13Calendar() :
        this(new StandardScope(new Egyptian13Schema(), Armenian13Scope.Epoch))
    { }

    private Armenian13Calendar(StandardScope scope) : base("Armenian", scope)
    {
        Adjuster = new Armenian13Adjuster(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public Armenian13Adjuster Adjuster { get; }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    internal Armenian13Adjuster(Armenian13Calendar calendar) : base(calendar) { }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    private static readonly DayNumber s_Epoch = Armenian13Scope.Instance.Epoch;
    private static readonly Range<DayNumber> s_Domain = Armenian13Scope.Instance.Domain;

    private static readonly int s_MinDaysSinceEpoch = Armenian13Scope.Instance.Segment.SupportedDays.Min;
    private static readonly int s_MaxDaysSinceEpoch = Armenian13Scope.Instance.Segment.SupportedDays.Max;

    private static readonly Armenian13Date s_MinValue = new(s_MinDaysSinceEpoch);
    private static readonly Armenian13Date s_MaxValue = new(s_MaxDaysSinceEpoch);

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
        Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
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
        Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
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

    /// <inheritdoc />
    public static Armenian13Calendar Calendar => Armenian13Calendar.Instance;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Armenian13Adjuster Adjuster => Armenian13Calendar.Instance.Adjuster;

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
    public int Year => Schema.GetYear(_daysSinceEpoch);

    /// <inheritdoc />
    public int Month
    {
        get
        {
            Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out _);
            return m;
        }
    }

    /// <inheritdoc />
    public int DayOfYear
    {
        get
        {
            _ = Schema.GetYear(_daysSinceEpoch, out int doy);
            return doy;
        }
    }

    /// <inheritdoc />
    public int Day
    {
        get
        {
            Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
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
            Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return Schema.IsIntercalaryDay(y, m, d);
        }
    }

    /// <inheritdoc />
    public bool IsSupplementary
    {
        get
        {
            Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return Schema.IsSupplementaryDay(y, m, d);
        }
    }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    private static Egyptian13Schema Schema => Armenian13Scope.Schema;

    /// <summary>
    /// Gets the calendar scope.
    /// </summary>
    private static StandardScope Scope => Armenian13Scope.Instance;

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString()
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return FormattableString.Invariant($"{d:D2}/{m:D2}/{y:D4} ({Calendar})");
    }

    /// <inheritdoc />
    public void Deconstruct(out int year, out int month, out int day) =>
        Schema.GetDateParts(_daysSinceEpoch, out year, out month, out day);

    /// <inheritdoc />
    public void Deconstruct(out int year, out int dayOfYear) =>
        year = Schema.GetYear(_daysSinceEpoch, out dayOfYear);
}

public partial struct Armenian13Date // Factories
{
    /// <inheritdoc />
    [Pure]
    public static Armenian13Date FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        return new(dayNumber - s_Epoch);
    }
}

public partial struct Armenian13Date // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => Schema.CountDaysInYearBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => Schema.CountDaysInYearAfter(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Schema.CountDaysInMonthBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => Schema.CountDaysInMonthAfter(_daysSinceEpoch);
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
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        s_Domain.CheckOverflow(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber - s_Epoch);
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
        // > s_Domain.CheckOverflow(Epoch + daysSinceEpoch);
        if (daysSinceEpoch < s_MinDaysSinceEpoch || daysSinceEpoch > s_MaxDaysSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();

        return new(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public Armenian13Date PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}

