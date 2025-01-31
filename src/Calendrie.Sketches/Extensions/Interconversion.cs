// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Systems;

// TODO(code): specialized versions for years and months.
// Interconversion of a range. ToEnumerable()
// Segment<Year>.Count(), ToEnumerable(), GetCalendar().

public static class Interconversion
{
    /// <summary>
    /// Converts the specified date to a <typeparamref name="TDate"/> value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported <typeparamref name="TDate"/> values.</exception>
    [Pure]
    public static TDate ToAbsoluteDate<TDate>(this IAbsoluteDate date)
        where TDate : IDate<TDate>, IUnsafeFactory<TDate>
    {
        ArgumentNullException.ThrowIfNull(date);

        var scope = TDate.Calendar.Scope;
        scope.CheckOverflow(date.DayNumber);
        return TDate.UnsafeCreate(date.DayNumber - scope.Epoch);
    }

    /// <summary>
    /// Converts the specified date to a <typeparamref name="TDate"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="date"/>
    /// is outside the range of supported values.</exception>
    [Pure]
    public static TDate ConvertTo<TDate>(this IAbsoluteDate date)
        where TDate : IAbsoluteDate<TDate>
    {
        ArgumentNullException.ThrowIfNull(date);
        return TDate.FromDayNumber(date.DayNumber);
    }

    [Pure]
    public static Segment<TDate> ConvertTo<TDate>(this Segment<CivilDate> range)
        where TDate : struct, IAbsoluteDate<TDate>
    {
        return Interconvert<CivilDate, TDate>(range);
    }

    [Pure]
    public static Segment<TDate> ConvertTo<TDate>(this CivilMonth month)
        where TDate : struct, IAbsoluteDate<TDate>
    {
        return Interconvert<CivilDate, TDate>(month);
    }

    [Pure]
    public static Segment<TDate> ConvertTo<TDate>(this CivilYear year)
        where TDate : struct, IAbsoluteDate<TDate>
    {
        return Interconvert<CivilDate, TDate>(year);
    }

    //
    // Helpers
    //

    [Pure]
    private static Segment<TOut> Interconvert<TIn, TOut>(this Segment<TIn> @this)
        where TIn : struct, IAbsoluteDate<TIn>
        where TOut : struct, IAbsoluteDate<TOut>
    {
        var (min, max) = @this.Endpoints;
        return Segment.Create(interconv(min), interconv(max));

        [Pure]
        static TOut interconv(TIn value) => TOut.FromDayNumber(value.DayNumber);
    }

    [Pure]
    private static Segment<TOut> Interconvert<TIn, TOut>(this IDaySegment<TIn> @this)
        where TIn : struct, IAbsoluteDate<TIn>
        where TOut : struct, IAbsoluteDate<TOut>
    {
        ArgumentNullException.ThrowIfNull(@this);

        var (min, max) = @this.ToDayRange().Endpoints;
        return Segment.Create(interconv(min), interconv(max));

        [Pure]
        static TOut interconv(TIn value) => TOut.FromDayNumber(value.DayNumber);
    }
}
