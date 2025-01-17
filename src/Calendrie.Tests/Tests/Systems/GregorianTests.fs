// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.GregorianTests

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

open type Calendrie.Extensions.GregorianDateExtensions

let private chr = GregorianCalendar.Instance
// NB: notice the use of UnboundedGregorianDataSet.
let private calendarDataSet = UnboundedGregorianDataSet.Instance

module Prelude =
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
        let date = GregorianDate.UnsafeCreate(y, m, d)
        // Assert
        date.Year      === y
        date.Month     === m
        date.Day       === d
        date.DayOfYear === doy

module Conversions =
    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    // NB: FromAbsoluteDate(JulianDate) is tested in JulianTests alongside ToGregorianDate().

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``GregorianMonth:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new GregorianDate(y, m, d)
        let exp = new GregorianMonth(y, m)
        // Act & Assert
        GregorianMonth.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``GregorianYear:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new GregorianDate(y, m, d)
        let exp = new GregorianYear(y)
        // Act & Assert
        GregorianYear.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``GregorianYear:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new GregorianMonth(y, m)
        let exp = new GregorianYear(y)
        // Act & Assert
        GregorianYear.FromMonth(month) === exp

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
        inherit CalendarFacts<GregorianCalendar, UnboundedGregorianDataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = GregorianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = GregorianCalendar.MinYear === GregorianScope.MinYear

        [<Fact>]
        static member MaxYear() = GregorianCalendar.MaxYear === GregorianScope.MaxYear

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<GregorianDate, UnboundedGregorianDataSet>()

        [<Fact>]
        static member ``ToString() 31/12/999999`` () =
            let date = new GregorianDate(999_999, 12, 31);
            date.ToString() === "31/12/999999 (Gregorian)"

        [<Fact>]
        static member ``ToString() 31/12/0001 BCE`` () =
            let date = new GregorianDate(0, 12, 31);
            date.ToString() === "31/12/0001 BCE (Gregorian)"

        [<Fact>]
        static member ``ToString() 01/01/999999 BCE`` () =
            let date = new GregorianDate(-999_998, 1, 1);
            date.ToString() === "01/01/999999 BCE (Gregorian)"

    [<Sealed>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<GregorianDate, UnboundedGregorianDataSet>()

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<GregorianDate, UnboundedGregorianDataSet>()

        override __.GetDate(y, m, d) = new GregorianDate(y, m, d)

    //
    // Month and year types
    //

    [<Sealed>]
    type MonthFacts() =
        inherit IMonthFacts<GregorianMonth, GregorianDate, UnboundedGregorianDataSet>()

        [<Fact>]
        static member ``ToString() 12/999999`` () =
            let month = new GregorianMonth(999_999, 12);
            month.ToString() === "12/999999 (Gregorian)"

        [<Fact>]
        static member ``ToString() 12/0001 BCE`` () =
            let month = new GregorianMonth(0, 12);
            month.ToString() === "12/0001 BCE (Gregorian)"

        [<Fact>]
        static member ``ToString() 01/999999 BCE`` () =
            let month = new GregorianMonth(-999_998, 1);
            month.ToString() === "01/999999 BCE (Gregorian)"

        [<Theory; MemberData(nameof(calendarDataSet.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new GregorianMonth(y, m)
            let date = new GregorianDate(y, m, d);
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(calendarDataSet.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new GregorianMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<GregorianMonth, UnboundedGregorianDataSet>()

    [<Sealed>]
    type YearFacts() =
        inherit IYearFacts<GregorianYear, GregorianMonth, GregorianDate, UnboundedGregorianDataSet>()

        [<Fact>]
        static member ``ToString() 999999`` () =
            let year = new GregorianYear(999_999);
            year.ToString() === "999999 (Gregorian)"

        [<Fact>]
        static member ``ToString() 0001 BCE`` () =
            let year = new GregorianYear(0);
            year.ToString() === "0001 BCE (Gregorian)"

        [<Fact>]
        static member ``ToString() 999999 BCE`` () =
            let year = new GregorianYear(-999_998);
            year.ToString() === "999999 BCE (Gregorian)"

        [<Theory; MemberData(nameof(calendarDataSet.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new GregorianYear(y)
            let date = new GregorianMonth(y, m);
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(calendarDataSet.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
            let year = new GregorianYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(calendarDataSet.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new GregorianYear(y)
            let date = new GregorianDate(y, doy);
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(calendarDataSet.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new GregorianYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))

    //
    // Math
    //

    [<Sealed>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<GregorianDate, UnboundedGregorianDataSet>(
            new GregorianDateMath(AdditionRule.Truncate))
