﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Intervals;

/// <summary>Provides static helpers and extension methods for <see cref="UpperRay{T}"/>.
/// <para>This class cannot be inherited.</para></summary>
public static partial class UpperRay { }

public partial class UpperRay // Factories
{
    /// <summary>Creates a new instance of the <see cref="UpperRay{T}"/> struct representing the ray
    /// [<paramref name="min"/>..], the set of values greater than or equal to
    /// <paramref name="min"/>.</summary>
    [Pure]
    public static UpperRay<T> StartingAt<T>(T min)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(min);
    }
}

public partial class UpperRay // UpperRay<int>
{
    /// <summary>Obtains the set complement of this ray.</summary>
    /// <exception cref="InvalidOperationException">The ray represents the whole <see cref="int"/>
    /// range.</exception>
    [Pure]
    public static LowerRay<int> Complement(this UpperRay<int> ray)
    {
        int min = ray.Min;
        return min == int.MinValue
            ? throw new InvalidOperationException("The complement would be an empty set.")
            : new(min - 1);
    }
}

public partial class UpperRay // UpperRay<DayNumber>
{
    /// <summary>Obtains the set complement of this ray.</summary>
    /// <exception cref="InvalidOperationException">The ray represents the whole
    /// <see cref="DayNumber"/> range.</exception>
    [Pure]
    public static LowerRay<DayNumber> Complement(this UpperRay<DayNumber> ray)
    {
        var min = ray.Min;
        return min == DayNumber.MinValue
            ? throw new InvalidOperationException("The complement would be an empty set.")
            : new(min - 1);
    }
}
