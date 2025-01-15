// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.Persian2820Tests

#nowarn 3391 // Implicit conversion to DayNumber or GregorianDate

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of Persian2820Calendar.Epoch.DaysZinceZero`` () =
        Persian2820Calendar.Instance.Epoch.DaysSinceZero === 226_895

    [<Fact>]
    let ``default(Persian2820Date) is Persian2820Calendar.Epoch`` () =
        Unchecked.defaultof<Persian2820Date>.DayNumber === Persian2820Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of Persian2820Calendar.MinDaysSinceEpoch`` () =
        Persian2820Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of Persian2820Calendar.MaxDaysSinceEpoch`` () =
        Persian2820Calendar.Instance.MaxDaysSinceEpoch === 3_652_055

    [<Fact>]
    let ``Value of Persian2820Calendar.MinMonthsSinceEpoch`` () =
        Persian2820Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of Persian2820Calendar.MaxMonthsSinceEpoch`` () =
        Persian2820Calendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

module Conversions =
    let private calendarDataSet = StandardPersian2820DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Persian2820Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = Persian2820Date.op_Explicit

    type JulianDateCaster = Persian2820Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = Persian2820Date.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new Persian2820Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Persian2820Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Persian2820Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Persian2820Date.MinValue.DayNumber)

        Persian2820Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Persian2820Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Persian2820Date.MaxValue.DayNumber)

        Persian2820Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Persian2820Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Persian2820Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Persian2820Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Persian2820Date.MinValue.DayNumber)

        op_Explicit_Gregorian Persian2820Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Persian2820Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Persian2820Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Persian2820Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Persian2820Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Persian2820Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Persian2820Date.MinValue.DayNumber)

        Persian2820Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Persian2820Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Persian2820Date.MaxValue.DayNumber)

        Persian2820Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Persian2820Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Persian2820Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Persian2820Date.MinValue.DayNumber)

        op_Explicit_Julian Persian2820Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Persian2820Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Persian2820Date.MaxValue.DayNumber)

        op_Explicit_Julian Persian2820Date.MaxValue === exp

module Bundles =
    let private chr = Persian2820Calendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Persian2820Calendar, StandardPersian2820DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = Persian2820Calendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = Persian2820Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Persian2820Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Persian2820Date, StandardPersian2820DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Persian2820Date, StandardPersian2820DataSet>()
