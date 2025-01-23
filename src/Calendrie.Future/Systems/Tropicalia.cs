// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

// TODO(code): add PlusYears(roundoff) to all month types.
// Interfaces: currently in IDateBase, but shouldn't it be in IYearFieldMath and
// IMonthFieldMath.
// XML doc.
// Add more tests in CivilTests and GregorianTests.
// Add a warning about the data (CountYearsBetweenData & co) which only
// offer symmetrical results; see DefaultDateMathFacts and DefaultMonthMathFacts.
// Supprimer EmitDateCustomMath(Non)Regular()
// Ajouter à DateMath CountPeriodBetween(), mieux DateDifference ?

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
