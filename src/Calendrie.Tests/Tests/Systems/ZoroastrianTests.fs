// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.ZoroastrianTests

#nowarn 3391 // Implicit conversion to DayNumber or GregorianDate

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude12 =
    let private calendarDataSet = StandardZoroastrian12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of Zoroastrian12Calendar.Epoch.DaysZinceZero`` () =
        Zoroastrian12Calendar.Instance.Epoch.DaysSinceZero === 230_637

    [<Fact>]
    let ``default(Zoroastrian12Date) is Zoroastrian12Calendar.Epoch`` () =
        Unchecked.defaultof<Zoroastrian12Date>.DayNumber === Zoroastrian12Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of Zoroastrian12Calendar.MinDaysSinceEpoch`` () =
        Zoroastrian12Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of Zoroastrian12Calendar.MaxDaysSinceEpoch`` () =
        Zoroastrian12Calendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of Zoroastrian12Calendar.MinMonthsSinceEpoch`` () =
        Zoroastrian12Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of Zoroastrian12Calendar.MaxMonthsSinceEpoch`` () =
        Zoroastrian12Calendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Zoroastrian12Month(Zoroastrian12Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Zoroastrian12Date(y, m, d)
        let exp = new Zoroastrian12Month(y, m)
        // Act & Assert
        new Zoroastrian12Month(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Zoroastrian12Year(Zoroastrian12Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Zoroastrian12Date(y, m, d)
        let exp = new Zoroastrian12Year(y)
        // Act & Assert
        new Zoroastrian12Year(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Zoroastrian12Year(Zoroastrian12Month)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Zoroastrian12Month(y, m)
        let exp = new Zoroastrian12Year(y)
        // Act & Assert
        new Zoroastrian12Year(month) === exp

module Prelude13 =
    let private calendarDataSet = StandardZoroastrian13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of ZoroastrianCalendar.Epoch.DaysZinceZero`` () =
        ZoroastrianCalendar.Instance.Epoch.DaysSinceZero === 230_637

    [<Fact>]
    let ``default(ZoroastrianDate) is ZoroastrianCalendar.Epoch`` () =
        Unchecked.defaultof<ZoroastrianDate>.DayNumber === ZoroastrianCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of ZoroastrianCalendar.MinDaysSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of ZoroastrianCalendar.MaxDaysSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of ZoroastrianCalendar.MinMonthsSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of ZoroastrianCalendar.MaxMonthsSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ZoroastrianMonth(ZoroastrianDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new ZoroastrianDate(y, m, d)
        let exp = new ZoroastrianMonth(y, m)
        // Act & Assert
        new ZoroastrianMonth(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ZoroastrianYear(ZoroastrianDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new ZoroastrianDate(y, m, d)
        let exp = new ZoroastrianYear(y)
        // Act & Assert
        new ZoroastrianYear(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``ZoroastrianYear(ZoroastrianMonth)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new ZoroastrianMonth(y, m)
        let exp = new ZoroastrianYear(y)
        // Act & Assert
        new ZoroastrianYear(month) === exp

module Conversions12 =
    let private calendarDataSet = StandardZoroastrian12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Zoroastrian12Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = Zoroastrian12Date.op_Explicit

    type JulianDateCaster = Zoroastrian12Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = Zoroastrian12Date.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new Zoroastrian12Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Zoroastrian12Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Zoroastrian12Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian12Date.MinValue.DayNumber)

        Zoroastrian12Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Zoroastrian12Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian12Date.MaxValue.DayNumber)

        Zoroastrian12Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Zoroastrian12Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Zoroastrian12Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Zoroastrian12Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian12Date.MinValue.DayNumber)

        op_Explicit_Gregorian Zoroastrian12Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Zoroastrian12Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian12Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Zoroastrian12Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Zoroastrian12Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Zoroastrian12Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian12Date.MinValue.DayNumber)

        Zoroastrian12Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Zoroastrian12Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian12Date.MaxValue.DayNumber)

        Zoroastrian12Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Zoroastrian12Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Zoroastrian12Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian12Date.MinValue.DayNumber)

        op_Explicit_Julian Zoroastrian12Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Zoroastrian12Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian12Date.MaxValue.DayNumber)

        op_Explicit_Julian Zoroastrian12Date.MaxValue === exp

module Conversions13 =
    let private calendarDataSet = StandardZoroastrian13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = ZoroastrianDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = ZoroastrianDate.op_Explicit

    type JulianDateCaster = ZoroastrianDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = ZoroastrianDate.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new ZoroastrianDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ZoroastrianDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at ZoroastrianDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ZoroastrianDate.MinValue.DayNumber)

        ZoroastrianDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at ZoroastrianDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ZoroastrianDate.MaxValue.DayNumber)

        ZoroastrianDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to ZoroastrianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ZoroastrianDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at ZoroastrianDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ZoroastrianDate.MinValue.DayNumber)

        op_Explicit_Gregorian ZoroastrianDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at ZoroastrianDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ZoroastrianDate.MaxValue.DayNumber)

        op_Explicit_Gregorian ZoroastrianDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ZoroastrianDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at ZoroastrianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ZoroastrianDate.MinValue.DayNumber)

        ZoroastrianDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at ZoroastrianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ZoroastrianDate.MaxValue.DayNumber)

        ZoroastrianDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ZoroastrianDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at ZoroastrianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ZoroastrianDate.MinValue.DayNumber)

        op_Explicit_Julian ZoroastrianDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at ZoroastrianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ZoroastrianDate.MaxValue.DayNumber)

        op_Explicit_Julian ZoroastrianDate.MaxValue === exp

