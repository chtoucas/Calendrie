﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.TropicaliaTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

let chr = TropicaliaCalendar.Instance

module Prelude =
    [<Fact>]
    let ``Value of TropicaliaCalendar.Epoch.DaysZinceZero`` () =
        TropicaliaCalendar.Instance.Epoch.DaysSinceZero === 0

    [<Fact>]
    let ``default(TropicaliaDate) is TropicaliaCalendar.Epoch`` () =
        Unchecked.defaultof<TropicaliaDate>.DayNumber === TropicaliaCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of TropicaliaCalendar.MinDaysSinceEpoch`` () =
        TropicaliaCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of TropicaliaCalendar.MaxDaysSinceEpoch`` () =
        TropicaliaCalendar.Instance.MaxDaysSinceEpoch === 3_652_055

    [<Fact>]
    let ``Value of TropicaliaCalendar.MinMonthsSinceEpoch`` () =
        TropicaliaCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of TropicaliaCalendar.MaxMonthsSinceEpoch`` () =
        TropicaliaCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif


module Bundles =
    let dateInfoData = TropicaliaDataSet.Instance.DateInfoData

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<TropicaliaDate, TropicaliaCalendar, StandardTropicaliaDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new TropicaliaDate(y, m, d);
        override __.GetDate(y, doy) = new TropicaliaDate(y, doy);
        override __.GetDate(dayNumber) = TropicaliaDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MinYear() = TropicaliaCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = TropicaliaCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<TropicaliaDate, TropicaliaCalendar, StandardTropicaliaDataSet>(chr)

        override __.MinDate = TropicaliaDate.MinValue
        override __.MaxDate = TropicaliaDate.MaxValue

        override __.GetDate(y, m, d) = new TropicaliaDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = TropicaliaDate.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<TropicaliaDate, StandardTropicaliaDataSet>(chr)

        override __.GetDate(y, m, d) = new TropicaliaDate(y, m, d)
        override __.GetDate(y, doy) = new TropicaliaDate(y, doy)
