﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.GregorianTests

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

open type Calendrie.Extensions.GregorianDateExtensions

// NB: notice the use of UnboundedGregorianDataSet.
let private calendarDataSet = UnboundedGregorianDataSet.Instance

module Prelude =
    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let daysSinceEpochInfoData = calendarDataSet.DaysSinceEpochInfoData

    [<Fact>]
    let ``Value of GregorianCalendar.Epoch.DaysZinceZero`` () =
        GregorianCalendar.Instance.Epoch.DaysSinceZero === 0

#if DEBUG
    [<Fact>]
    let ``Value of GregorianCalendar.MinDaysSinceEpoch`` () =
        GregorianCalendar.Instance.MinDaysSinceEpoch === -365_242_135

    [<Fact>]
    let ``Value of GregorianCalendar.MaxDaysSinceEpoch`` () =
        GregorianCalendar.Instance.MaxDaysSinceEpoch === 365_242_133

    [<Fact>]
    let ``Value of GregorianCalendar.MinMonthsSinceEpoch`` () =
        GregorianCalendar.Instance.MinMonthsSinceEpoch === -11_999_988

    [<Fact>]
    let ``Value of GregorianCalendar.MaxMonthsSinceEpoch`` () =
        GregorianCalendar.Instance.MaxMonthsSinceEpoch === 11_999_987
