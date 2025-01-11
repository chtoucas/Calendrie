// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Validation.PreValidatorTestSuite

open System

open Calendrie.Core.Schemas
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core
open Calendrie.Testing.Faux

open Xunit

// TODO(fact): lunisolar (fake).

// Quand on teste la propriété PreValidatorUT, c'est juste pour être sûr qu'on
// teste bien les 8 types de pré-validateurs.

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type CivilTests() =
    inherit ICalendricalPreValidatorFacts<StandardGregorianDataSet>(new CivilSchema())

// Solar12PreValidator
[<Sealed>]
type Coptic12Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic12DataSet>(new Coptic12Schema())

    [<Fact>]
    member x.PreValidator() = x.PreValidatorUT |> is<Solar12PreValidator>

// PlainPreValidator
[<Sealed>]
type Coptic13Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic13DataSet>(new Coptic13Schema())

    [<Fact>]
    member x.PreValidator() = x.PreValidatorUT |> is<PlainPreValidator>

    override x.ValidateMonthDay_AtAbsoluteMaxYear() =
        let validator = x.PreValidatorUT
        // PlainPreValidator: no shortcut for short values of the day of the
        // month and Coptic13Schema.CountDaysInMonth() overflows.
        (fun () -> validator.ValidateMonthDay(Int32.MaxValue, 1, 1)) |> overflows

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian12Tests() =
    inherit ICalendricalPreValidatorFacts<Egyptian12DataSet>(new Egyptian12Schema())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian13Tests() =
    inherit ICalendricalPreValidatorFacts<Egyptian13DataSet>(new Egyptian13Schema())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican12Tests() =
    inherit ICalendricalPreValidatorFacts<FrenchRepublican12DataSet>(new FrenchRepublican12Schema())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican13Tests() =
    inherit ICalendricalPreValidatorFacts<FrenchRepublican13DataSet>(new FrenchRepublican13Schema())

// GregorianPreValidator
[<Sealed>]
type GregorianTests() =
    inherit ICalendricalPreValidatorFacts<GregorianDataSet>(new GregorianSchema())

    [<Fact>]
    member x.PreValidator() = x.PreValidatorUT |> is<GregorianPreValidator>

// Solar13PreValidator
[<Sealed>]
type InternationalFixedTests() =
    inherit ICalendricalPreValidatorFacts<InternationalFixedDataSet>(new InternationalFixedSchema())

    [<Fact>]
    member x.PreValidator() = x.PreValidatorUT |> is<Solar13PreValidator>

// JulianPreValidator
[<Sealed>]
type JulianTests() =
    inherit ICalendricalPreValidatorFacts<JulianDataSet>(new JulianSchema())

    [<Fact>]
    member x.PreValidator() = x.PreValidatorUT |> is<JulianPreValidator>

// LunisolarPreValidator
[<Sealed>]
type FauxLunisolarTests() =
    inherit ICalendricalPreValidatorFacts<FauxLunisolarDataSet>(new FauxLunisolarSchema())

    [<Fact>]
    member x.PreValidator() = x.PreValidatorUT |> is<LunisolarPreValidator>

// PaxPreValidator
[<Sealed>]
type PaxTests() =
    inherit ICalendricalPreValidatorFacts<PaxDataSet>(new PaxSchema())

    [<Fact>]
    member x.PreValidator() = x.PreValidatorUT |> is<PaxPreValidator>

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Persian2820Tests() =
    inherit ICalendricalPreValidatorFacts<Persian2820DataSet>(new Persian2820Schema())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PositivistTests() =
    inherit ICalendricalPreValidatorFacts<PositivistDataSet>(new PositivistSchema())

// LunarPreValidator
[<Sealed>]
type TabularIslamicTests() =
    inherit ICalendricalPreValidatorFacts<TabularIslamicDataSet>(new TabularIslamicSchema())

    [<Fact>]
    member x.PreValidator() = x.PreValidatorUT |> is<LunarPreValidator>

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TropicaliaTests() =
    inherit ICalendricalPreValidatorFacts<TropicaliaDataSet>(new TropicaliaSchema())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3031Tests() =
    inherit ICalendricalPreValidatorFacts<Tropicalia3031DataSet>(new Tropicalia3031Schema())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3130Tests() =
    inherit ICalendricalPreValidatorFacts<Tropicalia3130DataSet>(new Tropicalia3130Schema())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type WorldTests() =
    inherit ICalendricalPreValidatorFacts<WorldDataSet>(new WorldSchema())
