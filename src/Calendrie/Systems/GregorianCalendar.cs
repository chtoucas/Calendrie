// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

/// <summary>
/// Represents the Gregorian calendar.
/// <para>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within
/// the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class GregorianCalendar : Calendar
{
    /// <summary>
    /// Represents the display name.
    /// <para>This field is a constant.</para>
    /// </summary>
    internal const string DisplayName = "Gregorian";

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianCalendar"/> class.
    /// </summary>
    public GregorianCalendar() : this(new GregorianSchema()) { }

    private GregorianCalendar(GregorianSchema schema) : base(DisplayName, new GregorianScope(schema))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="GregorianCalendar"/> class.
    /// <para>See <see cref="GregorianDate.Calendar"/>.</para>
    /// </summary>
    internal static GregorianCalendar Instance { get; } = new();

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => GregorianScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => GregorianScope.MaxYear;

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    internal GregorianSchema Schema { get; }
}
