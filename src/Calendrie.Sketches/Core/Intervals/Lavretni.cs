﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Intervals;

/// <summary>
/// Provides static helpers for interval types.
/// <para>Methods from <see cref="Interval"/> and <see cref="IntervalExtra"/> with arguments
/// reversed.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class Lavretni { }

public partial class Lavretni // Intersection
{
    /// <summary>
    /// Obtains the set intersection of the two specified intervals.
    /// </summary>
    [Pure]
    public static SegmentSet<T> Intersect<T>(LowerRay<T> x, Segment<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
        Interval.Intersect(y, x);

    /// <summary>
    /// Obtains the set intersection of the two specified intervals.
    /// </summary>
    [Pure]
    public static SegmentSet<T> Intersect<T>(UpperRay<T> x, Segment<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
        Interval.Intersect(y, x);

    /// <summary>
    /// Obtains the set intersection of the two specified intervals.
    /// </summary>
    [Pure]
    public static SegmentSet<T> Intersect<T>(UpperRay<T> x, LowerRay<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
        Interval.Intersect(y, x);
}

public partial class Lavretni // Convex hull
{
    /// <summary>
    /// Obtains the smallest range containing the two specified intervals.
    /// </summary>
    [Pure]
    public static LowerRay<T> Span<T>(LowerRay<T> x, Segment<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
        Interval.Span(y, x);

    /// <summary>
    /// Obtains the smallest range containing the two specified intervals.
    /// </summary>
    [Pure]
    public static UpperRay<T> Span<T>(UpperRay<T> x, Segment<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
        Interval.Span(y, x);

    /// <summary>
    /// Obtains the smallest range containing the two specified intervals.
    /// </summary>
    /// <returns>The whole "interval".</returns>
    [Pure]
    public static Unbounded<T> Span<T>(UpperRay<T> x, LowerRay<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
        IntervalExtra.Span(y, x);
}

public partial class Lavretni // Disjoint
{
    /// <summary>
    /// Determines whether the two specified intervals are disjoint or not.
    /// </summary>
    [Pure]
    public static bool Disjoint<T>(LowerRay<T> x, Segment<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
        Interval.Disjoint(y, x);

    /// <summary>
    /// Determines whether the two specified intervals are disjoint or not.
    /// </summary>
    [Pure]
    public static bool Disjoint<T>(UpperRay<T> x, Segment<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
        Interval.Disjoint(y, x);

    /// <summary>
    /// Determines whether the two specified intervals are disjoint or not.
    /// </summary>
    [Pure]
    public static bool Disjoint<T>(UpperRay<T> x, LowerRay<T> y) where T : struct, IEquatable<T>, IComparable<T> =>
        Interval.Disjoint(y, x);
}

public partial class Lavretni // Coalesce
{
    #region Int32

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// </summary>
    /// <returns><see langword="null"/> if the set union is not an interval.</returns>
    [Pure]
    public static LowerRay<int>? Coalesce(LowerRay<int> x, Segment<int> y) =>
        Interval.Coalesce(y, x);

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// </summary>
    /// <returns><see langword="null"/> if the set union is not an interval.</returns>
    [Pure]
    public static UpperRay<int>? Coalesce(UpperRay<int> x, Segment<int> y) =>
        Interval.Coalesce(y, x);

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// </summary>
    /// <returns>The whole "interval" if the set union is an interval; otherwise
    /// <see langword="null"/>.</returns>
    [Pure]
    public static Unbounded<int>? Coalesce(UpperRay<int> x, LowerRay<int> y) =>
        IntervalExtra.Coalesce(y, x);

    #endregion
    #region DayNumber

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// </summary>
    /// <returns><see langword="null"/> if the set union is not an interval.</returns>
    [Pure]
    public static LowerRay<DayNumber>? Coalesce(LowerRay<DayNumber> x, Segment<DayNumber> y) =>
        Interval.Coalesce(y, x);

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// </summary>
    /// <returns><see langword="null"/> if the set union is not an interval.</returns>
    [Pure]
    public static UpperRay<DayNumber>? Coalesce(UpperRay<DayNumber> x, Segment<DayNumber> y) =>
        Interval.Coalesce(y, x);

    /// <summary>
    /// Attempts to obtain the set union of the two specified intervals.
    /// </summary>
    /// <returns>The whole "interval" if the set union is an interval; otherwise
    /// <see langword="null"/>.</returns>
    [Pure]
    public static Unbounded<DayNumber>? Coalesce(UpperRay<DayNumber> x, LowerRay<DayNumber> y) =>
        IntervalExtra.Coalesce(y, x);

    #endregion
}

public partial class Lavretni // Gap
{
    #region Int32

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static SegmentSet<int> Gap(LowerRay<int> x, Segment<int> y) =>
        Interval.Gap(y, x);

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static SegmentSet<int> Gap(UpperRay<int> x, Segment<int> y) =>
        Interval.Gap(y, x);

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static SegmentSet<int> Gap(UpperRay<int> x, LowerRay<int> y) =>
        Interval.Gap(y, x);

    #endregion
    #region DayNumber

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static SegmentSet<DayNumber> Gap(LowerRay<DayNumber> x, Segment<DayNumber> y) =>
        Interval.Gap(y, x);

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static SegmentSet<DayNumber> Gap(UpperRay<DayNumber> x, Segment<DayNumber> y) =>
        Interval.Gap(y, x);

    /// <summary>
    /// Obtains the largest interval lying between the two specified intervals.
    /// </summary>
    /// <returns>The empty interval if the two intervals overlap or are adjacent.</returns>
    [Pure]
    public static SegmentSet<DayNumber> Gap(UpperRay<DayNumber> x, LowerRay<DayNumber> y) =>
        Interval.Gap(y, x);

    #endregion
}

public partial class Lavretni // Adjacency
{
    #region Int32

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(LowerRay<int> x, Segment<int> y) =>
        Interval.Adjacent(y, x);

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(UpperRay<int> x, Segment<int> y) =>
        Interval.Adjacent(y, x);

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(UpperRay<int> x, LowerRay<int> y) =>
        Interval.Adjacent(y, x);

    #endregion
    #region DayNumber

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(LowerRay<DayNumber> x, Segment<DayNumber> y) =>
        Interval.Adjacent(y, x);

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(UpperRay<DayNumber> x, Segment<DayNumber> y) =>
        Interval.Adjacent(y, x);

    /// <summary>
    /// Determines whether the two specified intervals are adjacent or not.
    /// <para>Two intervals are said to be <i>adjacent</i> if they are disjoint and there is an
    /// empty gap between them.</para>
    /// </summary>
    [Pure]
    public static bool Adjacent(UpperRay<DayNumber> x, LowerRay<DayNumber> y) =>
        Interval.Adjacent(y, x);

    #endregion
}

public partial class Lavretni // Connectedness
{
    #region Int32

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(LowerRay<int> x, Segment<int> y) =>
        Interval.Connected(y, x);

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(UpperRay<int> x, Segment<int> y) =>
        Interval.Connected(y, x);

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(UpperRay<int> x, LowerRay<int> y) =>
        Interval.Connected(y, x);

    #endregion
    #region DayNumber

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(LowerRay<DayNumber> x, Segment<DayNumber> y) =>
        Interval.Connected(y, x);

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(UpperRay<DayNumber> x, Segment<DayNumber> y) =>
        Interval.Connected(y, x);

    /// <summary>
    /// Determines whether the two specified intervals are connected or not.
    /// <para>Two intervals are said to be <i>connected</i> if their set union is an interval
    /// too.</para>
    /// </summary>
    [Pure]
    public static bool Connected(UpperRay<DayNumber> x, LowerRay<DayNumber> y) =>
        Interval.Connected(y, x);

    #endregion
}
