// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.MonthsCalculatorTests

open Calendrie.Core
open Calendrie.Core.Schemas
open Calendrie.Testing
open Calendrie.Testing.Data
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Faux

open Xunit

module Factories =
    [<Fact>]
    let ``Create() throws for null schema`` () =
        nullExn "schema" (fun () -> MonthsCalculator.Create(null))

    [<Fact>]
    let ``Create()`` () =
        MonthsCalculator.Create(FauxCalendricalSchema.Regular14)      |> is<MonthsCalculator.Regular>

        MonthsCalculator.Create(new Coptic12Schema())           |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(new Coptic13Schema())           |> is<MonthsCalculator.Regular13>
        MonthsCalculator.Create(new Egyptian12Schema())         |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(new Egyptian13Schema())         |> is<MonthsCalculator.Regular13>
        MonthsCalculator.Create(new FrenchRepublican12Schema()) |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(new FrenchRepublican13Schema()) |> is<MonthsCalculator.Regular13>
        MonthsCalculator.Create(new GregorianSchema())          |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(new InternationalFixedSchema()) |> is<MonthsCalculator.Regular13>
        MonthsCalculator.Create(new JulianSchema())             |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(new Persian2820Schema())        |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(new PositivistSchema())         |> is<MonthsCalculator.Regular13>
        MonthsCalculator.Create(new TabularIslamicSchema())     |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(new TropicaliaSchema())         |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(new Tropicalia3031Schema())     |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(new Tropicalia3130Schema())     |> is<MonthsCalculator.Regular12>
        MonthsCalculator.Create(new WorldSchema())              |> is<MonthsCalculator.Regular12>

module PlainCase =
    let private dataSet = GregorianDataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let private calc = new MonthsCalculator.Plain(new GregorianSchema())

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetEndOfYear(x.Year) === x.MonthsSinceEpoch

module RegularCase =
    let private dataSet = GregorianDataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let private calc = new MonthsCalculator.Regular(new GregorianSchema(), 12)

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetEndOfYear(x.Year) === x.MonthsSinceEpoch

module Regular12Case =
    let private dataSet = GregorianDataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let private calc = new MonthsCalculator.Regular12(new GregorianSchema())

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetEndOfYear(x.Year) === x.MonthsSinceEpoch

module Regular13Case =
    let private dataSet = Coptic13DataSet.Instance

    let startOfYearMonthsSinceEpochData = dataSet.StartOfYearMonthsSinceEpochData
    let endOfYearMonthsSinceEpochData = dataSet.EndOfYearMonthsSinceEpochData

    let private calc = new MonthsCalculator.Regular13(new Coptic13Schema())

    [<Theory; MemberData(nameof(startOfYearMonthsSinceEpochData))>]
    let ``GetStartOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetStartOfYear(x.Year) === x.MonthsSinceEpoch

    [<Theory; MemberData(nameof(endOfYearMonthsSinceEpochData))>]
    let ``GetEndOfYear()`` (x: YearMonthsSinceEpoch) =
        calc.GetEndOfYear(x.Year) === x.MonthsSinceEpoch
