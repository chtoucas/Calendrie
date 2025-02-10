// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.CopticTests

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
    let private calendarDataSet = StandardCoptic12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of Coptic12Calendar.Epoch.DaysZinceZero`` () =
        Coptic12Calendar.Instance.Epoch.DaysSinceZero === 103_604

    [<Fact>]
    let ``default(Coptic12Date) is Coptic12Calendar.Epoch`` () =
        Unchecked.defaultof<Coptic12Date>.DayNumber === Coptic12Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of Coptic12Calendar.MinDaysSinceEpoch`` () =
        Coptic12Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of Coptic12Calendar.MaxDaysSinceEpoch`` () =
        Coptic12Calendar.Instance.MaxDaysSinceEpoch === 3_652_134

    [<Fact>]
    let ``Value of Coptic12Calendar.MinMonthsSinceEpoch`` () =
        Coptic12Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of Coptic12Calendar.MaxMonthsSinceEpoch`` () =
        Coptic12Calendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Coptic12Month(Coptic12Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Coptic12Date(y, m, d)
        let exp = new Coptic12Month(y, m)
        // Act & Assert
        new Coptic12Month(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Coptic12Year(Coptic12Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Coptic12Date(y, m, d)
        let exp = new Coptic12Year(y)
        // Act & Assert
        new Coptic12Year(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Coptic12Year(Coptic12Month)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Coptic12Month(y, m)
        let exp = new Coptic12Year(y)
        // Act & Assert
        new Coptic12Year(month) === exp

module Prelude13 =
    let private calendarDataSet = StandardCoptic13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of Coptic13Calendar.Epoch.DaysZinceZero`` () =
        Coptic13Calendar.Instance.Epoch.DaysSinceZero === 103_604

    [<Fact>]
    let ``default(Coptic13Date) is Coptic13Calendar.Epoch`` () =
        Unchecked.defaultof<Coptic13Date>.DayNumber === Coptic13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of Coptic13Calendar.MinDaysSinceEpoch`` () =
        Coptic13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of Coptic13Calendar.MaxDaysSinceEpoch`` () =
        Coptic13Calendar.Instance.MaxDaysSinceEpoch === 3_652_134

    [<Fact>]
    let ``Value of Coptic13Calendar.MinMonthsSinceEpoch`` () =
        Coptic13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of Coptic13Calendar.MaxMonthsSinceEpoch`` () =
        Coptic13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Coptic13Month(Coptic13Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Coptic13Date(y, m, d)
        let exp = new Coptic13Month(y, m)
        // Act & Assert
        new Coptic13Month(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Coptic13Year(Coptic13Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Coptic13Date(y, m, d)
        let exp = new Coptic13Year(y)
        // Act & Assert
        new Coptic13Year(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Coptic13Year(Coptic13Month)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Coptic13Month(y, m)
        let exp = new Coptic13Year(y)
        // Act & Assert
        new Coptic13Year(month) === exp

module Conversions12 =
    let private calendarDataSet = StandardCoptic12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Coptic12Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = Coptic12Date.op_Explicit

    type JulianDateCaster = Coptic12Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = Coptic12Date.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new Coptic12Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Coptic12Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Coptic12Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Coptic12Date.MinValue.DayNumber)

        Coptic12Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Coptic12Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Coptic12Date.MaxValue.DayNumber)

        Coptic12Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Coptic12Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Coptic12Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Coptic12Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Coptic12Date.MinValue.DayNumber)

        op_Explicit_Gregorian Coptic12Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Coptic12Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Coptic12Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Coptic12Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Coptic12Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Coptic12Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Coptic12Date.MinValue.DayNumber)

        Coptic12Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Coptic12Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Coptic12Date.MaxValue.DayNumber)

        Coptic12Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Coptic12Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Coptic12Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Coptic12Date.MinValue.DayNumber)

        op_Explicit_Julian Coptic12Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Coptic12Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Coptic12Date.MaxValue.DayNumber)

        op_Explicit_Julian Coptic12Date.MaxValue === exp

module Conversions13 =
    let private calendarDataSet = StandardCoptic13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Coptic13Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = Coptic13Date.op_Explicit

    type JulianDateCaster = Coptic13Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = Coptic13Date.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new Coptic13Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Coptic13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Coptic13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Coptic13Date.MinValue.DayNumber)

        Coptic13Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Coptic13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Coptic13Date.MaxValue.DayNumber)

        Coptic13Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Coptic13Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Coptic13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Coptic13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Coptic13Date.MinValue.DayNumber)

        op_Explicit_Gregorian Coptic13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Coptic13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Coptic13Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Coptic13Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Coptic13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Coptic13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Coptic13Date.MinValue.DayNumber)

        Coptic13Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Coptic13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Coptic13Date.MaxValue.DayNumber)

        Coptic13Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Coptic13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Coptic13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Coptic13Date.MinValue.DayNumber)

        op_Explicit_Julian Coptic13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Coptic13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Coptic13Date.MaxValue.DayNumber)

        op_Explicit_Julian Coptic13Date.MaxValue === exp

