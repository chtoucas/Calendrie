// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core.Utilities;

/// <summary>
/// Represents the exact difference between two dates.
/// <para><see cref="DateDifference"/> is an immutable struct.</para>
/// </summary>
/// <param name="Years">Number of years.</param>
/// <param name="Months">Number of months.</param>
/// <param name="Weeks">Number of weeks.</param>
/// <param name="Days">Number of days.</param>
public readonly record struct DateDifference(int Years, int Months, int Weeks, int Days) :
    IEqualityOperators<DateDifference, DateDifference, bool>,
    IEquatable<DateDifference>,
    IComparisonOperators<DateDifference, DateDifference, bool>,
    IComparable<DateDifference>,
    IComparable
{
    public static DateDifference Zero { get; }

    /// <inheritdoc />
    public static bool operator <(DateDifference left, DateDifference right) =>
        left.CompareTo(right) < 0;

    /// <inheritdoc />
    public static bool operator <=(DateDifference left, DateDifference right) =>
        left.CompareTo(right) <= 0;

    /// <inheritdoc />
    public static bool operator >(DateDifference left, DateDifference right) =>
        left.CompareTo(right) > 0;

    /// <inheritdoc />
    public static bool operator >=(DateDifference left, DateDifference right) =>
        left.CompareTo(right) >= 0;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(DateDifference other)
    {
        int c = Years.CompareTo(other.Years);
        if (c == 0)
        {
            c = Months.CompareTo(other.Months);
            if (c == 0)
            {
                c = Weeks.CompareTo(other.Weeks);
                if (c == 0)
                {
                    c = Days.CompareTo(other.Days);
                }
            }
        }
        return c;
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is DateDifference diff ? CompareTo(diff)
        : ThrowHelpers.ThrowNonComparable(typeof(DateDifference), obj);
}
