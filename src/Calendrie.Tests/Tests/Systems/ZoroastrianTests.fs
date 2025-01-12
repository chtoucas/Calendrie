// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.ZoroastrianTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of ZoroastrianCalendar.Epoch.DaysZinceZero`` () =
        ZoroastrianCalendar.Instance.Epoch.DaysSinceZero === 230_637
    [<Fact>]
    let ``Value of Zoroastrian13Calendar.Epoch.DaysZinceZero`` () =
        Zoroastrian13Calendar.Instance.Epoch.DaysSinceZero === 230_637

    [<Fact>]
    let ``default(ZoroastrianDate) is ZoroastrianCalendar.Epoch`` () =
        Unchecked.defaultof<ZoroastrianDate>.DayNumber === ZoroastrianCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Zoroastrian13Date) is Zoroastrian13Calendar.Epoch`` () =
        Unchecked.defaultof<Zoroastrian13Date>.DayNumber === Zoroastrian13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of ZoroastrianCalendar.MinDaysSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Zoroastrian13Calendar.MinDaysSinceEpoch`` () =
        Zoroastrian13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of ZoroastrianCalendar.MaxDaysSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MaxDaysSinceEpoch === 3_649_634
    [<Fact>]
    let ``Value of Zoroastrian13Calendar.MaxDaysSinceEpoch`` () =
        Zoroastrian13Calendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of ZoroastrianCalendar.MinMonthsSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of Zoroastrian13Calendar.MinMonthsSinceEpoch`` () =
        Zoroastrian13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of ZoroastrianCalendar.MaxMonthsSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of Zoroastrian13Calendar.MaxMonthsSinceEpoch`` () =
        Zoroastrian13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Bundles =
    let private chr = ZoroastrianCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<ZoroastrianCalendar, StandardZoroastrian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = ZoroastrianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = ZoroastrianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = ZoroastrianCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<ZoroastrianDate, ZoroastrianCalendar, StandardZoroastrian12DataSet>(chr)

        override __.MinDate = ZoroastrianDate.MinValue
        override __.MaxDate = ZoroastrianDate.MaxValue

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = ZoroastrianDate.Calendar |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<ZoroastrianDate, StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d)
        override __.GetDate(y, doy) = new ZoroastrianDate(y, doy)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<ZoroastrianDate, StandardZoroastrian12DataSet>()

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d)

module Bundles13 =
    let private chr = Zoroastrian13Calendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Zoroastrian13Calendar, StandardZoroastrian13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = Zoroastrian13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Zoroastrian13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Zoroastrian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Zoroastrian13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Zoroastrian13Date, Zoroastrian13Calendar, StandardZoroastrian13DataSet>(chr)

        override __.MinDate = Zoroastrian13Date.MinValue
        override __.MaxDate = Zoroastrian13Date.MaxValue

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = Zoroastrian13Date.Calendar |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>(Zoroastrian13Calendar.Instance)

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d)
        override __.GetDate(y, doy) = new Zoroastrian13Date(y, doy)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>()

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d)
