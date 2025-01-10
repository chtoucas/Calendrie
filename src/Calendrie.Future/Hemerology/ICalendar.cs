// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

public interface ICalendar<TDate> where TDate : IAbsoluteDate<TDate>
{
    /// <summary>
    /// Creates a new <typeparamref name="TDate"/> value from the specified
    /// <see cref="DayNumber"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayNumber"/>
    /// is outside the range of values supported by this calendar.</exception>
    TDate NewDate(DayNumber dayNumber) => TDate.FromDayNumber(dayNumber);

    /// <summary>
    /// Creates a new <typeparamref name="TDate"/> value from the specified
    /// <typeparamref name="TSource"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="source"/>
    /// is outside the range of values supported by this calendar.</exception>
    TDate NewDate<TSource>(TSource source) where TSource : IAbsoluteDate
    {
        return TDate.FromDayNumber(source.DayNumber);
    }
}
