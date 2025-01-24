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
    [<Fact>]
    let ``Value of ArmenianCalendar.Epoch.DaysZinceZero`` () =
        ArmenianCalendar.Instance.Epoch.DaysSinceZero === 201_442
    [<Fact>]
    let ``Value of Armenian13Calendar.Epoch.DaysZinceZero`` () =
        Armenian13Calendar.Instance.Epoch.DaysSinceZero === 201_442

    [<Fact>]
    let ``default(ArmenianDate) is ArmenianCalendar.Epoch`` () =
        Unchecked.defaultof<ArmenianDate>.DayNumber === ArmenianCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Armenian13Date) is Armenian13Calendar.Epoch`` () =
        Unchecked.defaultof<Armenian13Date>.DayNumber === Armenian13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of ArmenianCalendar.MinDaysSinceEpoch`` () =
        ArmenianCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Armenian13Calendar.MinDaysSinceEpoch`` () =
        Armenian13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of ArmenianCalendar.MaxDaysSinceEpoch`` () =
        ArmenianCalendar.Instance.MaxDaysSinceEpoch === 3_649_634
    [<Fact>]
    let ``Value of Armenian13Calendar.MaxDaysSinceEpoch`` () =
        Armenian13Calendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of ArmenianCalendar.MinMonthsSinceEpoch`` () =
        ArmenianCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of Armenian13Calendar.MinMonthsSinceEpoch`` () =
        Armenian13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of ArmenianCalendar.MaxMonthsSinceEpoch`` () =
        ArmenianCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of Armenian13Calendar.MaxMonthsSinceEpoch`` () =
        Armenian13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Conversions =
    let private calendarDataSet = StandardArmenian12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = ArmenianDate -> GregorianDate
    let op_Explicit_Gregorian: GregorianDateCaster = ArmenianDate.op_Explicit

    type JulianDateCaster = ArmenianDate -> JulianDate
    let op_Explicit_Julian: JulianDateCaster = ArmenianDate.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ArmenianMonth:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new ArmenianDate(y, m, d)
        let exp = new ArmenianMonth(y, m)
        // Act & Assert
        ArmenianMonth.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ArmenianYear:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new ArmenianDate(y, m, d)
        let exp = new ArmenianYear(y)
        // Act & Assert
        ArmenianYear.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``ArmenianYear:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new ArmenianMonth(y, m)
        let exp = new ArmenianYear(y)
        // Act & Assert
        ArmenianYear.FromMonth(month) === exp

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new ArmenianDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ArmenianDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at ArmenianDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ArmenianDate.MinValue.DayNumber)

        ArmenianDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at ArmenianDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ArmenianDate.MaxValue.DayNumber)

        ArmenianDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to ArmenianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ArmenianDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at ArmenianDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ArmenianDate.MinValue.DayNumber)

        op_Explicit_Gregorian ArmenianDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at ArmenianDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ArmenianDate.MaxValue.DayNumber)

        op_Explicit_Gregorian ArmenianDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ArmenianDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at ArmenianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ArmenianDate.MinValue.DayNumber)

        ArmenianDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at ArmenianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ArmenianDate.MaxValue.DayNumber)

        ArmenianDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ArmenianDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at ArmenianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ArmenianDate.MinValue.DayNumber)

        op_Explicit_Julian ArmenianDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at ArmenianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ArmenianDate.MaxValue.DayNumber)

        op_Explicit_Julian ArmenianDate.MaxValue === exp

module Conversions13 =
    let private calendarDataSet = StandardArmenian13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Armenian13Date -> GregorianDate
    let op_Explicit_Gregorian: GregorianDateCaster = Armenian13Date.op_Explicit

    type JulianDateCaster = Armenian13Date -> JulianDate
    let op_Explicit_Julian: JulianDateCaster = Armenian13Date.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Armenian13Month:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Armenian13Date(y, m, d)
        let exp = new Armenian13Month(y, m)
        // Act & Assert
        Armenian13Month.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Armenian13Year:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Armenian13Date(y, m, d)
        let exp = new Armenian13Year(y)
        // Act & Assert
        Armenian13Year.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Armenian13Year:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Armenian13Month(y, m)
        let exp = new Armenian13Year(y)
        // Act & Assert
        Armenian13Year.FromMonth(month) === exp

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
        inherit CalendarFacts<ArmenianCalendar, StandardArmenian12DataSet>(ArmenianCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MinYear() = ArmenianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = ArmenianCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<ArmenianDate, StandardArmenian12DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new ArmenianDate(y, m, d)
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
            let date = new ArmenianDate(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<ArmenianDate, StandardArmenian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<ArmenianDate, StandardArmenian12DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<ArmenianMonth, ArmenianDate, StandardArmenian12DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new ArmenianMonth(y, m)
            let date = new ArmenianDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new ArmenianMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<ArmenianMonth, StandardArmenian12DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<ArmenianYear, ArmenianMonth, ArmenianDate, StandardArmenian12DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new ArmenianYear(y)
            let date = new ArmenianMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
            let year = new ArmenianYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new ArmenianYear(y)
            let date = new ArmenianDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new ArmenianYear(y)
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
        static member ``Property IsVirtual`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let month = new Armenian13Month(y, m)
            // Act & Assert
            month.IsVirtual === (m = 13)

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
        static member ``GetMonthOfYear() with an invalid month`` y m =
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
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new Armenian13Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
