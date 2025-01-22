// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.CivilTests

#nowarn 3391 // Implicit conversion to DayNumber or GregorianDate

open System

open Calendrie
open Calendrie.Hemerology
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

open type Calendrie.Extensions.CivilDateExtensions

let private calendarDataSet = StandardGregorianDataSet.Instance

module Prelude =
    let daysSinceEpochInfoData = calendarDataSet.DaysSinceEpochInfoData

    [<Fact>]
    let ``Value of CivilCalendar.Epoch.DaysZinceZero`` () =
        CivilCalendar.Instance.Epoch.DaysSinceZero === 0

    [<Fact>]
    let ``default(CivilDate) is CivilCalendar.Epoch`` () =
        Unchecked.defaultof<CivilDate>.DayNumber === CivilCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of CivilCalendar.MinDaysSinceEpoch`` () =
        CivilCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of CivilCalendar.MaxDaysSinceEpoch`` () =
        CivilCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of CivilCalendar.MinMonthsSinceEpoch`` () =
        CivilCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of CivilCalendar.MaxMonthsSinceEpoch`` () =
        CivilCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

    [<Theory; MemberData(nameof(daysSinceEpochInfoData))>]
    let ``Property DaysSinceZero`` (x: DaysSinceEpochInfo) =
        let (daysSinceEpoch, y, m, d) = x.Deconstruct()
        let date = new CivilDate(y, m, d)

        date.DaysSinceZero === daysSinceEpoch

module Factories =
    let dateInfoData = calendarDataSet.DateInfoData
    let monthsSinceEpochInfoData = calendarDataSet.MonthsSinceEpochInfoData

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``UnsafeCreate(y, m, d)`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        // Act
        let date = CivilDate.UnsafeCreate(y, m, d)
        // Assert
        date.Year      === y
        date.Month     === m
        date.Day       === d
        date.DayOfYear === doy

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``UnsafeCreate(y, doy)`` (x: DateInfo) =
        let y, m, d, doy = x.Deconstruct()
        // Act
        let date = CivilDate.UnsafeCreate(y, doy)
        // Assert
        date.Year      === y
        date.Month     === m
        date.Day       === d
        date.DayOfYear === doy

module Conversions =
    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let yearInfoData = calendarDataSet.YearInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``CivilMonth:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new CivilDate(y, m, d)
        let exp = new CivilMonth(y, m)
        // Act & Assert
        CivilMonth.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``CivilYear:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new CivilDate(y, m, d)
        let exp = new CivilYear(y)
        // Act & Assert
        CivilYear.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``CivilYear:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new CivilMonth(y, m)
        let exp = new CivilYear(y)
        // Act & Assert
        CivilYear.FromMonth(month) === exp

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new CivilDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CivilDate(y, m, d)
        let exp = new GregorianDate(y, m, d)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at CivilDate:MinValue`` () =
        let y, m, d = CivilDate.MinValue.Deconstruct()
        let exp = new GregorianDate(y, m, d)

        CivilDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at CivilDate:MaxValue`` () =
        let y, m, d = CivilDate.MaxValue.Deconstruct()
        let exp = new GregorianDate(y, m, d)

        CivilDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Implicit conversion to GregorianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CivilDate(y, m, d)
        let exp = new GregorianDate(y, m, d)

        (date : GregorianDate) === exp

    [<Fact>]
    let ``Implicit conversion to GregorianDate at CivilDate:MinValue`` () =
        let y, m, d = CivilDate.MinValue.Deconstruct()
        let exp = new GregorianDate(y, m, d)

        (CivilDate.MinValue : GregorianDate) === exp

    [<Fact>]
    let ``Implicit conversion to GregorianDate at CivilDate:MaxValue`` () =
        let y, m, d = CivilDate.MaxValue.Deconstruct()
        let exp = new GregorianDate(y, m, d)

        (CivilDate.MaxValue : GregorianDate) === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CivilDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at CivilDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CivilDate.MinValue.DayNumber)

        CivilDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at CivilDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CivilDate.MaxValue.DayNumber)

        CivilDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CivilDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        CivilDate.op_Explicit date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at CivilDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CivilDate.MinValue.DayNumber)

        CivilDate.op_Explicit CivilDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at CivilDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CivilDate.MaxValue.DayNumber)

        CivilDate.op_Explicit CivilDate.MaxValue === exp

    //
    // Conversion to GregorianMonth
    //

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``ToGregorianMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let date = new CivilMonth(y, m)
        let exp = new GregorianMonth(y, m)

        date.ToGregorianMonth() === exp

    [<Fact>]
    let ``ToGregorianMonth() at CivilMonth:MinValue`` () =
        let y, m = CivilMonth.MinValue.Deconstruct()
        let exp = new GregorianMonth(y, m)

        CivilMonth.MinValue.ToGregorianMonth() === exp

    [<Fact>]
    let ``ToGregorianMonth() at CivilMonth:MaxValue`` () =
        let y, m = CivilMonth.MaxValue.Deconstruct()
        let exp = new GregorianMonth(y, m)

        CivilMonth.MaxValue.ToGregorianMonth() === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Implicit conversion to GregorianMonth`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let date = new CivilMonth(y, m)
        let exp = new GregorianMonth(y, m)

        (date : GregorianMonth) === exp

    [<Fact>]
    let ``Implicit conversion to GregorianMonth at CivilMonth:MinValue`` () =
        let y, m = CivilMonth.MinValue.Deconstruct()
        let exp = new GregorianMonth(y, m)

        (CivilMonth.MinValue : GregorianMonth) === exp

    [<Fact>]
    let ``Implicit conversion to GregorianMonth at CivilMonth:MaxValue`` () =
        let y, m = CivilMonth.MaxValue.Deconstruct()
        let exp = new GregorianMonth(y, m)

        (CivilMonth.MaxValue : GregorianMonth) === exp

    //
    // Conversion to GregorianYear
    //

    [<Theory; MemberData(nameof(yearInfoData))>]
    let ``ToGregorianYear()`` (x: YearInfo) =
        let y = x.Year
        let date = new CivilYear(y)
        let exp = new GregorianYear(y)

        date.ToGregorianYear() === exp

    [<Fact>]
    let ``ToGregorianYear() at CivilYear:MinValue`` () =
        let y = CivilYear.MinValue.Year
        let exp = new GregorianYear(y)

        CivilYear.MinValue.ToGregorianYear() === exp

    [<Fact>]
    let ``ToGregorianYear() at CivilYear:MaxValue`` () =
        let y = CivilYear.MaxValue.Year
        let exp = new GregorianYear(y)

        CivilYear.MaxValue.ToGregorianYear() === exp

    [<Theory; MemberData(nameof(yearInfoData))>]
    let ``Implicit conversion to GregorianYear`` (x: YearInfo) =
        let y = x.Year
        let date = new CivilYear(y)
        let exp = new GregorianYear(y)

        (date : GregorianYear) === exp

    [<Fact>]
    let ``Implicit conversion to GregorianYear at CivilYear:MinValue`` () =
        let y = CivilYear.MinValue.Year
        let exp = new GregorianYear(y)

        (CivilYear.MinValue : GregorianYear) === exp

    [<Fact>]
    let ``Implicit conversion to GregorianYear at CivilYear:MaxValue`` () =
        let y = CivilYear.MaxValue.Year
        let exp = new GregorianYear(y)

        (CivilYear.MaxValue : GregorianYear) === exp

