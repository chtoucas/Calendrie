// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

internal static class JulianFormulae2
{
    /// <summary>
    /// Determines whether the specified year is leap or not.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(long y) => (y & 3) == 0;

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

    /// <summary>Counts the number of consecutive days from the epoch to the first day of the
    /// specified year.</summary>
    [Pure]
    public static long GetStartOfYear(long y)
    {
        y--;
        return GJSchema.DaysInCommonYear * y + (y >> 2);
    }
}
