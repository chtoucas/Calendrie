﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Intervals;

#region Developer Notes

// Types Implementing ICalendricalSchema
// -------------------------------------
//
// ### Public Hierarchy
//
// ICalendricalCore
// └─ ICalendricalSchema
//    ├─ OffsettedSchema                            DRAFT
//    ├─ StrictSchema                               DRAFT
//    └─ ICalendricalSchemaPlus
//       └─ CalendricalSchema [A]
//          └─ LimitSchema [A]
//             ├─ HebrewSchema                      DRAFT
//             ├─ InternationalFixedSchema
//             ├─ Persian2820Schema
//             ├─ PositivistSchema
//             ├─ TabularIslamicSchema
//             ├─ WorldSchema
//             ├─ EgyptianSchema [A]
//             │  ├─ Egyptian12Schema
//             │  └─ Egyptian13Schema
//             ├─ GJSchema [A]
//             │  ├─ GregorianSchema
//             │  └─ JulianSchema
//             ├─ LeapWeekSchema [A]                DRAFT
//             │  └─ PaxSchema                      DRAFT
//             ├─ PtolemaicSchema [A]
//             │  ├─ CopticSchema [A]
//             │  │  ├─ Coptic12Schema
//             │  │  └─ Coptic13Schema
//             │  └─ FrenchRepublicanSchema [A]
//             │     ├─ FrenchRepublican12Schema
//             │     └─ FrenchRepublican13Schema
//             └─ TropicalistaSchema [A]
//                ├─ TropicaliaSchema
//                ├─ Tropicalia3031Schema
//                └─ Tropicalia3130Schema
//
// Annotation: [A] = abstract
//
// Comments
// --------
//
// Included in LimitSchema but not in ICalendricalSchema:
// - methods using Yemoda or Yedoy.
// - MinDaysInYear
// - MinDaysInMonth
//
// Everything related to ICalendricalArithmetic should be excluded too if we
// wish to completely ignore Yemoda and Yedoy dependent methods, but we
// chose not to. First, as for ICalendricalValidator there are good reasons
// to externalize them; see the comments in ICalendricalArithmetic. Next,
// everything would work perfectly without Yemoda and Yedoy, but the
// signature of the methods would be way too complex. Finally, I think it's
// worth keeping Arithmetic even if the resulting object only works with
// input within the Yemoda/Yedoy ranges.
// Notice that this is only a problem for schemas with support for an
// exceptionally large range of years, which is not the case of schemas
// extending CalendricalSchema.
//
// There are methods that should work even if year is outside the interval
// [MinYear, MaxYear]:
// - IsLeapYear(y)
// - IsIntercalaryMonth(y, m)
// - IsIntercalaryDay(y, m, d)
// - IsSupplementaryDay(y, m, d)
// - CountMonthsInYear(y)
// - CountDaysInYear(y)
// - CountDaysInMonth(y, m)
// The last three are those necessary to build a ICalendricalValidator
// (we also require a prop MinDaysInYear, but formally it is not mandatory).
// Unfortunately this is not always possible, but we shall make sure that
// all methods work for years within [Yemoda.MinYear, Yemoda.MaxYear].
// The first two are included only as dependencies for the methods for
// counting. Notice that we could have ignored the next two.
// Regarding the month and day parameters, we only require that they do not
// overflow if they are within the ranges defined by Yemoda.

#endregion

/// <summary>
/// Defines a calendrical schema.
/// <para>A calendrical schema is a "pure" calendar:</para>
/// <list type="bullet">
/// <item>It has only one era. Dates are either in the main era or in the "era"
/// before.</item>
/// <item>The epoch, the origin of the main era, is the date labelled 1/1/1.
/// </item>
/// <item>Concrete implementations MUST ensure that all methods won't overflow
/// with years within the range defined hereby.</item>
/// <item>A method does NOT check whether a parameter, like "year" or
/// "daysSinceEpoch", is in the range defined hereby or not.</item>
/// <item>A schema is <i>lenient</i>, its methods assume that their parameters
/// are valid from a calendrical point of view, nevertheless they MUST ensure
/// that all returned values are valid when the previous condition is met.</item>
/// </list>
/// </summary>
public partial interface ICalendricalSchema : ICalendricalCore
{
    /// <summary>
    /// Gets the minimal total number of days there is at least in a year.
    /// </summary>
    int MinDaysInYear { get; }

    /// <summary>
    /// Gets the minimal total number of days there is at least in a month.
    /// </summary>
    int MinDaysInMonth { get; }

    /// <summary>
    /// Gets the range of supported days, that is the range of supported numbers
    /// of consecutive days from the epoch for which the methods are known not
    /// to overflow.
    /// </summary>
    /// <returns>The range from the first day of the first supported year to the
    /// last day of the last supported year.</returns>
    Range<int> SupportedDays { get; }

