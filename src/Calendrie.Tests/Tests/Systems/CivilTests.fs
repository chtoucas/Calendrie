// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.CivilTests

#nowarn 3391 // Implicit conversion to DayNumber or GregorianDate

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

let private chr = CivilCalendar.Instance
let private calendarDataSet = StandardGregorianDataSet.Instance

module Prelude =
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
    let ``Property DaysSinceZero`` (x: DaysSinceEpochInfo) =
        let (daysSinceEpoch, y, m, d) = x.Deconstruct()
        let date = new CivilDate(y, m, d)

        date.DaysSinceZero === daysSinceEpoch

module Factories =
    let dateInfoData = calendarDataSet.DateInfoData

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``UnsafeCreate(y, m, d)`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        // Act
        let date = CivilDate.UnsafeCreate(y, m, d)
        // Assert
        date.Year      === y
        date.Month     === m
        date.Day       === d
        date.DayOfYear === doy

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``UnsafeCreate(y, doy)`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        // Act
        let date = CivilDate.UnsafeCreate(y, doy)
        // Assert
        date.Year      === y
        date.Month     === m
        date.Day       === d
        date.DayOfYear === doy

module Conversions =
    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new CivilDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CivilDate(y, m, d)
        let exp = new GregorianDate(y, m, d)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at CivilDate:MaxValue`` () =
        let y, m, d = CivilDate.MaxValue.Deconstruct()
        let exp = new GregorianDate(y, m, d)

        CivilDate.MaxValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at CivilDate:MinValue`` () =
        let y, m, d = CivilDate.MinValue.Deconstruct()
        let exp = new GregorianDate(y, m, d)

        CivilDate.MinValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Implicit conversion to GregorianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CivilDate(y, m, d)
        let exp = new GregorianDate(y, m, d)

        (date : GregorianDate) === exp

    [<Fact>]
    let ``Implicit conversion to GregorianDate at CivilDate:MaxValue`` () =
        let y, m, d = CivilDate.MaxValue.Deconstruct()
        let exp = new GregorianDate(y, m, d)

        (CivilDate.MaxValue : GregorianDate) === exp

    [<Fact>]
    let ``Implicit conversion to GregorianDate at CivilDate:MinValue`` () =
        let y, m, d = CivilDate.MinValue.Deconstruct()
        let exp = new GregorianDate(y, m, d)

        (CivilDate.MinValue : GregorianDate) === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CivilDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at CivilDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CivilDate.MaxValue.DayNumber)

        CivilDate.MaxValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at CivilDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CivilDate.MinValue.DayNumber)

        CivilDate.MinValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CivilDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        CivilDate.op_Explicit date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at CivilDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CivilDate.MaxValue.DayNumber)

        CivilDate.op_Explicit CivilDate.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at CivilDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CivilDate.MinValue.DayNumber)

        CivilDate.op_Explicit CivilDate.MinValue === exp

module Extensions =
    let dayOfWeekData = calendarDataSet.DayOfWeekData
    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(chr.Scope.Domain)

    //
    // GetDayOfWeek() via DoomsdayRule
    //

    [<Theory; MemberData(nameof(dayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``CivilDate:GetDayOfWeek()`` (x: YemodaAnd<DayOfWeek>) =
        let (y, m, d, dayOfWeek) = x.Deconstruct()
        let date = new CivilDate(y, m, d)

        date.GetDayOfWeek() === dayOfWeek

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``CivilDate:GetDayOfWeek() via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = CivilDate.FromAbsoluteDate(dayNumber)

        date.GetDayOfWeek() === dayOfWeek

module Bundles =
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
        inherit IDateFacts<CivilDate, StandardGregorianDataSet>()

    [<Sealed>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<CivilDate, StandardGregorianDataSet>()

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<CivilDate, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)
