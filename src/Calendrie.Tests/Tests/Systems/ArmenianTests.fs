// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.ArmenianTests

#nowarn 3391 // Implicit conversion to DayNumber

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    let private calendarDataSet = StandardArmenian12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of Armenian12Calendar.Epoch.DaysZinceZero`` () =
        Armenian12Calendar.Instance.Epoch.DaysSinceZero === 201_442

    [<Fact>]
    let ``default(Armenian12Date) is Armenian12Calendar.Epoch`` () =
        Unchecked.defaultof<Armenian12Date>.DayNumber === Armenian12Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of Armenian12Calendar.MinDaysSinceEpoch`` () =
        Armenian12Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of Armenian12Calendar.MaxDaysSinceEpoch`` () =
        Armenian12Calendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of Armenian12Calendar.MinMonthsSinceEpoch`` () =
        Armenian12Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of Armenian12Calendar.MaxMonthsSinceEpoch`` () =
        Armenian12Calendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Armenian12Month(Armenian12Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Armenian12Date(y, m, d)
        let exp = new Armenian12Month(y, m)
        // Act & Assert
        new Armenian12Month(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Armenian12Year(Armenian12Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Armenian12Date(y, m, d)
        let exp = new Armenian12Year(y)
        // Act & Assert
        new Armenian12Year(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Armenian12Year(Armenian12Month)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Armenian12Month(y, m)
        let exp = new Armenian12Year(y)
        // Act & Assert
        new Armenian12Year(month) === exp

module Prelude13 =
    let private calendarDataSet = StandardArmenian13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of Armenian13Calendar.Epoch.DaysZinceZero`` () =
        Armenian13Calendar.Instance.Epoch.DaysSinceZero === 201_442

    [<Fact>]
    let ``default(Armenian13Date) is Armenian13Calendar.Epoch`` () =
        Unchecked.defaultof<Armenian13Date>.DayNumber === Armenian13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of Armenian13Calendar.MinDaysSinceEpoch`` () =
        Armenian13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of Armenian13Calendar.MaxDaysSinceEpoch`` () =
        Armenian13Calendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of Armenian13Calendar.MinMonthsSinceEpoch`` () =
        Armenian13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of Armenian13Calendar.MaxMonthsSinceEpoch`` () =
        Armenian13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Armenian13Month(Armenian13Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Armenian13Date(y, m, d)
        let exp = new Armenian13Month(y, m)
        // Act & Assert
        new Armenian13Month(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Armenian13Year(Armenian13Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Armenian13Date(y, m, d)
        let exp = new Armenian13Year(y)
        // Act & Assert
        new Armenian13Year(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Armenian13Year(Armenian13Month)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Armenian13Month(y, m)
        let exp = new Armenian13Year(y)
        // Act & Assert
        new Armenian13Year(month) === exp

module Conversions =
    let private calendarDataSet = StandardArmenian12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Armenian12Date -> GregorianDate
    let op_Explicit_Gregorian: GregorianDateCaster = Armenian12Date.op_Explicit

    type JulianDateCaster = Armenian12Date -> JulianDate
    let op_Explicit_Julian: JulianDateCaster = Armenian12Date.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new Armenian12Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Armenian12Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Armenian12Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian12Date.MinValue.DayNumber)

        Armenian12Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Armenian12Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian12Date.MaxValue.DayNumber)

        Armenian12Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Armenian12Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Armenian12Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Armenian12Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian12Date.MinValue.DayNumber)

        op_Explicit_Gregorian Armenian12Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Armenian12Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian12Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Armenian12Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Armenian12Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Armenian12Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian12Date.MinValue.DayNumber)

        Armenian12Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Armenian12Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian12Date.MaxValue.DayNumber)

        Armenian12Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Armenian12Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Armenian12Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian12Date.MinValue.DayNumber)

        op_Explicit_Julian Armenian12Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Armenian12Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian12Date.MaxValue.DayNumber)

        op_Explicit_Julian Armenian12Date.MaxValue === exp

module Conversions13 =
    let private calendarDataSet = StandardArmenian13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Armenian13Date -> GregorianDate
    let op_Explicit_Gregorian: GregorianDateCaster = Armenian13Date.op_Explicit

    type JulianDateCaster = Armenian13Date -> JulianDate
    let op_Explicit_Julian: JulianDateCaster = Armenian13Date.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new Armenian13Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Armenian13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Armenian13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian13Date.MinValue.DayNumber)

        Armenian13Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Armenian13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian13Date.MaxValue.DayNumber)

        Armenian13Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Armenian13Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Armenian13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Armenian13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian13Date.MinValue.DayNumber)

        op_Explicit_Gregorian Armenian13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Armenian13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian13Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Armenian13Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Armenian13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Armenian13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian13Date.MinValue.DayNumber)

        Armenian13Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Armenian13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian13Date.MaxValue.DayNumber)

        Armenian13Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Armenian13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Armenian13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian13Date.MinValue.DayNumber)

        op_Explicit_Julian Armenian13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Armenian13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian13Date.MaxValue.DayNumber)

        op_Explicit_Julian Armenian13Date.MaxValue === exp

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Armenian12Calendar, StandardArmenian12DataSet>(Armenian12Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MinYear() = Armenian12Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Armenian12Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Armenian12Date, StandardArmenian12DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new Armenian12Date(y, m, d)
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
            let date = new Armenian12Date(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Armenian12Date, StandardArmenian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<Armenian12Date, StandardArmenian12DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<Armenian12Month, Armenian12Date, StandardArmenian12DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new Armenian12Month(y, m)
            let date = new Armenian12Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new Armenian12Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<Armenian12Month, StandardArmenian12DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<Armenian12Year, Armenian12Month, Armenian12Date, StandardArmenian12DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new Armenian12Year(y)
            let date = new Armenian12Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new Armenian12Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new Armenian12Year(y)
            let date = new Armenian12Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new Armenian12Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))

module Bundles13 =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Armenian13Calendar, StandardArmenian13DataSet>(Armenian13Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MinYear() = Armenian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Armenian13Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Armenian13Date, StandardArmenian13DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new Armenian13Date(y, m, d)
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
            let date = new Armenian13Date(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Armenian13Date, StandardArmenian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<Armenian13Date, StandardArmenian13DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<Armenian13Month, Armenian13Date, StandardArmenian13DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new Armenian13Month(y, m)
            let date = new Armenian13Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new Armenian13Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(MonthFacts.MonthInfoData))>]
        static member ``Property IsIntercalary`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let month = new Armenian13Month(y, m)
            // Act & Assert
            month.IsIntercalary === (m = 13)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<Armenian13Month, StandardArmenian13DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<Armenian13Year, Armenian13Month, Armenian13Date, StandardArmenian13DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new Armenian13Year(y)
            let date = new Armenian13Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new Armenian13Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new Armenian13Year(y)
            let date = new Armenian13Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new Armenian13Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
