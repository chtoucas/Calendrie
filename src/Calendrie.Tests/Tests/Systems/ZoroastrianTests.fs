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
    let ``ToGregorianDate() at ZoroastrianDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ZoroastrianDate.MaxValue.DayNumber)

        ZoroastrianDate.MaxValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at ZoroastrianDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ZoroastrianDate.MinValue.DayNumber)

        ZoroastrianDate.MinValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to ZoroastrianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ZoroastrianDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at ZoroastrianDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ZoroastrianDate.MaxValue.DayNumber)

        op_Explicit_Gregorian ZoroastrianDate.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at ZoroastrianDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(ZoroastrianDate.MinValue.DayNumber)

        op_Explicit_Gregorian ZoroastrianDate.MinValue === exp

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
    let ``ToJulianDate() at ZoroastrianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ZoroastrianDate.MaxValue.DayNumber)

        ZoroastrianDate.MaxValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at ZoroastrianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ZoroastrianDate.MinValue.DayNumber)

        ZoroastrianDate.MinValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new ZoroastrianDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at ZoroastrianDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ZoroastrianDate.MaxValue.DayNumber)

        op_Explicit_Julian ZoroastrianDate.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at ZoroastrianDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(ZoroastrianDate.MinValue.DayNumber)

        op_Explicit_Julian ZoroastrianDate.MinValue === exp

module Conversions13 =
    let private calendarDataSet = StandardZoroastrian13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = Zoroastrian13Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = Zoroastrian13Date.op_Explicit

    type JulianDateCaster = Zoroastrian13Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = Zoroastrian13Date.op_Explicit

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
    let ``ToGregorianDate() at Zoroastrian13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian13Date.MaxValue.DayNumber)

        Zoroastrian13Date.MaxValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at Zoroastrian13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian13Date.MinValue.DayNumber)

        Zoroastrian13Date.MinValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to Zoroastrian13Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Zoroastrian13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Zoroastrian13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian13Date.MaxValue.DayNumber)

        op_Explicit_Gregorian Zoroastrian13Date.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at Zoroastrian13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(Zoroastrian13Date.MinValue.DayNumber)

        op_Explicit_Gregorian Zoroastrian13Date.MinValue === exp

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
    let ``ToJulianDate() at Zoroastrian13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian13Date.MaxValue.DayNumber)

        Zoroastrian13Date.MaxValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at Zoroastrian13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian13Date.MinValue.DayNumber)

        Zoroastrian13Date.MinValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new Zoroastrian13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Zoroastrian13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian13Date.MaxValue.DayNumber)

        op_Explicit_Julian Zoroastrian13Date.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at Zoroastrian13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(Zoroastrian13Date.MinValue.DayNumber)

        op_Explicit_Julian Zoroastrian13Date.MinValue === exp

module Bundles =
    let private chr = ZoroastrianCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<ZoroastrianCalendar, StandardZoroastrian12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = ZoroastrianCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = ZoroastrianCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = ZoroastrianCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<ZoroastrianDate, StandardZoroastrian12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<ZoroastrianDate, StandardZoroastrian12DataSet>()

        override __.GetDate(y, m, d) = new ZoroastrianDate(y, m, d)

module Bundles13 =
    let private chr = Zoroastrian13Calendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<Zoroastrian13Calendar, StandardZoroastrian13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.AnnusVagus
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.None

        [<Fact>]
        static member MonthsInYear() = Zoroastrian13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = Zoroastrian13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = Zoroastrian13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = Zoroastrian13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<Zoroastrian13Date, StandardZoroastrian13DataSet>()

        override __.GetDate(y, m, d) = new Zoroastrian13Date(y, m, d)
