// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.TropicaliaTests

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

let private calendarDataSet = StandardTropicaliaDataSet.Instance

module Prelude =
    [<Fact>]
    let ``Value of TropicaliaCalendar.Epoch.DaysZinceZero`` () =
        TropicaliaCalendar.Instance.Epoch.DaysSinceZero === 0

    [<Fact>]
    let ``default(TropicaliaDate) is TropicaliaCalendar.Epoch`` () =
        Unchecked.defaultof<TropicaliaDate>.DayNumber === TropicaliaCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of TropicaliaCalendar.MinDaysSinceEpoch`` () =
        TropicaliaCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of TropicaliaCalendar.MaxDaysSinceEpoch`` () =
        TropicaliaCalendar.Instance.MaxDaysSinceEpoch === 3_652_055

    [<Fact>]
    let ``Value of TropicaliaCalendar.MinMonthsSinceEpoch`` () =
        TropicaliaCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of TropicaliaCalendar.MaxMonthsSinceEpoch`` () =
        TropicaliaCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

module Conversions =
    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = TropicaliaDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = TropicaliaDate.op_Explicit

    type JulianDateCaster = TropicaliaDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = TropicaliaDate.op_Explicit

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = TropicaliaDate.Create(y, m, d)
        let exp = TropicaliaMonth.Create(y, m)
        // Act & Assert
        TropicaliaMonth.FromDate(date) === exp

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new TropicaliaDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new TropicaliaDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at TropicaliaDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(TropicaliaDate.MinValue.DayNumber)

        TropicaliaDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at TropicaliaDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(TropicaliaDate.MaxValue.DayNumber)

        TropicaliaDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to TropicaliaDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new TropicaliaDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at TropicaliaDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(TropicaliaDate.MinValue.DayNumber)

        op_Explicit_Gregorian TropicaliaDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at TropicaliaDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(TropicaliaDate.MaxValue.DayNumber)

        op_Explicit_Gregorian TropicaliaDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new TropicaliaDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at TropicaliaDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(TropicaliaDate.MinValue.DayNumber)

        TropicaliaDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at TropicaliaDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(TropicaliaDate.MaxValue.DayNumber)

        TropicaliaDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new TropicaliaDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at TropicaliaDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(TropicaliaDate.MinValue.DayNumber)

        op_Explicit_Julian TropicaliaDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at TropicaliaDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(TropicaliaDate.MaxValue.DayNumber)

        op_Explicit_Julian TropicaliaDate.MaxValue === exp

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<TropicaliaCalendar, StandardTropicaliaDataSet>(TropicaliaCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = TropicaliaCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = TropicaliaCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<TropicaliaDate, StandardTropicaliaDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<TropicaliaDate, StandardTropicaliaDataSet>()

    //
    // Month and year types
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<TropicaliaMonth, TropicaliaDate, StandardTropicaliaDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<TropicaliaMonth, StandardTropicaliaDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<TropicaliaYear, TropicaliaMonth, TropicaliaDate, StandardTropicaliaDataSet>()

    //
    // Math
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<TropicaliaDate, StandardTropicaliaDataSet>(
            new TropicaliaDateMath(AdditionRule.Truncate))
