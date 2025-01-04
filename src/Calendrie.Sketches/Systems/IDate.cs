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
    ICalendarDate<TSelf>,
    IAdjustableDate<TSelf>,
    IDateFactory<TSelf>
    where TCalendar : Calendar
    where TSelf : IDate<TSelf, TCalendar>
{
    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    static abstract TCalendar Calendar { get; }
}
