﻿// SPDX-License-Identifier: BSD-3-Clause
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
    let ``Value of GregorianCalendar.Epoch.DaysZinceZero`` () =
        GregorianCalendar.Instance.Epoch.DaysSinceZero === 0

#if DEBUG
    [<Fact>]
    let ``Value of GregorianCalendar.MinDaysSinceEpoch`` () =
        GregorianCalendar.Instance.MinDaysSinceEpoch === -365_242_135

    [<Fact>]
    let ``Value of GregorianCalendar.MaxDaysSinceEpoch`` () =
        GregorianCalendar.Instance.MaxDaysSinceEpoch === 365_242_133
#endif

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

        override x.ToString_InvariantCulture () =
            let date = x.GetDate(1, 1, 1);
            let exp = FormattableString.Invariant($"01/01/0001 ({chr})");
            date.ToString() === exp

        member x.ToString_InvariantCulture2 () =
            let date = x.GetDate(999_999, 1, 1);
            let exp = FormattableString.Invariant($"01/01/999999 ({chr})");
            date.ToString() === exp

        member x.ToString_InvariantCulture3 () =
            let date = x.GetDate(0, 1, 1);
            let exp = FormattableString.Invariant($"01/01/1 BCE ({chr})");
            date.ToString() === exp

        member x.ToString_InvariantCulture4 () =
            let date = x.GetDate(-999_998, 1, 1);
            let exp = FormattableString.Invariant($"01/01/999999 BCE ({chr})");
            date.ToString() === exp

        [<Fact>]
        static member Calendar_Prop() = GregorianDate.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<GregorianDate, UnboundedGregorianDataSet>(GregorianCalendar.Instance)

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)
        override __.GetDate(y, doy) = new GregorianDate(y, doy)

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<GregorianDate, UnboundedGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)
