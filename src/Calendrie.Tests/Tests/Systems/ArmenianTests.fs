// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.ArmenianTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of ArmenianCalendar.Epoch.DaysZinceZero`` () =
        ArmenianCalendar.Instance.Epoch.DaysSinceZero === 201_442
    [<Fact>]
    let ``Value of Armenian13Calendar.Epoch.DaysZinceZero`` () =
        Armenian13Calendar.Instance.Epoch.DaysSinceZero === 201_442

    [<Fact>]
    let ``default(ArmenianDate) is ArmenianCalendar.Epoch`` () =
        Unchecked.defaultof<ArmenianDate>.DayNumber === ArmenianCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Armenian13Date) is Armenian13Calendar.Epoch`` () =
        Unchecked.defaultof<Armenian13Date>.DayNumber === Armenian13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of ArmenianCalendar.MinDaysSinceEpoch`` () =
        ArmenianCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Armenian13Calendar.MinDaysSinceEpoch`` () =
        Armenian13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of ArmenianCalendar.MaxDaysSinceEpoch`` () =
        ArmenianCalendar.Instance.MaxDaysSinceEpoch === 3_649_634
    [<Fact>]
    let ``Value of Armenian13Calendar.MaxDaysSinceEpoch`` () =
        Armenian13Calendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of ArmenianCalendar.MinMonthsSinceEpoch`` () =
        ArmenianCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of Armenian13Calendar.MinMonthsSinceEpoch`` () =
        Armenian13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of ArmenianCalendar.MaxMonthsSinceEpoch`` () =
        ArmenianCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of Armenian13Calendar.MaxMonthsSinceEpoch`` () =
        Armenian13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Bundles =
    let private chr = ArmenianCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<ArmenianCalendar, StandardArmenian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = ArmenianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = ArmenianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = ArmenianCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<ArmenianDate, StandardArmenian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<ArmenianDate, StandardArmenian12DataSet>(ArmenianCalendar.Instance)

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d)
        override __.GetDate(y, doy) = new ArmenianDate(y, doy)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<ArmenianDate, StandardArmenian12DataSet>()

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d)


module Bundles13 =
    let private chr = Armenian13Calendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Armenian13Calendar, StandardArmenian13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = Armenian13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Armenian13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Armenian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Armenian13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Armenian13Date, StandardArmenian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<Armenian13Date, StandardArmenian13DataSet>(Armenian13Calendar.Instance)

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d)
        override __.GetDate(y, doy) = new Armenian13Date(y, doy)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Armenian13Date, StandardArmenian13DataSet>()

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d)
