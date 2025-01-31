// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Intervals;

using Calendrie.Core.Utilities;

// Omitted methods: see IntervalExtra in Calendrie.Sketches.
// Methods with args reversed: see Lavretni in Calendrie.Sketches.

/// <summary>
/// Provides static helpers for interval types.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class Interval { }

public partial class Interval // Intersection
{
    /// <summary>
    /// Obtains the set intersection of the two specified intervals.
    /// </summary>
    [Pure]
    public static SegmentSet<T> Intersect<T>(Segment<T> x, Segment<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        // [a, b] ⋂ [m, n] = [max(a, m), min(b, n)]
        // [a, b] ⋂ [m, n] = empty if a > n or m > b

        var min = MathT.Max(x.Min, y.Min);
        var max = MathT.Min(x.Max, y.Max);

        return min.CompareTo(max) > 0 ? SegmentSet<T>.Empty : SegmentSet.UnsafeCreate(min, max);
    }

    /// <summary>
    /// Obtains the set intersection of the two specified intervals.
    /// </summary>
    [Pure]
    public static LowerRay<T> Intersect<T>(LowerRay<T> x, LowerRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(MathT.Min(x.Max, y.Max));
    }

    /// <summary>
    /// Obtains the set intersection of the two specified intervals.
    /// </summary>
    [Pure]
    public static UpperRay<T> Intersect<T>(UpperRay<T> x, UpperRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(MathT.Max(x.Min, y.Min));
    }

    /// <summary>
    /// Obtains the set intersection of the two specified intervals.
    /// </summary>
    [Pure]
    public static SegmentSet<T> Intersect<T>(Segment<T> x, LowerRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return x.Min.CompareTo(y.Max) > 0 ? SegmentSet<T>.Empty
            : SegmentSet.UnsafeCreate(x.Min, MathT.Min(x.Max, y.Max));
    }

    /// <summary>
    /// Obtains the set intersection of the two specified intervals.
    /// </summary>
    [Pure]
    public static SegmentSet<T> Intersect<T>(Segment<T> x, UpperRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return y.Min.CompareTo(x.Max) > 0 ? SegmentSet<T>.Empty
            : SegmentSet.UnsafeCreate(MathT.Max(x.Min, y.Min), x.Max);
    }

    /// <summary>
    /// Obtains the set intersection of the two specified intervals.
    /// </summary>
    [Pure]
    public static SegmentSet<T> Intersect<T>(LowerRay<T> x, UpperRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return y.Min.CompareTo(x.Max) > 0 ? SegmentSet<T>.Empty
            : SegmentSet.UnsafeCreate(y.Min, x.Max);
    }
}

public partial class Interval // Union
{
    /// <summary>
    /// Obtains the set union the two specified intervals.
    /// </summary>
    [Pure]
    public static LowerRay<T> Union<T>(LowerRay<T> x, LowerRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(MathT.Max(x.Max, y.Max));
    }

    /// <summary>
    /// Obtains the set union the two specified intervals.
    /// </summary>
    [Pure]
    public static UpperRay<T> Union<T>(UpperRay<T> x, UpperRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(MathT.Min(x.Min, y.Min));
    }
}

// Omitted:
// - Span(LowerRay<T> x, LowerRay<T> y) => Union.
// - Span(UpperRay<T> x, UpperRay<T> y) => Union.
// - Span(LowerRay<T> x, UpperRay<T> y) => the whole "interval".
public partial class Interval // Convex hull
{
    /// <summary>
    /// Obtains the smallest range containing the two specified intervals.
    /// </summary>
    [Pure]
    public static Segment<T> Span<T>(Segment<T> x, Segment<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        // hull([a, b] ⋃ [m, n]) = [min(a, m), max(b, n)]

        var min = MathT.Min(x.Min, y.Min);
        var max = MathT.Max(x.Max, y.Max);

        return Range.UnsafeCreate(min, max);
    }

    /// <summary>
    /// Obtains the smallest range containing the two specified intervals.
    /// </summary>
    [Pure]
    public static LowerRay<T> Span<T>(Segment<T> x, LowerRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(MathT.Max(x.Max, y.Max));
    }

    /// <summary>
    /// Obtains the smallest range containing the two specified intervals.
    /// </summary>
    [Pure]
    public static UpperRay<T> Span<T>(Segment<T> x, UpperRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return new(MathT.Min(x.Min, y.Min));
    }
}

// Omitted:
// - Disjoint(LowerRay<T> x, LowerRay<T> y) => false.
// - Disjoint(UpperRay<T> x, UpperRay<T> y) => false.
public partial class Interval // Disjoint
{
    /// <summary>
    /// Determines whether the two specified intervals are disjoint or not.
    /// </summary>
    [Pure]
    public static bool Disjoint<T>(Segment<T> x, Segment<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        // [a, b] ⋂ [m, n] = {} iff a > n or m > b
        // [a, b] ⋂ [m, n] ≠ {} iff a <= n and m <= b

        return x.Max.CompareTo(y.Min) < 0 || y.Max.CompareTo(x.Min) < 0;
    }

    /// <summary>
    /// Determines whether the two specified intervals are disjoint or not.
    /// </summary>
    [Pure]
    public static bool Disjoint<T>(Segment<T> x, LowerRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return y.Max.CompareTo(x.Min) < 0;
    }

    /// <summary>
    /// Determines whether the two specified intervals are disjoint or not.
    /// </summary>
    [Pure]
    public static bool Disjoint<T>(Segment<T> x, UpperRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return x.Max.CompareTo(y.Min) < 0;
    }

    /// <summary>
    /// Determines whether the two specified intervals are disjoint or not.
    /// </summary>
    [Pure]
    public static bool Disjoint<T>(LowerRay<T> x, UpperRay<T> y)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        return x.Max.CompareTo(y.Min) < 0;
    }
}
