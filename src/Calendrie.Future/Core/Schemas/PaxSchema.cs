﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

#define PROTOTYPING

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Validation;

using static Calendrie.Core.CalendricalConstants;

// Month 12 = Columbus
// Common year:
// - month 13 = December
// Leap year:
// - month 13 = Pax (1 week), intercalary week/month
// - month 14 = December
//
// This is an early proposal for a leap-week calendar, certainly not a very
// good one.
//
// Main flaws: 13 or 14 months, complex leap rule without any improvement
// over the Gregorian one, position of the intercalary week.
//
// References:
// - http://myweb.ecu.edu/mccartyr/colligan.html
// - org.threeten.extra.chrono.PaxChronology

/// <summary>
/// Represents the Pax schema proposed by James A. Colligan, S.J. (1930).
/// <para>The Pax schema is a leap-week schema.</para>
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
public sealed partial class PaxSchema :
#if PROTOTYPING
    NonRegularSchemaPrototype,
#else
    CalendricalSchema,
#endif
    ILeapWeekSchema,
    IDaysInMonths,
    ISchemaActivator<PaxSchema>
{
    /// <summary>
    /// Represents the number of months in a common year.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    public const int MonthsPerCommonYear = 13;

    /// <summary>
    /// Represents the number of months in a leap year.
    /// <para>This field is a constant equal to 14.</para>
    /// </summary>
    public const int MonthsPerLeapYear = MonthsPerCommonYear + 1;

    /// <summary>
    /// Represents the number of weeks in a common year.
    /// <para>This field is a constant equal to 52.</para>
    /// </summary>
    public const int WeeksPerCommonYear = 52;

    /// <summary>
    /// Represents the number of weeks in a leap year.
    /// <para>This field is a constant equal to 53.</para>
    /// </summary>
    public const int WeeksPerLeapYear = WeeksPerCommonYear + 1;

    /// <summary>
    /// Represents the number of days per 400-year cycle.
    /// <para>This field is a constant equal to 146_097.</para>
    /// <para>On average, a year is 365.2425 days long.</para>
    /// </summary>
    public const int DaysPer400YearCycle = 400 * DaysPerCommonYear + 71 * DaysInPaxMonth;

    /// <summary>
    /// Represents the number of days in a common year.
    /// <para>This field is a constant equal to 364.</para>
    /// </summary>
    public const int DaysPerCommonYear = WeeksPerCommonYear * DaysPerWeek;

    /// <summary>
    /// Represents the number of days in a leap year.
    /// <para>This field is a constant equal to 371.</para>
    /// </summary>
    public const int DaysPerLeapYear = DaysPerCommonYear + DaysInPaxMonth;

    /// <summary>
    /// Represents the Columbus month.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    public const int ColumbusMonth = 12;

    /// <summary>
    /// Represents the Pax month of a leap year.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    private const int PaxMonth = 13;

    /// <summary>
    /// Represents the Pax week of a leap year.
    /// <para>This field is a constant equal to 49.</para>
    /// </summary>
    private const int PaxWeek = 49;

    /// <summary>
    /// Represents the number of days in a standard month.
    /// <para>This field is a constant equal to 28.</para>
    /// </summary>
    private const int DaysPerMonth = 28;

    /// <summary>
    /// Represents the number of days in a Pax month.
    /// <para>This field is a constant equal to 7.</para>
    /// </summary>
    private const int DaysInPaxMonth = 7;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaxSchema"/> class.
    /// </summary>
#if PROTOTYPING
    internal PaxSchema()
        : base(
            proleptic: false,
            minMonthsInYear: MonthsPerCommonYear,
            minDaysInYear: DaysPerCommonYear,
            minDaysInMonth: 7)
#else
    internal PaxSchema()
        : base(
            supportedYears: DefaultSupportedYears.WithMin(1),
            minDaysInYear: DaysPerCommonYear,
            minDaysInMonth: 7)
#endif
    {
        PreValidator = new PaxPreValidator(this);
    }

    /// <inheritdoc />
    public sealed override CalendricalFamily Family => CalendricalFamily.Other;

    /// <inheritdoc />
    public sealed override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Weeks;

    /// <summary>
    /// Gets the number of days in each month of a common year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfCommonYear =>
        // Quarters: 84, 84, 84, 84, 28.
        [28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28];

    /// <summary>
    /// Gets the number of days in each month of a leap year.
    /// <para>The span index matches the month index <i>minus one</i>.</para>
    /// </summary>
    private static ReadOnlySpan<byte> DaysInMonthsOfLeapYear =>
        // Quarters: 84, 84, 84, 84, 7 + 28.
        [28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, DaysInPaxMonth, 28];

    /// <inheritdoc />
    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) =>
        leapYear ? DaysInMonthsOfLeapYear : DaysInMonthsOfCommonYear;

    /// <inheritdoc />
    [Pure]
    static PaxSchema ISchemaActivator<PaxSchema>.CreateInstance() => new();

    /// <summary>
    /// Determines whether the specified month is the Pax month of a year or
    /// not.
    /// </summary>
    [Pure]
    public bool IsPaxMonth(int y, int m) => m == PaxMonth && IsLeapYear(y);

    /// <summary>
    /// Determines whether the specified month is the last month of the year or
    /// not.
    /// <para>Whether the year is leap or not, the last month of the year is
    /// called December.</para>
    /// </summary>
    [Pure]
    public bool IsLastMonthOfYear(int y, int m) => m == 14 || (m == 13 && !IsLeapYear(y));

