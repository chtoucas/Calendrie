// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.Positivist

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of PositivistCalendar.Epoch.DaysZinceZero`` () =
        PositivistCalendar.Instance.Epoch.DaysSinceZero === 653_054

    [<Fact>]
    let ``default(PositivistDate) is PositivistCalendar.Epoch`` () =
        Unchecked.defaultof<PositivistDate>.DayNumber === PositivistCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of PositivistCalendar.MinDaysSinceEpoch`` () =
        PositivistCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of PositivistCalendar.MaxDaysSinceEpoch`` () =
        PositivistCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of PositivistCalendar.MinMonthsSinceEpoch`` () =
        PositivistCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of PositivistCalendar.MaxMonthsSinceEpoch`` () =
        PositivistCalendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Bundles =
    let private chr = PositivistCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<PositivistCalendar, StandardPositivistDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = PositivistCalendar.MonthsInYear === 13

        [<Fact>]
        static member MinYear() = PositivistCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = PositivistCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<PositivistDate, StandardPositivistDataSet>()
