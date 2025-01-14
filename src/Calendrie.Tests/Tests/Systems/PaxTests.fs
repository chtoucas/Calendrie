// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.PaxTests

#nowarn 3391 // Implicit conversion to DayNumber or GregorianDate

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of PaxCalendar.Epoch.DaysZinceZero`` () =
        PaxCalendar.Instance.Epoch.DaysSinceZero === -1

    [<Fact>]
    let ``default(PaxDate) is PaxCalendar.Epoch`` () =
        Unchecked.defaultof<PaxDate>.DayNumber === PaxCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of PaxCalendar.MinDaysSinceEpoch`` () =
        PaxCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of PaxCalendar.MaxDaysSinceEpoch`` () =
        PaxCalendar.Instance.MaxDaysSinceEpoch === 3_652_060

    [<Fact>]
    let ``Value of PaxCalendar.MinMonthsSinceEpoch`` () =
        PaxCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of PaxCalendar.MaxMonthsSinceEpoch`` () =
        PaxCalendar.Instance.MaxMonthsSinceEpoch === 131_761
#endif

module Conversions =
    let private calendarDataSet = StandardPaxDataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = PaxDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = PaxDate.op_Explicit

    type JulianDateCaster = PaxDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = PaxDate.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new PaxDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PaxDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at PaxDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PaxDate.MaxValue.DayNumber)

        PaxDate.MaxValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at PaxDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PaxDate.MinValue.DayNumber)

        PaxDate.MinValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to PaxDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PaxDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at PaxDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PaxDate.MaxValue.DayNumber)

        op_Explicit_Gregorian PaxDate.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at PaxDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PaxDate.MinValue.DayNumber)

        op_Explicit_Gregorian PaxDate.MinValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PaxDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at PaxDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PaxDate.MaxValue.DayNumber)

        PaxDate.MaxValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at PaxDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PaxDate.MinValue.DayNumber)

        PaxDate.MinValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PaxDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at PaxDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PaxDate.MaxValue.DayNumber)

        op_Explicit_Julian PaxDate.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at PaxDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PaxDate.MinValue.DayNumber)

        op_Explicit_Julian PaxDate.MinValue === exp

module Bundles =
    let private chr = PaxCalendar.Instance

    let dateInfoData = PaxDataSet.Instance.DateInfoData

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<PaxCalendar, StandardPaxDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Other
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Weeks

        [<Fact>]
        static member MinYear() = PaxCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = PaxCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<PaxDate, StandardPaxDataSet>()
