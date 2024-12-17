// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

internal static partial class GregorianFormulae2
{
    /// <summary>
    /// Obtains the number of days before the start of the specified month.
    /// </summary>
    [Pure]
    public static int CountDaysInYearBeforeMonth(int y, int m) =>
        m < 3 ? 31 * (m - 1)
        : IsLeapYear(y) ? (int)((uint)(153 * m - 157) / 5)
        : (int)((uint)(153 * m - 162) / 5);

    /// <summary>
    /// Obtains the date parts for the specified day count (the number of
    /// consecutive days from the epoch to a date).
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Yemoda GetDateParts(int daysSinceEpoch)
    {
        GregorianFormulae.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return new Yemoda(y, m, d);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the specified day count (the number
    /// of consecutive days from the epoch to a date).
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Yedoy GetOrdinalParts(int daysSinceEpoch)
    {
        int y = GregorianFormulae.GetYear(daysSinceEpoch, out int doy);
        return new Yedoy(y, doy);
    }

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the last day of
    /// the specified year.
    /// </summary>
    [Pure]
    public static int GetEndOfYear(int y) =>
        GregorianFormulae.GetStartOfYear(y) + GregorianFormulae.CountDaysInYear(y) - 1;
}
