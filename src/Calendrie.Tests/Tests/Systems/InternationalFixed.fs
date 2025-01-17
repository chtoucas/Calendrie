// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.InternationalFixedTests

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
    let ``Value of InternationalFixedCalendar.Epoch.DaysZinceZero`` () =
        InternationalFixedCalendar.Instance.Epoch.DaysSinceZero === 0

    [<Fact>]
    let ``default(InternationalFixedDate) is InternationalFixedCalendar.Epoch`` () =
        Unchecked.defaultof<InternationalFixedDate>.DayNumber === InternationalFixedCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of InternationalFixedCalendar.MinDaysSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of InternationalFixedCalendar.MaxDaysSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of InternationalFixedCalendar.MinMonthsSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of InternationalFixedCalendar.MaxMonthsSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Conversions =
    let private calendarDataSet = StandardInternationalFixedDataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = InternationalFixedDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = InternationalFixedDate.op_Explicit

    type JulianDateCaster = InternationalFixedDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = InternationalFixedDate.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new InternationalFixedDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new InternationalFixedDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at InternationalFixedDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(InternationalFixedDate.MinValue.DayNumber)

        InternationalFixedDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at InternationalFixedDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(InternationalFixedDate.MaxValue.DayNumber)

        InternationalFixedDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to InternationalFixedDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new InternationalFixedDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at InternationalFixedDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(InternationalFixedDate.MinValue.DayNumber)

        op_Explicit_Gregorian InternationalFixedDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at InternationalFixedDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(InternationalFixedDate.MaxValue.DayNumber)

        op_Explicit_Gregorian InternationalFixedDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new InternationalFixedDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at InternationalFixedDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(InternationalFixedDate.MinValue.DayNumber)

        InternationalFixedDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at InternationalFixedDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(InternationalFixedDate.MaxValue.DayNumber)

        InternationalFixedDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new InternationalFixedDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at InternationalFixedDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(InternationalFixedDate.MinValue.DayNumber)

        op_Explicit_Julian InternationalFixedDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at InternationalFixedDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(InternationalFixedDate.MaxValue.DayNumber)

        op_Explicit_Julian InternationalFixedDate.MaxValue === exp

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<InternationalFixedCalendar, StandardInternationalFixedDataSet>(InternationalFixedCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = InternationalFixedCalendar.MonthsInYear === 13

        [<Fact>]
        static member MinYear() = InternationalFixedCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = InternationalFixedCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<InternationalFixedDate, StandardInternationalFixedDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<InternationalFixedDate, StandardInternationalFixedDataSet>()