module Extensions =
    let private chr = CivilCalendar.Instance

    let dayOfWeekData = calendarDataSet.DayOfWeekData
    let dayNumberToDayOfWeekData = CalCalDataSet.GetDayNumberToDayOfWeekData(chr.Scope.Domain)

    //
    // GetDayOfWeek() via DoomsdayRule
    //

    [<Theory; MemberData(nameof(dayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``CivilDate:GetDayOfWeek()`` (x: YemodaAnd<DayOfWeek>) =
        let (y, m, d, dayOfWeek) = x.Deconstruct()
        let date = new CivilDate(y, m, d)

        date.GetDayOfWeek() === dayOfWeek

    [<Theory; MemberData(nameof(dayNumberToDayOfWeekData))>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    let ``CivilDate:GetDayOfWeek() via DayNumber`` (dayNumber: DayNumber) (dayOfWeek: DayOfWeek) =
        let date = CivilDate.FromAbsoluteDate(dayNumber)

        date.GetDayOfWeek() === dayOfWeek

module Bundles =
    [<Sealed>]
    type CalendaTests() =
        inherit CalendarFacts<CivilCalendar, StandardGregorianDataSet>(CivilCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = CivilCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = CivilCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    let private defaultDateMath   = new DateMath<CivilDate>(AdditionRule.Truncate)
    let private overspillDateMath = new DateMath<CivilDate>(AdditionRule.Overspill)
    let private exactDateMath     = new DateMath<CivilDate>(AdditionRule.Exact)

    [<Sealed>]
    type DateFacts() =
        inherit IDateFacts<CivilDate, StandardGregorianDataSet>()

        // NB: CountYearsSince() is not exact but only "in" february.

        [<Fact>]
        static member ``CountYearsSince() where only "date" is an intercalary day (a)`` () =
            // 28/2/2027 - 29/2/2024 = 3 years
            let date = new CivilDate(2027, 2, 28)
            let other = new CivilDate(2024, 2, 29)
            // Act & Assert
            date.CountYearsSince(other) === 3
            // With other rules (Overspill or Exact), the result of adding
            // 3 years would be 1/3/2027 which is too late -> diff = 2 years.
            other.PlusYears(3) === new CivilDate(2027, 2, 28) // Truncation happens here
            // Math
            defaultDateMath.CountYearsBetween(other, date)   |> fst === 3
            overspillDateMath.CountYearsBetween(other, date) |> fst === 2
            exactDateMath.CountYearsBetween(other, date)     |> fst === 2

        // Same as above but with dates switched.
        [<Fact>]
        static member ``CountYearsSince() where only "other" is an intercalary day (a)`` () =
            // 29/2/2024 - 28/2/2027 = -2 years
            let date = new CivilDate(2024, 2, 29)
            let other = new CivilDate(2027, 2, 28)
            // Act & Assert
            date.CountYearsSince(other) === -2
            other.PlusYears(-2) === new CivilDate(2025, 2, 28)
            other.PlusYears(-3) === new CivilDate(2024, 2, 28) // too early
            // Math
            defaultDateMath.CountYearsBetween(other, date)   |> fst === -2
            overspillDateMath.CountYearsBetween(other, date) |> fst === -2
            exactDateMath.CountYearsBetween(other, date)     |> fst === -2

        [<Fact>]
        static member ``CountYearsSince() where only "date" is an intercalary day (b)`` () =
            // 27/2/2027 - 29/2/2024 = 2 years
            let date = new CivilDate(2027, 2, 27)
            let other = new CivilDate(2024, 2, 29)
            // Act & Assert
            date.CountYearsSince(other) === 2
            // With other rules (Overspill or Exact), the result of adding
            // 3 years would be 1/3/2027 which is too late -> diff = 2 years.
            other.PlusYears(2) === new CivilDate(2026, 2, 28) // Truncation happens here
            other.PlusYears(3) === new CivilDate(2027, 2, 28) // too late
            // Math
            defaultDateMath.CountYearsBetween(other, date)   |> fst === 2
            overspillDateMath.CountYearsBetween(other, date) |> fst === 2
            exactDateMath.CountYearsBetween(other, date)     |> fst === 2

        // Same as above but with dates switched.
        [<Fact>]
        static member ``CountYearsSince() where only "other" is an intercalary day (b)`` () =
            // 29/2/2024 - 27/2/2027 = -2 years
            let date = new CivilDate(2024, 2, 29)
            let other = new CivilDate(2027, 2, 27)
            // Act & Assert
            date.CountYearsSince(other) === -2
            other.PlusYears(-2) === new CivilDate(2025, 2, 27)
            other.PlusYears(-3) === new CivilDate(2024, 2, 27) // too early
            // Math
            defaultDateMath.CountYearsBetween(other, date)   |> fst === -2
            overspillDateMath.CountYearsBetween(other, date) |> fst === -2
            exactDateMath.CountYearsBetween(other, date)     |> fst === -2

        [<Fact>]
        static member ``CountYearsSince() where both dates are intercalary`` () =
            // 29/2/2028 - 29/2/2024 = 4 years
            // 29/2/2024 - 29/2/2028 = -4 years
            let date = new CivilDate(2028, 2, 29)
            let other = new CivilDate(2024, 2, 29)
            // Act & Assert
            date.CountYearsSince(other) === 4
            other.CountYearsSince(date) === -4
            // Math
            let mutable x = new CivilDate()
            defaultDateMath.CountYearsBetween(other, date, &x)   ===  4
            defaultDateMath.CountYearsBetween(date, other, &x)   === -4
            overspillDateMath.CountYearsBetween(other, date, &x) ===  4
            overspillDateMath.CountYearsBetween(date, other, &x) === -4
            exactDateMath.CountYearsBetween(other, date, &x)     ===  4
            exactDateMath.CountYearsBetween(date, other, &x)     === -4

    [<Sealed>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<CivilDate, StandardGregorianDataSet>()

    [<Sealed>]
    type DayOfWeekFacts() =
        inherit IDateDayOfWeekFacts<CivilDate, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilDate(y, m, d)

    [<Sealed>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<CivilDate, StandardGregorianDataSet>(defaultDateMath)

    //
    // Month type
    //

    [<Sealed>]
    type MonthFacts() =
        inherit IMonthFacts<CivilMonth, CivilDate, StandardGregorianDataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new CivilMonth(y, m)
            let date = new CivilDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new CivilMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<CivilMonth, StandardGregorianDataSet>()

    //
    // Year type
    //

    [<Sealed>]
    type YearFacts() =
        inherit IYearFacts<CivilYear, CivilMonth, CivilDate, StandardGregorianDataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new CivilYear(y)
            let date = new CivilMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
            let year = new CivilYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new CivilYear(y)
            let date = new CivilDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new CivilYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
