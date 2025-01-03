// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

// Keep this interface internal, it was created only to simplify testing.

/// <summary>
/// Defines a date type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="TCalendar">The companion calendar type.</typeparam>
internal interface IDate<TSelf, out TCalendar> :
    IDateable,
    IAbsoluteDate<TSelf>,
    IDateFactory<TSelf>
    where TCalendar : Calendar
    where TSelf : IDate<TSelf, TCalendar>
{
    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    static abstract TCalendar Calendar { get; }

    //
    // Adjustments
    //

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

    //
    // Non-standard math ops
    //

    /// <summary>
    /// Adds a number of years to the year field of this date instance, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure] TSelf PlusYears(int years);

    /// <summary>
    /// Adds a number of months to the month field of this date instance,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure] TSelf PlusMonths(int months);

    /// <summary>
    /// Counts the number of years elapsed since the specified date.
    /// </summary>
    [Pure] int CountYearsSince(TSelf other);

    /// <summary>
    /// Counts the number of months elapsed since the specified date.
    /// </summary>
    [Pure] int CountMonthsSince(TSelf other);
}
