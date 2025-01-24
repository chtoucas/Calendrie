// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

// TODO(code): XML doc.
// Add more tests in CivilTests and GregorianTests.
// Add a warning about the data (CountYearsBetweenData & co) which only
// offer symmetrical results; see DefaultDateMathFacts and DefaultMonthMathFacts.

// Operations on "Yemoda" (and "Yedoy")
//
// The standard ops, those based on the day unit:
// - AddDays(Yemoda, days)
// - NextDay(Yemoda)
// - PreviousDay(Yemoda)
// - CountDaysBetween(Yemoda, Yemoda)
// The non-standard ops, those using the year or month units:
// - AddYears(Yemoda, years)
// - AddYears(Yemoda, years, out roundoff)
// - AddMonths(Yemoda, months)
// - AddMonths(Yemoda, months, out roundoff)

// Operations on "Yemo"
//
// The standard ops, those based on the month unit:
// - AddMonths(Yemo, months)
// - NextMonth(Yemo)
// - PreviousMonth(Yemo)
// - CountMonthsBetween(Yemo, Yemo)
// The non-standard ops:
// - AddYears(Yemo, years)
// - AddYears(Yemo, years, out roundoff)

public partial struct TropicaliaDate
{
    /// <summary>
    /// Calculates the exact difference (expressed in years, months, weeks and
    /// days) between the two specified dates.
    /// </summary>
    [Pure]
    public static DateDifference Subtract(TropicaliaDate start, TropicaliaDate end)
    {
        int years = end.CountYearsSince(start);
        var newStart = start.PlusYears(years);
        int months = end.CountMonthsSince(newStart);
        newStart = newStart.PlusMonths(months);
        int days = end.CountDaysSince(newStart);
        int weeks = MathZ.Divide(days, DaysInWeek, out days);
        return new DateDifference(years, months, weeks, days);
    }
}

public partial struct TropicaliaMonth
{
    /// <summary>
    /// Calculates the exact difference (expressed in years and months) between
    /// the two specified dates.
    /// </summary>
    [Pure]
    public static MonthDifference Subtract(TropicaliaMonth start, TropicaliaMonth end)
    {
        int totalMonths = end.CountMonthsSince(start);
        int years = MathZ.Divide(totalMonths, TropicalistaSchema.MonthsPerYear, out int months);
        return new MonthDifference(years, months);
    }
}
