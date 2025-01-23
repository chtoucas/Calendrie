// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

// TODO(code): XML doc.
// Add more tests in CivilTests and GregorianTests.
// Add a warning about the data (CountYearsBetweenData & co) which only
// offer symmetrical results; see DefaultDateMathFacts and DefaultMonthMathFacts.
// Ajouter Subtract() aux types date et mois ?

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
    /// Calculates the exact difference (expressed in years, months and days)
    /// between the two specified dates.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public static (int Years, int Months, int Days) Subtract(TropicaliaDate start, TropicaliaDate end)
    {
        int years = end.CountYearsSince(start);
        var newStart = start.PlusYears(years);
        int months = end.CountMonthsSince(newStart);
        newStart = newStart.PlusMonths(months);
        int days = end.CountDaysSince(newStart);
        return (years, months, days);
    }
}
