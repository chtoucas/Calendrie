// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

/// <summary>
/// Represents the Civil calendar.
/// <para>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class CivilCalendar : CalendarSystem<CivilDate>
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilCalendar"/> class.
    /// </summary>
    private CivilCalendar(CivilSchema schema, CivilScope scope) : base("Civil", scope)
    {
        UnderlyingSchema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="CivilCalendar"/> class.
    /// <para>See also <seealso cref="CivilDate.Calendar"/>.</para>
    /// </summary>
    public static CivilCalendar Instance { get; } = CreateInstance();

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
    //
    // Because I don't want Calendar to be generic [Calendar<TSchema>], we lose
    // the actual schema type; we only get an ICalendricalSchema. This property
    // is here to circumvent this problem.
    internal CivilSchema UnderlyingSchema { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="CivilCalendar"/> class.
    /// </summary>
    private static CivilCalendar CreateInstance()
    {
        var sch = new CivilSchema();

        return new(sch, new CivilScope(sch));
    }
}
