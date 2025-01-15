// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.TabularIslamicTests

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
    let ``Value of TabularIslamicCalendar.Epoch.DaysZinceZero`` () =
        TabularIslamicCalendar.Instance.Epoch.DaysSinceZero === 227_014

    [<Fact>]
    let ``default(TabularIslamicDate) is TabularIslamicCalendar.Epoch`` () =
        Unchecked.defaultof<TabularIslamicDate>.DayNumber === TabularIslamicCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of TabularIslamicCalendar.MinDaysSinceEpoch`` () =
        TabularIslamicCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of TabularIslamicCalendar.MaxDaysSinceEpoch`` () =
        TabularIslamicCalendar.Instance.MaxDaysSinceEpoch === 3_543_311

    [<Fact>]
    let ``Value of TabularIslamicCalendar.MinMonthsSinceEpoch`` () =
        TabularIslamicCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of TabularIslamicCalendar.MaxMonthsSinceEpoch`` () =
        TabularIslamicCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

module Conversions =
    let private calendarDataSet = StandardTabularIslamicDataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = TabularIslamicDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = TabularIslamicDate.op_Explicit

    type JulianDateCaster = TabularIslamicDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = TabularIslamicDate.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new TabularIslamicDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new TabularIslamicDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at TabularIslamicDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(TabularIslamicDate.MinValue.DayNumber)

        TabularIslamicDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at TabularIslamicDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(TabularIslamicDate.MaxValue.DayNumber)

        TabularIslamicDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to TabularIslamicDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new TabularIslamicDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at TabularIslamicDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(TabularIslamicDate.MinValue.DayNumber)

        op_Explicit_Gregorian TabularIslamicDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at TabularIslamicDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(TabularIslamicDate.MaxValue.DayNumber)

        op_Explicit_Gregorian TabularIslamicDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new TabularIslamicDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at TabularIslamicDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(TabularIslamicDate.MinValue.DayNumber)

        TabularIslamicDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at TabularIslamicDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(TabularIslamicDate.MaxValue.DayNumber)

        TabularIslamicDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new TabularIslamicDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at TabularIslamicDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(TabularIslamicDate.MinValue.DayNumber)

        op_Explicit_Julian TabularIslamicDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at TabularIslamicDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(TabularIslamicDate.MaxValue.DayNumber)

        op_Explicit_Julian TabularIslamicDate.MaxValue === exp

module Bundles =
    let private chr = TabularIslamicCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<TabularIslamicCalendar, StandardTabularIslamicDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Lunar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = TabularIslamicCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = TabularIslamicCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = TabularIslamicCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<TabularIslamicDate, StandardTabularIslamicDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<TabularIslamicDate, StandardTabularIslamicDataSet>()
