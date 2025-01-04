// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines a date type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface ICalendarDate<TSelf> :
    IDateable,
    IAbsoluteDate<TSelf>,
    // Non-standard math ops
    IMonthArithmetic<TSelf>,
    IYearArithmetic<TSelf>
    where TSelf : ICalendarDate<TSelf>
{
    /// <summary>
    /// Adjusts the year field to the specified value, yielding a new date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure] TSelf WithYear(int newYear);

    /// <summary>
    /// Adjusts the month field to the specified value, yielding a new date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure] TSelf WithMonth(int newMonth);

    /// <summary>
    /// Adjusts the day of the month field to the specified value, yielding a
    /// new date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure] TSelf WithDay(int newDay);

    /// <summary>
    /// Adjusts the day of the year field to the specified value, yielding a new
    /// date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure] TSelf WithDayOfYear(int newDayOfYear);
}
