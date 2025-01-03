﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.EthiopicTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of EthiopicCalendar.Epoch.DaysZinceZero`` () =
        EthiopicCalendar.Instance.Epoch.DaysSinceZero === 2795
    [<Fact>]
    let ``Value of Ethiopic13Calendar.Epoch.DaysZinceZero`` () =
        Ethiopic13Calendar.Instance.Epoch.DaysSinceZero === 2795

    [<Fact>]
    let ``default(EthiopicDate) is EthiopicCalendar.Epoch`` () =
        Unchecked.defaultof<EthiopicDate>.DayNumber === EthiopicCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Ethiopic13Date) is Ethiopic13Calendar.Epoch`` () =
        Unchecked.defaultof<Ethiopic13Date>.DayNumber === Ethiopic13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of EthiopicCalendar.MinDaysSinceEpoch`` () =
        EthiopicCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Ethiopic13Calendar.MinDaysSinceEpoch`` () =
        Ethiopic13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of EthiopicCalendar.MaxDaysSinceEpoch`` () =
        EthiopicCalendar.Instance.MaxDaysSinceEpoch === 3_652_134
    [<Fact>]
    let ``Value of Ethiopic13Calendar.MaxDaysSinceEpoch`` () =
        Ethiopic13Calendar.Instance.MaxDaysSinceEpoch === 3_652_134
#endif

module Bundles =
    let private chr = EthiopicCalendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<EthiopicDate, EthiopicCalendar, StandardEthiopic12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d);
        override __.GetDate(y, doy) = new EthiopicDate(y, doy);
        override __.GetDate(dayNumber) = EthiopicDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = EthiopicCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = EthiopicCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = EthiopicCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<EthiopicDate, EthiopicCalendar, StandardEthiopic12DataSet>(chr)

        override __.MinDate = EthiopicDate.MinValue
        override __.MaxDate = EthiopicDate.MaxValue

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = EthiopicDate.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<EthiopicDate, StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d)
        override __.GetDate(y, doy) = new EthiopicDate(y, doy)

    [<Sealed>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<EthiopicDate, StandardEthiopic12DataSet>()

        override __.GetDate(y, m, d) = new EthiopicDate(y, m, d)

module Bundles13 =
    let private chr = Ethiopic13Calendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<Ethiopic13Date, Ethiopic13Calendar, StandardEthiopic13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new Ethiopic13Date(y, m, d);
        override __.GetDate(y, doy) = new Ethiopic13Date(y, doy);
        override __.GetDate(dayNumber) = Ethiopic13Date.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = Ethiopic13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Ethiopic13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Ethiopic13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Ethiopic13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<Ethiopic13Date, Ethiopic13Calendar, StandardEthiopic13DataSet>(chr)

        override __.MinDate = Ethiopic13Date.MinValue
        override __.MaxDate = Ethiopic13Date.MaxValue

        override __.GetDate(y, m, d) = new Ethiopic13Date(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = Ethiopic13Date.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<Ethiopic13Date, StandardEthiopic13DataSet>(Ethiopic13Calendar.Instance)

        override __.GetDate(y, m, d) = new Ethiopic13Date(y, m, d)
        override __.GetDate(y, doy) = new Ethiopic13Date(y, doy)

    [<Sealed>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Ethiopic13Date, StandardEthiopic13DataSet>()

        override __.GetDate(y, m, d) = new Ethiopic13Date(y, m, d)
