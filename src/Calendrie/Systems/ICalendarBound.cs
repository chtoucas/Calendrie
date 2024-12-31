// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

// FIXME(code): math
// - ICalendarBound (naming)
// - CivilDate (interface)
// - CalendarScope.YearsValidator (public)
// - Calendar.IsRegular()

/// <summary>
/// Defines a type with a companion calendar system.
/// </summary>
/// <typeparam name="TCalendar">The companion calendar type.</typeparam>
public interface ICalendarBound<TCalendar>
    where TCalendar : Calendar
{
    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    static abstract TCalendar Calendar { get; }
}