module Bundles12 =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Zoroastrian12Calendar, StandardZoroastrian12DataSet>(Zoroastrian12Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MinYear() = Zoroastrian12Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Zoroastrian12Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Zoroastrian12Date, StandardZoroastrian12DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new Zoroastrian12Date(y, m, d)
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
            let date = new Zoroastrian12Date(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Zoroastrian12Date, StandardZoroastrian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<Zoroastrian12Date, StandardZoroastrian12DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<Zoroastrian12Month, Zoroastrian12Date, StandardZoroastrian12DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new Zoroastrian12Month(y, m)
            let date = new Zoroastrian12Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new Zoroastrian12Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<Zoroastrian12Month, StandardZoroastrian12DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<Zoroastrian12Year, Zoroastrian12Month, Zoroastrian12Date, StandardZoroastrian12DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new Zoroastrian12Year(y)
            let date = new Zoroastrian12Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new Zoroastrian12Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new Zoroastrian12Year(y)
            let date = new Zoroastrian12Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new Zoroastrian12Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))

module Bundles13 =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<ZoroastrianCalendar, StandardZoroastrian13DataSet>(ZoroastrianCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MinYear() = ZoroastrianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = ZoroastrianCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<ZoroastrianDate, StandardZoroastrian13DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new ZoroastrianDate(y, m, d)
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
            let date = new ZoroastrianDate(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<ZoroastrianDate, StandardZoroastrian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<ZoroastrianDate, StandardZoroastrian13DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<ZoroastrianMonth, ZoroastrianDate, StandardZoroastrian13DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new ZoroastrianMonth(y, m)
            let date = new ZoroastrianDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new ZoroastrianMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(MonthFacts.MonthInfoData))>]
        static member ``Property IsIntercalary`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let month = new ZoroastrianMonth(y, m)
            // Act & Assert
            month.IsIntercalary === (m = 13)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<ZoroastrianMonth, StandardZoroastrian13DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<ZoroastrianYear, ZoroastrianMonth, ZoroastrianDate, StandardZoroastrian13DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new ZoroastrianYear(y)
            let date = new ZoroastrianMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new ZoroastrianYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new ZoroastrianYear(y)
            let date = new ZoroastrianDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new ZoroastrianYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
