// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.CopticTests

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
    let ``Value of CopticCalendar.Epoch.DaysZinceZero`` () =
        CopticCalendar.Instance.Epoch.DaysSinceZero === 103_604
    [<Fact>]
    let ``Value of Coptic13Calendar.Epoch.DaysZinceZero`` () =
        Coptic13Calendar.Instance.Epoch.DaysSinceZero === 103_604

    [<Fact>]
    let ``default(CopticDate) is CopticCalendar.Epoch`` () =
        Unchecked.defaultof<CopticDate>.DayNumber === CopticCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Coptic13Date) is Coptic13Calendar.Epoch`` () =
        Unchecked.defaultof<Coptic13Date>.DayNumber === Coptic13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of CopticCalendar.MinDaysSinceEpoch`` () =
        CopticCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Coptic13Calendar.MinDaysSinceEpoch`` () =
        Coptic13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of CopticCalendar.MaxDaysSinceEpoch`` () =
        CopticCalendar.Instance.MaxDaysSinceEpoch === 3_652_134
    [<Fact>]
    let ``Value of Coptic13Calendar.MaxDaysSinceEpoch`` () =
        Coptic13Calendar.Instance.MaxDaysSinceEpoch === 3_652_134

    [<Fact>]
    let ``Value of CopticCalendar.MinMonthsSinceEpoch`` () =
        CopticCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of Coptic13Calendar.MinMonthsSinceEpoch`` () =
        Coptic13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of CopticCalendar.MaxMonthsSinceEpoch`` () =
        CopticCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of Coptic13Calendar.MaxMonthsSinceEpoch`` () =
        Coptic13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Conversions =
    let private calendarDataSet = StandardCoptic12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = CopticDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = CopticDate.op_Explicit

    type JulianDateCaster = CopticDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = CopticDate.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``CopticMonth:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new CopticDate(y, m, d)
        let exp = new CopticMonth(y, m)
        // Act & Assert
        CopticMonth.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``CopticYear:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new CopticDate(y, m, d)
        let exp = new CopticYear(y)
        // Act & Assert
        CopticYear.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``CopticYear:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new CopticMonth(y, m)
        let exp = new CopticYear(y)
        // Act & Assert
        CopticYear.FromMonth(month) === exp

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new CopticDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CopticDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at CopticDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(CopticDate.MinValue.DayNumber)

        CopticDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at CopticDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(CopticDate.MaxValue.DayNumber)

        CopticDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to CopticDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CopticDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at CopticDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(CopticDate.MinValue.DayNumber)

        op_Explicit_Gregorian CopticDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at CopticDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(CopticDate.MaxValue.DayNumber)

        op_Explicit_Gregorian CopticDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CopticDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at CopticDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CopticDate.MinValue.DayNumber)

        CopticDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at CopticDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CopticDate.MaxValue.DayNumber)

        CopticDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new CopticDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at CopticDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CopticDate.MinValue.DayNumber)

        op_Explicit_Julian CopticDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at CopticDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(CopticDate.MaxValue.DayNumber)

        op_Explicit_Julian CopticDate.MaxValue === exp

module Conversions13 =
    let private calendarDataSet = StandardCoptic13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Coptic13Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = Coptic13Date.op_Explicit

    type JulianDateCaster = Coptic13Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = Coptic13Date.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Coptic13Month:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Coptic13Date(y, m, d)
        let exp = new Coptic13Month(y, m)
        // Act & Assert
        Coptic13Month.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Coptic13Year:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Coptic13Date(y, m, d)
        let exp = new Coptic13Year(y)
        // Act & Assert
        Coptic13Year.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Coptic13Year:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Coptic13Month(y, m)
        let exp = new Coptic13Year(y)
        // Act & Assert
        Coptic13Year.FromMonth(month) === exp

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

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<CopticCalendar, StandardCoptic12DataSet>(CopticCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = CopticCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = CopticCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<CopticDate, StandardCoptic12DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Armenian-only property
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new CopticDate(y, m, d);
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
            let date = new CopticDate(y, m, d);
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<CopticDate, StandardCoptic12DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<CopticMonth, CopticDate, StandardCoptic12DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new CopticMonth(y, m)
            let date = new CopticDate(y, m, d);
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new CopticMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<CopticMonth, StandardCoptic12DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<CopticYear, CopticMonth, CopticDate, StandardCoptic12DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new CopticYear(y)
            let date = new CopticMonth(y, m);
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
            let year = new CopticYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new CopticYear(y)
            let date = new CopticDate(y, doy);
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new CopticYear(y)
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
        // Armenian-only property
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new Coptic13Date(y, m, d);
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
            let date = new Coptic13Date(y, m, d);
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Coptic13Date, StandardCoptic13DataSet>()

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
            let date = new Coptic13Date(y, m, d);
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new Coptic13Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // Coptic13-only property
        //

        [<Theory; MemberData(nameof(MonthFacts.MonthInfoData))>]
        static member ``Property IsVirtual`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let month = new Coptic13Month(y, m)
            // Act & Assert
            month.IsVirtual === (m = 13)

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
            let date = new Coptic13Month(y, m);
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
            let year = new Coptic13Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new Coptic13Year(y)
            let date = new Coptic13Date(y, doy);
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new Coptic13Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
