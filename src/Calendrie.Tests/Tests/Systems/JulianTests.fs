﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.JulianTests

open System

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Unbounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

open type Calendrie.Extensions.JulianDateExtensions

#if DEBUG
module Prelude =
    [<Fact>]
    let ``Value of JulianDate.MinDaysSinceEpoch`` () =
        // C# protected internal
        //JulianDate.MinDaysSinceEpoch === JulianCalendar.Instance.Scope.Segment.SupportedDays.Min
        JulianDate.MinDaysSinceEpoch === JulianCalendar.Instance.Segment.SupportedDays.Min

    [<Fact>]
    let ``Value of JulianDate.MaxDaysSinceEpoch`` () =
        // C# protected internal
        //JulianDate.MaxDaysSinceEpoch === JulianCalendar.Instance.Scope.Segment.SupportedDays.Max
        JulianDate.MaxDaysSinceEpoch === JulianCalendar.Instance.Segment.SupportedDays.Max
#endif

module Extensions =
    let private chr = JulianCalendar.Instance
    let private domain = chr.Scope.Domain

    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(domain)

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``GetDayOfWeek(CalendarDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = JulianDate.FromDayNumber(dayNumber)

        date.GetDayOfWeek() === dayOfWeek

module Bundles =
    // NB: notice the use of UnboundedJulianDataSet.

    let private chr = JulianCalendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<JulianDate, JulianCalendar, UnboundedJulianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new JulianDate(y, m, d);
        override __.GetDate(y, doy) = new JulianDate(y, doy);
        override __.GetDate(dayNumber) = JulianDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = JulianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = JulianCalendar.MinYear === ProlepticScope.MinYear

        [<Fact>]
        static member MaxYear() = JulianCalendar.MaxYear === ProlepticScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<JulianDate, JulianCalendar, UnboundedJulianDataSet>(chr)

        override __.MinDate = JulianDate.MinValue
        override __.MaxDate = JulianDate.MaxValue

        override __.GetDate(y, m, d) = new JulianDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = JulianDate.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<JulianDate, UnboundedJulianDataSet>(JulianDate.Adjuster)

        override __.GetDate(y, m, d) = new JulianDate(y, m, d)
        override __.GetDate(y, doy) = new JulianDate(y, doy)
