// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using Calendrie.Core.Intervals;

public static class RangeExtensions
{
    [Pure]
    public static Segment<TItem> Flatten<T, TItem>(this Segment<T> @this)
        where T : struct, IEquatable<T>, IComparable<T>, ISegment<TItem>
        where TItem : struct, IEquatable<TItem>, IComparable<TItem>
    {
        var (min, max) = @this.Endpoints;
        return Range.UnsafeCreate(min.LowerEnd, max.UpperEnd);
    }

    [Pure]
    public static bool IsSupersetOf<T, TRange>(this Segment<T> @this, TRange range)
        where T : struct, IEquatable<T>, IComparable<T>
        where TRange : ISegment<T>
    {
        // Simpler (faster) version of
        // > range.IsSupersetOf(seg.ToRangeOfDays());
        // when seg is a IDaySegment<T>.
        return @this.Min.CompareTo(range.LowerEnd) <= 0
            && range.UpperEnd.CompareTo(@this.Max) <= 0;
    }
}
