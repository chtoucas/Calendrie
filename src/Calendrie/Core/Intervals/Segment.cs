﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Intervals;

using System.Numerics;

// Enumerable:
// - ToEnumerable() for structs
// - Impl IEnumerable<T> for classes

/// <summary>
/// Provides static helpers and extension methods for <see cref="Segment{T}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class Segment { }

public partial class Segment // Factories
{
    /// <summary>
    /// Creates a new instance of the <see cref="Segment{T}"/> struct representing
    /// the range [<paramref name="min"/>..<paramref name="max"/>].
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is
    /// less than <paramref name="min"/>.</exception>
    [Pure]
    public static Segment<T> Create<T>(T min, T max)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(min, max);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Segment{T}"/> struct representing
    /// the range [<paramref name="min"/>..<paramref name="max"/>].
    /// <para>This factory method does NOT validate its parameters.</para>
    /// </summary>
    [Pure]
    internal static Segment<T> UnsafeCreate<T>(T min, T max)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(OrderedPair.UnsafeCreate(min, max));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Segment{T}"/> struct representing
    /// the degenerate range [<paramref name="value"/>].
    /// </summary>
    [Pure]
    public static Segment<T> Singleton<T>(T value)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return UnsafeCreate(value, value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Segment{T}"/> struct representing
    /// the range [<see cref="IMinMaxValue{T}.MinValue"/>..<see cref="IMinMaxValue{T}.MaxValue"/>].
    /// </summary>
    [Pure]
    public static Segment<T> Maximal<T>()
        where T : struct, IEquatable<T>, IComparable<T>, IMinMaxValue<T>
    {
        return UnsafeCreate(T.MinValue, T.MaxValue);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Segment{T}"/> struct representing
    /// the range [<paramref name="min"/>..<see cref="IMinMaxValue{T}.MaxValue"/>].
    /// </summary>
    [Pure]
    public static Segment<T> StartingAt<T>(T min)
        where T : struct, IEquatable<T>, IComparable<T>, IMinMaxValue<T>
    {
        return UnsafeCreate(min, T.MaxValue);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Segment{T}"/> struct from the specified
    /// minimum and length.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/>
    /// is less than 1.</exception>
    [Pure]
    public static Segment<T> StartingAt<T>(T min, int length)
        where T : struct, IEquatable<T>, IComparable<T>, IAdditionOperators<T, int, T>
    {
        return new(min, min + (length - 1));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Segment{T}"/> struct representing
    /// the range [<see cref="IMinMaxValue{T}.MinValue"/>..<paramref name="max"/>].
    /// </summary>
    [Pure]
    public static Segment<T> EndingAt<T>(T max)
        where T : struct, IEquatable<T>, IComparable<T>, IMinMaxValue<T>
    {
        return UnsafeCreate(T.MinValue, max);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Segment{T}"/> struct from the
    /// specified maximum and length.
    /// </summary>
    [Pure]
    public static Segment<T> EndingAt<T>(T max, int length)
        where T : struct, IEquatable<T>, IComparable<T>, ISubtractionOperators<T, int, T>
    {
        return new(max - (length - 1), max);
    }
}

public partial class Segment // Conversions, transformations
{
    /// <summary>
    /// Creates a new instance of the <see cref="Segment{T}"/> struct from the
    /// specified endpoints.
    /// </summary>
    [Pure]
    public static Segment<T> FromEndpoints<T>(OrderedPair<T> endpoints)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new Segment<T>(endpoints);
    }
}

public partial class Segment // Segment<int>
{
    // A range of int's is finite and enumerable.

    /// <summary>
    /// Gets the range [<see cref="int.MinValue"/>..<see cref="int.MaxValue"/>].
    /// <para>This is the largest range of 32-bit signed integers representable
    /// by the system.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static Segment<int> Maximal32 { get; } = Create(int.MinValue, int.MaxValue);

    /// <summary>
    /// Obtains the number of elements in the specified range.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public static int Count(this Segment<int> range) => checked(range.Max - range.Min + 1);

    /// <summary>
    /// Obtains the number of elements in the specified range.
    /// </summary>
    [Pure]
    public static long LongCount(this Segment<int> range) => (long)range.Max - range.Min + 1;

    /// <summary>
    /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
    /// </summary>
    [Pure]
    public static IEnumerable<int> ToEnumerable(this Segment<int> range)
    {
        // We could have written:
        // > Enumerable.Range(range.Min, range.Count());
        // but it would overflow when Count() does.

        int min = range.Min;
        int max = range.Max;

        for (int i = min; i <= max; i++)
        {
            yield return i;
        }
    }
}

public partial class Segment // Segment<DayNumber>
{
    // A range of DayNumber's is finite and enumerable.

    /// <summary>
    /// Obtains the number of elements in the specified range.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public static int Count(this Segment<DayNumber> range) => range.Max - range.Min + 1;

    /// <summary>
    /// Obtains the number of elements in the specified range.
    /// </summary>
    [Pure]
    public static long LongCount(this Segment<DayNumber> range) =>
        (long)range.Max.DaysSinceZero - range.Min.DaysSinceZero + 1;

    /// <summary>
    /// Obtains an <see cref="IEnumerable{T}"/> view of the specified range.
    /// </summary>
    [Pure]
    public static IEnumerable<DayNumber> ToEnumerable(this Segment<DayNumber> range)
    {
        var min = range.Min;
        var max = range.Max;

        for (var i = min; i <= max; i++)
        {
            yield return i;
        }
    }
}
