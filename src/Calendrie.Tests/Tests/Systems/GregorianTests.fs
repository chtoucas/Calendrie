// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.GregorianTests

open System

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Unbounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

open type Calendrie.Extensions.GregorianDateExtensions

// NB: notice the use of UnboundedGregorianDataSet.

module Prelude =
    let private calendarDataSet = UnboundedGregorianDataSet.Instance

    let daysSinceEpochInfoData = calendarDataSet.DaysSinceEpochInfoData

    [<Fact>]
    let ``Value of GregorianDate.MinDaysSinceZero`` () =
        GregorianDate.MinDaysSinceZero === GregorianCalendar.UnderlyingScope.Segment.SupportedDays.Min

    [<Fact>]
    let ``Value of GregorianDate.MaxDaysSinceZero`` () =
        GregorianDate.MaxDaysSinceZero === GregorianCalendar.UnderlyingScope.Segment.SupportedDays.Max

    [<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
    let ``Property DaysSinceZero`` (info: DaysSinceEpochInfo) =
        let (daysSinceEpoch, y, m, d) = info.Deconstruct()
        let date = new GregorianDate(y, m, d)

        date.DaysSinceZero === daysSinceEpoch

module Extensions =
    let private chr = GregorianCalendar.Instance
    let private domain = chr.Scope.Domain

    let private calendarDataSet = StandardGregorianDataSet.Instance
    let dayOfWeekData = calendarDataSet.DayOfWeekData
    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(domain)

    //
    // GetDayOfWeek()
    //

    [<Theory; MemberData(nameof(dayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``GregorianDate:GetDayOfWeek()`` (info: YemodaAnd<DayOfWeek>) =
        let (y, m, d, dayOfWeek) = info.Deconstruct()
        let date = new GregorianDate(y, m, d)

        date.GetDayOfWeek() === dayOfWeek

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``GregorianDate:GetDayOfWeek() via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = GregorianDate.FromDayNumber(dayNumber)

        date.GetDayOfWeek() === dayOfWeek

module Bundles =
    let private chr = GregorianCalendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<GregorianDate, GregorianCalendar, UnboundedGregorianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d);
        override __.GetDate(y, doy) = new GregorianDate(y, doy);
        override __.GetDate(dayNumber) = GregorianDate.FromDayNumber(dayNumber);

        [<Fact>]
        static member MonthsInYear() = GregorianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = GregorianCalendar.MinYear === ProlepticScope.MinYear

        [<Fact>]
        static member MaxYear() = GregorianCalendar.MaxYear === ProlepticScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<GregorianDate, GregorianCalendar, UnboundedGregorianDataSet>(chr)

        override __.MinDate = GregorianDate.MinValue
        override __.MaxDate = GregorianDate.MaxValue

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = GregorianDate.Calendar |> isnotnull

        [<Fact>]
        static member Adjuster_Prop() = GregorianDate.Adjuster |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<GregorianDate, UnboundedGregorianDataSet>(GregorianDate.Adjuster)

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)
        override __.GetDate(y, doy) = new GregorianDate(y, doy)

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<GregorianDate, UnboundedGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)
