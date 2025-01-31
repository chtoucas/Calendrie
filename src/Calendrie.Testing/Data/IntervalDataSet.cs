// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data;

using Calendrie.Core.Intervals;

public sealed record SegmentSegmentInfo(
    Segment<int> First,
    Segment<int> Second,
    Segment<int> Span,
    SegmentSet<int> Intersection,
    SegmentSet<int> Gap,
    bool Disjoint,
    bool Adjacent,
    bool Connected);

public sealed record LowerRaySegmentInfo(
    LowerRay<int> First,
    Segment<int> Second,
    LowerRay<int> Span,
    SegmentSet<int> Intersection,
    SegmentSet<int> Gap,
    bool Disjoint,
    bool Adjacent,
    bool Connected);

public sealed record UpperRaySegmentInfo(
    UpperRay<int> First,
    Segment<int> Second,
    UpperRay<int> Span,
    SegmentSet<int> Intersection,
    SegmentSet<int> Gap,
    bool Disjoint,
    bool Adjacent,
    bool Connected);

public sealed record LowerRayUpperRayInfo(
    LowerRay<int> First,
    UpperRay<int> Second,
    SegmentSet<int> Intersection,
    SegmentSet<int> Gap,
    bool Disjoint,
    bool Adjacent,
    bool Connected);

public static class IntervalDataSet
{
    public static DataGroup<SegmentSegmentInfo> SegmentSegmentInfoData { get; } =
    [
        //
        // Overlapping ranges
        //

        // Equal (degenerate)
        new(new(1, 1), new(1, 1), new(1, 1), new(1, 1), SegmentSet<int>.Empty, false, false, true),
        // Equal (non-degenerate)
        new(new(1, 4), new(1, 4), new(1, 4), new(1, 4), SegmentSet<int>.Empty, false, false, true),
        // Strict subset (degenerate)
        new(new(1, 1), new(1, 4), new(1, 4), new(1, 1), SegmentSet<int>.Empty, false, false, true),
        new(new(2, 2), new(1, 4), new(1, 4), new(2, 2), SegmentSet<int>.Empty, false, false, true),
        new(new(3, 3), new(1, 4), new(1, 4), new(3, 3), SegmentSet<int>.Empty, false, false, true),
        new(new(4, 4), new(1, 4), new(1, 4), new(4, 4), SegmentSet<int>.Empty, false, false, true),
        // Strict subset (non-degenerate)
        new(new(1, 2), new(1, 4), new(1, 4), new(1, 2), SegmentSet<int>.Empty, false, false, true),
        new(new(1, 3), new(1, 4), new(1, 4), new(1, 3), SegmentSet<int>.Empty, false, false, true),
        new(new(2, 3), new(1, 4), new(1, 4), new(2, 3), SegmentSet<int>.Empty, false, false, true),
        new(new(2, 4), new(1, 4), new(1, 4), new(2, 4), SegmentSet<int>.Empty, false, false, true),
        new(new(3, 4), new(1, 4), new(1, 4), new(3, 4), SegmentSet<int>.Empty, false, false, true),
        // Other non-disjoint cases
        new(new(1, 4), new(4, 7), new(1, 7), new(4, 4), SegmentSet<int>.Empty, false, false, true),
        new(new(1, 4), new(3, 7), new(1, 7), new(3, 4), SegmentSet<int>.Empty, false, false, true),

        //
        // Disjoint ranges
        //

        // Disjoint and connected
        new(new(1, 4), new(5, 5), new(1, 5), SegmentSet<int>.Empty, SegmentSet<int>.Empty, true, true, true),
        new(new(1, 4), new(5, 7), new(1, 7), SegmentSet<int>.Empty, SegmentSet<int>.Empty, true, true, true),
        // Disjoint and disconnected
        new(new(1, 1), new(4, 4), new(1, 4), SegmentSet<int>.Empty, new(2, 3), true, false, false),
        new(new(1, 1), new(4, 7), new(1, 7), SegmentSet<int>.Empty, new(2, 3), true, false, false),
        new(new(1, 4), new(6, 9), new(1, 9), SegmentSet<int>.Empty, new(5, 5), true, false, false),
    ];

