﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.CalendricalSchemaTests

open System

open Calendrie.Core
open Calendrie.Core.Intervals
open Calendrie.Core.Schemas
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Faux

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor expects supportedYears:Min to be >= MaxSupportedYears:Min`` () =
        let maxrange = CalendricalSchema.MaxSupportedYears
        let range = maxrange.WithMin(maxrange.Min - 1)

        argExn "supportedYears" (fun () -> new FauxCalendricalSchema(range))

    [<Fact>]
    let ``Constructor expects supportedYears:Max to be <= MaxSupportedYears:Max`` () =
        let maxrange = CalendricalSchema.MaxSupportedYears
        let range = maxrange.WithMax(maxrange.Max + 1)

        argExn "supportedYears" (fun () -> new FauxCalendricalSchema(range))

    [<Fact>]
    let ``Constructor succeeds with supportedYears = DefaultSupportedYears`` () =
        new FauxCalendricalSchema(CalendricalSchema.DefaultSupportedYears) |> ignore

    [<Fact>]
    let ``Constructor succeeds with supportedYears = MaxSupportedYears`` () =
        new FauxCalendricalSchema(CalendricalSchema.MaxSupportedYears) |> ignore

    [<Fact>]
    let ``Constructor throws when supportedYearsCore is not a superset of supportedYears`` () =
        let range = Segment.Create(1, 100)
        let rangeCore = Segment.Create(2, 99)

        argExn "value" (fun () -> new FauxCalendricalSchema(range, rangeCore))

    [<Fact>]
    let ``Constructor succeeds when supportedYearsCore = supportedYears`` () =
        let range = Segment.Create(1, 100)
        new FauxCalendricalSchema(range, range) |> ignore

    [<Fact>]
    let ``Constructor succeeds when supportedYearsCore is a superset of supportedYears`` () =
        let range = Segment.Create(1, 100)
        let rangeCore = Segment.Create(0, 101)
        new FauxCalendricalSchema(range, rangeCore) |> ignore

    [<Fact>]
    let ``Default value for SupportedYearsCore is any int`` () =
        let sch = new FauxCalendricalSchema()

        sch.SupportedYearsCore === Segment.Maximal32

    [<Fact>]
    let ``Default value for SupportedYears is DefaultSupportedYears`` () =
        let sch = new FauxCalendricalSchema()

        sch.SupportedYears === CalendricalSchema.DefaultSupportedYears

    [<Fact>]
    let ``Constructor expects minDaysInYear to be > 0`` () =
        outOfRangeExn "minDaysInYear" (fun () -> FauxCalendricalSchema.WithMinDaysInYear(0))
        outOfRangeExn "minDaysInYear" (fun () -> FauxCalendricalSchema.WithMinDaysInYear(-1))

    [<Fact>]
    let ``Constructor expects minDaysInMonth to be > 0`` () =
        outOfRangeExn "minDaysInMonth" (fun () -> FauxCalendricalSchema.WithMinDaysInMonth(0))
        outOfRangeExn "minDaysInMonth" (fun () -> FauxCalendricalSchema.WithMinDaysInMonth(-1))

    [<Fact>]
    let ``Constructor succeeds with minDaysInYear > 0`` () =
        FauxCalendricalSchema.WithMinDaysInYear(1) |> ignore

    [<Fact>]
    let ``Constructor succeeds with minDaysInMonth > 0`` () =
        FauxCalendricalSchema.WithMinDaysInMonth(1) |> ignore

    [<Fact>]
    let ``Property PreValidator: default value, repeated`` () =
        let sch = new FauxCalendricalSchema()

        let validator1 = sch.PreValidator
        validator1 |> is<PlainPreValidator>

        let validator2 = sch.PreValidator
        validator2 |> is<PlainPreValidator>

        validator2 ==& validator1

    [<Fact>]
    let ``Property PreValidator: setter throws for null value`` () =
        let factory _ = null :> ICalendricalPreValidator
        nullExn "value" (fun () -> FauxCalendricalSchema.WithPreValidator(factory))

    [<Fact>]
    let ``Property PreValidator: setter`` () =
        let factory (x: CalendricalSchema) = new Solar12PreValidator(x) :> ICalendricalPreValidator
        let sch = FauxCalendricalSchema.WithPreValidator(factory)

        sch.PreValidator |> is<Solar12PreValidator>

    [<Fact>]
    let ``Property Profile: default value, repeated`` () =
        let sch = new FauxCalendricalSchema()

        let profile1 = sch.Profile
        let profile2 = sch.Profile

        // The faux schema is not regular.
        profile1 === CalendricalProfile.Other
        profile2 === profile1

module Coptic13Case =
    [<Fact>]
    let ``Constant field IntercalaryMonth`` () =
        Coptic13Schema.IntercalaryMonth === 13

module Egyptian13Case =
    [<Fact>]
    let ``Constant field IntercalaryMonth`` () =
        Egyptian13Schema.IntercalaryMonth === 13

module FrenchRepublican13Case =
    [<Fact>]
    let ``Constant field IntercalaryMonth`` () =
        FrenchRepublican13Schema.IntercalaryMonth === 13

