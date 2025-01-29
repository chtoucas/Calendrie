// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.TropicaliaTests

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
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = TropicaliaDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = TropicaliaDate.op_Explicit

    type JulianDateCaster = TropicaliaDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = TropicaliaDate.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``TropicaliaMonth:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new TropicaliaDate(y, m, d)
        let exp = new TropicaliaMonth(y, m)
        // Act & Assert
        TropicaliaMonth.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``TropicaliaYear:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new TropicaliaDate(y, m, d)
        let exp = new TropicaliaYear(y)
        // Act & Assert
        TropicaliaYear.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``TropicaliaYear:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new TropicaliaMonth(y, m)
        let exp = new TropicaliaYear(y)
        // Act & Assert
        TropicaliaYear.FromMonth(month) === exp

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

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<TropicaliaDate, StandardTropicaliaDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<TropicaliaDate, StandardTropicaliaDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateMathFacts() =
        inherit DefaultDateMathFacts<TropicaliaDate, StandardTropicaliaDataSet>()

        static let defaultMath   = new DateMath()
        static let overspillMath = new DateMath(AdditionRule.Overspill)
        static let exactMath     = new DateMath(AdditionRule.Exact)

        [<Fact>]
        static member ``PlusYears() when roundof != 0`` () =
            let date = new TropicaliaDate(4, 2, 29)
            // Act & Assert
            date.IsIntercalary |> ok

            let result: TropicaliaDate * int = date.PlusYears(1)
            result === (new TropicaliaDate(5, 2, 28), 1)

            date.PlusYears(1) === new TropicaliaDate(5, 2, 28)

            defaultMath.AddYears(date, 1)   === new TropicaliaDate(5, 2, 28)
            overspillMath.AddYears(date, 1) === new TropicaliaDate(5, 3, 1)
            exactMath.AddYears(date, 1)     === new TropicaliaDate(5, 3, 1)


    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<TropicaliaMonth, TropicaliaDate, StandardTropicaliaDataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new TropicaliaMonth(y, m)
            let date = new TropicaliaDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new TropicaliaMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<TropicaliaMonth, StandardTropicaliaDataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<TropicaliaYear, TropicaliaMonth, TropicaliaDate, StandardTropicaliaDataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new TropicaliaYear(y)
            let date = new TropicaliaMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new TropicaliaYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new TropicaliaYear(y)
            let date = new TropicaliaDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new TropicaliaYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
