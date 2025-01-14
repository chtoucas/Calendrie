// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Systems;

// TODO(code): specialized versions for years and months.
// Interconversion of a range. ToEnumerable()
// Range<Year>.Count(), ToEnumerable(), GetCalendar().

public static class Interconversion
{
    // See also TropicaliaDate.From/ToAbsoluteDate()
    [Pure]
    public static TDate ConvertTo<TDate>(this IAbsoluteDate date)
        where TDate : IAbsoluteDate<TDate>
    {
        ArgumentNullException.ThrowIfNull(date);
        return TDate.FromDayNumber(date.DayNumber);
    }

    [Pure]
    public static Range<TDate> ConvertTo<TDate>(this Range<CivilDate> range)
        where TDate : struct, IAbsoluteDate<TDate>
    {
        return Interconvert<CivilDate, TDate>(range);
    }

    [Pure]
    public static Range<TDate> ConvertTo<TDate>(this CivilMonth month)
        where TDate : struct, IAbsoluteDate<TDate>
    {
        return Interconvert<CivilDate, TDate>(month);
    }

    [Pure]
    public static Range<TDate> ConvertTo<TDate>(this CivilYear year)
        where TDate : struct, IAbsoluteDate<TDate>
    {
        return Interconvert<CivilDate, TDate>(year);
    }

    //
    // Helpers
    //

    [Pure]
    private static Range<TOut> Interconvert<TIn, TOut>(this Range<TIn> @this)
        where TIn : struct, IAbsoluteDate<TIn>
        where TOut : struct, IAbsoluteDate<TOut>
    {
        var (min, max) = @this.Endpoints;
        return Range.Create(interconv(min), interconv(max));

        [Pure]
        static TOut interconv(TIn value) => TOut.FromDayNumber(value.DayNumber);
    }

    [Pure]
    private static Range<TOut> Interconvert<TIn, TOut>(this IDaySegment<TIn> @this)
        where TIn : struct, IAbsoluteDate<TIn>
        where TOut : struct, IAbsoluteDate<TOut>
    {
        ArgumentNullException.ThrowIfNull(@this);

        var (min, max) = @this.ToDayRange().Endpoints;
        return Range.Create(interconv(min), interconv(max));

        [Pure]
        static TOut interconv(TIn value) => TOut.FromDayNumber(value.DayNumber);
    }
}
