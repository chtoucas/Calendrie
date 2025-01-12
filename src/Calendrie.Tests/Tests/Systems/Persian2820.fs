// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.Persian2820

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of Persian2820Calendar.Epoch.DaysZinceZero`` () =
        Persian2820Calendar.Instance.Epoch.DaysSinceZero === 226_895

    [<Fact>]
    let ``default(Persian2820Date) is Persian2820Calendar.Epoch`` () =
        Unchecked.defaultof<Persian2820Date>.DayNumber === Persian2820Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of Persian2820Calendar.MinDaysSinceEpoch`` () =
        Persian2820Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of Persian2820Calendar.MaxDaysSinceEpoch`` () =
        Persian2820Calendar.Instance.MaxDaysSinceEpoch === 3_652_055

    [<Fact>]
    let ``Value of Persian2820Calendar.MinMonthsSinceEpoch`` () =
        Persian2820Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of Persian2820Calendar.MaxMonthsSinceEpoch`` () =
        Persian2820Calendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

module Bundles =
    let private chr = Persian2820Calendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<Persian2820Date, Persian2820Calendar, StandardPersian2820DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new Persian2820Date(y, m, d);
        override __.GetDate(y, doy) = new Persian2820Date(y, doy);
        override __.GetDate(dayNumber) = Persian2820Date.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = Persian2820Calendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = Persian2820Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Persian2820Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<Persian2820Date, Persian2820Calendar, StandardPersian2820DataSet>(chr)

        override __.MinDate = Persian2820Date.MinValue
        override __.MaxDate = Persian2820Date.MaxValue

        override __.GetDate(y, m, d) = new Persian2820Date(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = Persian2820Date.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<Persian2820Date, StandardPersian2820DataSet>(Persian2820Calendar.Instance)

        override __.GetDate(y, m, d) = new Persian2820Date(y, m, d)
        override __.GetDate(y, doy) = new Persian2820Date(y, doy)



