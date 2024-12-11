// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

// TODO(code): (y, m, d) <-> (y, woy, dow). See also CalendarWeek. Maybe create CalendarWeekdate?

// References:
// https://en.wikipedia.org/wiki/Leap_week_calendar

/// <summary>
/// Defines a leap week schema.
/// <para>A leap week schema features a whole number of weeks every year.</para>
/// <para>Leap week calendars belong to the larger family of perennial calendars.
/// </para>
/// </summary>
public interface ILeapWeekSchema : ICalendricalSchema
{
    DayOfWeek FirstDayOfWeek { get; }

    /// <summary>
    /// Determines whether the specified week is intercalary or not.
    /// </summary>
    [Pure] bool IsIntercalaryWeek(int y, int woy);

    /// <summary>
    /// Obtains the number of weeks in the specified year.
    /// </summary>
    [Pure] int CountWeeksInYear(int y);

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the
    /// specified day of a week.
    /// </summary>
    [Pure] int CountDaysSinceEpoch(int y, int woy, DayOfWeek dow);

    void GetWeekdateParts(int daysSinceEpoch, out int y, out int woy, out DayOfWeek dow);
}
