// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core.Utilities;

/// <summary>
/// Represents the result of <see cref="MonthMath.Subtract{TMonth}(TMonth, TMonth)"/>,
/// that is the exact difference between two months.
/// <para><see cref="MonthDifference"/> is an immutable struct.</para>
/// </summary>
public readonly record struct MonthDifference :
    // Comparison
    IEqualityOperators<MonthDifference, MonthDifference, bool>,
    IEquatable<MonthDifference>,
    IComparisonOperators<MonthDifference, MonthDifference, bool>,
    IComparable<MonthDifference>,
    IComparable,
    // Arithmetic
    IUnaryPlusOperators<MonthDifference, MonthDifference>,
    IUnaryNegationOperators<MonthDifference, MonthDifference>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MonthDifference"/> struct.
    /// </summary>
    internal MonthDifference(int years, int months)
    {
        Years = years;
        Months = months;
    }

    /// <summary>
    /// Gets the zero difference.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MonthDifference Zero { get; }

    /// <summary>
    /// Gets the number of years.
    /// </summary>
    public int Years { get; }

    /// <summary>
    /// Gets the number of months.
    /// </summary>
    public int Months { get; }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int years, out int months) => (years, months) = (Years, Months);

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
        : obj is MonthDifference other ? CompareTo(other)
        : ThrowHelpers.ThrowNonComparable(typeof(MonthDifference), obj);

    /// <inheritdoc />
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Meaningless here")]
    public static MonthDifference operator +(MonthDifference value) => value;

    /// <inheritdoc />
    public static MonthDifference operator -(MonthDifference value) => value.Negate();

    /// <summary>
    /// Negates the current instance.
    /// </summary>
    public MonthDifference Negate() => new(-Years, -Months);
}
