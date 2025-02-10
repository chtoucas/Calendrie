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

module Prelude =
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
    let ``Value of FrenchRepublican13Calendar.Epoch.DaysZinceZero`` () =
        FrenchRepublican13Calendar.Instance.Epoch.DaysSinceZero === 654_414

    [<Fact>]
    let ``default(FrenchRepublican13Date) is FrenchRepublican13Calendar.Epoch`` () =
        Unchecked.defaultof<FrenchRepublican13Date>.DayNumber === FrenchRepublican13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MinDaysSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MaxDaysSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MaxDaysSinceEpoch === 3_652_056

    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MinMonthsSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MaxMonthsSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``FrenchRepublican13Month(FrenchRepublican13Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new FrenchRepublican13Date(y, m, d)
        let exp = new FrenchRepublican13Month(y, m)
        // Act & Assert
        new FrenchRepublican13Month(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``FrenchRepublican13Year(FrenchRepublican13Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new FrenchRepublican13Date(y, m, d)
        let exp = new FrenchRepublican13Year(y)
        // Act & Assert
        new FrenchRepublican13Year(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``FrenchRepublican13Year(FrenchRepublican13Month)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new FrenchRepublican13Month(y, m)
        let exp = new FrenchRepublican13Year(y)
        // Act & Assert
        new FrenchRepublican13Year(month) === exp

module Conversions =
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

    type GregorianDateCaster = FrenchRepublican13Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = FrenchRepublican13Date.op_Explicit

    type JulianDateCaster = FrenchRepublican13Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = FrenchRepublican13Date.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new FrenchRepublican13Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at FrenchRepublican13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican13Date.MinValue.DayNumber)

        FrenchRepublican13Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at FrenchRepublican13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican13Date.MaxValue.DayNumber)

        FrenchRepublican13Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to FrenchRepublican13Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at FrenchRepublican13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican13Date.MinValue.DayNumber)

        op_Explicit_Gregorian FrenchRepublican13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at FrenchRepublican13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican13Date.MaxValue.DayNumber)

        op_Explicit_Gregorian FrenchRepublican13Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at FrenchRepublican13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican13Date.MinValue.DayNumber)

        FrenchRepublican13Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at FrenchRepublican13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican13Date.MaxValue.DayNumber)

        FrenchRepublican13Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at FrenchRepublican13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican13Date.MinValue.DayNumber)

        op_Explicit_Julian FrenchRepublican13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at FrenchRepublican13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican13Date.MaxValue.DayNumber)

        op_Explicit_Julian FrenchRepublican13Date.MaxValue === exp

module Bundles =
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
        inherit CalendarFacts<FrenchRepublican13Calendar, StandardFrenchRepublican13DataSet>(FrenchRepublican13Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = FrenchRepublican13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = FrenchRepublican13Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<FrenchRepublican13Date, StandardFrenchRepublican13DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new FrenchRepublican13Date(y, m, d)
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
            let date = new FrenchRepublican13Date(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<FrenchRepublican13Date, StandardFrenchRepublican13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateMathFacts() =
        inherit DefaultDateMathFacts<FrenchRepublican13Date, StandardFrenchRepublican13DataSet>()

        static let defaultMath   = new DateMath()
        static let overspillMath = new DateMath(AdditionRule.Overspill)
        static let exactMath     = new DateMath(AdditionRule.Exact)

        [<Fact>]
        static member ``PlusYears() when roundof != 0`` () =
            let date = new FrenchRepublican13Date(4, 13, 6)
            // Act & Assert
            date.IsIntercalary |> ok

            let result: FrenchRepublican13Date * int = date.PlusYears(1)
            result === (new FrenchRepublican13Date(5, 13, 5), 1)

            date.PlusYears(1) === new FrenchRepublican13Date(5, 13, 5)

            defaultMath.AddYears(date, 1)   === new FrenchRepublican13Date(5, 13, 5)
            overspillMath.AddYears(date, 1) === new FrenchRepublican13Date(6, 1, 1)
            exactMath.AddYears(date, 1)     === new FrenchRepublican13Date(6, 1, 1)

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<FrenchRepublican13Month, FrenchRepublican13Date, StandardFrenchRepublican13DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new FrenchRepublican13Month(y, m)
            let date = new FrenchRepublican13Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new FrenchRepublican13Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(MonthFacts.MonthInfoData))>]
        static member ``Property IsIntercalary`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let month = new FrenchRepublican13Month(y, m)
            // Act & Assert
            month.IsIntercalary === (m = 13)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<FrenchRepublican13Month, StandardFrenchRepublican13DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<FrenchRepublican13Year, FrenchRepublican13Month, FrenchRepublican13Date, StandardFrenchRepublican13DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new FrenchRepublican13Year(y)
            let date = new FrenchRepublican13Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new FrenchRepublican13Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new FrenchRepublican13Year(y)
            let date = new FrenchRepublican13Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new FrenchRepublican13Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
