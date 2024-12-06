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
/// <see cref="TabularIslamicCalendar"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static partial class TabularIslamicScope
{
    public static readonly DayNumber Epoch = DayZero.TabularIslamic;
    public static readonly TabularIslamicSchema Schema = new();

    public static readonly StandardScope Instance = new(Schema, Epoch);

    public static StandardScope Create() => new(new TabularIslamicSchema(), Epoch);
}

/// <summary>
/// Represents the Tabular Islamic calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class TabularIslamicCalendar : SpecialCalendar<TabularIslamicDate>
{
    internal static readonly TabularIslamicCalendar Instance = new(TabularIslamicScope.Instance);

    /// <summary>
    /// Initializes a new instance of the <see cref="TabularIslamicCalendar"/>
    /// class.
    /// <para>See also <seealso cref="TabularIslamicDate.Calendar"/>.</para>
    /// </summary>
    public TabularIslamicCalendar() : this(TabularIslamicScope.Create()) { }

    private TabularIslamicCalendar(StandardScope scope) : base("Tabular Islamic", scope)
    {
        Adjuster = new TabularIslamicAdjuster(this);
    }

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    public TabularIslamicAdjuster Adjuster { get; }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override TabularIslamicDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Provides common adjusters for <see cref="TabularIslamicDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class TabularIslamicAdjuster : SpecialAdjuster<TabularIslamicDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TabularIslamicAdjuster"/>
    /// class.
    /// </summary>
    internal TabularIslamicAdjuster(TabularIslamicCalendar calendar) : base(calendar) { }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    private protected sealed override TabularIslamicDate NewDate(int daysSinceEpoch) => new(daysSinceEpoch);
}

/// <summary>
/// Represents the Tabular Islamic date.
/// <para><see cref="TabularIslamicDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct TabularIslamicDate :
    IDate<TabularIslamicDate, TabularIslamicCalendar>,
    IAdjustable<TabularIslamicDate>
{ }

public partial struct TabularIslamicDate // Preamble
{
    // WARNING: the order in which the static fields are written is __important__.

    private static readonly DayNumber s_Epoch = TabularIslamicScope.Instance.Epoch;
    private static readonly Range<DayNumber> s_Domain = TabularIslamicScope.Instance.Domain;

    private static readonly int s_MinDaysSinceEpoch = TabularIslamicScope.Instance.Segment.SupportedDays.Min;
    private static readonly int s_MaxDaysSinceEpoch = TabularIslamicScope.Instance.Segment.SupportedDays.Max;

    private static readonly TabularIslamicDate s_MinValue = new(s_MinDaysSinceEpoch);
    private static readonly TabularIslamicDate s_MaxValue = new(s_MaxDaysSinceEpoch);

    private readonly int _daysSinceEpoch;

    /// <summary>
    /// Initializes a new instance of the <see cref="TabularIslamicDate"/> struct
    /// to the specified date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid date or <paramref name="year"/> is outside the range of supported
    /// years.</exception>
    public TabularIslamicDate(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TabularIslamicDate"/> struct
    /// to the specified ordinal date parts.
    /// </summary>
    /// <exception cref="AoorException">The specified components do not form a
    /// valid ordinal date or <paramref name="year"/> is outside the range of
    /// supported years.</exception>
    public TabularIslamicDate(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);

        _daysSinceEpoch = Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// This constructor does NOT validate its parameter.
    /// </summary>
    internal TabularIslamicDate(int daysSinceEpoch)
    {
        _daysSinceEpoch = daysSinceEpoch;
    }

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static TabularIslamicDate MinValue => s_MinValue;

    /// <inheritdoc />
    /// <remarks>This static property is thread-safe.</remarks>
    public static TabularIslamicDate MaxValue => s_MaxValue;

    /// <inheritdoc />
    public static TabularIslamicCalendar Calendar => TabularIslamicCalendar.Instance;

    /// <summary>
    /// Gets the date adjuster.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static TabularIslamicAdjuster Adjuster => TabularIslamicCalendar.Instance.Adjuster;

    /// <inheritdoc />
    // We already know that the resulting day number is valid so instead of
    // > public DayNumber DayNumber => s_Epoch + _daysSinceEpoch;
    // we can use an unchecked addition
    public DayNumber DayNumber => s_Epoch.AddDaysUnchecked(_daysSinceEpoch);

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
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    private static TabularIslamicSchema Schema => TabularIslamicScope.Schema;

    /// <summary>
    /// Gets the calendar scope.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    private static StandardScope Scope => TabularIslamicScope.Instance;

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

public partial struct TabularIslamicDate // Factories
{
    /// <inheritdoc />
    [Pure]
    public static TabularIslamicDate FromDayNumber(DayNumber dayNumber)
    {
        s_Domain.Validate(dayNumber);

        return new(dayNumber - s_Epoch);
    }
}

public partial struct TabularIslamicDate // Counting
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

public partial struct TabularIslamicDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public TabularIslamicDate Adjust(Func<TabularIslamicDate, TabularIslamicDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        s_Domain.CheckLowerBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        s_Domain.CheckOverflow(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber - s_Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        s_Domain.CheckUpperBound(dayNumber);
        return new(dayNumber - s_Epoch);
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
    /// <inheritdoc />
    public static bool operator <(TabularIslamicDate left, TabularIslamicDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(TabularIslamicDate left, TabularIslamicDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(TabularIslamicDate left, TabularIslamicDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
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

public partial struct TabularIslamicDate // Math
{
#pragma warning disable CA2225 // Operator overloads have named alternates (Usage) ✓
    // Friendly alternates do exist but use domain-specific names.

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    public static int operator -(TabularIslamicDate left, TabularIslamicDate right) => left.CountDaysSince(right);

    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static TabularIslamicDate operator +(TabularIslamicDate value, int days) => value.AddDays(days);

    /// <summary>
    /// Subtracts a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    public static TabularIslamicDate operator -(TabularIslamicDate value, int days) => value.AddDays(-days);

    /// <summary>
    /// Adds one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    public static TabularIslamicDate operator ++(TabularIslamicDate value) => value.NextDay();

    /// <summary>
    /// Subtracts one day to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    public static TabularIslamicDate operator --(TabularIslamicDate value) => value.PreviousDay();

#pragma warning restore CA2225

    /// <inheritdoc />
    [Pure]
    public int CountDaysSince(TabularIslamicDate other) =>
        // No need to use a checked context here.
        _daysSinceEpoch - other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate AddDays(int days)
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
    public TabularIslamicDate NextDay()
    {
        if (this == s_MaxValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch + 1);
    }

    /// <inheritdoc />
    [Pure]
    public TabularIslamicDate PreviousDay()
    {
        if (this == s_MinValue) ThrowHelpers.ThrowDateOverflow();
        return new(_daysSinceEpoch - 1);
    }
}