module GregorianCase =
    let private dataSet = GregorianDataSet.Instance
    let private sch = new GregorianSchema()

    let monthInfoData = dataSet.MonthInfoData
    let daysInYearAfterDateData = dataSet.DaysInYearAfterDateData
    let daysInMonthAfterDateData = dataSet.DaysInMonthAfterDateData

    [<Theory>]
    [<InlineData -10>] [<InlineData -9>] [<InlineData -8>] [<InlineData -7>] [<InlineData -6>] [<InlineData -5>]
    [<InlineData -4>] [<InlineData -3>] [<InlineData -2>] [<InlineData -1>] [<InlineData 0>] [<InlineData 1>]
    [<InlineData 2>] [<InlineData 3>] [<InlineData 4>] [<InlineData 5>] [<InlineData 6>] [<InlineData 7>]
    [<InlineData 8>] [<InlineData 9>] [<InlineData 10>]
    let ``Constant DaysPer400YearCycle`` c =
        let daysInCycle = sch.CountDaysSinceEpoch((c + 1) * 400, 1, 1) - sch.CountDaysSinceEpoch(c * 400, 1, 1)
        GregorianSchema.DaysPer400YearCycle === daysInCycle

    [<Fact>]
    let ``Property DaysIn4YearCycle`` () =
        // We also check DaysInCommonYear, DaysInLeapYear and DaysPer4YearSubcycle.

        let daysInYear0 = sch.CountDaysSinceEpoch(1, 1, 1) - sch.CountDaysSinceEpoch(0, 1, 1)
        let daysInYear1 = sch.CountDaysSinceEpoch(2, 1, 1) - sch.CountDaysSinceEpoch(1, 1, 1)
        let daysInYear2 = sch.CountDaysSinceEpoch(3, 1, 1) - sch.CountDaysSinceEpoch(2, 1, 1)
        let daysInYear3 = sch.CountDaysSinceEpoch(4, 1, 1) - sch.CountDaysSinceEpoch(3, 1, 1)

        let daysIn4YearCycle = GJSchema.DaysIn4YearCycle
        daysIn4YearCycle.Length === 4
        int(daysIn4YearCycle[0]) === daysInYear0
        int(daysIn4YearCycle[1]) === daysInYear1
        int(daysIn4YearCycle[2]) === daysInYear2
        int(daysIn4YearCycle[3]) === daysInYear3

        daysInYear0 === GJSchema.DaysPerLeapYear // Year 0 is leap
        daysInYear1 === GJSchema.DaysPerCommonYear
        daysInYear2 === GJSchema.DaysPerCommonYear
        daysInYear3 === GJSchema.DaysPerCommonYear

        let sum = Array.sum <| daysIn4YearCycle.ToArray()
        int(sum) === GregorianSchema.DaysPer4YearSubcycle

    [<Fact>]
    let ``Property DaysIn4CenturyCycle`` () =
        // We also check DaysPer100YearSubcycle.

        let daysInCentury0 = sch.CountDaysSinceEpoch(1, 1, 1) - sch.CountDaysSinceEpoch(-99, 1, 1)
        let daysInCentury1 = sch.CountDaysSinceEpoch(101, 1, 1) - sch.CountDaysSinceEpoch(1, 1, 1)
        let daysInCentury2 = sch.CountDaysSinceEpoch(201, 1, 1) - sch.CountDaysSinceEpoch(101, 1, 1)
        let daysInCentury3 = sch.CountDaysSinceEpoch(301, 1, 1) - sch.CountDaysSinceEpoch(201, 1, 1)

        let daysIn4CenturyCycle = GregorianSchema.DaysIn4CenturyCycle
        daysIn4CenturyCycle.Length === 4
        int(daysIn4CenturyCycle[0]) === daysInCentury0
        int(daysIn4CenturyCycle[1]) === daysInCentury1
        int(daysIn4CenturyCycle[2]) === daysInCentury2
        int(daysIn4CenturyCycle[3]) === daysInCentury3

        daysInCentury0 === GregorianSchema.DaysPer100YearSubcycle + 1 // Long century
        daysInCentury1 === GregorianSchema.DaysPer100YearSubcycle
        daysInCentury2 === GregorianSchema.DaysPer100YearSubcycle
        daysInCentury3 === GregorianSchema.DaysPer100YearSubcycle

    // ICalendricalSchemaPlus

    [<Theory; MemberData(nameof(monthInfoData))>]
    let CountDaysInYearAfterMonth (info: MonthInfo) =
        let y, m = info.Yemo.Deconstruct()
        sch.CountDaysInYearAfterMonth(y, m) === int(info.DaysInYearAfterMonth)

    [<Theory; MemberData(nameof(daysInYearAfterDateData))>]
    let CountDaysInYearAfter (info: YemodaAnd<int>) =
        let y, m, d, daysInYearAfter = info.Deconstruct()
        sch.CountDaysInYearAfter(y, m, d) === daysInYearAfter

    [<Theory; MemberData(nameof(daysInMonthAfterDateData))>]
    let CountDaysInMonthAfter (info: YemodaAnd<int>) =
        let y, m, d, daysInMonthAfter = info.Deconstruct()
        sch.CountDaysInMonthAfter(y, m, d) === daysInMonthAfter

module TropicalistaCase =
    let private dataSet = TropicaliaDataSet.Instance

    let yearInfoData = dataSet.YearInfoData

    [<Theory; MemberData(nameof(yearInfoData))>]
    let IsLeapYearImpl (x: YearInfo) =
        TropicalistaSchema.IsLeapYearImpl(x.Year) === x.IsLeap

    let ``IsLeapYearImpl() does not overflow`` () =
        TropicalistaSchema.IsLeapYearImpl(Int32.MinValue) |> ignore
        TropicalistaSchema.IsLeapYearImpl(Int32.MaxValue) |> ignore

module WorldCase =
    let private sch = new WorldSchema()

    let moreMonthInfoData = WorldDataSet.MoreMonthInfoData

    [<Theory; MemberData(nameof(moreMonthInfoData))>]
    let CountDaysInWorldMonth (info: YemoAnd<int>) =
        let (y, m, daysInMonth) = info.Deconstruct()

        sch.CountDaysInWorldMonth(y, m) === daysInMonth
        WorldSchema.CountDaysInWorldMonthImpl(m) === daysInMonth
