// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

/// <summary>
/// Provides static formulae for the Julian schema (32-bit and 64-bit versions).
/// <para>See also <seealso cref="JulianSchema"/>.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static partial class JulianFormulae
{
    /// <summary>
    /// Determines whether the specified date is an intercalary day or not.
    /// </summary>
    [Pure]
    public static bool IsIntercalaryDay(int m, int d) => m == 2 && d == 29;

    /// <summary>
    /// Determines whether the specified year is leap or not.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(int y) => (y & 3) == 0;

    /// <summary>
    /// Obtains the number of days in the specified year.
    /// </summary>
    [Pure]
    public static int CountDaysInYear(int y) =>
        IsLeapYear(y) ? GJSchema.DaysInLeapYear : GJSchema.DaysInCommonYear;

    /// <summary>
    /// Obtains the number of days in the specified month.
    /// </summary>
    [Pure]
    public static int CountDaysInMonth(int y, int m) =>
        m != 2 ? 30 + ((m + (m >> 3)) & 1)
        : IsLeapYear(y) ? 29
        : 28;

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the specified
    /// date.
    /// </summary>
    [Pure]
    public static int CountDaysSinceEpoch(int y, int m, int d)
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

        return -GJSchema.DaysInYearAfterFebruary
            + (JulianSchema.DaysPer4YearCycle * y >> 2)
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
        daysSinceEpoch += GJSchema.DaysInYearAfterFebruary;

        y = MathZ.Divide((daysSinceEpoch << 2) + 3, JulianSchema.DaysPer4YearCycle);
        int d0y = daysSinceEpoch - (JulianSchema.DaysPer4YearCycle * y >> 2);

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
    public static int GetYear(int daysSinceEpoch) =>
        MathZ.Divide((daysSinceEpoch << 2) + 1464, JulianSchema.DaysPer4YearCycle);

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the first day of
    /// the specified year.
    /// </summary>
    [Pure]
    public static int GetStartOfYear(int y)
    {
        y--;
        return GJSchema.DaysInCommonYear * y + (y >> 2);
    }
}
