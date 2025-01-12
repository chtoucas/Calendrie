// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.TropicaliaTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology

open Xunit

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
    let private chr = TropicaliaCalendar.Instance

    let dateInfoData = TropicaliaDataSet.Instance.DateInfoData

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<TropicaliaCalendar, StandardTropicaliaDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = TropicaliaCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = TropicaliaCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<TropicaliaDate, StandardTropicaliaDataSet>()
