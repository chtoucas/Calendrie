﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behaviour and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calendrie.Systems;

using Calendrie.Core.Utilities;

public partial struct GregorianDate // Counting
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
