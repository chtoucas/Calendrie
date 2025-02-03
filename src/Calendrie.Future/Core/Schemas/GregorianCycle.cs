// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

public static class GregorianCycle
{
    /// <summary>
    /// Represents the number of years in a cycle.
    /// <para>This field is a constant equal to 400.</para>
    /// </summary>
    public const int YearsPerCycle = 400;

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
    /// Represents the number of leap years per 400-year cycle.
    /// <para>This field is a constant equal to 97.</para>
    /// </summary>
    public const int LeapYearsPerCycle = 97;

    /// <summary>
    /// Represents the number of days per 400-year cycle.
    /// <para>This field is a constant equal to 146_097.</para>
    /// <para>On average, a year is 365.2425 days long.</para>
    /// </summary>
    public const int DaysPerCycle = YearsPerCycle * DaysPerCommonYear + LeapYearsPerCycle;

    /// <summary>
    /// Represents the <i>average</i> number of days per 100-year subcycle.
    /// <para>This field is a constant equal to 36_524.</para>
    /// <para>On average, a year is 365.24 days long.</para>
    /// </summary>
    public const int DaysPer100YearSubcycle = 100 * DaysPerCommonYear + 24;

    /// <summary>
    /// Represents the <i>average</i> number of days per 4-year subcycle.
    /// <para>This field is a constant equal to 1461.</para>
    /// <para>On average, a year is 365.25 days long.</para>
    /// </summary>
    public const int DaysPer4YearSubcycle = CalendricalConstants.DaysPerJulianCycle;
}
