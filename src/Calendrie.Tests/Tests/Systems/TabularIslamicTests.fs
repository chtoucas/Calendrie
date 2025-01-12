// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.TabularIslamicTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of TabularIslamicCalendar.Epoch.DaysZinceZero`` () =
        TabularIslamicCalendar.Instance.Epoch.DaysSinceZero === 227_014

    [<Fact>]
    let ``default(TabularIslamicDate) is TabularIslamicCalendar.Epoch`` () =
        Unchecked.defaultof<TabularIslamicDate>.DayNumber === TabularIslamicCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of TabularIslamicCalendar.MinDaysSinceEpoch`` () =
        TabularIslamicCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of TabularIslamicCalendar.MaxDaysSinceEpoch`` () =
        TabularIslamicCalendar.Instance.MaxDaysSinceEpoch === 3_543_311

    [<Fact>]
    let ``Value of TabularIslamicCalendar.MinMonthsSinceEpoch`` () =
        TabularIslamicCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of TabularIslamicCalendar.MaxMonthsSinceEpoch`` () =
        TabularIslamicCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

module Bundles =
    let private chr = TabularIslamicCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<TabularIslamicCalendar, StandardTabularIslamicDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Lunar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = TabularIslamicCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = TabularIslamicCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = TabularIslamicCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<TabularIslamicDate, TabularIslamicCalendar, StandardTabularIslamicDataSet>(chr)

        [<Fact>]
        static member Calendar_Prop() = TabularIslamicDate.Calendar |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<TabularIslamicDate, StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

        override __.GetDate(y, m, d) = new TabularIslamicDate(y, m, d)
        override __.GetDate(y, doy) = new TabularIslamicDate(y, doy)
