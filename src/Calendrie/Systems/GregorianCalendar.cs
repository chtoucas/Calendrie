﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

/// <summary>
/// Represents the Gregorian calendar.
/// <para>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within
/// the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class GregorianCalendar : CalendarSystem<GregorianDate>
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianCalendar"/> class.
    /// </summary>
    public GregorianCalendar() : this(new GregorianSchema()) { }

    private GregorianCalendar(GregorianSchema schema) : this(schema, new GregorianScope(schema)) { }

    private GregorianCalendar(GregorianSchema schema, GregorianScope scope)
        : base("Gregorian", scope)
    {
        UnderlyingSchema = schema;
    }

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => GregorianScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => GregorianScope.MaxYear;

    /// <summary>
    /// Gets a singleton instance of the <see cref="GregorianCalendar"/> class.
    /// <para>See <see cref="GregorianDate.Calendar"/>.</para>
    /// </summary>
    internal static GregorianCalendar Instance { get; } = new();

    /// <summary>
    /// Gets the schema.
    /// </summary>
    internal GregorianSchema UnderlyingSchema { get; }
}