#endif

    [<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
    let ``Property DaysSinceZero`` (x: DaysSinceEpochInfo) =
        let (daysSinceEpoch, y, m, d) = x.Deconstruct()
        let date = new GregorianDate(y, m, d)

        date.DaysSinceZero === daysSinceEpoch

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``GregorianMonth(GregorianDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new GregorianDate(y, m, d)
        let exp = new GregorianMonth(y, m)
        // Act & Assert
        new GregorianMonth(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``GregorianYear(GregorianDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new GregorianDate(y, m, d)
        let exp = new GregorianYear(y)
        // Act & Assert
        new GregorianYear(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``GregorianYear(GregorianMonth)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new GregorianMonth(y, m)
        let exp = new GregorianYear(y)
        // Act & Assert
        new GregorianYear(month) === exp

module Factories =
    let dateInfoData = calendarDataSet.DateInfoData

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``UnsafeCreate(y, m, d)`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        // Act
        let date = GregorianDate.UnsafeCreate(y, m, d)
        // Assert
        date.Year      === y
        date.Month     === m
        date.Day       === d
        date.DayOfYear === doy

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``UnsafeCreate(y, doy)`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        // Act
        let date = GregorianDate.UnsafeCreate(y, doy)
        // Assert
        date.Year      === y
        date.Month     === m
        date.Day       === d
        date.DayOfYear === doy

module Conversions =
    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    // NB: FromAbsoluteDate(JulianDate) is tested in JulianTests alongside ToGregorianDate().

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new GregorianDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new GregorianDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at GregorianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(GregorianDate.MinValue.DayNumber)

        GregorianDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at GregorianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(GregorianDate.MaxValue.DayNumber)

        GregorianDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new GregorianDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        GregorianDate.op_Explicit date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at GregorianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(GregorianDate.MinValue.DayNumber)

        GregorianDate.op_Explicit GregorianDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at GregorianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(GregorianDate.MaxValue.DayNumber)

        GregorianDate.op_Explicit GregorianDate.MaxValue === exp

module Extensions =
    let private chr = GregorianCalendar.Instance

    let dayOfWeekData = calendarDataSet.DayOfWeekData
    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(chr.Scope.Domain)

    //
    // GetDayOfWeek() via DoomsdayRule
    //

    [<Theory; MemberData(nameof(dayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``GregorianDate:GetDayOfWeek()`` (x: YemodaAnd<DayOfWeek>) =
        let (y, m, d, dayOfWeek) = x.Deconstruct()
        let date = new GregorianDate(y, m, d)

        date.GetDayOfWeek() === dayOfWeek

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``GregorianDate:GetDayOfWeek() via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = GregorianDate.FromAbsoluteDate(dayNumber)

        date.GetDayOfWeek() === dayOfWeek

module Bundles =
    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<GregorianCalendar, UnboundedGregorianDataSet>(GregorianCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = GregorianCalendar.MinYear === GregorianScope.MinYear

        [<Fact>]
        static member MaxYear() = GregorianCalendar.MaxYear === GregorianScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<GregorianDate, UnboundedGregorianDataSet>()

        [<Fact>]
        static member ``ToString() 31/12/999999`` () =
            let date = new GregorianDate(999_999, 12, 31)
            date.ToString() === "31/12/999999 (Gregorian)"

        [<Fact>]
        static member ``ToString() 31/12/0001 BCE`` () =
            let date = new GregorianDate(0, 12, 31)
            date.ToString() === "31/12/0001 BCE (Gregorian)"

        [<Fact>]
        static member ``ToString() 01/01/999999 BCE`` () =
            let date = new GregorianDate(-999_998, 1, 1)
            date.ToString() === "01/01/999999 BCE (Gregorian)"

    [<Sealed>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<GregorianDate, UnboundedGregorianDataSet>()

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<GregorianDate, UnboundedGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)

    [<Sealed>]
    type DateMathFacts() =
        inherit DefaultDateMathFacts<GregorianDate, UnboundedGregorianDataSet>()

        static let defaultMath   = new DateMath()
        static let overspillMath = new DateMath(AdditionRule.Overspill)
        static let exactMath     = new DateMath(AdditionRule.Exact)

        //
        // CountYearsSince()
        //
        // NB: CountYearsSince() is not exact but only "in" february.

        [<Fact>]
        static member ``CountYearsSince() where only "date" is an intercalary day (a)`` () =
            // 28/2/2027 - 29/2/2024 = 3 years
            let date = new GregorianDate(2027, 2, 28)
            let other = new GregorianDate(2024, 2, 29)
            // Act & Assert
            date.CountYearsSince(other) === 3
            // With other rules (Overspill or Exact), the result of adding
            // 3 years would be 1/3/2027 which is too late -> diff = 2 years.
            other.PlusYears(3) === new GregorianDate(2027, 2, 28) // Truncation happens here
            // Math
            defaultMath.CountYearsBetween(other, date)   === 3
            overspillMath.CountYearsBetween(other, date) === 2
            exactMath.CountYearsBetween(other, date)     === 2
            defaultMath.AddYears(other, 3)   === new GregorianDate(2027, 2, 28)
            overspillMath.AddYears(other, 2) === new GregorianDate(2026, 3, 1)
            overspillMath.AddYears(other, 3) === new GregorianDate(2027, 3, 1) // too late
            exactMath.AddYears(other, 2)     === new GregorianDate(2026, 3, 1)
            exactMath.AddYears(other, 3)     === new GregorianDate(2027, 3, 1) // too late

        // Same as above but with dates switched.
        [<Fact>]
        static member ``CountYearsSince() where only "other" is an intercalary day (a)`` () =
            // 29/2/2024 - 28/2/2027 = -2 years
            let date = new GregorianDate(2024, 2, 29)
            let other = new GregorianDate(2027, 2, 28)
            // Act & Assert
            date.CountYearsSince(other) === -2
            other.PlusYears(-2) === new GregorianDate(2025, 2, 28)
            other.PlusYears(-3) === new GregorianDate(2024, 2, 28) // too early
            // Math
            defaultMath.CountYearsBetween(other, date)   === -2
            overspillMath.CountYearsBetween(other, date) === -2
            exactMath.CountYearsBetween(other, date)     === -2
            defaultMath.AddYears(other, -2)   === new GregorianDate(2025, 2, 28)
            defaultMath.AddYears(other, -3)   === new GregorianDate(2024, 2, 28) // too early
            overspillMath.AddYears(other, -2) === new GregorianDate(2025, 2, 28)
            overspillMath.AddYears(other, -3) === new GregorianDate(2024, 2, 28) // too early
            exactMath.AddYears(other, -2)     === new GregorianDate(2025, 2, 28)
            exactMath.AddYears(other, -3)     === new GregorianDate(2024, 2, 28) // too early

        [<Fact>]
        static member ``CountYearsSince() where only "date" is an intercalary day (b)`` () =
            // 27/2/2027 - 29/2/2024 = 2 years
            let date = new GregorianDate(2027, 2, 27)
            let other = new GregorianDate(2024, 2, 29)
            // Act & Assert
            date.CountYearsSince(other) === 2
            // With other rules (Overspill or Exact), the result of adding
            // 3 years would be 1/3/2027 which is too late -> diff = 2 years.
            other.PlusYears(2) === new GregorianDate(2026, 2, 28) // Truncation happens here
            other.PlusYears(3) === new GregorianDate(2027, 2, 28) // too late
            // Math
            defaultMath.CountYearsBetween(other, date)   === 2
            overspillMath.CountYearsBetween(other, date) === 2
            exactMath.CountYearsBetween(other, date)     === 2
            defaultMath.AddYears(other, 2)   === new GregorianDate(2026, 2, 28)
            defaultMath.AddYears(other, 3)   === new GregorianDate(2027, 2, 28) // too late
            overspillMath.AddYears(other, 2) === new GregorianDate(2026, 3, 1)
            overspillMath.AddYears(other, 3) === new GregorianDate(2027, 3, 1)  // too late
            exactMath.AddYears(other, 2)     === new GregorianDate(2026, 3, 1)
            exactMath.AddYears(other, 3)     === new GregorianDate(2027, 3, 1)  // too late

        // Same as above but with dates switched.
        [<Fact>]
        static member ``CountYearsSince() where only "other" is an intercalary day (b)`` () =
            // 29/2/2024 - 27/2/2027 = -2 years
            let date = new GregorianDate(2024, 2, 29)
            let other = new GregorianDate(2027, 2, 27)
            // Act & Assert
            date.CountYearsSince(other) === -2
            other.PlusYears(-2) === new GregorianDate(2025, 2, 27)
            other.PlusYears(-3) === new GregorianDate(2024, 2, 27) // too early
            // Math
            defaultMath.CountYearsBetween(other, date)   === -2
            overspillMath.CountYearsBetween(other, date) === -2
            exactMath.CountYearsBetween(other, date)     === -2
            defaultMath.AddYears(other, -2)   === new GregorianDate(2025, 2, 27)
            defaultMath.AddYears(other, -3)   === new GregorianDate(2024, 2, 27) // too early
            overspillMath.AddYears(other, -2) === new GregorianDate(2025, 2, 27)
            overspillMath.AddYears(other, -3) === new GregorianDate(2024, 2, 27) // too early
            exactMath.AddYears(other, -2)     === new GregorianDate(2025, 2, 27)
            exactMath.AddYears(other, -3)     === new GregorianDate(2024, 2, 27) // too early

        [<Fact>]
        static member ``CountYearsSince() where both dates are intercalary`` () =
            // 29/2/2028 - 29/2/2024 = 4 years
            // 29/2/2024 - 29/2/2028 = -4 years
            let date = new GregorianDate(2028, 2, 29)
            let other = new GregorianDate(2024, 2, 29)
            // Act & Assert
            date.CountYearsSince(other) === 4
            other.CountYearsSince(date) === -4
            let mutable x = new GregorianDate()
            // Math
            defaultMath.CountYearsBetween(other, date, &x)   === 4
            defaultMath.CountYearsBetween(date, other, &x)   === -4
            overspillMath.CountYearsBetween(other, date, &x) ===  4
            overspillMath.CountYearsBetween(date, other, &x) === -4
            exactMath.CountYearsBetween(other, date, &x)     ===  4
            exactMath.CountYearsBetween(date, other, &x)     === -4

        //
        // Substract()
        //

        [<Fact>]
        static member ``Substract() when start = end`` () =
            let date = new GregorianDate(1970, 3, 18)
            // Act & Assert
            defaultMath.Subtract(date, date)   === DateDifference.Zero
            overspillMath.Subtract(date, date) === DateDifference.Zero
            exactMath.Subtract(date, date)     === DateDifference.Zero

        [<Fact>]
        static member ``Substract() when start < end`` () =
            let date = new GregorianDate(1970, 3, 18)
            // Act & Assert
            defaultMath.Subtract(date, new GregorianDate(2024, 3, 18)) === DateDifference.CreatePositive(54, 0, 0)
            defaultMath.Subtract(date, new GregorianDate(2024, 3, 19)) === DateDifference.CreatePositive(54, 0, 1)
            // 1 week
            defaultMath.Subtract(date, new GregorianDate(2024, 3, 25)) === DateDifference.CreatePositive(54, 0, 7)
            defaultMath.Subtract(date, new GregorianDate(2024, 3, 26)) === DateDifference.CreatePositive(54, 0, 8)
            // 2 weeks
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 1))  === DateDifference.CreatePositive(54, 0, 14)
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 2))  === DateDifference.CreatePositive(54, 0, 15)
            // 3 weeks
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 8))  === DateDifference.CreatePositive(54, 0, 21)
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 9))  === DateDifference.CreatePositive(54, 0, 22)
            // 4 weeks
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 15)) === DateDifference.CreatePositive(54, 0, 28)
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 16)) === DateDifference.CreatePositive(54, 0, 29)
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 17)) === DateDifference.CreatePositive(54, 0, 30)
            // 1 month
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 18)) === DateDifference.CreatePositive(54, 1, 0)
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 19)) === DateDifference.CreatePositive(54, 1, 1)
            // 1 month + 1 week
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 25)) === DateDifference.CreatePositive(54, 1, 7)
            defaultMath.Subtract(date, new GregorianDate(2024, 4, 26)) === DateDifference.CreatePositive(54, 1, 8)
            // 1 year
            defaultMath.Subtract(date, new GregorianDate(2025, 3, 18)) === DateDifference.CreatePositive(55, 0, 0)

        [<Fact>]
        static member ``Substract() when start > end`` () =
            let date = new GregorianDate(1970, 3, 18)
            // Act & Assert
            defaultMath.Subtract(new GregorianDate(2024, 3, 18), date) === DateDifference.CreateNegative(54, 0, 0)
            defaultMath.Subtract(new GregorianDate(2024, 3, 19), date) === DateDifference.CreateNegative(54, 0, 1)
            // 1 week
            defaultMath.Subtract(new GregorianDate(2024, 3, 25), date) === DateDifference.CreateNegative(54, 0, 7)
            defaultMath.Subtract(new GregorianDate(2024, 3, 26), date) === DateDifference.CreateNegative(54, 0, 8)
            // 2 weeks
            defaultMath.Subtract(new GregorianDate(2024, 4, 1), date)  === DateDifference.CreateNegative(54, 0, 14)
            defaultMath.Subtract(new GregorianDate(2024, 4, 2), date)  === DateDifference.CreateNegative(54, 0, 15)
            // 3 weeks
            defaultMath.Subtract(new GregorianDate(2024, 4, 8), date)  === DateDifference.CreateNegative(54, 0, 21)
            defaultMath.Subtract(new GregorianDate(2024, 4, 9), date)  === DateDifference.CreateNegative(54, 0, 22)
            // 4 weeks
            defaultMath.Subtract(new GregorianDate(2024, 4, 15), date) === DateDifference.CreateNegative(54, 0, 28)
            defaultMath.Subtract(new GregorianDate(2024, 4, 16), date) === DateDifference.CreateNegative(54, 0, 29)
            defaultMath.Subtract(new GregorianDate(2024, 4, 17), date) === DateDifference.CreateNegative(54, 0, 30)
            // 1 month
            defaultMath.Subtract(new GregorianDate(2024, 4, 18), date) === DateDifference.CreateNegative(54, 1, 0)
            defaultMath.Subtract(new GregorianDate(2024, 4, 19), date) === DateDifference.CreateNegative(54, 1, 1)
            // 1 month + 1 week
            defaultMath.Subtract(new GregorianDate(2024, 4, 25), date) === DateDifference.CreateNegative(54, 1, 7)
            defaultMath.Subtract(new GregorianDate(2024, 4, 26), date) === DateDifference.CreateNegative(54, 1, 8)
            // 1 year
            defaultMath.Subtract(new GregorianDate(2025, 3, 18), date) === DateDifference.CreateNegative(55, 0, 0)

    //
    // Month type
    //

    [<Sealed>]
    type MonthFacts() =
        inherit IMonthFacts<GregorianMonth, GregorianDate, UnboundedGregorianDataSet>()

        [<Fact>]
        static member ``ToString() 12/999999`` () =
            let month = new GregorianMonth(999_999, 12)
            month.ToString() === "12/999999 (Gregorian)"

        [<Fact>]
        static member ``ToString() 12/0001 BCE`` () =
            let month = new GregorianMonth(0, 12)
            month.ToString() === "12/0001 BCE (Gregorian)"

        [<Fact>]
        static member ``ToString() 01/999999 BCE`` () =
            let month = new GregorianMonth(-999_998, 1)
            month.ToString() === "01/999999 BCE (Gregorian)"

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new GregorianMonth(y, m)
            let date = new GregorianDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new GregorianMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<GregorianMonth, UnboundedGregorianDataSet>()

    //
    // Year type
    //

    [<Sealed>]
    type YearFacts() =
        inherit IYearFacts<GregorianYear, GregorianMonth, GregorianDate, UnboundedGregorianDataSet>()

        [<Fact>]
        static member ``ToString() 999999`` () =
            let year = new GregorianYear(999_999)
            year.ToString() === "999999 (Gregorian)"

        [<Fact>]
        static member ``ToString() 0001 BCE`` () =
            let year = new GregorianYear(0)
            year.ToString() === "0001 BCE (Gregorian)"

        [<Fact>]
        static member ``ToString() 999999 BCE`` () =
            let year = new GregorianYear(-999_998)
            year.ToString() === "999999 BCE (Gregorian)"

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new GregorianYear(y)
            let date = new GregorianMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new GregorianYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new GregorianYear(y)
            let date = new GregorianDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new GregorianYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
