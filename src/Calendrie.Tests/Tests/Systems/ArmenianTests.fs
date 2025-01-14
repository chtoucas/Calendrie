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
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = ArmenianDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = ArmenianDate.op_Explicit

    type JulianDateCaster = ArmenianDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = ArmenianDate.op_Explicit

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
    let ``ToGregorianDate() at ArmenianDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ArmenianDate.MaxValue.DayNumber)

        ArmenianDate.MaxValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at ArmenianDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ArmenianDate.MinValue.DayNumber)

        ArmenianDate.MinValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to ArmenianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ArmenianDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at ArmenianDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ArmenianDate.MaxValue.DayNumber)

        op_Explicit_Gregorian ArmenianDate.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at ArmenianDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ArmenianDate.MinValue.DayNumber)

        op_Explicit_Gregorian ArmenianDate.MinValue === exp

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
    let ``ToJulianDate() at ArmenianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ArmenianDate.MaxValue.DayNumber)

        ArmenianDate.MaxValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at ArmenianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ArmenianDate.MinValue.DayNumber)

        ArmenianDate.MinValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ArmenianDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at ArmenianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ArmenianDate.MaxValue.DayNumber)

        op_Explicit_Julian ArmenianDate.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at ArmenianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ArmenianDate.MinValue.DayNumber)

        op_Explicit_Julian ArmenianDate.MinValue === exp

module Conversions13 =
    let private calendarDataSet = StandardArmenian13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Armenian13Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = Armenian13Date.op_Explicit

    type JulianDateCaster = Armenian13Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = Armenian13Date.op_Explicit

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
    let ``ToGregorianDate() at Armenian13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian13Date.MaxValue.DayNumber)

        Armenian13Date.MaxValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Armenian13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian13Date.MinValue.DayNumber)

        Armenian13Date.MinValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Armenian13Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Armenian13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Armenian13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian13Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Armenian13Date.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Armenian13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Armenian13Date.MinValue.DayNumber)

        op_Explicit_Gregorian Armenian13Date.MinValue === exp

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
    let ``ToJulianDate() at Armenian13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian13Date.MaxValue.DayNumber)

        Armenian13Date.MaxValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Armenian13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian13Date.MinValue.DayNumber)

        Armenian13Date.MinValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Armenian13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Armenian13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian13Date.MaxValue.DayNumber)

        op_Explicit_Julian Armenian13Date.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Armenian13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Armenian13Date.MinValue.DayNumber)

        op_Explicit_Julian Armenian13Date.MinValue === exp

module Bundles =
    let private chr = ArmenianCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<ArmenianCalendar, StandardArmenian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = ArmenianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = ArmenianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = ArmenianCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<ArmenianDate, StandardArmenian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<ArmenianDate, StandardArmenian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<ArmenianDate, StandardArmenian12DataSet>()

        override __.GetDate(y, m, d) = new ArmenianDate(y, m, d)

module Bundles13 =
    let private chr = Armenian13Calendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Armenian13Calendar, StandardArmenian13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = Armenian13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Armenian13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Armenian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Armenian13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Armenian13Date, StandardArmenian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<Armenian13Date, StandardArmenian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Armenian13Date, StandardArmenian13DataSet>()

        override __.GetDate(y, m, d) = new Armenian13Date(y, m, d)
