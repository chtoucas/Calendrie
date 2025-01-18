// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.InterfacesFacts

#nowarn 3536 // Conversion to IMonth

open Calendrie.Hemerology
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Faux

open Xunit

module DateInterfaces =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<FauxGregorianDate, StandardGregorianDataSet>()

        // TDate.FromDayNumber() throws an OverflowException here but only because
        // the base test uses the explicit implementation of FromDayNumber().
        override x.FromDayNumber_InvalidDayNumber () =
            x.DomainTester.TestInvalidDayNumber(FauxGregorianDate.FromDayNumber);

module MonthInterface =
    let calendarDataSet = StandardTropicaliaDataSet.Instance

    let monthInfoData = calendarDataSet.MonthInfoData

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``CountElapsedMonthsInYear()`` (info: MonthInfo) =
        let y, m = info.Yemo.Deconstruct()
        let month : IMonth = new FauxTropicaliaMonth(y, m)

        month.CountElapsedMonthsInYear() === m - 1
