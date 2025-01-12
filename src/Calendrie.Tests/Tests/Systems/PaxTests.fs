// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.PaxTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of PaxCalendar.Epoch.DaysZinceZero`` () =
        PaxCalendar.Instance.Epoch.DaysSinceZero === -1

    [<Fact>]
    let ``default(PaxDate) is PaxCalendar.Epoch`` () =
        Unchecked.defaultof<PaxDate>.DayNumber === PaxCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of PaxCalendar.MinDaysSinceEpoch`` () =
        PaxCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of PaxCalendar.MaxDaysSinceEpoch`` () =
        PaxCalendar.Instance.MaxDaysSinceEpoch === 3_652_060

    [<Fact>]
    let ``Value of PaxCalendar.MinMonthsSinceEpoch`` () =
        PaxCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of PaxCalendar.MaxMonthsSinceEpoch`` () =
        PaxCalendar.Instance.MaxMonthsSinceEpoch === 131_761
#endif

module Bundles =
    let private chr = PaxCalendar.Instance

    let dateInfoData = PaxDataSet.Instance.DateInfoData

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<PaxCalendar, StandardPaxDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Other
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Weeks

        [<Fact>]
        static member MinYear() = PaxCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = PaxCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<PaxDate, PaxCalendar, StandardPaxDataSet>(chr)

        [<Fact>]
        static member Calendar_Prop() = PaxDate.Calendar |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<PaxDate, StandardPaxDataSet>(chr)

        override __.GetDate(y, m, d) = new PaxDate(y, m, d)
        override __.GetDate(y, doy) = new PaxDate(y, doy)