module Bundles12 =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Coptic12Calendar, StandardCoptic12DataSet>(Coptic12Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = Coptic12Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Coptic12Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Coptic12Date, StandardCoptic12DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new Coptic12Date(y, m, d)
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
            let date = new Coptic12Date(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Coptic12Date, StandardCoptic12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateMathFacts() =
        inherit DefaultDateMathFacts<Coptic12Date, StandardCoptic12DataSet>()

        static let defaultMath   = new DateMath()
        static let overspillMath = new DateMath(AdditionRule.Overspill)
        static let exactMath     = new DateMath(AdditionRule.Exact)

        [<Fact>]
        static member ``PlusYears() when roundof != 0`` () =
            let date = new Coptic12Date(3, 12, 36)
            // Act & Assert
            date.IsIntercalary |> ok

            let result: Coptic12Date * int = date.PlusYears(1)
            result === (new Coptic12Date(4, 12, 35), 1)

            date.PlusYears(1) === new Coptic12Date(4, 12, 35)

            defaultMath.AddYears(date, 1)   === new Coptic12Date(4, 12, 35)
            overspillMath.AddYears(date, 1) === new Coptic12Date(5, 1, 1)
            exactMath.AddYears(date, 1)     === new Coptic12Date(5, 1, 1)

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<Coptic12Month, Coptic12Date, StandardCoptic12DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new Coptic12Month(y, m)
            let date = new Coptic12Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new Coptic12Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<Coptic12Month, StandardCoptic12DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<Coptic12Year, Coptic12Month, Coptic12Date, StandardCoptic12DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new Coptic12Year(y)
            let date = new Coptic12Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new Coptic12Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new Coptic12Year(y)
            let date = new Coptic12Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new Coptic12Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))

module Bundles13 =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Coptic13Calendar, StandardCoptic13DataSet>(Coptic13Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = Coptic13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Coptic13Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Coptic13Date, StandardCoptic13DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new Coptic13Date(y, m, d)
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
            let date = new Coptic13Date(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Coptic13Date, StandardCoptic13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<Coptic13Date, StandardCoptic13DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<Coptic13Month, Coptic13Date, StandardCoptic13DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new Coptic13Month(y, m)
            let date = new Coptic13Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new Coptic13Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(MonthFacts.MonthInfoData))>]
        static member ``Property IsIntercalary`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let month = new Coptic13Month(y, m)
            // Act & Assert
            month.IsIntercalary === (m = 13)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<Coptic13Month, StandardCoptic13DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<Coptic13Year, Coptic13Month, Coptic13Date, StandardCoptic13DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new Coptic13Year(y)
            let date = new Coptic13Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new Coptic13Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new Coptic13Year(y)
            let date = new Coptic13Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new Coptic13Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
