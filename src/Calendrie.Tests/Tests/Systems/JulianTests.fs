﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.JulianTests

#nowarn 3391 // Implicit conversion to DayNumber or GregorianDate

open System

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Unbounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

open type Calendrie.Extensions.JulianDateExtensions

let private chr = JulianCalendar.Instance
// NB: notice the use of UnboundedJulianDataSet.
let private calendarDataSet = UnboundedJulianDataSet.Instance

module Prelude =
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

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = JulianDate.Create(y, m, d)
        let exp = JulianMonth.Create(y, m)
        // Act & Assert
        JulianMonth.FromDate(date) === exp

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
        inherit CalendarFacts<JulianCalendar, UnboundedJulianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = JulianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = JulianCalendar.MinYear === JulianScope.MinYear

        [<Fact>]
        static member MaxYear() = JulianCalendar.MaxYear === JulianScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<JulianDate, UnboundedJulianDataSet>()

        [<Fact>]
        static member ``ToString() 31/12/999999`` () =
            let date = new JulianDate(999_999, 12, 31);
            date.ToString() === "31/12/999999 (Julian)"

        [<Fact>]
        static member ``ToString() 31/12/0001 BCE`` () =
            let date = new JulianDate(0, 12, 31);
            date.ToString() === "31/12/0001 BCE (Julian)"

        [<Fact>]
        static member ``ToString() 01/01/999999 BCE`` () =
            let date = new JulianDate(-999_998, 1, 1);
            date.ToString() === "01/01/999999 BCE (Julian)"

    [<Sealed>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<JulianDate, UnboundedJulianDataSet>()

    //
    // Month and year types
    //

    [<Sealed>]
    type MonthFacts() =
        inherit IMonthFacts<JulianMonth, JulianDate, UnboundedJulianDataSet>()

        [<Fact>]
        static member ``ToString() 12/999999`` () =
            let month = new JulianMonth(999_999, 12);
            month.ToString() === "12/999999 (Julian)"

        [<Fact>]
        static member ``ToString() 12/0001 BCE`` () =
            let month = new JulianMonth(0, 12);
            month.ToString() === "12/0001 BCE (Julian)"

        [<Fact>]
        static member ``ToString() 01/999999 BCE`` () =
            let month = new JulianMonth(-999_998, 1);
            month.ToString() === "01/999999 BCE (Julian)"

    [<Sealed>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<JulianMonth, UnboundedJulianDataSet>()

    [<Sealed>]
    type YearFacts() =
        inherit IYearFacts<JulianYear, JulianMonth, JulianDate, UnboundedJulianDataSet>()

        [<Fact>]
        static member ``ToString() 999999`` () =
            let year = new JulianYear(999_999);
            year.ToString() === "999999 (Julian)"

        [<Fact>]
        static member ``ToString() 0001 BCE`` () =
            let year = new JulianYear(0);
            year.ToString() === "0001 BCE (Julian)"

        [<Fact>]
        static member ``ToString() 999999 BCE`` () =
            let year = new JulianYear(-999_998);
            year.ToString() === "999999 BCE (Julian)"

    //
    // Math
    //

    [<Sealed>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<JulianDate, UnboundedJulianDataSet>(
            new JulianDateMath(AdditionRule.Truncate))
