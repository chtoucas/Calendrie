// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;

/// <summary>
/// Represents a user-defined calendar.
/// </summary>
public class UserCalendar : Calendar
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="UserCalendar"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    public UserCalendar(string name, CalendarScope scope) : base(name, scope)
    {
        Debug.Assert(scope != null);
        Schema = scope.Schema;
    }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    //
    // Without this property, a derived class won't have access to the
    // underlying schema.
    protected ICalendricalSchema Schema { get; }
}
