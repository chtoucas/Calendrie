// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.InternationalFixed

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of InternationalFixedCalendar.Epoch.DaysZinceZero`` () =
        InternationalFixedCalendar.Instance.Epoch.DaysSinceZero === 0

    [<Fact>]
    let ``default(InternationalFixedDate) is InternationalFixedCalendar.Epoch`` () =
        Unchecked.defaultof<InternationalFixedDate>.DayNumber === InternationalFixedCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of InternationalFixedCalendar.MinDaysSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of InternationalFixedCalendar.MaxDaysSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of InternationalFixedCalendar.MinMonthsSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of InternationalFixedCalendar.MaxMonthsSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Bundles =
    let private chr = InternationalFixedCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<InternationalFixedCalendar, StandardInternationalFixedDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = InternationalFixedCalendar.MonthsInYear === 13

        [<Fact>]
        static member MinYear() = InternationalFixedCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = InternationalFixedCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<InternationalFixedDate, InternationalFixedCalendar, StandardInternationalFixedDataSet>(chr)

        override __.MinDate = InternationalFixedDate.MinValue
        override __.MaxDate = InternationalFixedDate.MaxValue

        override __.GetDate(y, m, d) = new InternationalFixedDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = InternationalFixedDate.Calendar |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<InternationalFixedDate, StandardInternationalFixedDataSet>(InternationalFixedCalendar.Instance)

        override __.GetDate(y, m, d) = new InternationalFixedDate(y, m, d)
        override __.GetDate(y, doy) = new InternationalFixedDate(y, doy)
