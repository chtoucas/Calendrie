// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.EthiopicTests

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
    let ``Value of EthiopicCalendar.Epoch.DaysZinceZero`` () =
        EthiopicCalendar.Instance.Epoch.DaysSinceZero === 2795
    [<Fact>]
    let ``Value of Ethiopic13Calendar.Epoch.DaysZinceZero`` () =
        Ethiopic13Calendar.Instance.Epoch.DaysSinceZero === 2795

    [<Fact>]
    let ``default(EthiopicDate) is EthiopicCalendar.Epoch`` () =
        Unchecked.defaultof<EthiopicDate>.DayNumber === EthiopicCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Ethiopic13Date) is Ethiopic13Calendar.Epoch`` () =
        Unchecked.defaultof<Ethiopic13Date>.DayNumber === Ethiopic13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of EthiopicCalendar.MinDaysSinceEpoch`` () =
        EthiopicCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Ethiopic13Calendar.MinDaysSinceEpoch`` () =
        Ethiopic13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of EthiopicCalendar.MaxDaysSinceEpoch`` () =
        EthiopicCalendar.Instance.MaxDaysSinceEpoch === 3_652_134
    [<Fact>]
    let ``Value of Ethiopic13Calendar.MaxDaysSinceEpoch`` () =
        Ethiopic13Calendar.Instance.MaxDaysSinceEpoch === 3_652_134

    [<Fact>]
    let ``Value of EthiopicCalendar.MinMonthsSinceEpoch`` () =
        EthiopicCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of Ethiopic13Calendar.MinMonthsSinceEpoch`` () =
        Ethiopic13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of EthiopicCalendar.MaxMonthsSinceEpoch`` () =
        EthiopicCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of Ethiopic13Calendar.MaxMonthsSinceEpoch`` () =
        Ethiopic13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Conversions =
    let private calendarDataSet = StandardEthiopic12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = EthiopicDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = EthiopicDate.op_Explicit

    type JulianDateCaster = EthiopicDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = EthiopicDate.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``EthiopicMonth:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new EthiopicDate(y, m, d)
        let exp = new EthiopicMonth(y, m)
        // Act & Assert
        EthiopicMonth.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``EthiopicYear:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new EthiopicDate(y, m, d)
        let exp = new EthiopicYear(y)
        // Act & Assert
        EthiopicYear.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``EthiopicYear:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new EthiopicMonth(y, m)
        let exp = new EthiopicYear(y)
        // Act & Assert
        EthiopicYear.FromMonth(month) === exp

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new EthiopicDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new EthiopicDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at EthiopicDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(EthiopicDate.MinValue.DayNumber)

        EthiopicDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at EthiopicDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(EthiopicDate.MaxValue.DayNumber)

        EthiopicDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to EthiopicDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new EthiopicDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at EthiopicDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(EthiopicDate.MinValue.DayNumber)

        op_Explicit_Gregorian EthiopicDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at EthiopicDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(EthiopicDate.MaxValue.DayNumber)

        op_Explicit_Gregorian EthiopicDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new EthiopicDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at EthiopicDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(EthiopicDate.MinValue.DayNumber)

        EthiopicDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at EthiopicDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(EthiopicDate.MaxValue.DayNumber)

        EthiopicDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new EthiopicDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at EthiopicDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(EthiopicDate.MinValue.DayNumber)

        op_Explicit_Julian EthiopicDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at EthiopicDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(EthiopicDate.MaxValue.DayNumber)

        op_Explicit_Julian EthiopicDate.MaxValue === exp

