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
/// Represents the Gregorian date.
/// <para><see cref="GregorianDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct GregorianDate :
    IDate<GregorianDate, GregorianCalendar>,
    IDateFactory<GregorianDate>,
    ISubtractionOperators<GregorianDate, GregorianDate, int>
{ }

public partial struct GregorianDate // Factories & conversions
{
    /// <inheritdoc />
    [Pure]
    public static GregorianDate FromDayNumber(DayNumber dayNumber)
    {
        Calendar.Scope.Validate(dayNumber);

        return new(dayNumber.DaysSinceZero);
    }

    /// <inheritdoc />
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static GregorianDate IDateFactory<GregorianDate>.FromDayNumberUnchecked(DayNumber dayNumber) =>
        new(dayNumber.DaysSinceZero);

    /// <inheritdoc />
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static GregorianDate IDateFactory<GregorianDate>.FromDaysSinceEpochUnchecked(int daysSinceEpoch) =>
        new(daysSinceEpoch);
}

public partial struct GregorianDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() =>
        Calendar.UnderlyingSchema.CountDaysInYearBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() =>
        Calendar.UnderlyingSchema.CountDaysInYearAfter(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() =>
        Calendar.UnderlyingSchema.CountDaysInMonthBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() =>
        Calendar.UnderlyingSchema.CountDaysInMonthAfter(_daysSinceZero);
}

public partial struct GregorianDate // Adjustments
{
    /// <inheritdoc />
    [Pure]
    public GregorianDate Adjust(Func<GregorianDate, GregorianDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        return adjuster.Invoke(this);
    }

    //
    // "Nondestructive mutation"
    //

    /// <inheritdoc />
    [Pure]
    public GregorianDate WithYear(int newYear) =>
        Calendar.Adjuster.AdjustYear(this, newYear);

    /// <inheritdoc />
    [Pure]
    public GregorianDate WithMonth(int newMonth) =>
        Calendar.Adjuster.AdjustMonth(this, newMonth);

    /// <inheritdoc />
    [Pure]
    public GregorianDate WithDay(int newDay) =>
        Calendar.Adjuster.AdjustDay(this, newDay);

    /// <inheritdoc />
    [Pure]
    public GregorianDate WithDayOfYear(int newDayOfYear) =>
        Calendar.Adjuster.AdjustDayOfYear(this, newDayOfYear);

    //
    // Adjust the day of the week
    //

    /// <inheritdoc />
    [Pure]
    public GregorianDate Previous(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.Previous(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public GregorianDate PreviousOrSame(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.PreviousOrSame(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public GregorianDate Nearest(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.Nearest(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public GregorianDate NextOrSame(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.NextOrSame(this, dayOfWeek);

    /// <inheritdoc />
    [Pure]
    public GregorianDate Next(DayOfWeek dayOfWeek) =>
        Calendar.Adjuster.Next(this, dayOfWeek);
}

public partial struct GregorianDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero == right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator !=(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero != right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public bool Equals(GregorianDate other) => _daysSinceZero == other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is GregorianDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceZero;
}

public partial struct GregorianDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero < right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator <=(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero <= right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero > right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >=(GregorianDate left, GregorianDate right) =>
        left._daysSinceZero >= right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public static GregorianDate Min(GregorianDate x, GregorianDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static GregorianDate Max(GregorianDate x, GregorianDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(GregorianDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is GregorianDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(GregorianDate), obj);
}
