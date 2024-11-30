// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

/// <summary>
/// Defines an extended calendrical schema.
/// </summary>
public interface ICalendricalSchemaPlus : ICalendricalSchema
{
    /// <summary>
    /// Obtains the number of whole days remaining after the specified month and
    /// until the end of the year.
    /// </summary>
    [Pure] int CountDaysInYearAfterMonth(int y, int m);

    #region CountDaysInYearBefore()

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the year and
    /// before the specified day.
    /// <para>The result should match the value of <c>(DayOfYear - 1)</c>.</para>
    /// </summary>
    [Pure] int CountDaysInYearBefore(int y, int m, int d);

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the year and
    /// before the specified day.
    /// <para>Trivial (<c>= <paramref name="doy"/> - 1</c>), only added for
    /// completeness.</para>
    /// </summary>
    [Pure] int CountDaysInYearBefore(int y, int doy);

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the year and
    /// before the specified day.
    /// <para>The result should match the value of <c>(DayOfYear - 1)</c>.</para>
    /// </summary>
    [Pure] int CountDaysInYearBefore(int daysSinceEpoch);

    #endregion
    #region CountDaysInYearAfter()

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the year.
    /// </summary>
    [Pure] int CountDaysInYearAfter(int y, int m, int d);

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the year.
    /// </summary>
    [Pure] int CountDaysInYearAfter(int y, int doy);

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the year.
    /// </summary>
    [Pure] int CountDaysInYearAfter(int daysSinceEpoch);

    #endregion
    #region CountDaysInMonthBefore()

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the month
    /// and before the specified day.
    /// <para>Trivial (<c>= <paramref name="d"/> - 1</c>), only added for
    /// completeness.</para>
    /// </summary>
    [Pure] int CountDaysInMonthBefore(int y, int m, int d);

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the month
    /// and before the specified day.
    /// <para>The result should match the value of <c>(Day - 1)</c>.</para>
    /// </summary>
    [Pure] int CountDaysInMonthBefore(int y, int doy);

    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the month
    /// and before the specified day.
    /// <para>The result should match the value of <c>(Day - 1)</c>.</para>
    /// </summary>
    [Pure] int CountDaysInMonthBefore(int daysSinceEpoch);

    #endregion
    #region CountDaysInMonthAfter()

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the month.
    /// </summary>
    [Pure] int CountDaysInMonthAfter(int y, int m, int d);

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the month.
    /// </summary>
    [Pure] int CountDaysInMonthAfter(int y, int doy);

    /// <summary>
    /// Obtains the number of whole days remaining after the specified date and
    /// until the end of the month.
    /// </summary>
    [Pure] int CountDaysInMonthAfter(int daysSinceEpoch);

    #endregion
}
