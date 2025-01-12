// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.CopticTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology

open Xunit

module Prelude =
    let ``Value of CopticCalendar.Epoch.DaysZinceZero`` () =
        CopticCalendar.Instance.Epoch.DaysSinceZero === 103_604
    [<Fact>]
    let ``Value of Coptic13Calendar.Epoch.DaysZinceZero`` () =
        Coptic13Calendar.Instance.Epoch.DaysSinceZero === 103_604

    [<Fact>]
    let ``default(CopticDate) is CopticCalendar.Epoch`` () =
        Unchecked.defaultof<CopticDate>.DayNumber === CopticCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Coptic13Date) is Coptic13Calendar.Epoch`` () =
        Unchecked.defaultof<Coptic13Date>.DayNumber === Coptic13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of CopticCalendar.MinDaysSinceEpoch`` () =
        CopticCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Coptic13Calendar.MinDaysSinceEpoch`` () =
        Coptic13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of CopticCalendar.MaxDaysSinceEpoch`` () =
        CopticCalendar.Instance.MaxDaysSinceEpoch === 3_652_134
    [<Fact>]
    let ``Value of Coptic13Calendar.MaxDaysSinceEpoch`` () =
        Coptic13Calendar.Instance.MaxDaysSinceEpoch === 3_652_134

    [<Fact>]
    let ``Value of CopticCalendar.MinMonthsSinceEpoch`` () =
        CopticCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of Coptic13Calendar.MinMonthsSinceEpoch`` () =
        Coptic13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of CopticCalendar.MaxMonthsSinceEpoch`` () =
        CopticCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of Coptic13Calendar.MaxMonthsSinceEpoch`` () =
        Coptic13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Bundles =
    let private chr = CopticCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<CopticCalendar, StandardCoptic12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = CopticCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = CopticCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = CopticCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<CopticDate, StandardCoptic12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<CopticDate, StandardCoptic12DataSet>()

        override __.GetDate(y, m, d) = new CopticDate(y, m, d)

module Bundles13 =
    let private chr = Coptic13Calendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Coptic13Calendar, StandardCoptic13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = Coptic13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Coptic13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Coptic13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Coptic13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Coptic13Date, StandardCoptic13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<Coptic13Date, StandardCoptic13DataSet>(Coptic13Calendar.Instance)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Coptic13Date, StandardCoptic13DataSet>()

        override __.GetDate(y, m, d) = new Coptic13Date(y, m, d)