#if !PROTOTYPING
    /// <inheritdoc />
    public sealed override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = 0;
        return false;
    }
#endif
}

public partial class PaxSchema // ILeapWeekSchema
{
    public DayOfWeek FirstDayOfWeek { get; } = DayOfWeek.Sunday;

    /// <summary>
    /// Determines whether the specified week is intercalary or not.
    /// <para>An intercalary week is also called Pax.</para>
    /// </summary>
    [Pure]
    public bool IsIntercalaryWeek(int y, int woy) =>
        // Intercalary week = the week of the Pax month on a leap year.
        woy == PaxWeek && IsLeapYear(y);

    /// <inheritdoc />
    [Pure]
    public int CountWeeksInYear(int y) => IsLeapYear(y) ? WeeksPerLeapYear : WeeksPerCommonYear;

    /// <inheritdoc />
    [Pure]
    public int CountDaysSinceEpoch(int y, int woy, DayOfWeek dow) =>
        // The first day of the week is a Sunday not a Monday, therefore
        // we do not have to use the adjusted day of the week.
        GetStartOfYear(y) + DaysPerWeek * (woy - 1) + (int)dow;

    public void GetWeekdateParts(int weeksSinceEpoch, out int y, out int woy, out DayOfWeek dow)
    {
        throw new NotImplementedException();
    }
}

public partial class PaxSchema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y)
    {
        int Y = y % 100;
        // - last two digits of y = 99 or is a multiple of 6.
        // - a century always satisfies (Y % 6 == 0), but we only keep it
        //   if it is not a multiple of 400.
        return Y == 99 || (Y % 6 == 0 && (Y != 0 || y % 400 != 0));
    }

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryMonth(int y, int m) => false;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryDay(int y, int m, int d) => false;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsSupplementaryDay(int y, int m, int d) => false;
}

public partial class PaxSchema // Counting months and days within a year or a month
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsInYear(int y) =>
        IsLeapYear(y) ? MonthsPerLeapYear : MonthsPerCommonYear;

    /// <inheritdoc />
    //
    // 364 days = 13 months of 28 days
    // 371 days = 364 days + 7 days (Pax month)
    [Pure]
    public sealed override int CountDaysInYear(int y) =>
        IsLeapYear(y) ? DaysPerLeapYear : DaysPerCommonYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int y, int m) =>
        IsPaxMonth(y, m) ? DaysInPaxMonth : DaysPerMonth;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m) =>
        // PaxWeek * DaysPerWeek = 49 * 7 = 343
        m == 14 ? PaxWeek * DaysPerWeek : DaysPerMonth * (m - 1);
}

public partial class PaxSchema // Conversions
{
#if !PROTOTYPING
    /// <inheritdoc />
    public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    [Pure]
    public override int GetYear(int daysSinceEpoch, out int doy)
    {
        throw new NotImplementedException();
    }


    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch)
    {
        throw new NotImplementedException();
    }
#endif

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        int m;

        if (doy < 337 || !IsLeapYear(y))
        {
            int d0y = doy - 1;
            m = 1 + d0y / DaysPerMonth;
            d = 1 + d0y % DaysPerMonth;
        }
        else if (doy < 344)
        {
            m = 13;
            d = doy - 336;
        }
        else
        {
            m = 14;
            d = doy - 343;
        }

        return m;
    }
}

public partial class PaxSchema // Counting months and days since the epoch
{
#if !PROTOTYPING
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYearInMonths(int y)
    {
        throw new NotImplementedException();
    }
#endif

    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y)
    {
        // In a century we have 18 leap years, except if the century is a
        // multiple of 400, in which case we only have 17 leap years.
        y--;
        int C = y / 100;
        int Y = y % 100;
        return DaysPerCommonYear * y + 7 * (18 * C - (C >> 2) + Y / 6 + Y / 99);
    }
}
