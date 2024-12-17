// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.ZoroastrianTests

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Bundles =
    let private chr = ZoroastrianCalendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<ZoroastrianDate, ZoroastrianCalendar, StandardZoroastrian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d);
        override __.GetDate(y, doy) = new ZoroastrianDate(y, doy);
        override __.GetDate(dayNumber) = ZoroastrianDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = ZoroastrianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = ZoroastrianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = ZoroastrianCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<ZoroastrianDate, ZoroastrianCalendar, StandardZoroastrian12DataSet>(chr)

        override __.MinDate = ZoroastrianDate.MinValue
        override __.MaxDate = ZoroastrianDate.MaxValue

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = ZoroastrianDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = ZoroastrianDate.Adjuster |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<ZoroastrianDate, StandardZoroastrian12DataSet>(ZoroastrianDate.Adjuster)

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d)
        override __.GetDate(y, doy) = new ZoroastrianDate(y, doy)

    [<Sealed>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<ZoroastrianDate, StandardZoroastrian12DataSet>()

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d)

module Bundles13 =
    let private chr = Zoroastrian13Calendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<Zoroastrian13Date, Zoroastrian13Calendar, StandardZoroastrian13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d);
        override __.GetDate(y, doy) = new Zoroastrian13Date(y, doy);
        override __.GetDate(dayNumber) = Zoroastrian13Date.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = Zoroastrian13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Zoroastrian13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Zoroastrian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Zoroastrian13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<Zoroastrian13Date, Zoroastrian13Calendar, StandardZoroastrian13DataSet>(chr)

        override __.MinDate = Zoroastrian13Date.MinValue
        override __.MaxDate = Zoroastrian13Date.MaxValue

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = Zoroastrian13Date.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = Zoroastrian13Date.Adjuster |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>(Zoroastrian13Date.Adjuster)

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d)
        override __.GetDate(y, doy) = new Zoroastrian13Date(y, doy)

    [<Sealed>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>()

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d)
