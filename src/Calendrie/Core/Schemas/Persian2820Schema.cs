﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

// Three cycles:
// - a grand cycle consisting of 2820 years,
// - divided into cycles consisting of 128 or 132 years,
// - divided into subcycles consisting of 29, 33 or 37 years.
//
// The 2820-year cycle
//   = twenty-one 128-year cycles followed by a 132-year cycle.
//   = 21 x 128 + 132
// The 128-year cycle
//   = one 29-year cycle followed by three 33-year cycle.
//   = 29 + 3 x 33
//   Counts 31 leap years and 97 x 365 + 31 x 366 = 46,751 days.
// The 132-year cycle
//   = one 29-year cycle followed by two 33-year cycle, followed by one
//     37-year cycle.
//   = 29 + 2 x 33 + 37
//   Counts 32 leap years and 100 x 365 + 32 x 366 = 48,212.
// A year "y" is leap if the year in a 29 or 33 or 37-year cycle
// if y % 4 = 1 AND y != 1 (first year is not leap).
// This gives us the years 5, 9, 13, etc. Therefore, in a 2820-year cycle
// there are 683 leap years:
//   21 * (7 + 3 × 8) + (7 + 2 × 8 + 9)

/// <summary>
/// Represents the Persian schema, proposed arithmetical form by Ahmad Birashk.
/// <para>This is not the calendar currently in use in Iran. Furthermore, it has
/// "been criticized by calendar researchers, among them the Iranian astronomers
/// Malakpour (2004) and Sayyâd (2000)";
/// see http://aramis.obspm.fr/~heydari/divers/ir-cal-eng.pdf.</para>
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
public sealed partial class Persian2820Schema :
    RegularSchema,
    IDaysInMonths,
    ISchemaActivator<Persian2820Schema>
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsPerYear = 12;

    /// <summary>
    /// Represents the number of days per 2820-year cycle.
    /// <para>This field is a constant equal to 1_029_983.</para>
    /// <para>On average, a year is approximately 365.242196... days long.</para>
    /// </summary>
    public const int DaysPer2820YearCycle = 2820 * DaysPerCommonYear + 683;

    /// <summary>
    /// Represents the number of days per 128-year subcycle.
    /// <para>This field is a constant equal to 46_751.</para>
    /// <para>On average, a year is approximately 365.242187... days long.</para>
    /// </summary>
    public const int DaysPer128YearSubcycle = 97 * DaysPerCommonYear + 31 * DaysPerLeapYear;

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
    /// Represents the number of whole days elapsed since the start of the
    /// year and before July.
    /// <para>This field is a constant equal to 186.</para>
    /// </summary>
    public const int DaysPerYearBeforeJuly = 186;

    /// <summary>
    /// Represents the year "zero" of the first whole 2820-year cycle.
    /// <para>The first 2820-year cycle is the range 475-3295. Before that we
    /// have 86 years followed by two 128-year cycles and one 132-year cycle.
    /// </para>
    /// </summary>
    public const int YearZero = 474;

    /// <summary>
    /// Initializes a new instance of the <see cref="Persian2820Schema"/> class.
    /// </summary>
    internal Persian2820Schema()
        : base(
            minDaysInYear: DaysPerCommonYear,
            minDaysInMonth: 29)
    {
        SupportedYearsCore = Segment.StartingAt(int.MinValue + YearZero);
    }

    /// <inheritdoc />
    public sealed override int MonthsInYear => MonthsPerYear;

    /// <inheritdoc />
    public sealed override CalendricalFamily Family => CalendricalFamily.Solar;

    /// <inheritdoc />
    public sealed override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Days;

    /// <summary>
    /// Gets the number of days in each month of a common year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfCommonYear =>
        // Quarters: 93, 93, 90, 89.
        [31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29];

    /// <summary>
    /// Gets the number of days in each month of a leap year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfLeapYear =>
        // Quarters: 93, 93, 90, 90.
        [31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 30];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) =>
        leapYear ? DaysInMonthsOfLeapYear : DaysInMonthsOfCommonYear;

    /// <inheritdoc />
    [Pure]
    static Persian2820Schema ISchemaActivator<Persian2820Schema>.CreateInstance() => new();
}

public partial class Persian2820Schema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y)
    {
        checked { y -= YearZero; }
        // WARNING: even if MinYear > 0, after the above shift "y" may
        // become negative.
        int Y = YearZero + MathZ.Modulo(y, 2820);
        return 31 * (Y + 38) % 128 < 31;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => m == 12 && d == 30;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => false;
}

public partial class Persian2820Schema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int y) =>
        IsLeapYear(y) ? DaysPerLeapYear : DaysPerCommonYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m) =>
        m <= 7 ? 31 * (m - 1) : 6 + 30 * (m - 1);

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        m < 7 ? 31
        : m < 12 ? 30
        : IsLeapYear(y) ? 30 : 29;
}

public partial class Persian2820Schema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        int d0y = doy - 1;
        int m = d0y < DaysPerYearBeforeJuly ? 1 + d0y / 31 : 1 + (d0y - 6) / 30;
        d = 1 + d0y - CountDaysInYearBeforeMonth(y, m);
        return m;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch)
    {
        daysSinceEpoch -= GetStartOfYear(YearZero + 1);
        // WARNING: even if MinYear > 0, after the above shift
        // daysSinceEpoch may become negative.
        int C = MathZ.Divide(daysSinceEpoch, DaysPer2820YearCycle, out int D);
        // The last year of a 2820-year cycle is leap, its last day is
        // numbered (DaysPer2820Years - 1) and requires a special treatment.
        int Y = D == DaysPer2820YearCycle - 1 ? 2820
            : (128 * D + DaysPer128YearSubcycle + 127) / DaysPer128YearSubcycle;
        return YearZero + 2820 * C + Y;
    }
}

public partial class Persian2820Schema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y)
    {
        y -= YearZero;
        int Y = YearZero + MathZ.Modulo(y, 2820, out int C);
        return DaysPer2820YearCycle * C + DaysPerCommonYear * (Y - 1) + (31 * Y - 5) / 128;
    }
}
