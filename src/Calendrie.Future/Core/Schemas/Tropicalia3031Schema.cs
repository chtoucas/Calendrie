// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

/// <summary>
/// Represents the "Tropicália" schema (30-31).
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
public sealed partial class Tropicalia3031Schema :
    TropicalistaSchema,
    IDaysInMonths,
    ISchemaActivator<Tropicalia3031Schema>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Tropicalia3031Schema"/> class.
    /// </summary>
    internal Tropicalia3031Schema() : base(30) { }

    /// <summary>
    /// Gets the number of days in each month of a common year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfCommonYear =>
        // Quarters: 91, 92, 91, 91.
        [30, 31, 30, 31, 30, 31, 30, 31, 30, 31, 30, 30];

    /// <summary>
    /// Gets the number of days in each month of a leap year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfLeapYear =>
        // Quarters: 91, 92, 91, 92.
        [30, 31, 30, 31, 30, 31, 30, 31, 30, 31, 30, 31];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) =>
        leapYear ? DaysInMonthsOfLeapYear : DaysInMonthsOfCommonYear;

    /// <inheritdoc />
    [Pure]
    static Tropicalia3031Schema ISchemaActivator<Tropicalia3031Schema>.CreateInstance() => new();
}

public partial class Tropicalia3031Schema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => m == 12 && d == 31;
}

public partial class Tropicalia3031Schema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m) => 30 * --m + (m >> 1);

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        m != 12 ? 31 - (m & 1)
        : IsLeapYearImpl(y) ? 31
        : 30;
}

public partial class Tropicalia3031Schema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysSinceEpoch(int y, int m, int d)
    {
        y--;
        m--;
        return 365 * y + (y >> 2) - (y >> 7)
            + 30 * m + (m >> 1)
            + d - 1;
    }

    /// <inheritdoc />
    public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        int C = MathZ.Divide(daysSinceEpoch, DaysPer128YearCycle, out int D);

        int Y = ((D << 2) + 3) / AverageDaysPer4YearSubcycle;
        int d0y = D - (AverageDaysPer4YearSubcycle * Y >> 2);

        m = ((d0y << 1) + 62) / 61;
        d = 1 + d0y - 30 * (m - 1) - ((m - 1) >> 1);
        y = (C << 7) + Y + 1;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        int d0y = doy - 1;
        int m = ((d0y << 1) + 62) / 61;
        d = 1 + d0y - 30 * (m - 1) - ((m - 1) >> 1);
        return m;
    }
}
