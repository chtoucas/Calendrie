﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

// In fact, the formulae should also work with year = 0 as an input parameter.
// Nevertheless, since daysSinceEpoch < 0 when year = 0, it's better to ignore
// that.

/// <summary>
/// Provides static formulae for the Gregorian schema (year > 0).
/// <para>See also <seealso cref="GregorianFormulae"/>.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class CivilFormulae
{
    /// <summary>
    /// Counts the number of consecutive days from the epoch to the specified
    /// date.
    /// </summary>
    [Pure]
    public static int CountDaysSinceEpoch(int y, int m, int d)
    {
        Debug.Assert(y > 0);

        if (m < 3)
        {
            y--;
            m += 9;
        }
        else
        {
            m -= 3;
        }

        int C = MathN.Divide(y, 100, out int Y);

        return -GJSchema.DaysPerYearAfterFebruary
            + (GregorianSchema.DaysPer400YearCycle * C >> 2)
            + (GregorianSchema.DaysPer4YearSubcycle * Y >> 2)
            + (int)((uint)(153 * m + 2) / 5) + d - 1;
    }

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the specified
    /// ordinal date.
    /// <para>Conversion year/dayOfYear -&gt; daysSinceEpoch.</para>
    /// </summary>
    [Pure]
    public static int CountDaysSinceEpoch(int y, int doy) => GetStartOfYear(y) + doy - 1;

    /// <summary>
    /// Obtains the date parts for the specified day count (the number of
    /// consecutive days from the epoch to a date); the results are given in
    /// output parameters.
    /// </summary>
    public static void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        Debug.Assert(daysSinceEpoch >= 0);

        daysSinceEpoch += GJSchema.DaysPerYearAfterFebruary;

        int C = (int)((uint)((daysSinceEpoch << 2) + 3) / GregorianSchema.DaysPer400YearCycle);
        int D = daysSinceEpoch - (GregorianSchema.DaysPer400YearCycle * C >> 2);

        int Y = (int)((uint)((D << 2) + 3) / GregorianSchema.DaysPer4YearSubcycle);
        int d0y = D - (GregorianSchema.DaysPer4YearSubcycle * Y >> 2);

        m = (int)((uint)(5 * d0y + 2) / 153);
        d = 1 + d0y - (int)((uint)(153 * m + 2) / 5);

        if (m > 9)
        {
            Y++;
            m -= 9;
        }
        else
        {
            m += 3;
        }

        y = 100 * C + Y;
    }

    /// <summary>
    /// Obtains the ordinal date parts for the specified day count (the number
    /// of consecutive days from the epoch to a date); the results are given in
    /// output parameters.
    /// </summary>
    [Pure]
    public static int GetYear(int daysSinceEpoch, out int doy)
    {
        int y = GetYear(daysSinceEpoch);
        doy = 1 + daysSinceEpoch - GetStartOfYear(y);
        return y;
    }

    /// <summary>
    /// Obtains the year from the specified day count (the number of consecutive
    /// days from the epoch to a date).
    /// </summary>
    [Pure]
    public static int GetYear(int daysSinceEpoch)
    {
        Debug.Assert(daysSinceEpoch >= 0);

        // Int64 to prevent overflows.
        int y = (int)(400L * (daysSinceEpoch + 2) / GregorianSchema.DaysPer400YearCycle);
        int c = y / 100;
        int startOfYearAfter = GJSchema.DaysPerCommonYear * y + (y >> 2) - c + (c >> 2);

        return daysSinceEpoch < startOfYearAfter ? y : y + 1;
    }

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the first day of
    /// the specified year.
    /// </summary>
    [Pure]
    public static int GetStartOfYear(int y)
    {
        Debug.Assert(y > 0);

        y--;
        int c = y / 100;
        return GJSchema.DaysPerCommonYear * y + (y >> 2) - c + (c >> 2);
    }
}
