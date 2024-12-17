// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.ArmenianTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Bundles =
    let private chr = ArmenianCalendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<ArmenianDate, ArmenianCalendar, StandardArmenian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d);
        override __.GetDate(y, doy) = new ArmenianDate(y, doy);
        override __.GetDate(dayNumber) = ArmenianDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = ArmenianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = ArmenianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = ArmenianCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<ArmenianDate, ArmenianCalendar, StandardArmenian12DataSet>(chr)

        override __.MinDate = ArmenianDate.MinValue
        override __.MaxDate = ArmenianDate.MaxValue

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = ArmenianDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = ArmenianDate.Adjuster |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<ArmenianDate, StandardArmenian12DataSet>(ArmenianDate.Adjuster)

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d)
        override __.GetDate(y, doy) = new ArmenianDate(y, doy)

    [<Sealed>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<ArmenianDate, StandardArmenian12DataSet>()

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d)


module Bundles13 =
    let private chr = Armenian13Calendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<Armenian13Date, Armenian13Calendar, StandardArmenian13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d);
        override __.GetDate(y, doy) = new Armenian13Date(y, doy);
        override __.GetDate(dayNumber) = Armenian13Date.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = Armenian13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Armenian13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Armenian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Armenian13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<Armenian13Date, Armenian13Calendar, StandardArmenian13DataSet>(chr)

        override __.MinDate = Armenian13Date.MinValue
        override __.MaxDate = Armenian13Date.MaxValue

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = Armenian13Date.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = Armenian13Date.Adjuster |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<Armenian13Date, StandardArmenian13DataSet>(Armenian13Date.Adjuster)

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d)
        override __.GetDate(y, doy) = new Armenian13Date(y, doy)

    [<Sealed>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Armenian13Date, StandardArmenian13DataSet>()

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d)
