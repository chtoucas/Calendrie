// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using Calendrie.Core.Intervals;
using Calendrie.Hemerology;

// TODO(code): same but with months. Interconversion of a range. ToEnumerable()
// Les contraintes génériques sont à revoir.
// Range<Year>.Count(), ToEnumerable(), GetCalendar().

public static partial class RangeExtensions { }

public partial class RangeExtensions // Range<TYear>
{
    /// <summary>
    /// Converts the specified range of years to a range of days.
    /// </summary>
    [Pure]
    public static Range<TDate> ToRangeOfDays<TYear, TDate>(this Range<TYear> range)
        where TYear : struct, IEquatable<TYear>, IComparable<TYear>, IRangeOfDays<TDate>
        where TDate : struct, IEquatable<TDate>, IComparable<TDate>
    {
        var (min, max) = range.Endpoints;
        return Range.UnsafeCreate(min.MinDay, max.MaxDay);
    }
}

public partial class RangeExtensions // Range<TDate>
{
    /// <summary>
    /// Determines whether the specified range contains the specified year or not.
    /// </summary>
    [Pure]
    public static bool Contains<TDate, TYear>(this Range<TDate> range, TYear year)
        where TDate : struct, IAbsoluteDate<TDate>
        where TYear : IRangeOfDays<TDate>
    {
        return range.Min <= year.MinDay && year.MaxDay <= range.Max;
    }
}
