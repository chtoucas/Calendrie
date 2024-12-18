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

using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Represents the Julian date.
/// <para><see cref="JulianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct JulianDate :
    IDate<JulianDate, JulianCalendar>,
    IFixedDateFactory<JulianDate>,
    ISubtractionOperators<JulianDate, JulianDate, int>
{ }

public partial struct JulianDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() =>
        Calendar.UnderlyingSchema.CountDaysInYearBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() =>
        Calendar.UnderlyingSchema.CountDaysInYearAfter(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() =>
        Calendar.UnderlyingSchema.CountDaysInMonthBefore(_daysSinceEpoch);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() =>
        Calendar.UnderlyingSchema.CountDaysInMonthAfter(_daysSinceEpoch);
}

public partial struct JulianDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public JulianDate Adjust(Func<JulianDate, JulianDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    //
    // "Nondestructive mutation"
    //

    /// <inheritdoc />
    [Pure]
    public JulianDate WithYear(int newYear) =>
        Calendar.Adjuster.AdjustYear(this, newYear);

    /// <inheritdoc />
    [Pure]
    public JulianDate WithMonth(int newMonth) =>
        Calendar.Adjuster.AdjustMonth(this, newMonth);

    /// <inheritdoc />
    [Pure]
    public JulianDate WithDay(int newDay) =>
        Calendar.Adjuster.AdjustDay(this, newDay);

    /// <inheritdoc />
    [Pure]
    public JulianDate WithDayOfYear(int newDayOfYear) =>
        Calendar.Adjuster.AdjustDayOfYear(this, newDayOfYear);

    //
    // Adjust the day of the week
    //

    /// <inheritdoc />
    [Pure]
    public JulianDate Previous(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.Previous(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public JulianDate PreviousOrSame(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.PreviousOrSame(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public JulianDate Nearest(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.Nearest(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public JulianDate NextOrSame(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.NextOrSame(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public JulianDate Next(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.Next(this, dayOfWeek);
}

public partial struct JulianDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(JulianDate left, JulianDate right) =>
        left._daysSinceEpoch == right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator !=(JulianDate left, JulianDate right) =>
        left._daysSinceEpoch != right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public bool Equals(JulianDate other) => _daysSinceEpoch == other._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is JulianDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceEpoch;
}

public partial struct JulianDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(JulianDate left, JulianDate right) =>
        left._daysSinceEpoch < right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator <=(JulianDate left, JulianDate right) =>
        left._daysSinceEpoch <= right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >(JulianDate left, JulianDate right) =>
        left._daysSinceEpoch > right._daysSinceEpoch;

    /// <inheritdoc />
    public static bool operator >=(JulianDate left, JulianDate right) =>
        left._daysSinceEpoch >= right._daysSinceEpoch;

    /// <inheritdoc />
    [Pure]
    public static JulianDate Min(JulianDate x, JulianDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static JulianDate Max(JulianDate x, JulianDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(JulianDate other) => _daysSinceEpoch.CompareTo(other._daysSinceEpoch);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is JulianDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(JulianDate), obj);
}
