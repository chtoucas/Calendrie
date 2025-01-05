// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using Calendrie.Core.Intervals;

// TODO(code): specialized versions for years and months.
// Interconversion of a range. ToEnumerable()
// Range<Year>.Count(), ToEnumerable(), GetCalendar().

public static partial class RangeExtensions { }

public partial class RangeExtensions // Range<T>
{
    [Pure]
    public static Range<TItem> Flatten<T, TItem>(this Range<T> @this)
        where T : struct, IEquatable<T>, IComparable<T>, ISegment<TItem>
        where TItem : struct, IEquatable<TItem>, IComparable<TItem>
    {
        var (min, max) = @this.Endpoints;
        return Range.UnsafeCreate(min.LowerEnd, max.UpperEnd);
    }

    [Pure]
    public static bool IsSupersetOf<T, TRange>(this Range<T> @this, TRange range)
        where T : struct, IEquatable<T>, IComparable<T>
        where TRange : ISegment<T>
    {
        // Simpler (faster) version of
        // > range.IsSupersetOf(year.ToRangeOfDays());
        // when year is an IRangeOfDays<T>.
        return @this.Min.CompareTo(range.LowerEnd) <= 0
            && range.UpperEnd.CompareTo(@this.Max) <= 0;
    }
}
