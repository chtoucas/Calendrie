// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.FrenchRepublicanTests

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

module Prelude12 =
    let private calendarDataSet = StandardFrenchRepublican12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of FrenchRepublican12Calendar.Epoch.DaysZinceZero`` () =
        FrenchRepublican12Calendar.Instance.Epoch.DaysSinceZero === 654_414

    [<Fact>]
    let ``default(FrenchRepublican12Date) is FrenchRepublican12Calendar.Epoch`` () =
        Unchecked.defaultof<FrenchRepublican12Date>.DayNumber === FrenchRepublican12Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of FrenchRepublican12Calendar.MinDaysSinceEpoch`` () =
        FrenchRepublican12Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublican12Calendar.MaxDaysSinceEpoch`` () =
        FrenchRepublican12Calendar.Instance.MaxDaysSinceEpoch === 3_652_056

    [<Fact>]
    let ``Value of FrenchRepublican12Calendar.MinMonthsSinceEpoch`` () =
        FrenchRepublican12Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublican12Calendar.MaxMonthsSinceEpoch`` () =
        FrenchRepublican12Calendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``FrenchRepublican12Month(FrenchRepublican12Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new FrenchRepublican12Date(y, m, d)
        let exp = new FrenchRepublican12Month(y, m)
        // Act & Assert
        new FrenchRepublican12Month(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``FrenchRepublican12Year(FrenchRepublican12Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new FrenchRepublican12Date(y, m, d)
        let exp = new FrenchRepublican12Year(y)
        // Act & Assert
        new FrenchRepublican12Year(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``FrenchRepublican12Year(FrenchRepublican12Month)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new FrenchRepublican12Month(y, m)
        let exp = new FrenchRepublican12Year(y)
        // Act & Assert
        new FrenchRepublican12Year(month) === exp

module Prelude13 =
    let private calendarDataSet = StandardFrenchRepublican13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.Epoch.DaysZinceZero`` () =
        FrenchRepublicanCalendar.Instance.Epoch.DaysSinceZero === 654_414

    [<Fact>]
    let ``default(FrenchRepublicanDate) is FrenchRepublicanCalendar.Epoch`` () =
        Unchecked.defaultof<FrenchRepublicanDate>.DayNumber === FrenchRepublicanCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MinDaysSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MaxDaysSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MaxDaysSinceEpoch === 3_652_056

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MinMonthsSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MaxMonthsSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``FrenchRepublicanMonth(FrenchRepublicanDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new FrenchRepublicanDate(y, m, d)
        let exp = new FrenchRepublicanMonth(y, m)
        // Act & Assert
        new FrenchRepublicanMonth(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``FrenchRepublicanYear(FrenchRepublicanDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new FrenchRepublicanDate(y, m, d)
        let exp = new FrenchRepublicanYear(y)
        // Act & Assert
        new FrenchRepublicanYear(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``FrenchRepublicanYear(FrenchRepublicanMonth)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new FrenchRepublicanMonth(y, m)
        let exp = new FrenchRepublicanYear(y)
        // Act & Assert
        new FrenchRepublicanYear(month) === exp

module Conversions12 =
    let private calendarDataSet = StandardFrenchRepublican12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = FrenchRepublican12Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = FrenchRepublican12Date.op_Explicit

    type JulianDateCaster = FrenchRepublican12Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = FrenchRepublican12Date.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new FrenchRepublican12Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican12Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at FrenchRepublican12Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican12Date.MinValue.DayNumber)

        FrenchRepublican12Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at FrenchRepublican12Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican12Date.MaxValue.DayNumber)

        FrenchRepublican12Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to FrenchRepublican12Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican12Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at FrenchRepublican12Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican12Date.MinValue.DayNumber)

        op_Explicit_Gregorian FrenchRepublican12Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at FrenchRepublican12Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican12Date.MaxValue.DayNumber)

        op_Explicit_Gregorian FrenchRepublican12Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican12Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at FrenchRepublican12Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican12Date.MinValue.DayNumber)

        FrenchRepublican12Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at FrenchRepublican12Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican12Date.MaxValue.DayNumber)

        FrenchRepublican12Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican12Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at FrenchRepublican12Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican12Date.MinValue.DayNumber)

        op_Explicit_Julian FrenchRepublican12Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at FrenchRepublican12Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican12Date.MaxValue.DayNumber)

        op_Explicit_Julian FrenchRepublican12Date.MaxValue === exp

module Conversions13 =
    let private calendarDataSet = StandardFrenchRepublican13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = FrenchRepublicanDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = FrenchRepublicanDate.op_Explicit

    type JulianDateCaster = FrenchRepublicanDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = FrenchRepublicanDate.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new FrenchRepublicanDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublicanDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at FrenchRepublicanDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublicanDate.MinValue.DayNumber)

        FrenchRepublicanDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at FrenchRepublicanDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublicanDate.MaxValue.DayNumber)

        FrenchRepublicanDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to FrenchRepublicanDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublicanDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at FrenchRepublicanDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublicanDate.MinValue.DayNumber)

        op_Explicit_Gregorian FrenchRepublicanDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at FrenchRepublicanDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublicanDate.MaxValue.DayNumber)

        op_Explicit_Gregorian FrenchRepublicanDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublicanDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at FrenchRepublicanDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublicanDate.MinValue.DayNumber)

        FrenchRepublicanDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at FrenchRepublicanDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublicanDate.MaxValue.DayNumber)

        FrenchRepublicanDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublicanDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at FrenchRepublicanDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublicanDate.MinValue.DayNumber)

        op_Explicit_Julian FrenchRepublicanDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at FrenchRepublicanDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublicanDate.MaxValue.DayNumber)

        op_Explicit_Julian FrenchRepublicanDate.MaxValue === exp

