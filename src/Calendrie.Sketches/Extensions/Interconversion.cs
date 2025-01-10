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
    [Pure]
    public static TropicaliaDate ToTropicaliaDate(this CivilDate date) =>
        WithCalendar(date, TropicaliaCalendar.Instance);

    [Pure]
    public static TDate WithCalendar<TDate>(this CivilDate date, ICalendar<TDate> calendar)
        where TDate : IAbsoluteDate<TDate>
    {
        ArgumentNullException.ThrowIfNull(calendar);
        return calendar.NewDate(date.DayNumber);
    }

    /// <summary>
    /// Converts the specified Civil date to a <typeparamref name="TDate"/> value.
    /// </summary>
    [Pure]
    public static TDate InterconvertTo<TDate>(this CivilDate date)
        where TDate : IAbsoluteDate<TDate>
    {
        return TDate.FromDayNumber(date.DayNumber);
    }

    /// <summary>
    /// Converts the specified Civil month to a range of <typeparamref name="TDate"/>
    /// values.
    /// </summary>
    [Pure]
    public static Range<TDate> InterconvertTo<TDate>(this CivilMonth month)
        where TDate : struct, IAbsoluteDate<TDate>
    {
        return InterconvertToRange<CivilDate, TDate>(month);
    }

    /// <summary>
    /// Converts the specified Civil year to a range of <typeparamref name="TDate"/>
    /// values.
    /// </summary>
    [Pure]
    public static Range<TDate> InterconvertTo<TDate>(this CivilYear year)
        where TDate : struct, IAbsoluteDate<TDate>
    {
        return InterconvertToRange<CivilDate, TDate>(year);
    }

    //
    // Helpers
    //

    //[Pure]
    //public static Range<TDate> InterconvertTo<TDate>(this IDateSegment<CivilDate> @this)
    //    where TDate : struct, IAbsoluteDate<TDate>
    //{
    //    ArgumentNullException.ThrowIfNull(@this);

    //    var (min, max) = @this.ToDayRange().Endpoints;
    //    return Range.Create(min.InterconvertTo<TDate>(), max.InterconvertTo<TDate>());
    //}

    // Pas très praticable tous ces paramètres génériques.

    [Pure]
    private static Range<TOut> InterconvertToRange<TIn, TOut>(this IDateSegment<TIn> @this)
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
