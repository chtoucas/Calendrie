// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.WorldTests

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

let private calendarDataSet = StandardWorldDataSet.Instance

module Prelude =
    [<Fact>]
    let ``Value of WorldCalendar.Epoch.DaysZinceZero`` () =
        WorldCalendar.Instance.Epoch.DaysSinceZero === -1

    [<Fact>]
    let ``default(WorldDate) is WorldCalendar.Epoch`` () =
        Unchecked.defaultof<WorldDate>.DayNumber === WorldCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of WorldCalendar.MinDaysSinceEpoch`` () =
        WorldCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of WorldCalendar.MaxDaysSinceEpoch`` () =
        WorldCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of WorldCalendar.MinMonthsSinceEpoch`` () =
        WorldCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of WorldCalendar.MaxMonthsSinceEpoch`` () =
        WorldCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

module Conversions =
    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = WorldDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = WorldDate.op_Explicit

    type JulianDateCaster = WorldDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = WorldDate.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``WorldMonth:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new WorldDate(y, m, d)
        let exp = new WorldMonth(y, m)
        // Act & Assert
        WorldMonth.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``WorldYear:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new WorldDate(y, m, d)
        let exp = new WorldYear(y)
        // Act & Assert
        WorldYear.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``WorldYear:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new WorldMonth(y, m)
        let exp = new WorldYear(y)
        // Act & Assert
        WorldYear.FromMonth(month) === exp

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new WorldDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new WorldDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at WorldDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(WorldDate.MinValue.DayNumber)

        WorldDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at WorldDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(WorldDate.MaxValue.DayNumber)

        WorldDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to WorldDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new WorldDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at WorldDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(WorldDate.MinValue.DayNumber)

        op_Explicit_Gregorian WorldDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at WorldDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(WorldDate.MaxValue.DayNumber)

        op_Explicit_Gregorian WorldDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new WorldDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at WorldDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(WorldDate.MinValue.DayNumber)

        WorldDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at WorldDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(WorldDate.MaxValue.DayNumber)

        WorldDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new WorldDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at WorldDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(WorldDate.MinValue.DayNumber)

        op_Explicit_Julian WorldDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at WorldDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(WorldDate.MaxValue.DayNumber)

        op_Explicit_Julian WorldDate.MaxValue === exp

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<WorldCalendar, StandardWorldDataSet>(WorldCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = WorldCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = WorldCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<WorldDate, StandardWorldDataSet>()

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``Property IsBlank`` (info: DateInfo) =
            let (y, m, d) = info.Yemoda.Deconstruct()
            let date = new WorldDate(y, m, d)
            // Act & Assert
            date.IsBlank === date.IsSupplementary

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<WorldDate, StandardWorldDataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<WorldMonth, WorldDate, StandardWorldDataSet>()

        static member MoreMonthInfoData with get() = WorldDataSet.MoreMonthInfoData

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new WorldMonth(y, m)
            let date = new WorldDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new WorldMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // World-only
        //

        [<Theory; MemberData(nameof(MonthFacts.MoreMonthInfoData))>]
        static member ``WorldMonth.CountDaysInWorldMonth()`` (info: YemoAnd<int>) =
            let (y, m, daysInMonth) = info.Deconstruct()
            let month = new WorldMonth(y, m)

            month.CountDaysInWorldMonth() === daysInMonth

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<WorldMonth, StandardWorldDataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<WorldYear, WorldMonth, WorldDate, StandardWorldDataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new WorldYear(y)
            let date = new WorldMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
            let year = new WorldYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new WorldYear(y)
            let date = new WorldDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new WorldYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
