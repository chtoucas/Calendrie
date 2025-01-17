// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.PositivistTests

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
    let ``Value of PositivistCalendar.Epoch.DaysZinceZero`` () =
        PositivistCalendar.Instance.Epoch.DaysSinceZero === 653_054

    [<Fact>]
    let ``default(PositivistDate) is PositivistCalendar.Epoch`` () =
        Unchecked.defaultof<PositivistDate>.DayNumber === PositivistCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of PositivistCalendar.MinDaysSinceEpoch`` () =
        PositivistCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of PositivistCalendar.MaxDaysSinceEpoch`` () =
        PositivistCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of PositivistCalendar.MinMonthsSinceEpoch`` () =
        PositivistCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of PositivistCalendar.MaxMonthsSinceEpoch`` () =
        PositivistCalendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Conversions =
    let private calendarDataSet = StandardPositivistDataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = PositivistDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = PositivistDate.op_Explicit

    type JulianDateCaster = PositivistDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = PositivistDate.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new PositivistDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PositivistDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at PositivistDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PositivistDate.MinValue.DayNumber)

        PositivistDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at PositivistDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PositivistDate.MaxValue.DayNumber)

        PositivistDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to PositivistDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PositivistDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at PositivistDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PositivistDate.MinValue.DayNumber)

        op_Explicit_Gregorian PositivistDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at PositivistDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PositivistDate.MaxValue.DayNumber)

        op_Explicit_Gregorian PositivistDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PositivistDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at PositivistDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PositivistDate.MinValue.DayNumber)

        PositivistDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at PositivistDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PositivistDate.MaxValue.DayNumber)

        PositivistDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PositivistDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at PositivistDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PositivistDate.MinValue.DayNumber)

        op_Explicit_Julian PositivistDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at PositivistDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PositivistDate.MaxValue.DayNumber)

        op_Explicit_Julian PositivistDate.MaxValue === exp

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<PositivistCalendar, StandardPositivistDataSet>(PositivistCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = PositivistCalendar.MonthsInYear === 13

        [<Fact>]
        static member MinYear() = PositivistCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = PositivistCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<PositivistDate, StandardPositivistDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<PositivistDate, StandardPositivistDataSet>()
