﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Specialized.CopticTests

open Calendrie
open Calendrie.Specialized
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Specialized

open Xunit

module Bundles =
    let private chr = new CopticCalendar()

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
        member x.MonthsInYear() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<CopticDate, CopticCalendar, StandardCoptic12DataSet>(chr)

        override __.MinDate = CopticDate.MinValue
        override __.MaxDate = CopticDate.MaxValue

        override __.GetDate(y, m, d) = new CopticDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = CopticDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = CopticDate.Adjuster |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit SpecialAdjusterFacts<CopticDate, StandardCoptic12DataSet>(new CopticAdjuster())

        override __.GetDate(y, m, d) = new CopticDate(y, m, d)
        override __.GetDate(y, doy) = new CopticDate(y, doy)

    [<Sealed>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<CopticDate, StandardCoptic12DataSet>()

        override __.GetDate(y, m, d) = new CopticDate(y, m, d)

module Bundles13 =
    let private chr = new Coptic13Calendar()

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
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 13

        [<Fact>]
        member x.VirtualMonth_Prop() = x.CalendarUT.VirtualMonth === 13

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<Coptic13Date, Coptic13Calendar, StandardCoptic13DataSet>(chr)

        override __.MinDate = Coptic13Date.MinValue
        override __.MaxDate = Coptic13Date.MaxValue

        override __.GetDate(y, m, d) = new Coptic13Date(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = Coptic13Date.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = Coptic13Date.Adjuster |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit SpecialAdjusterFacts<Coptic13Date, StandardCoptic13DataSet>(new Coptic13Adjuster())

        override __.GetDate(y, m, d) = new Coptic13Date(y, m, d)
        override __.GetDate(y, doy) = new Coptic13Date(y, doy)

    [<Sealed>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Coptic13Date, StandardCoptic13DataSet>()

        override __.GetDate(y, m, d) = new Coptic13Date(y, m, d)