module Bundles12 =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<FrenchRepublican12Calendar, StandardFrenchRepublican12DataSet>(FrenchRepublican12Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = FrenchRepublican12Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = FrenchRepublican12Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<FrenchRepublican12Date, StandardFrenchRepublican12DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new FrenchRepublican12Date(y, m, d)
            // Act
            let isEpagomenal, epanum = date.IsEpagomenal()
            // Assert
            info.IsSupplementary === isEpagomenal
            if isEpagomenal then
                epanum > 0 |> ok
            else
                epanum === 0

        [<Theory; MemberData(nameof(DateFacts.EpagomenalDayInfoData))>]
        static member ``IsEpagomenal() check out param`` (info: YemodaAnd<int>) =
            let y, m, d, epanum = info.Deconstruct()
            let date = new FrenchRepublican12Date(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<FrenchRepublican12Date, StandardFrenchRepublican12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateMathFacts() =
        inherit DefaultDateMathFacts<FrenchRepublican12Date, StandardFrenchRepublican12DataSet>()

        static let defaultMath   = new DateMath()
        static let overspillMath = new DateMath(AdditionRule.Overspill)
        static let exactMath     = new DateMath(AdditionRule.Exact)

        [<Fact>]
        static member ``PlusYears() when roundof != 0`` () =
            let date = new FrenchRepublican12Date(4, 12, 36)
            // Act & Assert
            date.IsIntercalary |> ok

            let result: FrenchRepublican12Date * int = date.PlusYears(1)
            result === (new FrenchRepublican12Date(5, 12, 35), 1)

            date.PlusYears(1) === new FrenchRepublican12Date(5, 12, 35)

            defaultMath.AddYears(date, 1)   === new FrenchRepublican12Date(5, 12, 35)
            overspillMath.AddYears(date, 1) === new FrenchRepublican12Date(6, 1, 1)
            exactMath.AddYears(date, 1)     === new FrenchRepublican12Date(6, 1, 1)

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<FrenchRepublican12Month, FrenchRepublican12Date, StandardFrenchRepublican12DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new FrenchRepublican12Month(y, m)
            let date = new FrenchRepublican12Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new FrenchRepublican12Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<FrenchRepublican12Month, StandardFrenchRepublican12DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<FrenchRepublican12Year, FrenchRepublican12Month, FrenchRepublican12Date, StandardFrenchRepublican12DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new FrenchRepublican12Year(y)
            let date = new FrenchRepublican12Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new FrenchRepublican12Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new FrenchRepublican12Year(y)
            let date = new FrenchRepublican12Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new FrenchRepublican12Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))

module Bundles13 =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<FrenchRepublicanCalendar, StandardFrenchRepublican13DataSet>(FrenchRepublicanCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = FrenchRepublicanCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = FrenchRepublicanCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<FrenchRepublicanDate, StandardFrenchRepublican13DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new FrenchRepublicanDate(y, m, d)
            // Act
            let isEpagomenal, epanum = date.IsEpagomenal()
            // Assert
            info.IsSupplementary === isEpagomenal
            if isEpagomenal then
                epanum > 0 |> ok
            else
                epanum === 0

        [<Theory; MemberData(nameof(DateFacts.EpagomenalDayInfoData))>]
        static member ``IsEpagomenal() check out param`` (info: YemodaAnd<int>) =
            let y, m, d, epanum = info.Deconstruct()
            let date = new FrenchRepublicanDate(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<FrenchRepublicanDate, StandardFrenchRepublican13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateMathFacts() =
        inherit DefaultDateMathFacts<FrenchRepublicanDate, StandardFrenchRepublican13DataSet>()

        static let defaultMath   = new DateMath()
        static let overspillMath = new DateMath(AdditionRule.Overspill)
        static let exactMath     = new DateMath(AdditionRule.Exact)

        [<Fact>]
        static member ``PlusYears() when roundof != 0`` () =
            let date = new FrenchRepublicanDate(4, 13, 6)
            // Act & Assert
            date.IsIntercalary |> ok

            let result: FrenchRepublicanDate * int = date.PlusYears(1)
            result === (new FrenchRepublicanDate(5, 13, 5), 1)

            date.PlusYears(1) === new FrenchRepublicanDate(5, 13, 5)

            defaultMath.AddYears(date, 1)   === new FrenchRepublicanDate(5, 13, 5)
            overspillMath.AddYears(date, 1) === new FrenchRepublicanDate(6, 1, 1)
            exactMath.AddYears(date, 1)     === new FrenchRepublicanDate(6, 1, 1)

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<FrenchRepublicanMonth, FrenchRepublicanDate, StandardFrenchRepublican13DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new FrenchRepublicanMonth(y, m)
            let date = new FrenchRepublicanDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new FrenchRepublicanMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(MonthFacts.MonthInfoData))>]
        static member ``Property IsIntercalary`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let month = new FrenchRepublicanMonth(y, m)
            // Act & Assert
            month.IsIntercalary === (m = 13)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<FrenchRepublicanMonth, StandardFrenchRepublican13DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<FrenchRepublicanYear, FrenchRepublicanMonth, FrenchRepublicanDate, StandardFrenchRepublican13DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new FrenchRepublicanYear(y)
            let date = new FrenchRepublicanMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new FrenchRepublicanYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new FrenchRepublicanYear(y)
            let date = new FrenchRepublicanDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new FrenchRepublicanYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
