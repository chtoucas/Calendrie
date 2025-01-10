// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Collections.Generic;
using System.Linq;

using Calendrie.Systems;
using Calendrie.Core.Intervals;

using CalendrieRange = Calendrie.Core.Intervals.Range;

// Demonstrates a few things which can be done with the year and month types.
// Without them, these methods would be good candidates for inclusion in a
// calendar class.

/// <summary>
/// Provides static helpers and extension methods related to the Civil calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static partial class Civil { }

// Year and month characteristics
//
internal partial class Civil
{
    /// <summary>
    /// Determines whether the specified year is leap or not.
    /// <para>A leap year is a year with at least one intercalary day, week or
    /// month.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of supported years.</exception>
    public static bool IsLeapYear(int year) => new CivilYear(year).IsLeap;

    /// <summary>
    /// Obtains the number of days in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    public static int CountDaysInYear(int year) => new CivilYear(year).CountDays();

    /// <summary>
    /// Obtains the number of days in the specified month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The month is either invalid
    /// or outside the range of supported months.</exception>
    public static int CountDaysInMonth(int year, int month) => new CivilMonth(year, month).CountDays();
}

// Kind of IDateProvider<CivilDate>
//
internal partial class Civil
{
    /// <summary>
    /// Enumerates the days in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is not within the
    /// calendar boundaries.</exception>
    public static IEnumerable<CivilDate> GetDaysInYear(int year) =>
        new CivilYear(year).EnumerateDays();

    /// <summary>
    /// Enumerates the days in the specified month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The month is not within
    /// the calendar boundaries.</exception>
    public static IEnumerable<CivilDate> GetDaysInMonth(int year, int month) =>
        new CivilMonth(year, month).ToEnumerable();

    /// <summary>
    /// Obtains the date for the first supported day of the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is not within the
    /// calendar boundaries.</exception>
    public static CivilDate GetStartOfYear(int year) => new CivilYear(year).MinDay;

    /// <summary>
    /// Obtains the date for the last supported day of the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is not within the
    /// calendar boundaries.</exception>
    public static CivilDate GetEndOfYear(int year) => new CivilYear(year).MaxDay;

    /// <summary>
    /// Obtains the date for the first supported day of the specified month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The month is not within
    /// the calendar boundaries.</exception>
    public static CivilDate GetStartOfMonth(int year, int month) => new CivilMonth(year, month).MinDay;

    /// <summary>
    /// Obtains the date for the last supported day of the specified month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The month is not within
    /// the calendar boundaries.</exception>
    public static CivilDate GetEndOfMonth(int year, int month) => new CivilMonth(year, month).MaxDay;
}

// CivilDate extension methods
//
internal partial class Civil
{
    /// <summary>
    /// Obtains the first day of the year to which belongs the specified date.
    /// </summary>
    public static CivilDate GetStartOfYear(this CivilDate date) => CivilYear.FromDate(date).MinDay;

    /// <summary>
    /// Obtains the last day of the year to which belongs the specified date.
    /// </summary>
    public static CivilDate GetEndOfYear(this CivilDate date) => CivilYear.FromDate(date).MaxDay;

    /// <summary>
    /// Obtains the first day of the month to which belongs the specified date.
    /// </summary>
    public static CivilDate GetStartOfMonth(this CivilDate date) => CivilMonth.FromDate(date).MinDay;

    /// <summary>
    /// Obtains the last day of the month to which belongs the specified date.
    /// </summary>
    public static CivilDate GetEndOfMonth(this CivilDate date) => CivilMonth.FromDate(date).MaxDay;

    //
    // Interconversion: CivilDate -> other date types
    //

    // May throw an ArgumentOutOfRangeException.
    public static GregorianDate ToGregorianDate(this CivilDate date) =>
        GregorianDate.FromDayNumber(date.DayNumber);

    // Simpler, faster, no exceptions: there is an implicit conversion from
    // CivilDate to GregorianDate, or if you prefer you can use the more explicit
    // version: GregorianDate.FromCivilDate(date).
    public static GregorianDate AsGregorianDate(this CivilDate date) => date;

    // May throw an ArgumentOutOfRangeException.
    public static JulianDate ToJulianDate(this CivilDate date) =>
        JulianDate.FromDayNumber(date.DayNumber);

    // May throw an ArgumentOutOfRangeException.
    public static WorldDate ToWorldDate(this CivilDate date) =>
        WorldDate.FromDayNumber(date.DayNumber);

    //
    // Interconversion: Other date types -> CivilDate
    //

    // May throw an ArgumentOutOfRangeException.
    public static CivilDate FromGregorianDate(GregorianDate date) =>
        CivilDate.FromDayNumber(date.DayNumber);

    // May throw an ArgumentOutOfRangeException.
    public static CivilDate FromJulianDate(JulianDate date) =>
        CivilDate.FromDayNumber(date.DayNumber);

    // May throw an ArgumentOutOfRangeException.
    public static CivilDate FromWorldDate(WorldDate date) =>
        CivilDate.FromDayNumber(date.DayNumber);
}

// CivilMonth extension methods
//
internal partial class Civil
{
    /// <summary>
    /// Obtains the first day of the year to which belongs the specified date.
    /// </summary>
    public static CivilDate GetStartOfYear(this CivilMonth month) => CivilYear.FromMonth(month).MinDay;

    /// <summary>
    /// Obtains the last day of the year to which belongs the specified date.
    /// </summary>
    public static CivilDate GetEndOfYear(this CivilMonth month) => CivilYear.FromMonth(month).MaxDay;

    /// <summary>
    /// Interconverts the specified month to a range of <see cref="JulianDate"/>
    /// values (first version).
    /// </summary>
    public static Range<JulianDate> ToJulianRange(this CivilMonth month)
    {
        var (startOfMonth, endOfMonth) = month.ToRange().Endpoints;
        return CalendrieRange.Create(startOfMonth.ToJulianDate(), endOfMonth.ToJulianDate());
    }

    /// <summary>
    /// Interconverts the specified month to a range of <see cref="JulianDate"/>
    /// values (second version).
    /// </summary>
    public static Range<JulianDate> ToJulianRange2(this CivilMonth month) =>
        CalendrieRange.FromEndpoints(from x in month.ToRange().Endpoints select x.ToJulianDate());

    /// <summary>
    /// Interconverts the specified month to a range of <see cref="JulianDate"/>
    /// values (third version).
    /// </summary>
    public static Range<JulianDate> ToJulianRange3(this CivilMonth month) =>
        CalendrieRange.Create(month.MinDay.ToJulianDate(), month.MaxDay.ToJulianDate());

    /// <summary>
    /// Interconverts the specified month to an enumerable collection of
    /// <see cref="JulianDate"/> values.
    /// </summary>
    public static IEnumerable<JulianDate> ToJulianDates(this CivilMonth month) =>
        from date in month.ToEnumerable() select date.ToJulianDate();
}
