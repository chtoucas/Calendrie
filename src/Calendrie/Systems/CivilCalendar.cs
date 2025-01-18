// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

/// <summary>
/// Represents the Civil calendar.
/// <para>This calendar is <i>retropolated</i>. It supports <i>all</i> dates
/// within the range [1..9999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class CivilCalendar : Calendar
{
    /// <summary>
    /// Represents the display name.
    /// <para>This field is a constant.</para>
    /// </summary>
    internal const string DisplayName = "Civil";

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilCalendar"/> class.
    /// </summary>
    public CivilCalendar() : this(new CivilSchema()) { }

    private CivilCalendar(CivilSchema schema) : base(DisplayName, new CivilScope(schema))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="CivilCalendar"/> class.
    /// <para>See <see cref="CivilDate.Calendar"/>.</para>
    /// </summary>
    internal static CivilCalendar Instance { get; } = new();

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => CivilScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => CivilScope.MaxYear;

    /// <summary>
    /// Gets the schema.
    /// </summary>
    internal CivilSchema Schema { get; }
}
