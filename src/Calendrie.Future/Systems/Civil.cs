// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

// Only meant to demonstrate a few things that can be done with the year and
// month types.

internal static partial class Civil { }

internal partial class Civil // Kind of IDateProvider<CivilDate>
{
    /// <summary>
    /// Enumerates the days in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is not within the
    /// calendar boundaries.</exception>
    [Pure]
    public static IEnumerable<CivilDate> GetDaysInYear(int year) =>
        new CivilYear(year).EnumerateDays();

    /// <summary>
    /// Enumerates the days in the specified month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The month is not within
    /// the calendar boundaries.</exception>
    [Pure]
    public static IEnumerable<CivilDate> GetDaysInMonth(int year, int month) =>
        new CivilMonth(year, month).ToEnumerable();

    /// <summary>
    /// Obtains the date for the first supported day of the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is not within the
    /// calendar boundaries.</exception>
    [Pure]
    public static CivilDate GetStartOfYear(int year) => new CivilYear(year).MinDay;

    /// <summary>
    /// Obtains the date for the last supported day of the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is not within the
    /// calendar boundaries.</exception>
    [Pure]
    public static CivilDate GetEndOfYear(int year) => new CivilYear(year).MaxDay;

    /// <summary>
    /// Obtains the date for the first supported day of the specified month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The month is not within
    /// the calendar boundaries.</exception>
    [Pure]
    public static CivilDate GetStartOfMonth(int year, int month) => new CivilMonth(year, month).MinDay;

    /// <summary>
    /// Obtains the date for the last supported day of the specified month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The month is not within
    /// the calendar boundaries.</exception>
    [Pure]
    public static CivilDate GetEndOfMonth(int year, int month) => new CivilMonth(year, month).MaxDay;
}

internal partial class Civil // Extension methods
{
    /// <summary>
    /// Obtains the first day of the year to which belongs the specified date.
    /// </summary>
    [Pure]
    public static CivilDate GetStartOfYear(this CivilDate date) => CivilYear.FromDate(date).MinDay;

    /// <summary>
    /// Obtains the last day of the year to which belongs the specified date.
    /// </summary>
    [Pure]
    public static CivilDate GetEndOfYear(this CivilDate date) => CivilYear.FromDate(date).MaxDay;

    /// <summary>
    /// Obtains the first day of the month to which belongs the specified date.
    /// </summary>
    [Pure]
    public static CivilDate GetStartOfMonth(this CivilDate date) => CivilMonth.FromDate(date).MinDay;

    /// <summary>
    /// Obtains the last day of the month to which belongs the specified date.
    /// </summary>
    [Pure]
    public static CivilDate GetEndOfMonth(this CivilDate date) => CivilMonth.FromDate(date).MaxDay;
}
