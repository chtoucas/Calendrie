// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;

/// <summary>
/// Represents a user-defined calendar and provides a base for derived classes.
/// </summary>
public abstract class UserCalendar : Calendar
{
    protected UserCalendar(string name, CalendarScope scope) : base(name, scope)
    {
        Debug.Assert(scope != null);
        Schema = scope.Schema;
    }

    /// <summary>
    /// Gets the schema.
    /// </summary>
    protected ICalendricalSchema Schema { get; }
}
