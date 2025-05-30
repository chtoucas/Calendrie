﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.PaxTests

#nowarn 3391 // Implicit conversion to DayNumber or GregorianDate

open Calendrie
open Calendrie.Hemerology
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

let private calendarDataSet = StandardPaxDataSet.Instance

module Prelude =
    let dateInfoData = calendarDataSet.DateInfoData
    let monthInfoData = calendarDataSet.MonthInfoData

    [<Fact>]
    let ``Value of PaxCalendar.Epoch.DaysZinceZero`` () =
        PaxCalendar.Instance.Epoch.DaysSinceZero === -1

    [<Fact>]
    let ``default(PaxDate) is PaxCalendar.Epoch`` () =
        Unchecked.defaultof<PaxDate>.DayNumber === PaxCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of PaxCalendar.MinDaysSinceEpoch`` () =
        PaxCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of PaxCalendar.MaxDaysSinceEpoch`` () =
        PaxCalendar.Instance.MaxDaysSinceEpoch === 3_652_060

    [<Fact>]
    let ``Value of PaxCalendar.MinMonthsSinceEpoch`` () =
        PaxCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of PaxCalendar.MaxMonthsSinceEpoch`` () =
        PaxCalendar.Instance.MaxMonthsSinceEpoch === 131_761
#endif

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``PaxMonth(PaxDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new PaxDate(y, m, d)
        let exp = new PaxMonth(y, m)
        // Act & Assert
        new PaxMonth(date) === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``PaxYear(PaxDate)`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        let date = new PaxDate(y, m, d)
        let exp = new PaxYear(y)
        // Act & Assert
        new PaxYear(date) === exp

    [<Theory; MemberData(nameof(monthInfoData))>]
    let ``PaxYear(PaxMonth)`` (x: MonthInfo) =
        let y, m = x.Yemo.Deconstruct()
        let month = new PaxMonth(y, m)
        let exp = new PaxYear(y)
        // Act & Assert
        new PaxYear(month) === exp

module Conversions =
    let dateInfoData = calendarDataSet.DateInfoData
    let dayNumberInfoData = calendarDataSet.DayNumberInfoData

    type GregorianDateCaster = PaxDate -> GregorianDate
    let op_Explicit_Gregorian : GregorianDateCaster = PaxDate.op_Explicit

    type JulianDateCaster = PaxDate -> JulianDate
    let op_Explicit_Julian : JulianDateCaster = PaxDate.op_Explicit

    //
    // Conversion to DayNumber
    //

    [<Theory; MemberData(nameof(dayNumberInfoData))>]
    let ``Implicit conversion to DayNumber`` (x: DayNumberInfo) =
        let dayNumber, y, m, d = x.Deconstruct()
        let date  = new PaxDate(y, m, d)

        date : DayNumber === dayNumber

    //
    // Conversion to GregorianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToGregorianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PaxDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        date.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at PaxDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PaxDate.MinValue.DayNumber)

        PaxDate.MinValue.ToGregorianDate() === exp

    [<Fact>]
    let ``ToGregorianDate() at PaxDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PaxDate.MaxValue.DayNumber)

        PaxDate.MaxValue.ToGregorianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to PaxDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PaxDate(y, m, d)
        let exp = GregorianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Gregorian date === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at PaxDate:MinValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PaxDate.MinValue.DayNumber)

        op_Explicit_Gregorian PaxDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to GregorianDate at PaxDate:MaxValue`` () =
        let exp = GregorianDate.FromAbsoluteDate(PaxDate.MaxValue.DayNumber)

        op_Explicit_Gregorian PaxDate.MaxValue === exp

    //
    // Conversion to JulianDate
    //

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ToJulianDate()`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PaxDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        date.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at PaxDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PaxDate.MinValue.DayNumber)

        PaxDate.MinValue.ToJulianDate() === exp

    [<Fact>]
    let ``ToJulianDate() at PaxDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PaxDate.MaxValue.DayNumber)

        PaxDate.MaxValue.ToJulianDate() === exp

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``Explicit conversion to JulianDate`` (x: DateInfo) =
        let y, m, d, _ = x.Deconstruct()
        let date = new PaxDate(y, m, d)
        let exp = JulianDate.FromAbsoluteDate(date.DayNumber)

        op_Explicit_Julian date === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at PaxDate:MinValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PaxDate.MinValue.DayNumber)

        op_Explicit_Julian PaxDate.MinValue === exp

    [<Fact>]
    let ``Explicit conversion to JulianDate at PaxDate:MaxValue`` () =
        let exp = JulianDate.FromAbsoluteDate(PaxDate.MaxValue.DayNumber)

        op_Explicit_Julian PaxDate.MaxValue === exp

