﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Specialized.TabularIslamicTests

open Calendrie
open Calendrie.Specialized
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Specialized

open Xunit

module Bundles =
    let private chr = new TabularIslamicCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<TabularIslamicDate, TabularIslamicCalendar, StandardTabularIslamicDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Lunar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new TabularIslamicDate(y, m, d);
        override __.GetDate(y, doy) = new TabularIslamicDate(y, doy);
        override __.GetDate(dayNumber) = TabularIslamicDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = TabularIslamicCalendar.MonthsInYear === 12

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<TabularIslamicDate, TabularIslamicCalendar, StandardTabularIslamicDataSet>(chr)

        override __.MinDate = TabularIslamicDate.MinValue
        override __.MaxDate = TabularIslamicDate.MaxValue

        override __.GetDate(y, m, d) = new TabularIslamicDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = TabularIslamicDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = TabularIslamicDate.Adjuster |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit SpecialAdjusterFacts<TabularIslamicDate, StandardTabularIslamicDataSet>(TabularIslamicDate.Adjuster)

        override __.GetDate(y, m, d) = new TabularIslamicDate(y, m, d)
        override __.GetDate(y, doy) = new TabularIslamicDate(y, doy)
