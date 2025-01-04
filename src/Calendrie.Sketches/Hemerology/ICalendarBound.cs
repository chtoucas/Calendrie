// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

public interface ICalendarBound<out TCalendar>
{
    /// <summary>
    /// Gets the calendar to which belongs the current type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    static abstract TCalendar Calendar { get; }
}
