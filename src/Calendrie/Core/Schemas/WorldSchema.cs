﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

// World calendar or Universal calendar, attempt to improve on the fixed
// calendars which themselves descents from the Positivist (Georgian)
// calendar.
// Four identical quarters: 91 = 31+30+30 days, 13 weeks, 3 months.
// Each year begins on Sunday.
// Each quarter begins on Sunday, ends on Saturday.
// Promoted by Camille Flammarion in France, and lobbied by Elisabeth
// Achelis to be adopted by the United Nations.
// See
// - http://myweb.ecu.edu/mccartyr/world-calendar.html
// - https://en.wikipedia.org/wiki/World_Calendar
//
// Main flaws: blank-days, position of the intercalary day.

/// <summary>
/// Represents the World schema proposed by Gustave Armelin and Émile Manin
/// (1887), and Elisabeth Achelis (1930).
/// <para>The world calendar is a blank-day calendar using a 12-months schema
/// with four identical quarters: one month of 31 days followed by two months of
/// 30 days.</para>
/// <para>The two blank-days are the Leapyear Day following June on leap years,
/// and the Worldsday following December.</para>
/// <para>For technical reasons, the blank-days are attached to the month
/// preceding them.</para>
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
public sealed partial class WorldSchema :
    RegularSchema,
    IDaysInMonths,
    ISchemaActivator<WorldSchema>
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int MonthsPerYear = 12;

    /// <summary>
    /// Represents the number of days per 400-year cycle.
    /// <para>This field is a constant equal to 146_097.</para>
    /// <para>On average, a year is 365.2425 days long.</para>
    /// </summary>
    public const int DaysPer400YearCycle = GregorianSchema.DaysPer400YearCycle;

    /// <summary>
    /// Represents the number of days in a common year.
    /// <para>This field is a constant equal to 365.</para>
    /// </summary>
    public const int DaysPerCommonYear = GJSchema.DaysPerCommonYear;

    /// <summary>
    /// Represents the number of days in a leap year.
    /// <para>This field is a constant equal to 366.</para>
    /// </summary>
    public const int DaysPerLeapYear = DaysPerCommonYear + 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldSchema"/> class.
    /// </summary>
    internal WorldSchema()
        : base(
            minDaysInYear: DaysPerCommonYear,
            minDaysInMonth: 30)
    { }

    /// <inheritdoc />
    public sealed override CalendricalFamily Family => CalendricalFamily.Solar;

    /// <inheritdoc />
    public sealed override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Days;

    /// <inheritdoc />
    public sealed override int MonthsInYear => MonthsPerYear;

    /// <summary>
    /// Gets the number of days in each month of a common year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfCommonYear =>
        // Quarters: 91, 91, 91, 92.
        [31, 30, 30, 31, 30, 30, 31, 30, 30, 31, 30, 31];

    /// <summary>
    /// Gets the number of days in each month of a leap year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfLeapYear =>
        // Quarters: 91, 92, 91, 92.
        [31, 30, 30, 31, 30, 31, 31, 30, 30, 31, 30, 31];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) =>
        leapYear ? DaysInMonthsOfLeapYear : DaysInMonthsOfCommonYear;

    /// <inheritdoc />
    [Pure]
    static WorldSchema ISchemaActivator<WorldSchema>.CreateInstance() => new();

    /// <summary>
    /// Obtains the genuine number of days in a month (excluding the blank days
    /// that are formally outside any month).
    /// <para>See also <seealso cref="CountDaysInMonth(int, int)"/>.</para>
    /// </summary>
    [Pure]
    internal static int CountDaysInWorldMonthImpl(int m) =>
        // m = 1, 4, 7, 10              -> 31 days
        // m = 2, 3, 5, 6, 8, 9, 11, 12 -> 30 days
        (m - 1) % 3 == 0 ? 31 : 30;

    /// <summary>
    /// Obtains the genuine number of days in a month (excluding the blank days
    /// that are formally outside any month).
    /// <para>See also <seealso cref="CountDaysInMonth(int, int)"/>.</para>
    /// </summary>
    [Pure]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "A month has 2 components")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public int CountDaysInWorldMonth(int y, int m) => CountDaysInWorldMonthImpl(m);
}

public partial class WorldSchema // Year, month or day infos
{
    /// <summary>
    /// Determines whether the specified date is a blank day or not.
    /// </summary>
    [Pure]
    internal static bool IsBlankDayImpl(int m, int d) => d == 31 && (m == 6 || m == 12);

    /// <summary>
    /// Determines whether the specified date is a blank day or not.
    /// </summary>
    [Pure]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "A date has 3 components")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public bool IsBlankDay(int y, int m, int d) => IsBlankDayImpl(m, d);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y) => GregorianFormulae.IsLeapYear(y);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) =>
        // We check the day first since it is the rarest case.
        d == 31 && m == 6;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => IsBlankDayImpl(m, d);
}

public partial class WorldSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int y) =>
        GregorianFormulae.IsLeapYear(y) ? DaysPerLeapYear : DaysPerCommonYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m)
    {
        m--;
        int count = 91 * (m / 3);
        if (m > 5 && GregorianFormulae.IsLeapYear(y)) { count++; }
        m %= 3;
        return m == 0 ? count : count + 1 + 30 * m;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        // m = 1, 4, 7, 10, 12   -> 31 days
        // m = 2, 3, 5, 8, 9, 11 -> 30 days
        // m = 6 -> 31 days (leap year) or 30 days (common year)
        m == 12 || checked(m - 1) % 3 == 0 ? 31
        : m == 6 && GregorianFormulae.IsLeapYear(y) ? 31
        : 30;
}

public partial class WorldSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysSinceEpoch(int y, int m, int d) =>
        GregorianFormulae.GetStartOfYear(y) + CountDaysInYearBeforeMonth(y, m) + d - 1;

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        if (GregorianFormulae.IsLeapYear(y))
        {
            // On évacue d'emblée le cas du jour intercalaire.
            if (doy == 183) { d = 31; return 6; }
            if (doy > 183) { doy--; }
        }

        // Worldsday.
        if (doy == 365) { d = 31; return 12; }

        // Four identical quarter: 31, 30 and 30.
        int m;
        doy--;
        int q = doy / 91;
        int j = doy % 91;
        if (j < 31)
        {
            m = 1 + 3 * q;
            d = 1 + j % 31;
        }
        else
        {
            j--;
            m = 1 + 3 * q + j / 30;
            d = 1 + j % 30;
        }
        return m;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch) =>
        GregorianFormulae.GetYear(daysSinceEpoch);
}

public partial class WorldSchema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y) => GregorianFormulae.GetStartOfYear(y);
}