    public static DataGroup<LowerRaySegmentInfo> LowerRaySegmentInfoData { get; } =
    [
        //
        // Overlapping intervals
        //

        // Intersection is a singleton
        new(new(5), new(5, 8), new(8), new(5, 5), SegmentSet<int>.Empty, false, false, true),
        new(new(5), new(5, 5), new(5), new(5, 5), SegmentSet<int>.Empty, false, false, true),
        new(new(5), new(4, 4), new(5), new(4, 4), SegmentSet<int>.Empty, false, false, true),
        // Intersection is not a singleton and range is not a subset
        new(new(5), new(2, 7), new(7), new(2, 5), SegmentSet<int>.Empty, false, false, true),
        // Intersection is not a singleton and range is a subset
        new(new(5), new(2, 5), new(5), new(2, 5), SegmentSet<int>.Empty, false, false, true),
        new(new(5), new(1, 4), new(5), new(1, 4), SegmentSet<int>.Empty, false, false, true),

        //
        // Disjoint intervals
        //

        // Disjoint and connected
        new(new(5), new(6, 6), new(6), SegmentSet<int>.Empty, SegmentSet<int>.Empty, true, true, true),
        new(new(5), new(6, 9), new(9), SegmentSet<int>.Empty, SegmentSet<int>.Empty, true, true, true),
        // Disjoint and disconnected
        new(new(5), new(7, 7), new(7), SegmentSet<int>.Empty, new(6, 6), true, false, false),
        new(new(5), new(9, 9), new(9), SegmentSet<int>.Empty, new(6, 8), true, false, false),
        new(new(5), new(7, 9), new(9), SegmentSet<int>.Empty, new(6, 6), true, false, false),
        new(new(5), new(8, 9), new(9), SegmentSet<int>.Empty, new(6, 7), true, false, false),
    ];

    public static DataGroup<UpperRaySegmentInfo> UpperRaySegmentInfoData { get; } =
    [
        //
        // Overlapping intervals
        //

        // Intersection is a singleton
        new(new(5), new(1, 5), new(1), new(5, 5), SegmentSet<int>.Empty, false, false, true),
        new(new(5), new(5, 5), new(5), new(5, 5), SegmentSet<int>.Empty, false, false, true),
        new(new(5), new(6, 6), new(5), new(6, 6), SegmentSet<int>.Empty, false, false, true),
        // Intersection is not a singleton and range is not a subset
        new(new(5), new(2, 7), new(2), new(5, 7), SegmentSet<int>.Empty, false, false, true),
        // Intersection is not a singleton and range is a subset
        new(new(5), new(5, 7), new(5), new(5, 7), SegmentSet<int>.Empty, false, false, true),
        new(new(5), new(7, 9), new(5), new(7, 9), SegmentSet<int>.Empty, false, false, true),

        //
        // Disjoint intervals
        //

        // Disjoint and connected
        new(new(5), new(4, 4), new(4), SegmentSet<int>.Empty, SegmentSet<int>.Empty, true, true, true),
        new(new(5), new(1, 4), new(1), SegmentSet<int>.Empty, SegmentSet<int>.Empty, true, true, true),
        // Disjoint and disconnected
        new(new(5), new(3, 3), new(3), SegmentSet<int>.Empty, new(4, 4), true, false, false),
        new(new(5), new(1, 1), new(1), SegmentSet<int>.Empty, new(2, 4), true, false, false),
        new(new(5), new(1, 3), new(1), SegmentSet<int>.Empty, new(4, 4), true, false, false),
        new(new(5), new(1, 2), new(1), SegmentSet<int>.Empty, new(3, 4), true, false, false),
    ];

    public static DataGroup<LowerRayUpperRayInfo> LowerRayUpperRayInfoData { get; } =
    [
        //
        // Overlapping rays
        //

        new(new(5), new(5), new(5, 5), SegmentSet<int>.Empty, false, false, true),
        new(new(5), new(4), new(4, 5), SegmentSet<int>.Empty, false, false, true),

        //
        // Disjoint rays
        //

        // Disjoint and connected
        new(new(5), new(6), SegmentSet<int>.Empty, SegmentSet<int>.Empty, true, true, true),
        // Disjoint and disconnected
        new(new(5), new(7), SegmentSet<int>.Empty, new(6, 6), true, false, false),
        new(new(5), new(8), SegmentSet<int>.Empty, new(6, 7), true, false, false),
    ];
}
