// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;

internal partial class GregorianFormulae2 // 64-bit versions
{
    /// <summary>
    /// Determines whether the specified year is leap or not.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(long y) =>
        (y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);

    /// <summary>
    /// Obtains the number of days in the specified year.
    /// </summary>
    [Pure]
    public static int CountDaysInYear(long y) =>
        IsLeapYear(y) ? GJSchema.DaysInLeapYear : GJSchema.DaysInCommonYear;

    /// <summary>
    /// Obtains the number of days in the specified month.
    /// </summary>
    [Pure]
    public static int CountDaysInMonth(long y, int m) =>
        m != 2 ? 30 + ((m + (m >> 3)) & 1)
        : IsLeapYear(y) ? 29
        : 28;

    /// <summary>
    /// Obtains the number of days before the start of the specified month.
    /// </summary>
    [Pure]
    public static int CountDaysInYearBeforeMonth(long y, int m) =>
        m < 3 ? 31 * (m - 1)
        : IsLeapYear(y) ? (int)((uint)(153 * m - 157) / 5)
        : (int)((uint)(153 * m - 162) / 5);

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the first day of
    /// the specified year.
    /// </summary>
    [Pure]
    public static long GetStartOfYear(long y)
    {
        y--;
        long c = MathZ.Divide(y, 100);
        return GJSchema.DaysInCommonYear * y + (y >> 2) - c + (c >> 2);
    }
}
