// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Specialized.JulianTests

open Calendrie
open Calendrie.Specialized
open Calendrie.Testing
open Calendrie.Testing.Data.Unbounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Specialized

open Xunit

module Bundles =
    // NB: notice the use of UnboundedJulianDataSet.

    let private chr = new JulianCalendar()

    [<Sealed>]
    type CalendaTests() =
        inherit ICalendarTFacts<JulianDate, JulianCalendar, UnboundedJulianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new JulianDate(y, m, d);
        override __.GetDate(y, doy) = new JulianDate(y, doy);
        override __.GetDate(dayNumber) = JulianDate.FromDayNumber(dayNumber);

        [<Fact>]
        member x.MonthsInYear_Prop() = x.CalendarUT.MonthsInYear === 12

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<JulianDate, JulianCalendar, UnboundedJulianDataSet>(chr)

        override __.MinDate = JulianDate.MinValue
        override __.MaxDate = JulianDate.MaxValue

        override __.GetDate(y, m, d) = new JulianDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = JulianDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = JulianDate.Adjuster |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit SpecialAdjusterFacts<JulianDate, UnboundedJulianDataSet>(new JulianAdjuster())

        override __.GetDate(y, m, d) = new JulianDate(y, m, d)
        override __.GetDate(y, doy) = new JulianDate(y, doy)