    /// <summary>
    /// Gets the range of supported months, that is the range of supported
    /// numbers of consecutive months from the epoch for which the methods are
    /// known not to overflow.
    /// </summary>
    /// <returns>The range from the first month of the first supported year to
    /// the last month of the last supported year.</returns>
    Range<int> SupportedMonths { get; }

    /// <summary>
    /// Gets the range of years for which the methods are known not to overflow.
    /// </summary>
    Range<int> SupportedYears { get; }

    /// <summary>
    /// Gets the pre-validator for this schema.
    /// </summary>
    ICalendricalPreValidator PreValidator { get; }
}

public partial interface ICalendricalSchema // Counting months and days within a year or a month
{
    /// <summary>
    /// Obtains the number of whole days elapsed since the start of the year and
    /// before the specified month.
    /// </summary>
    [Pure] int CountDaysInYearBeforeMonth(int y, int m);
}

public partial interface ICalendricalSchema // Conversions
{
    /// <summary>
    /// Counts the number of consecutive months from the epoch to the specified
    /// month.
    /// <para>Conversion year/month -&gt; monthsSinceEpoch.</para>
    /// </summary>
    [Pure] int CountMonthsSinceEpoch(int y, int m);

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the specified
    /// date.
    /// <para>Conversion year/month/day -&gt; daysSinceEpoch.</para>
    /// </summary>
    [Pure] int CountDaysSinceEpoch(int y, int m, int d);

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the specified
    /// ordinal date.
    /// <para>Conversion year/dayOfYear -&gt; daysSinceEpoch.</para>
    /// </summary>
    [Pure] int CountDaysSinceEpoch(int y, int doy);

    /// <summary>
    /// Obtains the month parts for the specified month count (the number of
    /// consecutive months from the epoch to a month); the month parts are given
    /// in output parameters.
    /// <para>Conversion <paramref name="monthsSinceEpoch"/> -&gt; year/month.
    /// </para>
    /// </summary>
    void GetMonthParts(int monthsSinceEpoch, out int y, out int m);

    /// <summary>
    /// Obtains the date parts for the specified day count (the number of
    /// consecutive days from the epoch to a date); the date parts are given in
    /// output parameters.
    /// <para>Conversion <paramref name="daysSinceEpoch"/> -&gt; year/month/day.
    /// </para>
    /// </summary>
    void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d);

    // Initially GetYear() was defined as
    // > void GetOrdinalParts(int daysSinceEpoch, out int y, out int doy);
    // but it made ICalendricalSchemaPlus not CLS compliant because of
    // > Yedoy GetOrdinalParts(int y, int m, int d);

    /// <summary>
    /// Obtains the ordinal date parts for the specified day count (the number
    /// of consecutive days from the epoch to a date); the day of the year is
    /// given in an output parameter.
    /// <para>Conversion <paramref name="daysSinceEpoch"/> -&gt; year/dayOfYear.
    /// </para>
    /// </summary>
    [Pure] int GetYear(int daysSinceEpoch, out int doy);

    /// <summary>
    /// Obtains the year from the specified day count (the number of consecutive
    /// days from the epoch to a date).
    /// </summary>
    [Pure] int GetYear(int daysSinceEpoch);

    /// <summary>
    /// Obtains the month and day of the month for the specified ordinal date;
    /// the day of the month is given in an output parameter.
    /// <para>Conversion year/dayOfYear -&gt; year/month/day.</para>
    /// </summary>
    [Pure] int GetMonth(int y, int doy, out int d);

    /// <summary>
    /// Obtains the day of the year for the specified date.
    /// <para>Conversion year/month/day -&gt; year/dayOfYear.</para>
    /// </summary>
    [Pure] int GetDayOfYear(int y, int m, int d);
}

public partial interface ICalendricalSchema // Counting months and days since the epoch
{
    // "Fast" versions of CountMonthsSinceEpoch() and CountDaysSinceEpoch().

    /// <summary>
    /// Counts the number of consecutive months from the epoch to the first month
    /// of the specified year.
    /// </summary>
    [Pure] int GetStartOfYearInMonths(int y);

    /// <summary>
    /// Counts the number of consecutive months from the epoch to the last month
    /// of the specified year.
    /// </summary>
    [Pure] int GetEndOfYearInMonths(int y);

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the first day of
    /// the specified year.
    /// </summary>
    [Pure] int GetStartOfYear(int y);

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the last day of
    /// the specified year.
    /// </summary>
    [Pure] int GetEndOfYear(int y);

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the first day of
    /// the specified month.
    /// </summary>
    [Pure] int GetStartOfMonth(int y, int m);

    /// <summary>
    /// Counts the number of consecutive days from the epoch to the last day of
    /// the specified month.
    /// </summary>
    [Pure] int GetEndOfMonth(int y, int m);
}
