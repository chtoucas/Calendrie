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
open Calendrie.Testing.Facts.Systems

open Xunit

let private chr = WorldCalendar.Instance

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
#endif

module Methods =
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

        chr.CountDaysInWorldMonth(y, m) === daysInMonth

module Bundles =
    let dateInfoData = WorldDataSet.Instance.DateInfoData

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<WorldDate, WorldCalendar, StandardWorldDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new WorldDate(y, m, d);
        override __.GetDate(y, doy) = new WorldDate(y, doy);
        override __.GetDate(dayNumber) = WorldDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = WorldCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = WorldCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = WorldCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<WorldDate, WorldCalendar, StandardWorldDataSet>(chr)

        override __.MinDate = WorldDate.MinValue
        override __.MaxDate = WorldDate.MaxValue

        override __.GetDate(y, m, d) = new WorldDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = WorldDate.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<WorldDate, StandardWorldDataSet>(chr)

        override __.GetDate(y, m, d) = new WorldDate(y, m, d)
        override __.GetDate(y, doy) = new WorldDate(y, doy)
