﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

using System.Numerics;

using Calendrie.Core.Utilities;

/// <summary>
/// Represents a pair of a year and a day of the year.
/// <para>This type uses the lexicographic order on pairs (Year, DayOfYear).
/// </para>
/// <para><see cref="OrdinalParts"/> does NOT represent an ordinal date, its
/// default value is not even a valid ordinal date.</para>
/// <para><see cref="OrdinalParts"/> is an immutable struct.</para>
/// </summary>
/// <param name="Year">Algebraic year number.</param>
/// <param name="DayOfYear">Day of the year.</param>
public readonly record struct OrdinalParts(int Year, int DayOfYear) :
    IEqualityOperators<OrdinalParts, OrdinalParts, bool>,
    IEquatable<OrdinalParts>,
    IComparisonOperators<OrdinalParts, OrdinalParts, bool>,
    IComparable<OrdinalParts>,
    IComparable
{
    /// <summary>
    /// Creates a new instance of the <see cref="OrdinalParts"/> struct
    /// representing the first day of the specified year.
    /// </summary>
    [Pure]
    public static OrdinalParts AtStartOfYear(int y) => new(y, 1);

    /// <inheritdoc />
    public static bool operator <(OrdinalParts left, OrdinalParts right) =>
        left.CompareTo(right) < 0;

    /// <inheritdoc />
    public static bool operator <=(OrdinalParts left, OrdinalParts right) =>
        left.CompareTo(right) <= 0;

    /// <inheritdoc />
    public static bool operator >(OrdinalParts left, OrdinalParts right) =>
        left.CompareTo(right) > 0;

    /// <inheritdoc />
    public static bool operator >=(OrdinalParts left, OrdinalParts right) =>
        left.CompareTo(right) >= 0;

    /// <inheritdoc />
    [Pure]
    public int CompareTo(OrdinalParts other)
    {
        int c = Year.CompareTo(other.Year);
        return c == 0 ? DayOfYear.CompareTo(other.DayOfYear) : c;
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is OrdinalParts parts ? CompareTo(parts)
        : ThrowHelpers.ThrowNonComparable(typeof(OrdinalParts), obj);
}
