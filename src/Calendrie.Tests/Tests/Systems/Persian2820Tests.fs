﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.Persian2820Tests

#nowarn 3391 // Implicit conversion to DayNumber or GregorianDate

open Calendrie
open Calendrie.Hemerology
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

let private calendarDataSet = StandardPersian2820DataSet.Instance

module Prelude =
    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

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

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Persian2820Month(Persian2820Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Persian2820Date(y, m, d)
        let exp = new Persian2820Month(y, m)
        // Act & Assert
        new Persian2820Month(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Persian2820Year(Persian2820Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Persian2820Date(y, m, d)
        let exp = new Persian2820Year(y)
        // Act & Assert
        new Persian2820Year(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Persian2820Year(Persian2820Month)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Persian2820Month(y, m)
        let exp = new Persian2820Year(y)
        // Act & Assert
        new Persian2820Year(month) === exp

module Conversions =
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
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Persian2820Calendar, StandardPersian2820DataSet>(Persian2820Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = Persian2820Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Persian2820Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Persian2820Date, StandardPersian2820DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Persian2820Date, StandardPersian2820DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateMathFacts() =
        inherit DefaultDateMathFacts<Persian2820Date, StandardPersian2820DataSet>()

        static let defaultMath   = new DateMath()
        static let overspillMath = new DateMath(AdditionRule.Overspill)
        static let exactMath     = new DateMath(AdditionRule.Exact)

        [<Fact>]
        static member ``PlusYears() when roundof != 0`` () =
            let date = new Persian2820Date(4, 12, 30)
            // Act & Assert
            date.IsIntercalary |> ok

            let result: Persian2820Date * int = date.PlusYears(1)
            result === (new Persian2820Date(5, 12, 29), 1)

            date.PlusYears(1) === new Persian2820Date(5, 12, 29)

            defaultMath.AddYears(date, 1)   === new Persian2820Date(5, 12, 29)
            overspillMath.AddYears(date, 1) === new Persian2820Date(6, 1, 1)
            exactMath.AddYears(date, 1)     === new Persian2820Date(6, 1, 1)

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<Persian2820Month, Persian2820Date, StandardPersian2820DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new Persian2820Month(y, m)
            let date = new Persian2820Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new Persian2820Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<Persian2820Month, StandardPersian2820DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<Persian2820Year, Persian2820Month, Persian2820Date, StandardPersian2820DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new Persian2820Year(y)
            let date = new Persian2820Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new Persian2820Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new Persian2820Year(y)
            let date = new Persian2820Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new Persian2820Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
