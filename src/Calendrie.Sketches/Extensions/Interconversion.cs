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
    public static TDate Interconvert<TDate>(this CivilDate date)
        where TDate : IAbsoluteDate<TDate>
    {
        return TDate.FromDayNumber(date.DayNumber);
    }

    [Pure]
    public static Range<TDate> Interconvert<TDate>(this IDateSegment<CivilDate> @this)
        where TDate : struct, IAbsoluteDate<TDate>
    {
        ArgumentNullException.ThrowIfNull(@this);

        var (min, max) = @this.ToDayRange().Endpoints;
        return Range.Create(min.Interconvert<TDate>(), max.Interconvert<TDate>());
    }

    /// <summary>
    /// Converts the specified year to a range of <see cref="JulianDate"/> values.
    /// </summary>
    public static Range<JulianDate> Interconvert(this CivilYear year) =>
        year.Interconvert<JulianDate>();

    // Pas très praticable tous ces paramètres génériques.

    [Pure]
    public static Range<TOut> ConvertToRange<TIn, TOut>(this IDateSegment<TIn> @this)
        where TIn : struct, IDate<TIn>
        where TOut : struct, IDate<TOut>
    {
        ArgumentNullException.ThrowIfNull(@this);

        var (min, max) = @this.ToDayRange().Endpoints;
        return Range.Create(interconv(min), interconv(max));

        [Pure]
        static TOut interconv(TIn value) => TOut.FromDayNumber(value.DayNumber);
    }
}
