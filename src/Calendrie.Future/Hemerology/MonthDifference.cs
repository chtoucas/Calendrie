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
    private MonthDifference(int years, int months, int sign)
    {
        Years = years;
        Months = months;
        Sign = sign;
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
    /// Gets the common sign shared by <see cref="Years"/> and <see cref="Months"/>.
    /// <para>Returns +1 if positive, -1 if negative; otherwise returns 0.</para>
    /// </summary>
    public int Sign { get; }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    public void Deconstruct(out int years, out int months) => (years, months) = (Years, Months);
}

public partial record struct MonthDifference // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    internal static MonthDifference UnsafeCreate(int years, int months, int sign) =>
        new(years, months, sign);

    /// <summary>
    /// Creates a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    /// <exception cref="ArgumentException">All the parameters are equal to zero.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// less than zero.</exception>
    public static MonthDifference CreatePositive(int years, int months)
    {
        if (years == 0 && months == 0)
            throw new ArgumentException("All the parameters were equal to zero.");
        ArgumentOutOfRangeException.ThrowIfLessThan(years, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(months, 0);

        return new MonthDifference(years, months, 1);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DateDifference"/> struct.
    /// </summary>
    /// <exception cref="ArgumentException">All the parameters are equal to zero.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">One of the parameters is
    /// less than zero.</exception>
    public static MonthDifference CreateNegative(int years, int months)
    {
        if (years == 0 && months == 0)
            throw new ArgumentException("All the parameters were equal to zero.");
        ArgumentOutOfRangeException.ThrowIfLessThan(years, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(months, 0);

        return new MonthDifference(-years, -months, -1);
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
        var x = Sign > 0 ? this : -this;
        var y = other.Sign > 0 ? other : -other;

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
    /// <inheritdoc />
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Meaningless here")]
    public static MonthDifference operator +(MonthDifference value) => value;

    /// <inheritdoc />
    public static MonthDifference operator -(MonthDifference value) => value.Negate();

    /// <summary>
    /// Negates the current instance.
    /// </summary>
    public MonthDifference Negate() => new(-Years, -Months, -Sign);
}
