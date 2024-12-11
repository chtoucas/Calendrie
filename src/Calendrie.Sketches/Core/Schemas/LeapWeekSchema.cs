﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Intervals;

// TODO(api): should be an interface.
// (y, m, d) <-> (y, woy, dow). See also CalendarWeek. Maybe create CalendarWeekdate?

// References:
// https://en.wikipedia.org/wiki/Leap_week_calendar

/// <summary>
/// Defines a leap week schema.
/// <para>A leap week schema features a whole number of weeks every year.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.
/// </para>
/// <para>Leap week calendars belong to the larger family of perennial calendars.
/// </para>
/// </summary>
public abstract class LeapWeekSchema : SystemSchema
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="LeapWeekSchema"/> class.
    /// </summary>
    private protected LeapWeekSchema(
        Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }

    public abstract DayOfWeek FirstDayOfWeek { get; }

    /// <summary>
    /// Determines whether the specified week is intercalary or not.
    /// </summary>
    [Pure] public abstract bool IsIntercalaryWeek(int y, int woy);

    /// <summary>
    /// Obtains the number of weeks in the specified year.
    /// </summary>
    [Pure] public abstract int CountWeeksInYear(int y);

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the
    /// specified day of a week.
    /// </summary>
    [Pure] public abstract int CountDaysSinceEpoch(int y, int woy, DayOfWeek dow);

    public abstract void GetWeekdateParts(
        int daysSinceEpoch, out int y, out int woy, out DayOfWeek dow);
}
