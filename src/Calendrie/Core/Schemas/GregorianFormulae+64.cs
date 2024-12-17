// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

internal partial class GregorianFormulae // 64-bit versions
{
    /// <summary>
    /// Counts the number of consecutive days from the epoch to the specified
    /// date.
    /// </summary>
    [Pure]
    public static long CountDaysSinceEpoch(long y, int m, int d)
    {
        if (m < 3)
        {
            y--;
            m += 9;
        }
        else
        {
            m -= 3;
        }

        long C = MathZ.Divide(y, 100L, out long Y);

        return -GJSchema.DaysInYearAfterFebruary
            + (GregorianSchema.DaysPer400YearCycle * C >> 2)
            + (GregorianSchema.DaysPer4YearSubcycle * Y >> 2)
            + (int)((uint)(153 * m + 2) / 5 + d) - 1;
    }

    /// <summary>
    /// Obtains the date parts for the specified day count (the number of
    /// consecutive days from the epoch to a date); the results are given in
    /// output parameters.
    /// </summary>
    public static void GetDateParts(long daysSinceEpoch, out long y, out int m, out int d)
    {
        daysSinceEpoch += GJSchema.DaysInYearAfterFebruary;

        long C = MathZ.Divide((daysSinceEpoch << 2) + 3, GregorianSchema.DaysPer400YearCycle);
        long D = daysSinceEpoch - (GregorianSchema.DaysPer400YearCycle * C >> 2);

        long Y = ((D << 2) + 3) / GregorianSchema.DaysPer4YearSubcycle;
        int d0y = (int)(D - (GregorianSchema.DaysPer4YearSubcycle * Y >> 2));

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
    /// Obtains the year from the specified day count (the number of consecutive
    /// days from the epoch to a date).
    /// </summary>
    [Pure]
    public static long GetYear(long daysSinceEpoch)
    {
        long y = MathZ.Divide(400 * (daysSinceEpoch + 2), GregorianSchema.DaysPer400YearCycle);
        long c = MathZ.Divide(y, 100);
        long startOfYearAfter = GJSchema.DaysInCommonYear * y + (y >> 2) - c + (c >> 2);

        return daysSinceEpoch < startOfYearAfter ? y : y + 1;
    }
}
