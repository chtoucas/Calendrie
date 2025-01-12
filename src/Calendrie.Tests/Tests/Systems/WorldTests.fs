﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.WorldTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of WorldCalendar.Epoch.DaysZinceZero`` () =
        WorldCalendar.Instance.Epoch.DaysSinceZero === -1

    [<Fact>]
    let ``default(WorldDate) is WorldCalendar.Epoch`` () =
        Unchecked.defaultof<WorldDate>.DayNumber === WorldCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of WorldCalendar.MinDaysSinceEpoch`` () =
        WorldCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of WorldCalendar.MaxDaysSinceEpoch`` () =
        WorldCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of WorldCalendar.MinMonthsSinceEpoch`` () =
        WorldCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of WorldCalendar.MaxMonthsSinceEpoch`` () =
        WorldCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

module Methods =
    let private chr = WorldCalendar.Instance

    let dateInfoData = WorldDataSet.Instance.DateInfoData
    let moreMonthInfoData = WorldDataSet.MoreMonthInfoData

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Property IsBlank`` (info: DateInfo) =
        let (y, m, d) = info.Yemoda.Deconstruct()
        let date = new WorldDate(y, m, d)

        date.IsBlank === date.IsSupplementary

    [<Theory; MemberData(nameof(moreMonthInfoData))>]
    let ``CountDaysInWorldMonth()`` (info: YemoAnd<int>) =
        let (y, m, daysInMonth) = info.Deconstruct()

        WorldCalendar.CountDaysInWorldMonth(y, m) === daysInMonth

module Bundles =
    let private chr = WorldCalendar.Instance

    let dateInfoData = WorldDataSet.Instance.DateInfoData

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<WorldCalendar, StandardWorldDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = WorldCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = WorldCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = WorldCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<WorldDate, StandardWorldDataSet>()
