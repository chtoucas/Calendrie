// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

/// <summary>
/// Provides a base for the "Tropicália" schemas.
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
public abstract partial class TropicalistaSchema : RegularSchema
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsPerYear = 12;

    /// <summary>
    /// Represents the number of days per 128-year cycle.
    /// <para>This field is a constant equal to 46_751.</para>
    /// <para>On average, a year is 365.2421875 days long.</para>
    /// </summary>
    public const int DaysPer128YearCycle = 128 * DaysPerCommonYear + 31;

    /// <summary>
    /// Represents the <i>average</i> number of days per 4-year subcycle.
    /// <para>This field is a constant equal to 1461.</para>
    /// <para>On average, a year is 365.25 days long.</para>
    /// </summary>
    public const int AverageDaysPer4YearSubcycle = CalendricalConstants.DaysPer4JulianYearCycle;

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
    /// Called from constructors in derived classes to initialize the
    /// <see cref="TropicalistaSchema"/> class.
    /// </summary>
    private protected TropicalistaSchema(int minDaysInMonth)
        : base(DaysPerCommonYear, minDaysInMonth) { }

    /// <inheritdoc />
    public sealed override CalendricalFamily Family => CalendricalFamily.Solar;

    /// <inheritdoc />
    public sealed override CalendricalAdjustments PeriodicAdjustments =>
        CalendricalAdjustments.Days;

    /// <inheritdoc />
    public sealed override int MonthsInYear => MonthsPerYear;
}

public partial class TropicalistaSchema // Year, month or day infos
{
    /// <summary>
    /// Determines whether the specified year is leap or not.
    /// </summary>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    // NB: year zero is not leap.
    internal static bool IsLeapYearImpl(int y) => (y & 3) == 0 && (y & 127) != 0;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y) => IsLeapYearImpl(y);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => false;
}

public partial class TropicalistaSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int y) =>
        IsLeapYearImpl(y) ? DaysPerLeapYear : DaysPerCommonYear;
}

public partial class TropicalistaSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch)
    {
        int C = MathZ.Divide(daysSinceEpoch, DaysPer128YearCycle, out int D);
        return 1 + (C << 7) + ((D << 2) + 3) / AverageDaysPer4YearSubcycle;
    }
}

public partial class TropicalistaSchema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y)
    {
        y--;
        return 365 * y + (y >> 2) - (y >> 7);
    }
}
