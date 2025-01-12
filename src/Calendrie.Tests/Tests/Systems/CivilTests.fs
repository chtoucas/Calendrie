// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.CivilTests

#nowarn 3391 // Implicit conversion from CivilDate to GregorianDate

open System

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

open type Calendrie.Extensions.CivilDateExtensions

// NB: notice the use of StandardGregorianDataSet.

module Prelude =
    let private calendarDataSet = StandardGregorianDataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let daysSinceEpochInfoData = calendarDataSet.DaysSinceEpochInfoData

    [<Fact>]
    let ``Value of CivilCalendar.Epoch.DaysZinceZero`` () =
        CivilCalendar.Instance.Epoch.DaysSinceZero === 0

    [<Fact>]
    let ``default(CivilDate) is CivilCalendar.Epoch`` () =
        Unchecked.defaultof<CivilDate>.DayNumber === CivilCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of CivilCalendar.MinDaysSinceEpoch`` () =
        CivilCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of CivilCalendar.MaxDaysSinceEpoch`` () =
        CivilCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of CivilCalendar.MinMonthsSinceEpoch`` () =
        CivilCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of CivilCalendar.MaxMonthsSinceEpoch`` () =
        CivilCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

    [<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
    let ``Property DaysSinceZero`` (info: DaysSinceEpochInfo) =
        let (daysSinceEpoch, y, m, d) = info.Deconstruct()
        let date = new CivilDate(y, m, d)

        date.DaysSinceZero === daysSinceEpoch

    //
    // Conversions
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let civilDate = new CivilDate(y, m, d)
        let gregorianDate = new GregorianDate(y, m, d)

        civilDate.ToGregorianDate() === gregorianDate

    [<Fact>]
    let ``ToGregorianDate() at CivilDate:MaxValue`` () =
        let civilDate = CivilDate.MaxValue
        let y, m, d = civilDate.Deconstruct()
        let gregorianDate = new GregorianDate(y, m, d)

        civilDate.ToGregorianDate() === gregorianDate

    [<Fact>]
    let ``ToGregorianDate() at CivilDate:MinValue`` () =
        let civilDate = CivilDate.MinValue
        let y, m, d = civilDate.Deconstruct()
        let gregorianDate = new GregorianDate(y, m, d)

        civilDate.ToGregorianDate() === gregorianDate

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Implicit conversion to GregorianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let civilDate : GregorianDate = new CivilDate(y, m, d)
        let gregorianDate = new GregorianDate(y, m, d)

        civilDate === gregorianDate

    [<Fact>]
    let ``Implicit conversion to GregorianDate at CivilDate:MaxValue`` () =
        let civilDate : GregorianDate = CivilDate.MaxValue
        let y, m, d = CivilDate.MaxValue.Deconstruct()
        let gregorianDate = new GregorianDate(y, m, d)

        civilDate === gregorianDate

    [<Fact>]
    let ``Implicit conversion to GregorianDate at CivilDate:MinValue`` () =
        let civilDate : GregorianDate = CivilDate.MinValue
        let y, m, d = CivilDate.MinValue.Deconstruct()
        let gregorianDate = new GregorianDate(y, m, d)

        civilDate === gregorianDate

    //[<Theory; MemberData(nameof(dateInfoData))>]
    //let ``GregorianDate:FromCivilDate()`` (x: DateInfo) =
    //    let y, m, d, _ = x.Deconstruct()
    //    let gregorianDate = new GregorianDate(y, m, d)
    //    let civilDate = new CivilDate(y, m, d)

    //    GregorianDate.FromCivilDate(civilDate) === gregorianDate

    //[<Fact>]
    //let ``GregorianDate:FromCivilDate(CivilDate:MaxValue)`` () =
    //    let civilDate = CivilDate.MaxValue
    //    let y, m, d = civilDate.Deconstruct()
    //    let gregorianDate = new GregorianDate(y, m, d)

    //    GregorianDate.FromCivilDate(civilDate) === gregorianDate

    //[<Fact>]
    //let ``GregorianDate:FromCivilDate(CivilDate:MinValue)`` () =
    //    let civilDate = CivilDate.MinValue
    //    let y, m, d = civilDate.Deconstruct()
    //    let gregorianDate = new GregorianDate(y, m, d)

    //    GregorianDate.FromCivilDate(civilDate) === gregorianDate

module Extensions =
    let private chr = CivilCalendar.Instance
    let private domain = chr.Scope.Domain

    let private calendarDataSet = StandardGregorianDataSet.Instance
    let dayOfWeekData = calendarDataSet.DayOfWeekData
    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(domain)

    //
    // GetDayOfWeek()
    //

    [<Theory; MemberData(nameof(dayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``CivilDate:GetDayOfWeek()`` (info: YemodaAnd<DayOfWeek>) =
        let (y, m, d, dayOfWeek) = info.Deconstruct()
        let date = new CivilDate(y, m, d)

        date.GetDayOfWeek() === dayOfWeek

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``CivilDate:GetDayOfWeek() via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = CivilDate.FromDayNumber(dayNumber)

        date.GetDayOfWeek() === dayOfWeek

module Bundles =
    let private chr = CivilCalendar.Instance

    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<CivilCalendar, StandardGregorianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = CivilCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = CivilCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = CivilCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<CivilDate, CivilCalendar, StandardGregorianDataSet>(chr)

        override __.MinDate = CivilDate.MinValue
        override __.MaxDate = CivilDate.MaxValue

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)

        [<Fact>]
        static member Calendar_Prop() = CivilDate.Calendar |> isnotnull

    [<Sealed>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<CivilDate, StandardGregorianDataSet>(CivilCalendar.Instance)

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)
        override __.GetDate(y, doy) = new CivilDate(y, doy)

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<CivilDate, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)
