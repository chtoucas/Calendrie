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

let private calendarDataSet = StandardTabularIslamicDataSet.Instance

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
    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = TabularIslamicDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = TabularIslamicDate.op_Explicit

    type JulianDateCaster = TabularIslamicDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = TabularIslamicDate.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``TabularIslamicMonth:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new TabularIslamicDate(y, m, d)
        let exp = new TabularIslamicMonth(y, m)
        // Act & Assert
        TabularIslamicMonth.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``TabularIslamicYear:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new TabularIslamicDate(y, m, d)
        let exp = new TabularIslamicYear(y)
        // Act & Assert
        TabularIslamicYear.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``TabularIslamicYear:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new TabularIslamicMonth(y, m)
        let exp = new TabularIslamicYear(y)
        // Act & Assert
        TabularIslamicYear.FromMonth(month) === exp

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
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<TabularIslamicCalendar, StandardTabularIslamicDataSet>(TabularIslamicCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Lunar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = TabularIslamicCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = TabularIslamicCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = TabularIslamicCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<TabularIslamicDate, StandardTabularIslamicDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<TabularIslamicDate, StandardTabularIslamicDataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<TabularIslamicMonth, TabularIslamicDate, StandardTabularIslamicDataSet>()

        [<Theory; MemberData(nameof(calendarDataSet.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new TabularIslamicMonth(y, m)
            let date = new TabularIslamicDate(y, m, d);
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(calendarDataSet.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new TabularIslamicMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<TabularIslamicMonth, StandardTabularIslamicDataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<TabularIslamicYear, TabularIslamicMonth, TabularIslamicDate, StandardTabularIslamicDataSet>()

        [<Theory; MemberData(nameof(calendarDataSet.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new TabularIslamicYear(y)
            let date = new TabularIslamicMonth(y, m);
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(calendarDataSet.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
            let year = new TabularIslamicYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(calendarDataSet.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new TabularIslamicYear(y)
            let date = new TabularIslamicDate(y, doy);
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(calendarDataSet.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new TabularIslamicYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
