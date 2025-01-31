// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Collections.Generic;
using System.Linq;

using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Systems;

using CalendrieRange = Calendrie.Core.Intervals.Range;

// Among other things, demonstrates a few things which can be done with the year
// and month types. Without them, these methods would have been good candidates
// for inclusion in the Civil calendar class.

/// <summary>
/// Provides static helpers and extension methods related to the Civil calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class CivilHelpers { }

// Year and month characteristics
//
public partial class CivilHelpers
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
public partial class CivilHelpers
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
public partial class CivilHelpers
{
    /// <summary>
    /// Obtains the first day of the year to which belongs the specified date.
    /// </summary>
    public static CivilDate GetStartOfYear(this CivilDate date) => new CivilYear(date).MinDay;

    /// <summary>
    /// Obtains the last day of the year to which belongs the specified date.
    /// </summary>
    public static CivilDate GetEndOfYear(this CivilDate date) => new CivilYear(date).MaxDay;

    /// <summary>
    /// Obtains the first day of the month to which belongs the specified date.
    /// </summary>
    public static CivilDate GetStartOfMonth(this CivilDate date) => new CivilMonth(date).MinDay;

    /// <summary>
    /// Obtains the last day of the month to which belongs the specified date.
    /// </summary>
    public static CivilDate GetEndOfMonth(this CivilDate date) => new CivilMonth(date).MaxDay;
}

// CivilMonth extension methods
//
public partial class CivilHelpers
{
    /// <summary>
    /// Obtains the first day of the year to which belongs the specified date.
    /// </summary>
    public static CivilDate GetStartOfYear(this CivilMonth month) => new CivilYear(month).MinDay;

    /// <summary>
    /// Obtains the last day of the year to which belongs the specified date.
    /// </summary>
    public static CivilDate GetEndOfYear(this CivilMonth month) => new CivilYear(month).MaxDay;
}

// CivilDate interconversion
//
public partial class CivilHelpers
{
    // Conversion of a CivilDate value to JulianDate value is already available
    // via ToJulianDate().

    /// <summary>
    /// Converts the specified Julian date to a <see cref="CivilDate"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static CivilDate FromJulianDate(this JulianDate date) => CivilDate.FromAbsoluteDate(date);

    public static Segment<JulianDate> ToJulianRange(this Segment<CivilDate> range)
    {
        var (min, max) = range.Endpoints;
        return CalendrieRange.Create(min.ToJulianDate(), max.ToJulianDate());
    }

    // General interconversion methods are possible but not very user-friendly.

    /// <summary>
    /// Converts the specified Civil date to a <typeparamref name="TDate"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static TDate ToAbsoluteDate<TDate>(this CivilDate date)
        where TDate : IAbsoluteDate<TDate>
    {
        return TDate.FromDayNumber(date.DayNumber);
    }

    public static Segment<TDate> ToAbsoluteDateRange<TDate>(this Segment<CivilDate> range)
        where TDate : struct, IAbsoluteDate<TDate>
    {
        var (min, max) = range.Endpoints;
        return CalendrieRange.Create(min.ToAbsoluteDate<TDate>(), max.ToAbsoluteDate<TDate>());
    }
}

// CivilMonth interconversion
//
public partial class CivilHelpers
{
    // NB: interconversion to XXXMonth is NOT possible.

    /// <summary>
    /// Converts the specified month to a range of <see cref="JulianDate"/>
    /// values (first version).
    /// </summary>
    public static Segment<JulianDate> ToJulianRange(this CivilMonth month)
    {
        var (startOfMonth, endOfMonth) = month.ToRange().Endpoints;
        return CalendrieRange.Create(startOfMonth.ToJulianDate(), endOfMonth.ToJulianDate());
    }

    /// <summary>
    /// Converts the specified month to a range of <see cref="JulianDate"/>
    /// values (second version).
    /// </summary>
    public static Segment<JulianDate> ToJulianRange2(this CivilMonth month) =>
        CalendrieRange.FromEndpoints(from x in month.ToRange().Endpoints select x.ToJulianDate());

    /// <summary>
    /// Converts the specified month to a range of <see cref="JulianDate"/>
    /// values (third version).
    /// </summary>
    public static Segment<JulianDate> ToJulianRange3(this CivilMonth month) =>
        CalendrieRange.Create(month.MinDay.ToJulianDate(), month.MaxDay.ToJulianDate());

    /// <summary>
    /// Converts the specified month to an enumerable collection of
    /// <see cref="JulianDate"/> values.
    /// </summary>
    public static IEnumerable<JulianDate> ToJulianDates(this CivilMonth month) =>
        from date in month.ToEnumerable() select date.ToJulianDate();
}

// CivilYear interconversion
//
public partial class CivilHelpers
{
    // Notes:
    // - Interconversion to XXXYear is NOT possible.
    // - Conversion to a range or to an enumerable collection of XXXMonth
    //   is NOT possible.

    /// <summary>
    /// Converts the specified year to a range of <see cref="JulianDate"/> values.
    /// </summary>
    public static Segment<JulianDate> ToJulianDayRange(this CivilYear year)
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