module Bundles =
    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type CalendaTests() =
        inherit CalendarFacts<PaxCalendar, StandardPaxDataSet>(PaxCalendar.Instance)

        override x.Algorithm_Prop() = x.CalendarUT.Algorithm === CalendricalAlgorithm.Arithmetical
        override x.Family_Prop() = x.CalendarUT.Family === CalendricalFamily.Other
        override x.PeriodicAdjustments_Prop() = x.CalendarUT.PeriodicAdjustments === CalendricalAdjustments.Weeks

        [<Fact>]
        static member MinYear() = PaxCalendar.MinYear === StandardScope.MinYear

        [<Fact>]
        static member MaxYear() = PaxCalendar.MaxYear === StandardScope.MaxYear

    //
    // Date type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type DateFacts() =
        inherit IDateFacts<PaxDate, StandardPaxDataSet>()

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type UnsafeDateFactoryFacts() =
        inherit IUnsafeDateFactoryFacts<PaxDate, StandardPaxDataSet>()

    [<Sealed>]
    type DateMathFacts() =
        inherit DefaultDateMathFacts<PaxDate, StandardPaxDataSet>()

        static let defaultMath   = new DateMath()
        static let overspillMath = new DateMath(AdditionRule.Overspill)
        static let exactMath     = new DateMath(AdditionRule.Exact)

        // 6 est une année bissextile
        // 01/13/0006 + 1 année = 01/13/0007
        // 07/13/0006 + 1 année = 07/13/0007
        // 01/14/0006 + 1 année = 28/13/0007 (dernier jour de l'année) roundoff = 1
        // 01/14/0006 - 1 année = 28/13/0005 (dernier jour de l'année) roundoff = 1
        // 28/14/0006 + 1 année = 28/13/0007 (dernier jour de l'année) roundoff = 28
        // 28/14/0006 - 1 année = 28/13/0005 (dernier jour de l'année) roundoff = 28

        [<Fact>]
        static member ``AddYears(01/13/0006, 1)`` () =
            let date = new PaxDate(6, 13, 1)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(1)
            result === (new PaxDate(7, 13, 1), 0)

            date.PlusYears(1) === new PaxDate(7, 13, 1)

            defaultMath.AddYears(date, 1)   === new PaxDate(7, 13, 1)
            overspillMath.AddYears(date, 1) === new PaxDate(7, 13, 1)
            exactMath.AddYears(date, 1)     === new PaxDate(7, 13, 1)

        [<Fact>]
        static member ``AddYears(07/13/0006, 1)`` () =
            let date = new PaxDate(6, 13, 7)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(1)
            result === (new PaxDate(7, 13, 7), 0)

            date.PlusYears(1) === new PaxDate(7, 13, 7)

            defaultMath.AddYears(date, 1)   === new PaxDate(7, 13, 7)
            overspillMath.AddYears(date, 1) === new PaxDate(7, 13, 7)
            exactMath.AddYears(date, 1)     === new PaxDate(7, 13, 7)

        [<Fact>]
        static member ``AddYears(01/14/0006, 1)`` () =
            let date = new PaxDate(6, 14, 1)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(1)
            result === (new PaxDate(7, 13, 28), 1)

            date.PlusYears(1) === new PaxDate(7, 13, 28)

            defaultMath.AddYears(date, 1)   === new PaxDate(7, 13, 28)
            overspillMath.AddYears(date, 1) === new PaxDate(8, 1, 1)
            exactMath.AddYears(date, 1)     === new PaxDate(8, 1, 1)

        [<Fact>]
        static member ``AddYears(01/14/0006, -1)`` () =
            let date = new PaxDate(6, 14, 1)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(-1)
            result === (new PaxDate(5, 13, 28), 1)

            date.PlusYears(-1) === new PaxDate(5, 13, 28)

            defaultMath.AddYears(date, -1)   === new PaxDate(5, 13, 28)
            overspillMath.AddYears(date, -1) === new PaxDate(6, 1, 1)
            exactMath.AddYears(date, -1)     === new PaxDate(6, 1, 1)

        [<Fact>]
        static member ``AddYears(28/14/0006, 1)`` () =
            let date = new PaxDate(6, 14, 28)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(1)
            result === (new PaxDate(7, 13, 28), 28)

            date.PlusYears(1) === new PaxDate(7, 13, 28)

            defaultMath.AddYears(date, 1)   === new PaxDate(7, 13, 28)
            overspillMath.AddYears(date, 1) === new PaxDate(8, 1, 1)
            exactMath.AddYears(date, 1)     === new PaxDate(8, 1, 28)

        [<Fact>]
        static member ``AddYears(28/14/0006, -1)`` () =
            let date = new PaxDate(6, 14, 28)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(-1)
            result === (new PaxDate(5, 13, 28), 28)

            date.PlusYears(-1) === new PaxDate(5, 13, 28)

            defaultMath.AddYears(date, -1)   === new PaxDate(5, 13, 28)
            overspillMath.AddYears(date, -1) === new PaxDate(6, 1, 1)
            exactMath.AddYears(date, -1)     === new PaxDate(6, 1, 28)

        // 5 et 7 sont des années ordinaires
        // 01/13/0005 + 1 année = 01/13/0006
        // 07/13/0005 + 1 année = 07/13/0006
        // 08/13/0005 + 1 année = 07/13/0006 (dernier jour de l'année) roundoff = 1
        // 28/13/0005 + 1 année = 07/13/0006 (dernier jour de l'année) roundoff = 21
        // 08/13/0007 - 1 année = 07/13/0006 (dernier jour de l'année) roundoff = 1
        // 28/13/0007 - 1 année = 07/13/0006 (dernier jour de l'année) roundoff = 21

        [<Fact>]
        static member ``AddYears(01/13/0005, 1)`` () =
            let date = new PaxDate(5, 13, 1)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(1)
            result === (new PaxDate(6, 13, 1), 0)

            date.PlusYears(1) === new PaxDate(6, 13, 1)

            defaultMath.AddYears(date, 1)   === new PaxDate(6, 13, 1)
            overspillMath.AddYears(date, 1) === new PaxDate(6, 13, 1)
            exactMath.AddYears(date, 1)     === new PaxDate(6, 13, 1)

        [<Fact>]
        static member ``AddYears(07/13/0005, 1)`` () =
            let date = new PaxDate(5, 13, 7)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(1)
            result === (new PaxDate(6, 13, 7), 0)

            date.PlusYears(1) === new PaxDate(6, 13, 7)

            defaultMath.AddYears(date, 1)   === new PaxDate(6, 13, 7)
            overspillMath.AddYears(date, 1) === new PaxDate(6, 13, 7)
            exactMath.AddYears(date, 1)     === new PaxDate(6, 13, 7)

        [<Fact>]
        static member ``AddYears(08/13/0005, 1)`` () =
            let date = new PaxDate(5, 13, 8)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(1)
            result === (new PaxDate(6, 13, 7), 1)

            date.PlusYears(1) === new PaxDate(6, 13, 7)

            defaultMath.AddYears(date, 1)   === new PaxDate(6, 13, 7)
            overspillMath.AddYears(date, 1) === new PaxDate(6, 14, 1)
            exactMath.AddYears(date, 1)     === new PaxDate(6, 14, 1)

        [<Fact>]
        static member ``AddYears(28/13/0005, 1)`` () =
            let date = new PaxDate(5, 13, 28)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(1)
            result === (new PaxDate(6, 13, 7), 21)

            date.PlusYears(1) === new PaxDate(6, 13, 7)

            defaultMath.AddYears(date, 1)   === new PaxDate(6, 13, 7)
            overspillMath.AddYears(date, 1) === new PaxDate(6, 14, 1)
            exactMath.AddYears(date, 1)     === new PaxDate(6, 14, 21)

        [<Fact>]
        static member ``AddYears(08/13/0007, -1)`` () =
            let date = new PaxDate(7, 13, 8)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(-1)
            result === (new PaxDate(6, 13, 7), 1)

            date.PlusYears(-1) === new PaxDate(6, 13, 7)

            defaultMath.AddYears(date, -1)   === new PaxDate(6, 13, 7)
            overspillMath.AddYears(date, -1) === new PaxDate(6, 14, 1)
            exactMath.AddYears(date, -1)     === new PaxDate(6, 14, 1)

        [<Fact>]
        static member ``AddYears(28/13/0007, -1)`` () =
            let date = new PaxDate(7, 13, 28)
            // Act & Assert
            let result: PaxDate * int = date.PlusYears(-1)
            result === (new PaxDate(6, 13, 7), 21)

            date.PlusYears(-1) === new PaxDate(6, 13, 7)

            defaultMath.AddYears(date, -1)   === new PaxDate(6, 13, 7)
            overspillMath.AddYears(date, -1) === new PaxDate(6, 14, 1)
            exactMath.AddYears(date, -1)     === new PaxDate(6, 14, 21)

        // AddMonths()

        [<Fact>]
        static member ``AddMonths(07/13/0005, 13)`` () =
            let date = new PaxDate(5, 13, 7)
            // Act & Assert
            let result: PaxDate * int = date.PlusMonths(13)
            result === (new PaxDate(6, 13, 7), 0)

            date.PlusMonths(13) === new PaxDate(6, 13, 7)

            defaultMath.AddMonths(date, 13)   === new PaxDate(6, 13, 7)
            overspillMath.AddMonths(date, 13) === new PaxDate(6, 13, 7)
            exactMath.AddMonths(date, 13)     === new PaxDate(6, 13, 7)

        [<Fact>]
        static member ``AddMonths(08/13/0005, 13)`` () =
            let date = new PaxDate(5, 13, 8)
            // Act & Assert
            let result: PaxDate * int = date.PlusMonths(13)
            result === (new PaxDate(6, 13, 7), 1)

            date.PlusMonths(13) === new PaxDate(6, 13, 7)

            defaultMath.AddMonths(date, 13)   === new PaxDate(6, 13, 7)
            overspillMath.AddMonths(date, 13) === new PaxDate(6, 14, 1)
            exactMath.AddMonths(date, 13)     === new PaxDate(6, 14, 1)

        [<Fact>]
        static member ``AddMonths(09/13/0005, 13)`` () =
            let date = new PaxDate(5, 13, 9)
            // Act & Assert
            let result: PaxDate * int = date.PlusMonths(13)
            result === (new PaxDate(6, 13, 7), 2)

            date.PlusMonths(13) === new PaxDate(6, 13, 7)

            defaultMath.AddMonths(date, 13)   === new PaxDate(6, 13, 7)
            overspillMath.AddMonths(date, 13) === new PaxDate(6, 14, 1)
            exactMath.AddMonths(date, 13)     === new PaxDate(6, 14, 2)

        [<Fact>]
        static member ``AddMonths(08/13/0005, 14)`` () =
            let date = new PaxDate(5, 13, 8)
            // Act & Assert
            let result: PaxDate * int = date.PlusMonths(14)
            result === (new PaxDate(6, 14, 8), 0)

            date.PlusMonths(14) === new PaxDate(6, 14, 8)

            defaultMath.AddMonths(date, 14)   === new PaxDate(6, 14, 8)
            overspillMath.AddMonths(date, 14) === new PaxDate(6, 14, 8)
            exactMath.AddMonths(date, 14)     === new PaxDate(6, 14, 8)

        [<Fact>]
        static member ``AddMonths(01/13/0006, 14)`` () =
            let date = new PaxDate(6, 13, 1)
            // Act & Assert
            let result: PaxDate * int = date.PlusMonths(14)
            result === (new PaxDate(7, 13, 1), 0)

            date.PlusMonths(14) === new PaxDate(7, 13, 1)

            defaultMath.AddMonths(date, 14)   === new PaxDate(7, 13, 1)
            overspillMath.AddMonths(date, 14) === new PaxDate(7, 13, 1)
            exactMath.AddMonths(date, 14)     === new PaxDate(7, 13, 1)

    //
    // Month type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type MonthFacts() =
        inherit IMonthFacts<PaxMonth, PaxDate, StandardPaxDataSet>()

        static member MoreMonthInfoData with get() = PaxDataSet.MoreMonthInfoData

        [<Theory; MemberData(nameof(MonthFacts.DateInfoData))>]
        static member ``GetDayOfMonth()`` (info: DateInfo) =
            let y, m, d = info.Yemoda.Deconstruct()
            let year = new PaxMonth(y, m)
            let date = new PaxDate(y, m, d)
            // Act & Assert
            year.GetDayOfMonth(d) === date

        [<Theory; MemberData(nameof(MonthFacts.InvalidDayFieldData))>]
        static member ``GetDayOfMonth() with an invalid day`` y m d =
            let month = new PaxMonth(y, m)
            // Act & Assert
            outOfRangeExn "day" (fun () -> month.GetDayOfMonth(d))

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(MonthFacts.MoreMonthInfoData))>]
        static member ``PaxMonth.IsPaxMonthOfYear`` (info: YemoAnd<bool, bool>) =
            let (y, m, isPaxMonthOfYear, _) = info.Deconstruct()
            let month = new PaxMonth(y, m)
            // Act & Assert
            month.IsPaxMonthOfYear === isPaxMonthOfYear

        [<Theory; MemberData(nameof(MonthFacts.MoreMonthInfoData))>]
        static member ``PaxMonth.IsLastMonthOfYear`` (info: YemoAnd<bool, bool>) =
            let (y, m, _, isLastMonthOfYear) = info.Deconstruct()
            let month = new PaxMonth(y, m)
            // Act & Assert
            month.IsLastMonthOfYear === isLastMonthOfYear

    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    [<Sealed>]
    type UnsafeMonthFactoryFacts() =
        inherit IUnsafeMonthFactoryFacts<PaxMonth, StandardPaxDataSet>()

    [<Sealed>]
    // Do NOT exclude from Code Coverage or from the regular test plan.
    // This is the only place right now where we test MonthaMath and where this
    // is actually interesting as Pax is a non-regular calendar.
    type MonthMathFacts() =
        inherit DefaultMonthMathFacts<PaxMonth, StandardPaxDataSet>()

        static let defaultMath   = new MonthMath()
        static let overspillMath = new MonthMath(AdditionRule.Overspill)
        static let exactMath     = new MonthMath(AdditionRule.Exact)

        //
        // AddYears(), ¨PlusYears()
        //

        [<Fact>]
        static member ``AddYears() when roundof != 0`` () =
            let month = new PaxMonth(6, 14)
            // Act & Assert
            let result: PaxMonth * int = month.PlusYears(1)
            result === (new PaxMonth(7, 13), 1)

            month.PlusYears(1) === new PaxMonth(7, 13)

            defaultMath.AddYears(month, 1)   === new PaxMonth(7, 13)
            overspillMath.AddYears(month, 1) === new PaxMonth(8, 1)
            exactMath.AddYears(month, 1)     === new PaxMonth(8, 1)

        //
        // Substract()
        //

        [<Fact>]
        static member ``Substract() when start = end`` () =
            let month = new PaxMonth(4, 3)
            // Act & Assert
            defaultMath.Subtract(month, month)   === MonthDifference.Zero
            overspillMath.Subtract(month, month) === MonthDifference.Zero
            exactMath.Subtract(month, month)     === MonthDifference.Zero

        [<Fact>]
        static member ``Substract() when start < end`` () =
            let month = new PaxMonth(4, 3)
            // Act & Assert
            defaultMath.Subtract(month, new PaxMonth(4, 4))  === MonthDifference.CreatePositive(0, 1)
            defaultMath.Subtract(month, new PaxMonth(4, 13)) === MonthDifference.CreatePositive(0, 10)
            defaultMath.Subtract(month, new PaxMonth(5, 1))  === MonthDifference.CreatePositive(0, 11)
            defaultMath.Subtract(month, new PaxMonth(5, 2))  === MonthDifference.CreatePositive(0, 12)
            defaultMath.Subtract(month, new PaxMonth(5, 3))  === MonthDifference.CreatePositive(1, 0)
            defaultMath.Subtract(month, new PaxMonth(5, 4))  === MonthDifference.CreatePositive(1, 1)
            defaultMath.Subtract(month, new PaxMonth(6, 2))  === MonthDifference.CreatePositive(1, 12)
            defaultMath.Subtract(month, new PaxMonth(6, 3))  === MonthDifference.CreatePositive(2, 0)
            defaultMath.Subtract(month, new PaxMonth(6, 4))  === MonthDifference.CreatePositive(2, 1)
            // 6 is a leap year
            defaultMath.Subtract(month, new PaxMonth(6, 14)) === MonthDifference.CreatePositive(2, 11)
            defaultMath.Subtract(month, new PaxMonth(7, 1))  === MonthDifference.CreatePositive(2, 12)
            defaultMath.Subtract(month, new PaxMonth(7, 2))  === MonthDifference.CreatePositive(2, 13)
            defaultMath.Subtract(month, new PaxMonth(7, 3))  === MonthDifference.CreatePositive(3, 0)
            defaultMath.Subtract(month, new PaxMonth(7, 4))  === MonthDifference.CreatePositive(3, 1)

        [<Fact>]
        static member ``Substract() when start > end`` () =
            let month = new PaxMonth(4, 3)
            // Act & Assert
            defaultMath.Subtract(new PaxMonth(4, 4), month)  === MonthDifference.CreateNegative(0, 1)
            defaultMath.Subtract(new PaxMonth(4, 13), month) === MonthDifference.CreateNegative(0, 10)
            defaultMath.Subtract(new PaxMonth(5, 1), month)  === MonthDifference.CreateNegative(0, 11)
            defaultMath.Subtract(new PaxMonth(5, 2), month)  === MonthDifference.CreateNegative(0, 12)
            defaultMath.Subtract(new PaxMonth(5, 3), month)  === MonthDifference.CreateNegative(1, 0)
            defaultMath.Subtract(new PaxMonth(5, 4), month)  === MonthDifference.CreateNegative(1, 1)
            defaultMath.Subtract(new PaxMonth(6, 2), month)  === MonthDifference.CreateNegative(1, 12)
            defaultMath.Subtract(new PaxMonth(6, 3), month)  === MonthDifference.CreateNegative(2, 0)
            defaultMath.Subtract(new PaxMonth(6, 4), month)  === MonthDifference.CreateNegative(2, 1)
            defaultMath.Subtract(new PaxMonth(6, 12), month) === MonthDifference.CreateNegative(2, 9)
            // 6 is a leap year
            defaultMath.Subtract(new PaxMonth(6, 13), month) === MonthDifference.CreateNegative(2, 10)
            defaultMath.Subtract(new PaxMonth(6, 14), month) === MonthDifference.CreateNegative(2, 10)
            defaultMath.Subtract(new PaxMonth(7, 1), month)  === MonthDifference.CreateNegative(2, 11)
            defaultMath.Subtract(new PaxMonth(7, 2), month)  === MonthDifference.CreateNegative(2, 12)
            defaultMath.Subtract(new PaxMonth(7, 3), month)  === MonthDifference.CreateNegative(3, 0)
            defaultMath.Subtract(new PaxMonth(7, 4), month)  === MonthDifference.CreateNegative(3, 1)

    //
    // Year type
    //

    [<Sealed>]
    [<TestExcludeFrom(TestExcludeFrom.Regular)>]
    type YearFacts() =
        inherit IYearFacts<PaxYear, PaxMonth, PaxDate, StandardPaxDataSet>()

        static member MoreYearInfoData with get() = PaxDataSet.MoreYearInfoData

        [<Theory; MemberData(nameof(YearFacts.MonthInfoData))>]
        static member ``GetMonthOfYear()`` (info: MonthInfo) =
            let y, m = info.Yemo.Deconstruct()
            let year = new PaxYear(y)
            let date = new PaxMonth(y, m)
            // Act & Assert
            year.GetMonthOfYear(m) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidMonthFieldData))>]
        static member ``GetMonthOfYear() with an invalid month`` (y: int) m =
            let year = new PaxYear(y)
            // Act & Assert
            outOfRangeExn "month" (fun () -> year.GetMonthOfYear(m))

        [<Theory; MemberData(nameof(YearFacts.DateInfoData))>]
        static member ``GetDayOfYear()`` (info: DateInfo) =
            let y, doy = info.Yedoy.Deconstruct()
            let year = new PaxYear(y)
            let date = new PaxDate(y, doy)
            // Act & Assert
            year.GetDayOfYear(doy) === date

        [<Theory; MemberData(nameof(YearFacts.InvalidDayOfYearFieldData))>]
        static member ``GetDayOfYear() with an invalid day of the year`` (y: int) doy =
            let year = new PaxYear(y)
            // Act & Assert
            outOfRangeExn "dayOfYear" (fun () -> year.GetDayOfYear(doy))

        //
        // Featurettes
        //

        [<Theory; MemberData(nameof(YearFacts.MoreYearInfoData))>]
        static member ``PaxYear.CountWeeks()`` (info: YearAnd<int>) =
            let (y, weeksInYear) = info.Deconstruct()
            let year = new PaxYear(y)
            // Act & Assert
            year.CountWeeks() === weeksInYear
