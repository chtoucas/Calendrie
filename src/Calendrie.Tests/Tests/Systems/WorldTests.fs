// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.WorldTests

#nowarn 3391 // Implicit conversion to DayNumber or GregorianDate

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of WorldCalendar.Epoch.DaysZinceZero`` () =
        WorldCalendar.Instance.Epoch.DaysSinceZero === -1

    [<Fact>]
    let ``default(WorldDate) is WorldCalendar.Epoch`` () =
        Unchecked.defaultof<WorldDate>.DayNumber === WorldCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of WorldCalendar.MinDaysSinceEpoch`` () =
        WorldCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of WorldCalendar.MaxDaysSinceEpoch`` () =
        WorldCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of WorldCalendar.MinMonthsSinceEpoch`` () =
        WorldCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of WorldCalendar.MaxMonthsSinceEpoch`` () =
        WorldCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

module Conversions =
    let private calendarDataSet = StandardWorldDataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = WorldDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = WorldDate.op_Explicit

    type JulianDateCaster = WorldDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = WorldDate.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new WorldDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new WorldDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at WorldDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(WorldDate.MinValue.DayNumber)

        WorldDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at WorldDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(WorldDate.MaxValue.DayNumber)

        WorldDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to WorldDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new WorldDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at WorldDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(WorldDate.MinValue.DayNumber)

        op_Explicit_Gregorian WorldDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at WorldDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(WorldDate.MaxValue.DayNumber)

        op_Explicit_Gregorian WorldDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new WorldDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at WorldDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(WorldDate.MinValue.DayNumber)

        WorldDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at WorldDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(WorldDate.MaxValue.DayNumber)

        WorldDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new WorldDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at WorldDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(WorldDate.MinValue.DayNumber)

        op_Explicit_Julian WorldDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at WorldDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(WorldDate.MaxValue.DayNumber)

        op_Explicit_Julian WorldDate.MaxValue === exp

module Methods =
    let private chr = WorldCalendar.Instance

    let dateInfoData = WorldDataSet.Instance.DateInfoData
    let moreMonthInfoData = WorldDataSet.MoreMonthInfoData

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Property IsBlank`` (info: DateInfo) =
        let (y, m, d) = info.Yemoda.Deconstruct()
        let date = new WorldDate(y, m, d)

        date.IsBlank === date.IsSupplementary

    [<Theory; MemberData(nameof(moreMonthInfoData))>]
    let ``CountDaysInWorldMonth()`` (info: YemoAnd<int>) =
        let (y, m, daysInMonth) = info.Deconstruct()

        WorldCalendar.CountDaysInWorldMonth(y, m) === daysInMonth

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<WorldCalendar, StandardWorldDataSet>(WorldCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = WorldCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = WorldCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = WorldCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<WorldDate, StandardWorldDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<WorldDate, StandardWorldDataSet>()
