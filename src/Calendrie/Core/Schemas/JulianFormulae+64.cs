// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

internal partial class JulianFormulae // 64-bit versions
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

        return -GJSchema.DaysPerYearAfterFebruary
            + (JulianSchema.DaysPer4YearCycle * y >> 2)
            + (int)((uint)(153 * m + 2) / 5) + d - 1;
    }

    /// <summary>
    /// Obtains the date parts for the specified day count (the number of
    /// consecutive days from the epoch to a date); the results are given in
    /// output parameters.
    /// </summary>
    public static void GetDateParts(long daysSinceEpoch, out long y, out int m, out int d)
    {
        daysSinceEpoch += GJSchema.DaysPerYearAfterFebruary;

        y = MathZ.Divide((daysSinceEpoch << 2) + 3, JulianSchema.DaysPer4YearCycle);
        int d0y = (int)(daysSinceEpoch - (JulianSchema.DaysPer4YearCycle * y >> 2));

        m = (int)((uint)(5 * d0y + 2) / 153);
        d = 1 + d0y - (int)((uint)(153 * m + 2) / 5);

        if (m > 9)
        {
            y++;
            m -= 9;
        }
        else
        {
            m += 3;
        }
    }

    /// <summary>
    /// Obtains the year from the specified day count (the number of consecutive
    /// days from the epoch to a date).
    /// </summary>
    [Pure]
    public static long GetYear(long daysSinceEpoch) =>
        MathZ.Divide((daysSinceEpoch << 2) + 1464, JulianSchema.DaysPer4YearCycle);
}
