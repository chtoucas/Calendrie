﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Specialized;

using System.Numerics;

using Calendrie.Hemerology;

/// <summary>
/// Represents the Civil date.
/// <para><see cref="CivilDate"/> is an immutable struct.</para>
/// </summary>
public readonly partial struct CivilDate :
    IDate<CivilDate, CivilCalendar>,
    ISpecialDate<CivilDate>,
    IAdjustable<CivilDate>,
    ISubtractionOperators<CivilDate, CivilDate, int>
{ }

public partial struct CivilDate // Counting
{
    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInYear() => Schema.CountDaysInYearBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInYear() => Schema.CountDaysInYearAfter(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountElapsedDaysInMonth() => Schema.CountDaysInMonthBefore(_daysSinceZero);

    /// <inheritdoc />
    [Pure]
    public int CountRemainingDaysInMonth() => Schema.CountDaysInMonthAfter(_daysSinceZero);
}

public partial struct CivilDate // IEquatable
{
    /// <inheritdoc />
    public static bool operator ==(CivilDate left, CivilDate right) =>
        left._daysSinceZero == right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator !=(CivilDate left, CivilDate right) =>
        left._daysSinceZero != right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public bool Equals(CivilDate other) => _daysSinceZero == other._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is CivilDate date && Equals(date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => _daysSinceZero;
}

public partial struct CivilDate // IComparable
{
    /// <inheritdoc />
    public static bool operator <(CivilDate left, CivilDate right) =>
        left._daysSinceZero < right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator <=(CivilDate left, CivilDate right) =>
        left._daysSinceZero <= right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >(CivilDate left, CivilDate right) =>
        left._daysSinceZero > right._daysSinceZero;

    /// <inheritdoc />
    public static bool operator >=(CivilDate left, CivilDate right) =>
        left._daysSinceZero >= right._daysSinceZero;

    /// <inheritdoc />
    [Pure]
    public static CivilDate Min(CivilDate x, CivilDate y) => x < y ? x : y;

    /// <inheritdoc />
    [Pure]
    public static CivilDate Max(CivilDate x, CivilDate y) => x > y ? x : y;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(CivilDate other) => _daysSinceZero.CompareTo(other._daysSinceZero);

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is CivilDate date ? CompareTo(date)
        : ThrowHelpers.ThrowNonComparable(typeof(CivilDate), obj);
}
