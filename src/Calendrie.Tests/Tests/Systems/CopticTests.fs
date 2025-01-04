// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.CopticTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    let ``Value of CopticCalendar.Epoch.DaysZinceZero`` () =
        CopticCalendar.Instance.Epoch.DaysSinceZero === 103_604
    [<Fact>]
    let ``Value of Coptic13Calendar.Epoch.DaysZinceZero`` () =
        Coptic13Calendar.Instance.Epoch.DaysSinceZero === 103_604

    [<Fact>]
    let ``default(CopticDate) is CopticCalendar.Epoch`` () =
        Unchecked.defaultof<CopticDate>.DayNumber === CopticCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Coptic13Date) is Coptic13Calendar.Epoch`` () =
        Unchecked.defaultof<Coptic13Date>.DayNumber === Coptic13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of CopticCalendar.MinDaysSinceEpoch`` () =
        CopticCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Coptic13Calendar.MinDaysSinceEpoch`` () =
        Coptic13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of CopticCalendar.MaxDaysSinceEpoch`` () =
        CopticCalendar.Instance.MaxDaysSinceEpoch === 3_652_134
    [<Fact>]
    let ``Value of Coptic13Calendar.MaxDaysSinceEpoch`` () =
        Coptic13Calendar.Instance.MaxDaysSinceEpoch === 3_652_134

    [<Fact>]
    let ``Value of CopticCalendar.MinMonthsSinceEpoch`` () =
        CopticCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of Coptic13Calendar.MinMonthsSinceEpoch`` () =
        Coptic13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of CopticCalendar.MaxMonthsSinceEpoch`` () =
        CopticCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of Coptic13Calendar.MaxMonthsSinceEpoch`` () =
        Coptic13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Bundles =
    let private chr = CopticCalendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<CopticDate, CopticCalendar, StandardCoptic12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new CopticDate(y, m, d);
        override __.GetDate(y, doy) = new CopticDate(y, doy);
        override __.GetDate(dayNumber) = CopticDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = CopticCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = CopticCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = CopticCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<CopticDate, CopticCalendar, StandardCoptic12DataSet>(chr)

        override __.MinDate = CopticDate.MinValue
        override __.MaxDate = CopticDate.MaxValue

        override __.GetDate(y, m, d) = new CopticDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = CopticDate.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<CopticDate, StandardCoptic12DataSet>(CopticCalendar.Instance)

        override __.GetDate(y, m, d) = new CopticDate(y, m, d)
        override __.GetDate(y, doy) = new CopticDate(y, doy)

    [<Sealed>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<CopticDate, StandardCoptic12DataSet>()

        override __.GetDate(y, m, d) = new CopticDate(y, m, d)

module Bundles13 =
    let private chr = Coptic13Calendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<Coptic13Date, Coptic13Calendar, StandardCoptic13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new Coptic13Date(y, m, d);
        override __.GetDate(y, doy) = new Coptic13Date(y, doy);
        override __.GetDate(dayNumber) = Coptic13Date.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = Coptic13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Coptic13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Coptic13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Coptic13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<Coptic13Date, Coptic13Calendar, StandardCoptic13DataSet>(chr)

        override __.MinDate = Coptic13Date.MinValue
        override __.MaxDate = Coptic13Date.MaxValue

        override __.GetDate(y, m, d) = new Coptic13Date(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = Coptic13Date.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<Coptic13Date, StandardCoptic13DataSet>(Coptic13Calendar.Instance)

        override __.GetDate(y, m, d) = new Coptic13Date(y, m, d)
        override __.GetDate(y, doy) = new Coptic13Date(y, doy)

    [<Sealed>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Coptic13Date, StandardCoptic13DataSet>()

        override __.GetDate(y, m, d) = new Coptic13Date(y, m, d)
