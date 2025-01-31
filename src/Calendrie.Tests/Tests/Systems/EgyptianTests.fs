// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.EgyptianTests

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
    let private calendarDataSet = StandardEgyptian12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of EgyptianCalendar.Epoch.DaysZinceZero`` () =
        EgyptianCalendar.Instance.Epoch.DaysSinceZero === -272_788

    [<Fact>]
    let ``default(EgyptianDate) is EgyptianCalendar.Epoch`` () =
        Unchecked.defaultof<EgyptianDate>.DayNumber === EgyptianCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of EgyptianCalendar.MinDaysSinceEpoch`` () =
        EgyptianCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of EgyptianCalendar.MaxDaysSinceEpoch`` () =
        EgyptianCalendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of EgyptianCalendar.MinMonthsSinceEpoch`` () =
        EgyptianCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of EgyptianCalendar.MaxMonthsSinceEpoch`` () =
        EgyptianCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``EgyptianMonth(EgyptianDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new EgyptianDate(y, m, d)
        let exp = new EgyptianMonth(y, m)
        // Act & Assert
        new EgyptianMonth(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``EgyptianYear(EgyptianDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new EgyptianDate(y, m, d)
        let exp = new EgyptianYear(y)
        // Act & Assert
        new EgyptianYear(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``EgyptianYear(EgyptianMonth)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new EgyptianMonth(y, m)
        let exp = new EgyptianYear(y)
        // Act & Assert
        new EgyptianYear(month) === exp

module Prelude13 =
    let private calendarDataSet = StandardEgyptian13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of Egyptian13Calendar.Epoch.DaysZinceZero`` () =
        Egyptian13Calendar.Instance.Epoch.DaysSinceZero === -272_788

    [<Fact>]
    let ``default(Egyptian13Date) is Egyptian13Calendar.Epoch`` () =
        Unchecked.defaultof<Egyptian13Date>.DayNumber === Egyptian13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of Egyptian13Calendar.MinDaysSinceEpoch`` () =
        Egyptian13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of Egyptian13Calendar.MaxDaysSinceEpoch`` () =
        Egyptian13Calendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of Egyptian13Calendar.MinMonthsSinceEpoch`` () =
        Egyptian13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of Egyptian13Calendar.MaxMonthsSinceEpoch`` () =
        Egyptian13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Egyptian13Month(Egyptian13Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Egyptian13Date(y, m, d)
        let exp = new Egyptian13Month(y, m)
        // Act & Assert
        new Egyptian13Month(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Egyptian13Year(Egyptian13Date)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Egyptian13Date(y, m, d)
        let exp = new Egyptian13Year(y)
        // Act & Assert
        new Egyptian13Year(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Egyptian13Year(Egyptian13Month)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Egyptian13Month(y, m)
        let exp = new Egyptian13Year(y)
        // Act & Assert
        new Egyptian13Year(month) === exp

module Conversions =
    let private calendarDataSet = StandardEgyptian12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = EgyptianDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = EgyptianDate.op_Explicit

    type JulianDateCaster = EgyptianDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = EgyptianDate.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new EgyptianDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new EgyptianDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at EgyptianDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(EgyptianDate.MinValue.DayNumber)

        EgyptianDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at EgyptianDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(EgyptianDate.MaxValue.DayNumber)

        EgyptianDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to EgyptianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new EgyptianDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at EgyptianDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(EgyptianDate.MinValue.DayNumber)

        op_Explicit_Gregorian EgyptianDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at EgyptianDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(EgyptianDate.MaxValue.DayNumber)

        op_Explicit_Gregorian EgyptianDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new EgyptianDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at EgyptianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(EgyptianDate.MinValue.DayNumber)

        EgyptianDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at EgyptianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(EgyptianDate.MaxValue.DayNumber)

        EgyptianDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new EgyptianDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at EgyptianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(EgyptianDate.MinValue.DayNumber)

        op_Explicit_Julian EgyptianDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at EgyptianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(EgyptianDate.MaxValue.DayNumber)

        op_Explicit_Julian EgyptianDate.MaxValue === exp

module Conversions13 =
    let private calendarDataSet = StandardEgyptian13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Egyptian13Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = Egyptian13Date.op_Explicit

    type JulianDateCaster = Egyptian13Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = Egyptian13Date.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new Egyptian13Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Egyptian13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Egyptian13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Egyptian13Date.MinValue.DayNumber)

        Egyptian13Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Egyptian13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Egyptian13Date.MaxValue.DayNumber)

        Egyptian13Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Egyptian13Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Egyptian13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Egyptian13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Egyptian13Date.MinValue.DayNumber)

        op_Explicit_Gregorian Egyptian13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Egyptian13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Egyptian13Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Egyptian13Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Egyptian13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Egyptian13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Egyptian13Date.MinValue.DayNumber)

        Egyptian13Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Egyptian13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Egyptian13Date.MaxValue.DayNumber)

        Egyptian13Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Egyptian13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Egyptian13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Egyptian13Date.MinValue.DayNumber)

        op_Explicit_Julian Egyptian13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Egyptian13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Egyptian13Date.MaxValue.DayNumber)

        op_Explicit_Julian Egyptian13Date.MaxValue === exp

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<EgyptianCalendar, StandardEgyptian12DataSet>(EgyptianCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MinYear() = EgyptianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = EgyptianCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<EgyptianDate, StandardEgyptian12DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new EgyptianDate(y, m, d)
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
            let date = new EgyptianDate(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<EgyptianDate, StandardEgyptian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<EgyptianDate, StandardEgyptian12DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<EgyptianMonth, EgyptianDate, StandardEgyptian12DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new EgyptianMonth(y, m)
            let date = new EgyptianDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new EgyptianMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<EgyptianMonth, StandardEgyptian12DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<EgyptianYear, EgyptianMonth, EgyptianDate, StandardEgyptian12DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new EgyptianYear(y)
            let date = new EgyptianMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new EgyptianYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new EgyptianYear(y)
            let date = new EgyptianDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new EgyptianYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))

module Bundles13 =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Egyptian13Calendar, StandardEgyptian13DataSet>(Egyptian13Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MinYear() = Egyptian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Egyptian13Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Egyptian13Date, StandardEgyptian13DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new Egyptian13Date(y, m, d)
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
            let date = new Egyptian13Date(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Egyptian13Date, StandardEgyptian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    [<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<Egyptian13Date, StandardEgyptian13DataSet>()

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<Egyptian13Month, Egyptian13Date, StandardEgyptian13DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new Egyptian13Month(y, m)
            let date = new Egyptian13Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new Egyptian13Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(MonthFacts.MonthInfoData))>]
        static member ``Property IsVirtual`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let month = new Egyptian13Month(y, m)
            // Act & Assert
            month.IsVirtual === (m = 13)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<Egyptian13Month, StandardEgyptian13DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<Egyptian13Year, Egyptian13Month, Egyptian13Date, StandardEgyptian13DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new Egyptian13Year(y)
            let date = new Egyptian13Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new Egyptian13Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new Egyptian13Year(y)
            let date = new Egyptian13Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new Egyptian13Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
