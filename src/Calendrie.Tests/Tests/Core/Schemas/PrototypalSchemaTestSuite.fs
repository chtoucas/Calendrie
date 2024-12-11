// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.PrototypalSchemaTestSuite

open Calendrie
open Calendrie.Core
open Calendrie.Core.Schemas
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core
open Calendrie.Testing.Faux

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage.
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic12Tests() =
    inherit PrototypalSchemaFacts<Coptic12DataSet>(new PrototypalSchema(new Coptic12Schema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic13Tests() =
    inherit PrototypalSchemaFacts<Coptic13DataSet>(new PrototypalSchema(new Coptic13Schema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian12Tests() =
    inherit PrototypalSchemaFacts<Egyptian12DataSet>(new PrototypalSchema(new Egyptian12Schema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian13Tests() =
    inherit PrototypalSchemaFacts<Egyptian13DataSet>(new PrototypalSchema(new Egyptian13Schema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican12Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican12DataSet>(new PrototypalSchema(new FrenchRepublican12Schema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican13Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican13DataSet>(new PrototypalSchema(new FrenchRepublican13Schema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type GregorianTests() =
    inherit PrototypalSchemaFacts<GregorianDataSet>(new PrototypalSchema(new GregorianSchema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type InternationalFixedTests() =
    inherit PrototypalSchemaFacts<InternationalFixedDataSet>(new PrototypalSchema(new InternationalFixedSchema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type JulianTests() =
    inherit PrototypalSchemaFacts<JulianDataSet>(new PrototypalSchema(new JulianSchema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type LunisolarTests() =
    inherit PrototypalSchemaFacts<FauxLunisolarDataSet>(new PrototypalSchema(new FauxLunisolarSchema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Lunisolar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Months
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (false, 0)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PaxTests() =
    inherit PrototypalSchemaFacts<PaxDataSet>(new PrototypalSchema(new PaxSchema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Other
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Weeks
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (false, 0)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Persian2820Tests() =
    inherit PrototypalSchemaFacts<Persian2820DataSet>(new PrototypalSchema(new Persian2820Schema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PositivistTests() =
    inherit PrototypalSchemaFacts<PositivistDataSet>(new PrototypalSchema(new PositivistSchema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TabularIslamicTests() =
    inherit PrototypalSchemaFacts<TabularIslamicDataSet>(new PrototypalSchema(new TabularIslamicSchema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Lunar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TropicaliaTests() =
    inherit PrototypalSchemaFacts<TropicaliaDataSet>(new PrototypalSchema(new TropicaliaSchema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3031Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3031DataSet>(new PrototypalSchema(new Tropicalia3031Schema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3130Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3130DataSet>(new PrototypalSchema(new Tropicalia3130Schema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type WorldTests() =
    inherit PrototypalSchemaFacts<WorldDataSet>(new PrototypalSchema(new WorldSchema()))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)
