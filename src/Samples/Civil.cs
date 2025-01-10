// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Collections.Generic;
using System.Linq;

using Calendrie.Systems;
using Calendrie.Core.Intervals;

using CalendrieRange = Calendrie.Core.Intervals.Range;

// Among other things, demonstrates a few things which can be done with the year
// and month types. Without them, these methods would have been good candidates
// for inclusion in the Civil calendar class.

/// <summary>
/// Provides static helpers and extension methods related to the Civil calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class Civil { }

// Year and month characteristics
//
public partial class Civil
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
public partial class Civil
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
public partial class Civil
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

    /// <summary>
    /// Interconverts the specified Civil date to a <see cref="GregorianDate"/> value.
    /// </summary>
    public static GregorianDate ToGregorianDate(this CivilDate date) =>
        GregorianDate.FromDayNumber(date.DayNumber);

    /// <summary>
    /// Interconverts the specified Civil date to a <see cref="GregorianDate"/> value.
    /// <para>There is an implicit conversion from <see cref="CivilDate"/> to
    /// <see cref="GregorianDate"/>.</para>
    /// <para>One can also use the more explicit method
    /// <see cref="CivilDate.ToGregorianDate()"/>.</para>
    /// </summary>
    public static GregorianDate AsGregorianDate(this CivilDate date) => date;

    /// <summary>
    /// Interconverts the specified Civil date to a <see cref="JulianDate"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static JulianDate ToJulianDate(this CivilDate date) =>
        JulianDate.FromDayNumber(date.DayNumber);

    /// <summary>
    /// Interconverts the specified Civil date to a <see cref="WorldDate"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static WorldDate ToWorldDate(this CivilDate date) =>
        WorldDate.FromDayNumber(date.DayNumber);

    //
    // Interconversion: other date types -> CivilDate
    //

    /// <summary>
    /// Interconverts the specified Gregorian date to a <see cref="CivilDate"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static CivilDate ToCivilDate(this GregorianDate date) =>
        CivilDate.FromDayNumber(date.DayNumber);

    /// <summary>
    /// Interconverts the specified Julian date to a <see cref="CivilDate"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static CivilDate ToCivilDate(this JulianDate date) =>
        CivilDate.FromDayNumber(date.DayNumber);

    /// <summary>
    /// Interconverts the specified World date to a <see cref="CivilDate"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static CivilDate ToCivilDate(this WorldDate date) =>
        CivilDate.FromDayNumber(date.DayNumber);
}

// CivilMonth extension methods
//
public partial class Civil
{
    /// <summary>
    /// Obtains the first day of the year to which belongs the specified date.
    /// </summary>
    public static CivilDate GetStartOfYear(this CivilMonth month) => CivilYear.FromMonth(month).MinDay;

    /// <summary>
    /// Obtains the last day of the year to which belongs the specified date.
    /// </summary>
    public static CivilDate GetEndOfYear(this CivilMonth month) => CivilYear.FromMonth(month).MaxDay;

    // NB: interconversion to JulianMonth is NOT possible.

    /// <summary>
    /// Converts the specified month to a range of <see cref="JulianDate"/>
    /// values (first version).
    /// </summary>
    public static Range<JulianDate> ToJulianRange(this CivilMonth month)
    {
        var (startOfMonth, endOfMonth) = month.ToRange().Endpoints;
        return CalendrieRange.Create(startOfMonth.ToJulianDate(), endOfMonth.ToJulianDate());
    }

    /// <summary>
    /// Converts the specified month to a range of <see cref="JulianDate"/>
    /// values (second version).
    /// </summary>
    public static Range<JulianDate> ToJulianRange2(this CivilMonth month) =>
        CalendrieRange.FromEndpoints(from x in month.ToRange().Endpoints select x.ToJulianDate());

    /// <summary>
    /// Converts the specified month to a range of <see cref="JulianDate"/>
    /// values (third version).
    /// </summary>
    public static Range<JulianDate> ToJulianRange3(this CivilMonth month) =>
        CalendrieRange.Create(month.MinDay.ToJulianDate(), month.MaxDay.ToJulianDate());

    /// <summary>
    /// Converts the specified month to an enumerable collection of
    /// <see cref="JulianDate"/> values.
    /// </summary>
    public static IEnumerable<JulianDate> ToJulianDates(this CivilMonth month) =>
        from date in month.ToEnumerable() select date.ToJulianDate();
}

// CivilYear extension methods
//
public partial class Civil
{
    // Notes:
    // - Interconversion to JulianYear is NOT possible.
    // - Conversion to a range or to an enumerable collection of JulianMonth
    //   is NOT possible.

    /// <summary>
    /// Converts the specified year to a range of <see cref="JulianDate"/> values.
    /// </summary>
    public static Range<JulianDate> ToJulianDayRange(this CivilYear year)
    {
        var (startOfYear, endOfYear) = year.ToDayRange().Endpoints;
        return CalendrieRange.Create(startOfYear.ToJulianDate(), endOfYear.ToJulianDate());
    }

    /// <summary>
    /// Converts the specified year to an enumerable collection of
    /// <see cref="JulianDate"/> values.
    /// </summary>
    public static IEnumerable<JulianDate> ToJulianDates(this CivilYear year) =>
        from date in year.EnumerateDays() select date.ToJulianDate();
}
