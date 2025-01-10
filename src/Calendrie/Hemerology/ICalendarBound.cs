// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines a type bound to a single calendar type.
/// </summary>
/// <typeparam name="TCalendar">The companion calendar type.</typeparam>
public interface ICalendarBound<out TCalendar> where TCalendar : CalendarSystem
{
    /// <summary>
    /// Gets the calendar to which belongs the current type.
    /// </summary>
    static abstract TCalendar Calendar { get; }
}
