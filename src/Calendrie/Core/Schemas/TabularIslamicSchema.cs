﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents the proleptic Tabular Islamic schema.
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.
/// </para>
/// </summary>
public sealed partial class TabularIslamicSchema :
    LimitSchema,
    IDaysInMonths,
    ISchemaActivator<TabularIslamicSchema>
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = 12;

    /// <summary>
    /// Represents the number of days per 30-year cycle.
    /// <para>This field is a constant equal to 10_631.</para>
    /// </summary>
    /// <remarks>On average, a year is 354.36666... days long.</remarks>
    public const int DaysPer30YearCycle = 19 * DaysInCommonYear + 11 * 355;

    /// <summary>
    /// Represents the number of days in a common year.
    /// <para>This field is a constant equal to 354.</para>
    /// </summary>
    public const int DaysInCommonYear = 354;

    /// <summary>
    /// Represents the number of days in a leap year.
    /// <para>This field is a constant equal to 355.</para>
    /// </summary>
    public const int DaysInLeapYear = DaysInCommonYear + 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="TabularIslamicSchema"/>
    /// class.
    /// </summary>
    internal TabularIslamicSchema()
        // On choisit min/maxYear en fonction de GetYear(int daysSinceEpoch).
        // Dans cette méthode, on va multiplier daysSinceEpoch par 30.
        // Pour rester dans les limites de Int32,
        // 2^31 / 30 * 355 (DaysInLeapYear) = 201K années max
        : base(Range.Create(-199_999, 200_000), DaysInCommonYear, 29)
    {
        SupportedYearsCore = SupportedYears;
    }

    /// <inheritdoc />
    public sealed override CalendricalFamily Family => CalendricalFamily.Lunar;

    /// <inheritdoc />
    public sealed override CalendricalAdjustments PeriodicAdjustments =>
        CalendricalAdjustments.Days;

    /// <summary>
    /// Gets the number of days in each month of a common year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfCommonYear =>
        [30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29];

    /// <summary>
    /// Gets the number of days in each month of a leap year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfLeapYear =>
        [30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 30];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) =>
        leapYear ? DaysInMonthsOfLeapYear : DaysInMonthsOfCommonYear;

    /// <inheritdoc />
    [Pure]
    static TabularIslamicSchema ISchemaActivator<TabularIslamicSchema>.CreateInstance() => new();

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = MonthsInYear;
        return true;
    }
}

public partial class TabularIslamicSchema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y) => MathZ.Modulo(checked(14 + 11 * y), 30) < 11;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryMonth(int y, int m) => false;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => m == 12 && d == 30;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => false;
}

public partial class TabularIslamicSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsInYear(int y) => MonthsInYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int y) =>
        IsLeapYear(y) ? DaysInLeapYear : DaysInCommonYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m) => 29 * (m - 1) + (m >> 1);

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        (m & 1) == 0 && (m != 12 || !IsLeapYear(y)) ? 29 : 30;
}

public partial class TabularIslamicSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsSinceEpoch(int y, int m) =>
        MonthsCalculator.Regular12.CountMonthsSinceEpoch(y, m);

    /// <inheritdoc />
    public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m) =>
        MonthsCalculator.Regular12.GetMonthParts(monthsSinceEpoch, out y, out m);

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysSinceEpoch(int y, int m, int d) =>
        GetStartOfYear(y) + 29 * (m - 1) + (m >> 1) + d - 1;

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        int d0y = doy - 1;
        int m = (11 * d0y + 330) / 325;
        d = 1 + d0y - 29 * (m - 1) - (m >> 1);
        return m;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch) =>
        // NB: On a choisi Min/MaxYear de sorte que l'opération ne provoque
        // pas de dépassement arithmétique, sinon aurait dû écrire :
        // > (int)Divide(30L * daysSinceEpoch + 10_646, DaysPer30YearCycle);
        MathZ.Divide(30 * daysSinceEpoch + 10_646, DaysPer30YearCycle);
}

public partial class TabularIslamicSchema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYearInMonths(int y) =>
        MonthsCalculator.Regular12.GetStartOfYear(y);

    /// <inheritdoc />
    [Pure]
    public sealed override int GetEndOfYearInMonths(int y) =>
        MonthsCalculator.Regular12.GetEndOfYear(y);

    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y) =>
        DaysInCommonYear * (y - 1) + MathZ.Divide(3 + 11 * y, 30);
}

public partial class TabularIslamicSchema // Dates in a given year or month
{
    /// <inheritdoc />
    public sealed override void GetDatePartsAtEndOfYear(int y, out int m, out int d)
    {
        m = 12;
        d = IsLeapYear(y) ? 30 : 29;
    }
}
