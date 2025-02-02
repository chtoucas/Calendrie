// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

public static class JulianCycle
{
    /// <summary>
    /// Represents the number of years in a cycle.
    /// <para>This field is a constant equal to 4.</para>
    /// </summary>
    public const int YearLength = 4;

    /// <summary>
    /// Represents the number of leap years per 4-year cycle.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    public const int LeapYearsPerCycle = 1;

    /// <summary>
    /// Represents the number of days per 4-year cycle.
    /// <para>This field is a constant equal to 1461.</para>
    /// <para>On average, a year is 365.25 days long.</para>
    /// </summary>
    public const int DaysPerCycle = CalendricalConstants.DaysPer4JulianYearCycle;
}
