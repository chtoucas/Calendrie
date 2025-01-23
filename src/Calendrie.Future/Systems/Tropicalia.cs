// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

// TODO(code): add PlusYears(roundoff) to all month types.
// Interfaces: currently in IDateBase, but shouldn't it be in IYearFieldMath and
// IMonthFieldMath.
// XML doc. Naming: newStart or ???
// Add more tests in CivilTests and GregorianTests.
// Add a warning about the data (CountYearsBetweenData & co) which only
// offer symmetrical results; see DefaultDateMathFacts and DefaultMonthMathFacts.
// Supprimer EmitDateCustomMath(Non)Regular()
// Ajouter à DateMath CountPeriodBetween()

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

public partial class TropicaliaCalendar : ICalendarMath<TropicaliaDate>
{
    [Pure]
    TropicaliaDate ICalendarMath<TropicaliaDate>.AddYears(int y, int m, int d, int years) =>
        AddYears(y, m, d, years);

    [Pure]
    TropicaliaDate ICalendarMath<TropicaliaDate>.AddYears(int y, int m, int d, int years, out int roundoff) =>
        AddYears(y, m, d, years, out roundoff);

    [Pure]
    TropicaliaDate ICalendarMath<TropicaliaDate>.AddMonths(int y, int m, int d, int months) =>
        AddMonths(y, m, d, months);

    [Pure]
    TropicaliaDate ICalendarMath<TropicaliaDate>.AddMonths(int y, int m, int d, int months, out int roundoff) =>
        AddMonths(y, m, d, months, out roundoff);
}
