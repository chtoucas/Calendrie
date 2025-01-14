// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.FrenchRepublican

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
    let ``Value of FrenchRepublicanCalendar.Epoch.DaysZinceZero`` () =
        FrenchRepublicanCalendar.Instance.Epoch.DaysSinceZero === 654_414
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.Epoch.DaysZinceZero`` () =
        FrenchRepublican13Calendar.Instance.Epoch.DaysSinceZero === 654_414

    [<Fact>]
    let ``default(FrenchRepublicanDate) is FrenchRepublicanCalendar.Epoch`` () =
        Unchecked.defaultof<FrenchRepublicanDate>.DayNumber === FrenchRepublicanCalendar.Instance.Epoch
    [<Fact>]
    let ``default(FrenchRepublican13Date) is FrenchRepublican13Calendar.Epoch`` () =
        Unchecked.defaultof<FrenchRepublican13Date>.DayNumber === FrenchRepublican13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MinDaysSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MinDaysSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MaxDaysSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MaxDaysSinceEpoch === 3_652_056
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MaxDaysSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MaxDaysSinceEpoch === 3_652_056

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MinMonthsSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MinMonthsSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MaxMonthsSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MaxMonthsSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

module Conversions =
    let private calendarDataSet = StandardFrenchRepublican12DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = FrenchRepublicanDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = FrenchRepublicanDate.op_Explicit

    type JulianDateCaster = FrenchRepublicanDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = FrenchRepublicanDate.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new FrenchRepublicanDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublicanDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at FrenchRepublicanDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublicanDate.MaxValue.DayNumber)

        FrenchRepublicanDate.MaxValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at FrenchRepublicanDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublicanDate.MinValue.DayNumber)

        FrenchRepublicanDate.MinValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to FrenchRepublicanDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublicanDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at FrenchRepublicanDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublicanDate.MaxValue.DayNumber)

        op_Explicit_Gregorian FrenchRepublicanDate.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at FrenchRepublicanDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublicanDate.MinValue.DayNumber)

        op_Explicit_Gregorian FrenchRepublicanDate.MinValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublicanDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at FrenchRepublicanDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublicanDate.MaxValue.DayNumber)

        FrenchRepublicanDate.MaxValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at FrenchRepublicanDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublicanDate.MinValue.DayNumber)

        FrenchRepublicanDate.MinValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublicanDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at FrenchRepublicanDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublicanDate.MaxValue.DayNumber)

        op_Explicit_Julian FrenchRepublicanDate.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at FrenchRepublicanDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublicanDate.MinValue.DayNumber)

        op_Explicit_Julian FrenchRepublicanDate.MinValue === exp

module Conversions13 =
    let private calendarDataSet = StandardFrenchRepublican13DataSet.Instance

    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = FrenchRepublican13Date -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = FrenchRepublican13Date.op_Explicit

    type JulianDateCaster = FrenchRepublican13Date -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = FrenchRepublican13Date.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new FrenchRepublican13Date(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at FrenchRepublican13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican13Date.MaxValue.DayNumber)

        FrenchRepublican13Date.MaxValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at FrenchRepublican13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican13Date.MinValue.DayNumber)

        FrenchRepublican13Date.MinValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to FrenchRepublican13Date`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican13Date(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at FrenchRepublican13Date:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican13Date.MaxValue.DayNumber)

        op_Explicit_Gregorian FrenchRepublican13Date.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at FrenchRepublican13Date:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(FrenchRepublican13Date.MinValue.DayNumber)

        op_Explicit_Gregorian FrenchRepublican13Date.MinValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at FrenchRepublican13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican13Date.MaxValue.DayNumber)

        FrenchRepublican13Date.MaxValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at FrenchRepublican13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican13Date.MinValue.DayNumber)

        FrenchRepublican13Date.MinValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new FrenchRepublican13Date(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at FrenchRepublican13Date:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican13Date.MaxValue.DayNumber)

        op_Explicit_Julian FrenchRepublican13Date.MaxValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at FrenchRepublican13Date:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(FrenchRepublican13Date.MinValue.DayNumber)

        op_Explicit_Julian FrenchRepublican13Date.MinValue === exp

module Bundles =
    let private chr = FrenchRepublicanCalendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<FrenchRepublicanCalendar, StandardFrenchRepublican12DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = FrenchRepublicanCalendar.MonthsInYear === 12

        [<Fact>]
        static member MinYear() = FrenchRepublicanCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = FrenchRepublicanCalendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<FrenchRepublicanDate, StandardFrenchRepublican12DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<FrenchRepublicanDate, StandardFrenchRepublican12DataSet>()

        override __.GetDate(y, m, d) = new FrenchRepublicanDate(y, m, d)

module Bundles13 =
    let private chr = FrenchRepublican13Calendar.Instance

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<FrenchRepublican13Calendar, StandardFrenchRepublican13DataSet>(chr)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Solar
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Days

        [<Fact>]
        static member MonthsInYear() = FrenchRepublican13Calendar.MonthsInYear === 13

        [<Fact>]
        static member VirtualMonth() = FrenchRepublican13Calendar.VirtualMonth === 13

        [<Fact>]
        static member MinYear() = FrenchRepublican13Calendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = FrenchRepublican13Calendar.MaxYear === StandardScope.MaxYear

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<FrenchRepublican13Date, StandardFrenchRepublican13DataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateAdjusterFacts() =
        inherit DateAdjusterFacts<FrenchRepublican13Date, StandardFrenchRepublican13DataSet>(FrenchRepublican13Calendar.Instance)

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type EpagomenalDateFacts() =
        inherit IEpagomenalDayFacts<FrenchRepublican13Date, StandardFrenchRepublican13DataSet>()

        override __.GetDate(y, m, d) = new FrenchRepublican13Date(y, m, d)


