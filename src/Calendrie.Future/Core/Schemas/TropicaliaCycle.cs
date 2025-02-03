// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

public static class TropicaliaCycle
{
    /// <summary>
    /// Represents the number of years in a cycle.
    /// <para>This field is a constant equal to 128.</para>
    /// </summary>
    public const int YearsPerCycle = 128;

    /// <summary>
    /// Represents the number of days in a common year.
    /// <para>This field is a constant equal to 365.</para>
    /// </summary>
    public const int DaysPerCommonYear = CalendricalConstants.DaysPerWanderingYear;

    /// <summary>
    /// Represents the number of days in a leap year.
    /// <para>This field is a constant equal to 366.</para>
    /// </summary>
    public const int DaysPerLeapYear = DaysPerCommonYear + 1;

    /// <summary>
    /// Represents the number of leap years per 128-year cycle.
    /// <para>This field is a constant equal to 31.</para>
    /// </summary>
    public const int LeapYearsPerCycle = 31;

    /// <summary>
    /// Represents the number of days per 128-year cycle.
    /// <para>This field is a constant equal to 46_751.</para>
    /// <para>On average, a year is 365.2421875 days long.</para>
    /// </summary>
    public const int DaysPerCycle = YearsPerCycle * DaysPerCommonYear + LeapYearsPerCycle;

    /// <summary>
    /// Represents the <i>average</i> number of days per 4-year subcycle.
    /// <para>This field is a constant equal to 1461.</para>
    /// <para>On average, a year is 365.25 days long.</para>
    /// </summary>
    public const int DaysPer4YearSubcycle = CalendricalConstants.DaysPer4JulianYearCycle;

}
