﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.FrenchRepublican

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.Epoch.DaysZinceZero`` () =
        FrenchRepublicanCalendar.Instance.Epoch.DaysSinceZero === 654_414
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.Epoch.DaysZinceZero`` () =
        FrenchRepublican13Calendar.Instance.Epoch.DaysSinceZero === 654_414

    [<Fact>]
    let ``default(FrenchRepublicanDate) is FrenchRepublicanCalendar.Epoch`` () =
        Unchecked.defaultof<FrenchRepublicanDate>.DayNumber === FrenchRepublicanCalendar.Instance.Epoch
    [<Fact>]
    let ``default(FrenchRepublican13Date) is FrenchRepublican13Calendar.Epoch`` () =
        Unchecked.defaultof<FrenchRepublican13Date>.DayNumber === FrenchRepublican13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MinDaysSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MinDaysSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MaxDaysSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MaxDaysSinceEpoch === 3_652_056
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MaxDaysSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MaxDaysSinceEpoch === 3_652_056

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MinMonthsSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MinMonthsSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MaxMonthsSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MaxMonthsSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Bundles =
    let private chr = FrenchRepublicanCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<FrenchRepublicanDate, FrenchRepublicanCalendar, StandardFrenchRepublican12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new FrenchRepublicanDate(y, m, d);
        override __.GetDate(y, doy) = new FrenchRepublicanDate(y, doy);
        override __.GetDate(dayNumber) = FrenchRepublicanDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = FrenchRepublicanCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = FrenchRepublicanCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = FrenchRepublicanCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<FrenchRepublicanDate, FrenchRepublicanCalendar, StandardFrenchRepublican12DataSet>(chr)

        override __.MinDate = FrenchRepublicanDate.MinValue
        override __.MaxDate = FrenchRepublicanDate.MaxValue

        override __.GetDate(y, m, d) = new FrenchRepublicanDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = FrenchRepublicanDate.Calendar |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<FrenchRepublicanDate, StandardFrenchRepublican12DataSet>(FrenchRepublicanCalendar.Instance)

        override __.GetDate(y, m, d) = new FrenchRepublicanDate(y, m, d)
        override __.GetDate(y, doy) = new FrenchRepublicanDate(y, doy)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<FrenchRepublicanDate, StandardFrenchRepublican12DataSet>()

        override __.GetDate(y, m, d) = new FrenchRepublicanDate(y, m, d)

module Bundles13 =
    let private chr = FrenchRepublican13Calendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<FrenchRepublican13Date, FrenchRepublican13Calendar, StandardFrenchRepublican13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new FrenchRepublican13Date(y, m, d);
        override __.GetDate(y, doy) = new FrenchRepublican13Date(y, doy);
        override __.GetDate(dayNumber) = FrenchRepublican13Date.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = FrenchRepublican13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = FrenchRepublican13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = FrenchRepublican13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = FrenchRepublican13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<FrenchRepublican13Date, FrenchRepublican13Calendar, StandardFrenchRepublican13DataSet>(chr)

        override __.MinDate = FrenchRepublican13Date.MinValue
        override __.MaxDate = FrenchRepublican13Date.MaxValue

        override __.GetDate(y, m, d) = new FrenchRepublican13Date(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = FrenchRepublican13Date.Calendar |> isnotnull

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<FrenchRepublican13Date, StandardFrenchRepublican13DataSet>(FrenchRepublican13Calendar.Instance)

        override __.GetDate(y, m, d) = new FrenchRepublican13Date(y, m, d)
        override __.GetDate(y, doy) = new FrenchRepublican13Date(y, doy)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<FrenchRepublican13Date, StandardFrenchRepublican13DataSet>()

        override __.GetDate(y, m, d) = new FrenchRepublican13Date(y, m, d)


