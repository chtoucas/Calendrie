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

public partial struct JulianDate // Adjustments
{
    /// <inheritdoc />
    /// <remarks>See also <seealso cref="Adjuster"/>.</remarks>
    [Pure]
    public JulianDate Adjust(Func<JulianDate, JulianDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate WithYear(int newYear) => Adjuster.AdjustYear(this, newYear);

    /// <inheritdoc />
    [Pure]
    public JulianDate WithMonth(int newMonth) => Adjuster.AdjustMonth(this, newMonth);

    /// <inheritdoc />
    [Pure]
    public JulianDate WithDay(int newDay) => Adjuster.AdjustDay(this, newDay);

    /// <inheritdoc />
    [Pure]
    public JulianDate WithDayOfYear(int newDayOfYear) => Adjuster.AdjustDayOfYear(this, newDayOfYear);

    /// <inheritdoc />
    [Pure]
    public JulianDate Previous(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Previous(dayOfWeek);
        Scope.CheckLowerBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate PreviousOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.PreviousOrSame(dayOfWeek);
        Scope.CheckLowerBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate Nearest(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Nearest(dayOfWeek);
        Scope.CheckOverflow(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate NextOrSame(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.NextOrSame(dayOfWeek);
        Scope.CheckUpperBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }

    /// <inheritdoc />
    [Pure]
    public JulianDate Next(DayOfWeek dayOfWeek)
    {
        var dayNumber = DayNumber.Next(dayOfWeek);
        Scope.CheckUpperBound(dayNumber);
        return FromDayNumberUnchecked(dayNumber);
    }
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
