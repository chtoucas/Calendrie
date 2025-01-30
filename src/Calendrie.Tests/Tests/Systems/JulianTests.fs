// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.JulianTests

#nowarn 3391 // Implicit conversion to DayNumber or GregorianDate

open System

open Calendrie
open Calendrie.Hemerology
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Unbounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

open type Calendrie.Extensions.JulianDateExtensions

// NB: notice the use of UnboundedJulianDataSet.
let private calendarDataSet = UnboundedJulianDataSet.Instance

module Prelude =
    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    // Test for Benchmars.PlainJulian
    [<Fact>]
    let ``Value of PlainJulian.MaxDaysSinceEpoch`` () =
        let date = new JulianDate(9999, 1, 1)
        date.DaysSinceEpoch === 3_651_769

    [<Fact>]
    let ``Value of JulianCalendar.Epoch.DaysZinceZero`` () =
        JulianCalendar.Instance.Epoch.DaysSinceZero === -2

#if DEBUG
    [<Fact>]
    let ``Value of JulianCalendar.MinDaysSinceEpoch`` () =
        JulianCalendar.Instance.MinDaysSinceEpoch === -365_249_635

    [<Fact>]
    let ``Value of JulianCalendar.MaxDaysSinceEpoch`` () =
        JulianCalendar.Instance.MaxDaysSinceEpoch === 365_249_633

    [<Fact>]
    let ``Value of JulianCalendar.MinMonthsSinceEpoch`` () =
        JulianCalendar.Instance.MinMonthsSinceEpoch === -11_999_988

    [<Fact>]
    let ``Value of JulianCalendar.MaxMonthsSinceEpoch`` () =
        JulianCalendar.Instance.MaxMonthsSinceEpoch === 11_999_987
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``JulianMonth(JulianDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new JulianDate(y, m, d)
        let exp = new JulianMonth(y, m)
        // Act & Assert
        new JulianMonth(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``JulianYear(JulianDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new JulianDate(y, m, d)
        let exp = new JulianYear(y)
        // Act & Assert
        new JulianYear(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``JulianYear(JulianMonth)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new JulianMonth(y, m)
        let exp = new JulianYear(y)
        // Act & Assert
        new JulianYear(month) === exp

module Factories =
    let dateInfoData = calendarDataSet.DateInfoData

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``UnsafeCreate(y, m, d)`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        // Act
        let date = JulianDate.UnsafeCreate(y, m, d)
        // Assert
        date.Year      === y
        date.Month     === m
        date.Day       === d
        date.DayOfYear === doy

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``UnsafeCreate(y, doy)`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        // Act
        let date = JulianDate.UnsafeCreate(y, doy)
        // Assert
        date.Year      === y
        date.Month     === m
        date.Day       === d
        date.DayOfYear === doy

module Conversions =
    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    // NB: FromAbsoluteDate(GregorianDate) is tested in GregorianTests alongside ToJulianDate().

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new JulianDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Fact>]
    let ``Conversion to a GregorianDate value overflows at JulianDate:MinValue`` () =
        JulianDate.MinValue.DayNumber < GregorianDate.MinValue.DayNumber |> ok

        (fun () -> JulianDate.MinValue.ToGregorianDate())      |> overflows
        (fun () -> JulianDate.op_Explicit JulianDate.MinValue) |> overflows

    [<Fact>]
    let ``Conversion to a GregorianDate value overflows at JulianDate:MaxValue`` () =
        JulianDate.MaxValue.DayNumber > GregorianDate.MaxValue.DayNumber |> ok

        (fun () -> JulianDate.MaxValue.ToGregorianDate())      |> overflows
        (fun () -> JulianDate.op_Explicit JulianDate.MaxValue) |> overflows

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new JulianDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to GregorianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new JulianDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        JulianDate.op_Explicit date === exp

module Extensions =
    let private chr = JulianCalendar.Instance

    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(chr.Scope.Domain)

    //
    // GetDayOfWeek() via DoomsdayRule
    //

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``GetDayOfWeek(CalendarDate) via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = JulianDate.FromAbsoluteDate(dayNumber)

        date.GetDayOfWeek() === dayOfWeek

module Bundles =
    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<JulianCalendar, UnboundedJulianDataSet>(JulianCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = JulianCalendar.MinYear === JulianScope.MinYear

        [<Fact>]
        static member MaxYear() = JulianCalendar.MaxYear === JulianScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<JulianDate, UnboundedJulianDataSet>()

        [<Fact>]
        static member ``ToString() 31/12/999999`` () =
            let date = new JulianDate(999_999, 12, 31)
            date.ToString() === "31/12/999999 (Julian)"

        [<Fact>]
        static member ``ToString() 31/12/0001 BCE`` () =
            let date = new JulianDate(0, 12, 31)
            date.ToString() === "31/12/0001 BCE (Julian)"

        [<Fact>]
        static member ``ToString() 01/01/999999 BCE`` () =
            let date = new JulianDate(-999_998, 1, 1)
            date.ToString() === "01/01/999999 BCE (Julian)"

    [<Sealed>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<JulianDate, UnboundedJulianDataSet>()

    [<Sealed>]
    type DateMathFacts() =
        inherit DefaultDateMathFacts<JulianDate, UnboundedJulianDataSet>()

        static let defaultMath   = new DateMath()
        static let overspillMath = new DateMath(AdditionRule.Overspill)
        static let exactMath     = new DateMath(AdditionRule.Exact)

        [<Fact>]
        static member ``PlusYears() when roundof != 0`` () =
            let date = new JulianDate(4, 2, 29)
            // Act & Assert
            date.IsIntercalary |> ok

            let result: JulianDate * int = date.PlusYears(1)
            result === (new JulianDate(5, 2, 28), 1)

            date.PlusYears(1) === new JulianDate(5, 2, 28)

            defaultMath.AddYears(date, 1)   === new JulianDate(5, 2, 28)
            overspillMath.AddYears(date, 1) === new JulianDate(5, 3, 1)
            exactMath.AddYears(date, 1)     === new JulianDate(5, 3, 1)

    //
    // Month type
    //

    [<Sealed>]
    type MonthFacts() =
        inherit IMonthFacts<JulianMonth, JulianDate, UnboundedJulianDataSet>()

        [<Fact>]
        static member ``ToString() 12/999999`` () =
            let month = new JulianMonth(999_999, 12)
            month.ToString() === "12/999999 (Julian)"

        [<Fact>]
        static member ``ToString() 12/0001 BCE`` () =
            let month = new JulianMonth(0, 12)
            month.ToString() === "12/0001 BCE (Julian)"

        [<Fact>]
        static member ``ToString() 01/999999 BCE`` () =
            let month = new JulianMonth(-999_998, 1)
            month.ToString() === "01/999999 BCE (Julian)"

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new JulianMonth(y, m)
            let date = new JulianDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new JulianMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<JulianMonth, UnboundedJulianDataSet>()

    //
    // Year type
    //

    [<Sealed>]
    type YearFacts() =
        inherit IYearFacts<JulianYear, JulianMonth, JulianDate, UnboundedJulianDataSet>()

        [<Fact>]
        static member ``ToString() 999999`` () =
            let year = new JulianYear(999_999)
            year.ToString() === "999999 (Julian)"

        [<Fact>]
        static member ``ToString() 0001 BCE`` () =
            let year = new JulianYear(0)
            year.ToString() === "0001 BCE (Julian)"

        [<Fact>]
        static member ``ToString() 999999 BCE`` () =
            let year = new JulianYear(-999_998)
            year.ToString() === "999999 BCE (Julian)"

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new JulianYear(y)
            let date = new JulianMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new JulianYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new JulianYear(y)
            let date = new JulianDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new JulianYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
