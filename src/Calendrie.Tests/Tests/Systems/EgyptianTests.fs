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
    [<Fact>]
    let ``Value of EgyptianCalendar.Epoch.DaysZinceZero`` () =
        EgyptianCalendar.Instance.Epoch.DaysSinceZero === -272_788
    [<Fact>]
    let ``Value of Egyptian13Calendar.Epoch.DaysZinceZero`` () =
        Egyptian13Calendar.Instance.Epoch.DaysSinceZero === -272_788

    [<Fact>]
    let ``default(EgyptianDate) is EgyptianCalendar.Epoch`` () =
        Unchecked.defaultof<EgyptianDate>.DayNumber === EgyptianCalendar.Instance.Epoch
    [<Fact>]
    let ``default(Egyptian13Date) is Egyptian13Calendar.Epoch`` () =
        Unchecked.defaultof<Egyptian13Date>.DayNumber === Egyptian13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of EgyptianCalendar.MinDaysSinceEpoch`` () =
        EgyptianCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of Egyptian13Calendar.MinDaysSinceEpoch`` () =
        Egyptian13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of EgyptianCalendar.MaxDaysSinceEpoch`` () =
        EgyptianCalendar.Instance.MaxDaysSinceEpoch === 3_649_634
    [<Fact>]
    let ``Value of Egyptian13Calendar.MaxDaysSinceEpoch`` () =
        Egyptian13Calendar.Instance.MaxDaysSinceEpoch === 3_649_634

    [<Fact>]
    let ``Value of EgyptianCalendar.MinMonthsSinceEpoch`` () =
        EgyptianCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of Egyptian13Calendar.MinMonthsSinceEpoch`` () =
        Egyptian13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of EgyptianCalendar.MaxMonthsSinceEpoch`` () =
        EgyptianCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of Egyptian13Calendar.MaxMonthsSinceEpoch`` () =
        Egyptian13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

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
    let private chr = EgyptianCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<EgyptianCalendar, StandardEgyptian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = EgyptianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = EgyptianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = EgyptianCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<EgyptianDate, StandardEgyptian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<EgyptianDate, StandardEgyptian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<EgyptianDate, StandardEgyptian12DataSet>()

        override __.GetDate(y, m, d) = new EgyptianDate(y, m, d)

module Bundles13 =
    let private chr = Egyptian13Calendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Egyptian13Calendar, StandardEgyptian13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = Egyptian13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Egyptian13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Egyptian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Egyptian13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Egyptian13Date, StandardEgyptian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Egyptian13Date, StandardEgyptian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Egyptian13Date, StandardEgyptian13DataSet>()

        override __.GetDate(y, m, d) = new Egyptian13Date(y, m, d)
