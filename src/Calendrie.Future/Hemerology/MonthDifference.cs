// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core.Utilities;

/// <summary>
/// Represents the exact difference between two months.
/// <para><see cref="MonthDifference"/> is an immutable struct.</para>
/// </summary>
/// <param name="Years">Number of years.</param>
/// <param name="Months">Number of months.</param>
public readonly record struct MonthDifference(int Years, int Months) :
    IEqualityOperators<MonthDifference, MonthDifference, bool>,
    IEquatable<MonthDifference>,
    IComparisonOperators<MonthDifference, MonthDifference, bool>,
    IComparable<MonthDifference>,
    IComparable
{
    public static MonthDifference Zero { get; }

    /// <inheritdoc />
    public static bool operator <(MonthDifference left, MonthDifference right) =>
        left.CompareTo(right) < 0;

    /// <inheritdoc />
    public static bool operator <=(MonthDifference left, MonthDifference right) =>
        left.CompareTo(right) <= 0;

    /// <inheritdoc />
    public static bool operator >(MonthDifference left, MonthDifference right) =>
        left.CompareTo(right) > 0;

    /// <inheritdoc />
    public static bool operator >=(MonthDifference left, MonthDifference right) =>
        left.CompareTo(right) >= 0;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(MonthDifference other)
    {
        int c = Years.CompareTo(other.Years);
        return c == 0 ? Months.CompareTo(other.Months) : c;
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is MonthDifference diff ? CompareTo(diff)
        : ThrowHelpers.ThrowNonComparable(typeof(MonthDifference), obj);
}
