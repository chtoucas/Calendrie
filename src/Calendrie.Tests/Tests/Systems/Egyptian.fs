// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.Egyptian

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of EgyptianCalendar.Epoch.DaysZinceZero`` () =
        EgyptianCalendar.Instance.Epoch.DaysSinceZero === -272_788
    [<Fact>]
    let ``Value of Egyptian13Calendar.Epoch.DaysZinceZero`` () =
        Egyptian13Calendar.Instance.Epoch.DaysSinceZero === -272_788

    [<Fact>]
    let ``default(EgyptianDate) is EgyptianCalendar.Epoch`` () =
        Unchecked.defaultof<EgyptianDate>.DayNumber === EgyptianCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Egyptian13Date) is Egyptian13Calendar.Epoch`` () =
        Unchecked.defaultof<Egyptian13Date>.DayNumber === Egyptian13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of EgyptianCalendar.MinDaysSinceEpoch`` () =
        EgyptianCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Egyptian13Calendar.MinDaysSinceEpoch`` () =
        Egyptian13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of EgyptianCalendar.MaxDaysSinceEpoch`` () =
        EgyptianCalendar.Instance.MaxDaysSinceEpoch === 3_649_634
    [<Fact>]
    let ``Value of Egyptian13Calendar.MaxDaysSinceEpoch`` () =
        Egyptian13Calendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of EgyptianCalendar.MinMonthsSinceEpoch`` () =
        EgyptianCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of Egyptian13Calendar.MinMonthsSinceEpoch`` () =
        Egyptian13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of EgyptianCalendar.MaxMonthsSinceEpoch`` () =
        EgyptianCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of Egyptian13Calendar.MaxMonthsSinceEpoch`` () =
        Egyptian13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Bundles =
    let private chr = EgyptianCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<EgyptianCalendar, StandardEgyptian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = EgyptianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = EgyptianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = EgyptianCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<EgyptianDate, EgyptianCalendar, StandardEgyptian12DataSet>()

        [<Fact>]
        static member Calendar_Prop() = EgyptianDate.Calendar |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<EgyptianDate, StandardEgyptian12DataSet>(EgyptianCalendar.Instance)

        override __.GetDate(y, m, d) = new EgyptianDate(y, m, d)
        override __.GetDate(y, doy) = new EgyptianDate(y, doy)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<EgyptianDate, StandardEgyptian12DataSet>()

        override __.GetDate(y, m, d) = new EgyptianDate(y, m, d)

module Bundles13 =
    let private chr = Egyptian13Calendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Egyptian13Calendar, StandardEgyptian13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = Egyptian13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Egyptian13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Egyptian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Egyptian13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Egyptian13Date, Egyptian13Calendar, StandardEgyptian13DataSet>()

        [<Fact>]
        static member Calendar_Prop() = Egyptian13Date.Calendar |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<Egyptian13Date, StandardEgyptian13DataSet>(Egyptian13Calendar.Instance)

        override __.GetDate(y, m, d) = new Egyptian13Date(y, m, d)
        override __.GetDate(y, doy) = new Egyptian13Date(y, doy)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Egyptian13Date, StandardEgyptian13DataSet>()

        override __.GetDate(y, m, d) = new Egyptian13Date(y, m, d)