module Conversions13 =
    let private calendarDataSet = StandardEthiopic13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Ethiopic13Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = Ethiopic13Date.op_Explicit

    type JulianDateCaster = Ethiopic13Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = Ethiopic13Date.op_Explicit

    //
    // FromXXX()
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Ethiopic13Month:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Ethiopic13Date(y, m, d)
        let exp = new Ethiopic13Month(y, m)
        // Act & Assert
        Ethiopic13Month.FromDate(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Ethiopic13Year:FromDate()`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new Ethiopic13Date(y, m, d)
        let exp = new Ethiopic13Year(y)
        // Act & Assert
        Ethiopic13Year.FromDate(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``Ethiopic13Year:FromMonth()`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new Ethiopic13Month(y, m)
        let exp = new Ethiopic13Year(y)
        // Act & Assert
        Ethiopic13Year.FromMonth(month) === exp

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new Ethiopic13Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Ethiopic13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Ethiopic13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Ethiopic13Date.MinValue.DayNumber)

        Ethiopic13Date.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Ethiopic13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Ethiopic13Date.MaxValue.DayNumber)

        Ethiopic13Date.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Ethiopic13Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Ethiopic13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Ethiopic13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Ethiopic13Date.MinValue.DayNumber)

        op_Explicit_Gregorian Ethiopic13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Ethiopic13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Ethiopic13Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Ethiopic13Date.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Ethiopic13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Ethiopic13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Ethiopic13Date.MinValue.DayNumber)

        Ethiopic13Date.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Ethiopic13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Ethiopic13Date.MaxValue.DayNumber)

        Ethiopic13Date.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Ethiopic13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Ethiopic13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Ethiopic13Date.MinValue.DayNumber)

        op_Explicit_Julian Ethiopic13Date.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Ethiopic13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Ethiopic13Date.MaxValue.DayNumber)

        op_Explicit_Julian Ethiopic13Date.MaxValue === exp

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<EthiopicCalendar, StandardEthiopic12DataSet>(EthiopicCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = EthiopicCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = EthiopicCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<EthiopicDate, StandardEthiopic12DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new EthiopicDate(y, m, d)
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
            let date = new EthiopicDate(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<EthiopicDate, StandardEthiopic12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<EthiopicDate, StandardEthiopic12DataSet>(
            EthiopicDateMath.Default)

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<EthiopicMonth, EthiopicDate, StandardEthiopic12DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new EthiopicMonth(y, m)
            let date = new EthiopicDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new EthiopicMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<EthiopicMonth, StandardEthiopic12DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<EthiopicYear, EthiopicMonth, EthiopicDate, StandardEthiopic12DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new EthiopicYear(y)
            let date = new EthiopicMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
            let year = new EthiopicYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new EthiopicYear(y)
            let date = new EthiopicDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new EthiopicYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))

module Bundles13 =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Ethiopic13Calendar, StandardEthiopic13DataSet>(Ethiopic13Calendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MinYear() = Ethiopic13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Ethiopic13Calendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Ethiopic13Date, StandardEthiopic13DataSet>()

        static member EpagomenalDayInfoData with get() = DateFacts.DataSet.EpagomenalDayInfoData

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(DateFacts.DateInfoData))>]
        static member ``IsEpagomenal()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let date = new Ethiopic13Date(y, m, d)
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
            let date = new Ethiopic13Date(y, m, d)
            // Act
            let isEpagomenal, epagomenalNumber = date.IsEpagomenal()
            // Assert
            isEpagomenal |> ok
            epagomenalNumber === epanum

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Ethiopic13Date, StandardEthiopic13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DefaultDateMathFacts() =
        inherit DefaultDateMathFacts<Ethiopic13Date, StandardEthiopic13DataSet>(
            Ethiopic13DateMath.Default)

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<Ethiopic13Month, Ethiopic13Date, StandardEthiopic13DataSet>()

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new Ethiopic13Month(y, m)
            let date = new Ethiopic13Date(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new Ethiopic13Month(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(MonthFacts.MonthInfoData))>]
        static member ``Property IsVirtual`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let month = new Ethiopic13Month(y, m)
            // Act & Assert
            month.IsVirtual === (m = 13)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<Ethiopic13Month, StandardEthiopic13DataSet>()

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<Ethiopic13Year, Ethiopic13Month, Ethiopic13Date, StandardEthiopic13DataSet>()

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new Ethiopic13Year(y)
            let date = new Ethiopic13Month(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` y m =
            let year = new Ethiopic13Year(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new Ethiopic13Year(y)
            let date = new Ethiopic13Date(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` y doy =
            let year = new Ethiopic13Year(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))
