// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

public static class FrenchRepublicanCycle
{
    /// <summary>
    /// Represents the number of years in a cycle.
    /// <para>This field is a constant equal to 4000.</para>
    /// </summary>
    public const int YearsPerCycle = 4000;

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
    /// Represents the number of leap years per 4000-year cycle.
    /// <para>This field is a constant equal to 969.</para>
    /// </summary>
    public const int LeapYearsPerCycle = 969;

    /// <summary>
    /// Represents the number of days in a 4000-year cycle.
    /// <para>This field is a constant equal to 1_460_969.</para>
    /// <para>On average, a year is 365.24225 days long.</para>
    /// </summary>
    public const long DaysPerCycle = YearsPerCycle * DaysPerCommonYear + LeapYearsPerCycle;
}
