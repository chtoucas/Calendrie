﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

// A modern descendant of the Positivist calendar. The anglo-saxons
// attributes the authorship to Moses Cotsworth, but it seems that a similar
// proposal had been made around the same time by a guy named Paul Delaporte
// (Paul Courderc, PUF p. 99).
// George Eastman was a fervent promoter of this calendar.
//
// Main flaws: blank-days, 13 months, position of the intercalary day.

/// <summary>
/// Represents the International Fixed schema proposed by Paul Delaporte and
/// Moses Bruine Cotsworth (1913-1914).
/// <para>The International Fixed calendar is a blank-day calendar for which the
/// year is divided into 13 months of identical length (28 days). The extra month,
/// called Sol, is inserted after June.</para>
/// <para>The two blank-days are the Leap Day following June on leap years, and
/// the Year Day following December (the thirteenth month).</para>
/// <para>For technical reasons, the blank-days are attached to the month
/// preceding them.</para>
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
public sealed partial class InternationalFixedSchema :
    RegularSchema,
    IDaysInMonths,
    ISchemaActivator<InternationalFixedSchema>
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    public const int MonthsPerYear = 13;

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
    public const int DaysPerCommonYear = CalendricalConstants.DaysPerWanderingYear;

    /// <summary>
    /// Represents the number of days in a leap year.
    /// <para>This field is a constant equal to 366.</para>
    /// </summary>
    public const int DaysPerLeapYear = DaysPerCommonYear + 1;

    /// <summary>
    /// Represents the genuine number of days in a standard month (excluding the
    /// blank days that are formally outside any month).
    /// <para>This field is a constant equal to 28.</para>
    /// <para>See also <seealso cref="CountDaysInMonth(int, int)"/>.</para>
    /// </summary>
    public const int DaysPerMonth = 28;

    /// <summary>
    /// Initializes a new instance of the <see cref="InternationalFixedSchema"/> class.
    /// </summary>
    internal InternationalFixedSchema()
        : base(
            minDaysInYear: DaysPerCommonYear,
            minDaysInMonth: 28)
    { }

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
        // Quarters: 84, 84, 84, 84, 29.
        [28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29];

    /// <summary>
    /// Gets the number of days in each month of a leap year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfLeapYear =>
        // Quarters: 84, 85, 84, 84, 29.
        [28, 28, 28, 28, 28, 29, 28, 28, 28, 28, 28, 28, 29];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) =>
        leapYear ? DaysInMonthsOfLeapYear : DaysInMonthsOfCommonYear;

    /// <inheritdoc />
    [Pure]
    static InternationalFixedSchema ISchemaActivator<InternationalFixedSchema>.CreateInstance() => new();
}

public partial class InternationalFixedSchema // Year, month or day infos
{
    /// <summary>
    /// Determines whether the specified date is a blank day or not.
    /// </summary>
    [Pure]
    internal static bool IsBlankDayImpl(int d) => d > 28;

    /// <summary>
    /// Determines whether the specified date is a blank day or not.
    /// </summary>
    [Pure]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "A date has 3 components")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Static would force us to validate the parameters")]
    public bool IsBlankDay(int y, int m, int d) => IsBlankDayImpl(d);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y) => GregorianFormulae.IsLeapYear(y);

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) =>
        // We check the day first since it is the rarest case.
        d == 29 && m == 6;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => d > 28;
}

public partial class InternationalFixedSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int y) =>
        GregorianFormulae.IsLeapYear(y) ? DaysPerLeapYear : DaysPerCommonYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m)
    {
        int count = DaysPerMonth * (m - 1);
        if (m > 6 && GregorianFormulae.IsLeapYear(y)) { count++; }
        return count;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        m == 13 || (m == 6 && GregorianFormulae.IsLeapYear(y)) ? 29 : 28;
}

public partial class InternationalFixedSchema // Conversions
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
            if (doy == 169) { d = 29; return 6; }
            if (doy > 169) { doy--; }
        }

        int m;
        if (doy > 336)
        {
            m = 13;
            d = doy - 336;
        }
        else
        {
            int d0y = doy - 1;
            m = 1 + d0y / 28;
            d = 1 + d0y % 28;
        }
        return m;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch) =>
        GregorianFormulae.GetYear(daysSinceEpoch);
}

public partial class InternationalFixedSchema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y) => GregorianFormulae.GetStartOfYear(y);
}
