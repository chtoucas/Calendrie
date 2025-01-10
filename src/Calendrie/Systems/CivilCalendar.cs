﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents the Civil calendar.
/// <para>This calendar is <i>retropolated</i>. It supports <i>all</i> dates
/// within the range [1..9999] of years.</para>
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

    /// <summary>
    /// Obtains the number of days in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure]
    public int CountDaysInYear(int year)
    {
        if (year < CivilScope.MinYear || year > CivilScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);

        return Schema.CountDaysInYear(year);
    }

    /// <summary>
    /// Obtains the number of days in the specified month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The month is either invalid
    /// or outside the range of supported months.</exception>
    [Pure]
    public int CountDaysInMonth(int year, int month)
    {
        if (year < CivilScope.MinYear || year > CivilScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);
        if (month < 1 || month > MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month);

        return Schema.CountDaysInMonth(year, month);
    }
}
