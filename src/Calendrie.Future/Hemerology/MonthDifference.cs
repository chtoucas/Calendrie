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
public readonly partial record struct MonthDifference :
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
    private MonthDifference(int years, int months)
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
}

public partial record struct MonthDifference // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="MonthDifference"/> struct.
    /// </summary>
    internal static MonthDifference UnsafeCreate(int years, int months) => new(years, months);

    /// <summary>
    /// Creates a new instance of the <see cref="MonthDifference"/> struct.
    /// </summary>
    /// <exception cref="ArgumentException">All the parameters are equal to zero.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// less than zero.</exception>
    internal static MonthDifference CreatePositive(int years, int months)
    {
        if (years == 0 && months == 0)
            throw new ArgumentException("All the parameters were equal to zero.");
        ArgumentOutOfRangeException.ThrowIfLessThan(years, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(months, 0);

        return new MonthDifference(years, months);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="MonthDifference"/> struct.
    /// </summary>
    /// <exception cref="ArgumentException">All the parameters are equal to zero.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// less than zero.</exception>
    internal static MonthDifference CreateNegative(int years, int months)
    {
        if (years == 0 && months == 0)
            throw new ArgumentException("All the parameters were equal to zero.");
        ArgumentOutOfRangeException.ThrowIfLessThan(years, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(months, 0);

        return new MonthDifference(-years, -months);
    }
}

public partial record struct MonthDifference // IComparable
{
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
        // We compare the "absolute" values!
        var x = Abs(this);
        var y = Abs(other);

        int c = x.Years.CompareTo(y.Years);
        return c == 0 ? x.Months.CompareTo(y.Months) : c;
    }

    [Pure]
    int IComparable.CompareTo(object? obj) =>
        obj is null ? 1
        : obj is MonthDifference other ? CompareTo(other)
        : ThrowHelpers.ThrowNonComparable(typeof(MonthDifference), obj);
}

public partial record struct MonthDifference // Math
{
    /// <summary>
    /// Computes the absolute value of the specified <see cref="MonthDifference"/>
    /// value.
    /// </summary>
    public static MonthDifference Abs(MonthDifference value) => IsPositive(value) ? value : -value;

    /// <summary>
    /// Determines whether the specified value is equal to <see cref="Zero"/> or
    /// not.
    /// </summary>
    public static bool IsZero(MonthDifference value) => value == Zero;

    // NB: Years and Months have the same sign.

    /// <summary>
    /// Determines whether the specified value is greater than or equal to
    /// <see cref="Zero"/>.
    /// </summary>
    public static bool IsPositive(MonthDifference value) => value.Years >= 0;

    /// <summary>
    /// Determines whether the specified value is less than or equal to
    /// <see cref="Zero"/>.
    /// </summary>
    public static bool IsNegative(MonthDifference value) => value.Years <= 0;

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
