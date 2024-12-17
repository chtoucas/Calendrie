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
    private CivilCalendar() : base("Civil", new CivilScope(new CivilSchema()))
    {
        UnderlyingSchema = (CivilSchema)Schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="CivilCalendar"/> class.
    /// <para>See also <seealso cref="CivilDate.Calendar"/>.</para>
    /// </summary>
    public static CivilCalendar Instance { get; } = new();

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => CivilScope.MinYear;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => CivilScope.MaxYear;

    // TODO(code): UnderlyingSchema
    // Because I don't want Calendar to be generic [Calendar<TSchema>], we lose
    // the actual schema type; we only get an ICalendricalSchema. This property
    // is here to circumvent this problem. Used by:
    // 1. All instances of the <#= Prefix #>Date type via the property Schema.
    //    A CalendricalSchema is needed by the following methods:
    //    - Schema.GetYear(_daysSinceEpoch)
    //    - Schema.CountDaysInYearBefore(_daysSinceEpoch)
    //    - Schema.CountDaysInYearAfter(_daysSinceEpoch)
    //    - Schema.CountDaysInMonthBefore(_daysSinceEpoch)
    //    - Schema.CountDaysInMonthAfter(_daysSinceEpoch)
    // 2. <#= Prefix #>Calendar, custom methods only (see the file _Calendar.cs)
    //
    // Better solution? Regarding the second point, we don't actually need it as
    // we use static methods on the schema; see WorldCalendar.
    // Regarding the first point, expect for GetYear() which is abstract, we
    // could simply provide default implementations at the interface level, which
    // also means to move these methods from ICalendricalSchemaPlus to
    // ICalendricalSchema. Finally, we would also need to replace GetYear() by
    // GetYear(int daysSinceEpoch, out int doy).

    /// <summary>
    /// Gets the schema.
    /// </summary>
    internal CivilSchema UnderlyingSchema { get; }
}
