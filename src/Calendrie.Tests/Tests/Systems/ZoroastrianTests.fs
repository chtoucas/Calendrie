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

module Prelude =
    [<Fact>]
    let ``Value of ZoroastrianCalendar.Epoch.DaysZinceZero`` () =
        ZoroastrianCalendar.Instance.Epoch.DaysSinceZero === 230_637
    [<Fact>]
    let ``Value of Zoroastrian13Calendar.Epoch.DaysZinceZero`` () =
        Zoroastrian13Calendar.Instance.Epoch.DaysSinceZero === 230_637

    [<Fact>]
    let ``default(ZoroastrianDate) is ZoroastrianCalendar.Epoch`` () =
        Unchecked.defaultof<ZoroastrianDate>.DayNumber === ZoroastrianCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Zoroastrian13Date) is Zoroastrian13Calendar.Epoch`` () =
        Unchecked.defaultof<Zoroastrian13Date>.DayNumber === Zoroastrian13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of ZoroastrianCalendar.MinDaysSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Zoroastrian13Calendar.MinDaysSinceEpoch`` () =
        Zoroastrian13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of ZoroastrianCalendar.MaxDaysSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MaxDaysSinceEpoch === 3_649_634
    [<Fact>]
    let ``Value of Zoroastrian13Calendar.MaxDaysSinceEpoch`` () =
        Zoroastrian13Calendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of ZoroastrianCalendar.MinMonthsSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of Zoroastrian13Calendar.MinMonthsSinceEpoch`` () =
        Zoroastrian13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of ZoroastrianCalendar.MaxMonthsSinceEpoch`` () =
        ZoroastrianCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of Zoroastrian13Calendar.MaxMonthsSinceEpoch`` () =
        Zoroastrian13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Conversions =
    let private calendarDataSet = StandardZoroastrian12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = ZoroastrianDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = ZoroastrianDate.op_Explicit

    type JulianDateCaster = ZoroastrianDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = ZoroastrianDate.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ZoroastrianMonth:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new ZoroastrianDate(y, m, d)
        let exp = new ZoroastrianMonth(y, m)
        // Act & Assert
        ZoroastrianMonth.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ZoroastrianYear:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new ZoroastrianDate(y, m, d)
        let exp = new ZoroastrianYear(y)
        // Act & Assert
        ZoroastrianYear.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``ZoroastrianYear:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new ZoroastrianMonth(y, m)
        let exp = new ZoroastrianYear(y)
        // Act & Assert
        ZoroastrianYear.FromMonth(month) === exp

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

module Conversions13 =
    let private calendarDataSet = StandardZoroastrian13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Zoroastrian13Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = Zoroastrian13Date.op_Explicit

    type JulianDateCaster = Zoroastrian13Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = Zoroastrian13Date.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Zoroastrian13Month:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Zoroastrian13Date(y, m, d)
        let exp = new Zoroastrian13Month(y, m)
        // Act & Assert
        Zoroastrian13Month.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Zoroastrian13Year:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Zoroastrian13Date(y, m, d)
        let exp = new Zoroastrian13Year(y)
        // Act & Assert
        Zoroastrian13Year.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Zoroastrian13Year:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Zoroastrian13Month(y, m)
        let exp = new Zoroastrian13Year(y)
        // Act & Assert
        Zoroastrian13Year.FromMonth(month) === exp

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new Zoroastrian13Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Zoroastrian13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Zoroastrian13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian13Date.MinValue.DayNumber)

        Zoroastrian13Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Zoroastrian13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian13Date.MaxValue.DayNumber)

        Zoroastrian13Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Zoroastrian13Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Zoroastrian13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Zoroastrian13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian13Date.MinValue.DayNumber)

        op_Explicit_Gregorian Zoroastrian13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Zoroastrian13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian13Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Zoroastrian13Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Zoroastrian13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Zoroastrian13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian13Date.MinValue.DayNumber)

        Zoroastrian13Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Zoroastrian13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian13Date.MaxValue.DayNumber)

        Zoroastrian13Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Zoroastrian13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Zoroastrian13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian13Date.MinValue.DayNumber)

        op_Explicit_Julian Zoroastrian13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Zoroastrian13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian13Date.MaxValue.DayNumber)

        op_Explicit_Julian Zoroastrian13Date.MaxValue === exp

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<ZoroastrianCalendar, StandardZoroastrian12DataSet>(ZoroastrianCalendar.Instance)

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
        inherit IDateFacts<ZoroastrianDate, StandardZoroastrian12DataSet>()

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
        inherit IUnsafeDateFactoryFacts<ZoroastrianDate, StandardZoroastrian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<ZoroastrianDate, StandardZoroastrian12DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<ZoroastrianMonth, ZoroastrianDate, StandardZoroastrian12DataSet>()

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

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<ZoroastrianMonth, StandardZoroastrian12DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<ZoroastrianYear, ZoroastrianMonth, ZoroastrianDate, StandardZoroastrian12DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new ZoroastrianYear(y)
            let date = new ZoroastrianMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
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
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new ZoroastrianYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))

module Bundles13 =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Zoroastrian13Calendar, StandardZoroastrian13DataSet>(Zoroastrian13Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MinYear() = Zoroastrian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Zoroastrian13Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new Zoroastrian13Date(y, m, d)
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
            let date = new Zoroastrian13Date(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<Zoroastrian13Month, Zoroastrian13Date, StandardZoroastrian13DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new Zoroastrian13Month(y, m)
            let date = new Zoroastrian13Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new Zoroastrian13Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(MonthFacts.MonthInfoData))>]
        static member ``Property IsVirtual`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let month = new Zoroastrian13Month(y, m)
            // Act & Assert
            month.IsVirtual === (m = 13)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<Zoroastrian13Month, StandardZoroastrian13DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<Zoroastrian13Year, Zoroastrian13Month, Zoroastrian13Date, StandardZoroastrian13DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new Zoroastrian13Year(y)
            let date = new Zoroastrian13Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
            let year = new Zoroastrian13Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new Zoroastrian13Year(y)
            let date = new Zoroastrian13Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new Zoroastrian13Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
