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

module Prelude =
    // Test for Benchmars.PlainJulian
    [<Fact>]
    let ``Value of PlainJulian.MaxDaysSinceEpoch`` () =
        let date = new JulianDate(9999, 1, 1)
        date.DaysSinceEpoch === 3_651_769

    [<Fact>]
    let ``Value of JulianCalendar.Epoch.DaysZinceZero`` () =
        JulianCalendar.Instance.Epoch.DaysSinceZero === -2

#if DEBUG
    [<Fact>]
    let ``Value of JulianCalendar.MinDaysSinceEpoch`` () =
        JulianCalendar.Instance.MinDaysSinceEpoch === -365_249_635

    [<Fact>]
    let ``Value of JulianCalendar.MaxDaysSinceEpoch`` () =
        JulianCalendar.Instance.MaxDaysSinceEpoch === 365_249_633
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
        static member Calendar_Prop() = JulianDate.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<JulianDate, UnboundedJulianDataSet>(JulianCalendar.Instance)

        override __.GetDate(y, m, d) = new JulianDate(y, m, d)
        override __.GetDate(y, doy) = new JulianDate(y, doy)
