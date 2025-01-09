// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

/// <summary>
/// Represents the Julian calendar.
/// <para>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within
/// the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class JulianCalendar : CalendarSystem<JulianDate>
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;

    /// <summary>
    /// Represents the display name.
    /// <para>This field is a constant.</para>
    /// </summary>
    internal const string DisplayName = "Julian";

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianCalendar"/> class.
    /// </summary>
    public JulianCalendar() : this(new JulianSchema()) { }

    private JulianCalendar(JulianSchema schema) : base(DisplayName, new JulianScope(schema))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="JulianCalendar"/> class.
    /// <para>See <see cref="JulianDate.Calendar"/>.</para>
    /// </summary>
    internal static JulianCalendar Instance { get; } = new();

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => JulianScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => JulianScope.MaxYear;

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    internal JulianSchema Schema { get; }
}
