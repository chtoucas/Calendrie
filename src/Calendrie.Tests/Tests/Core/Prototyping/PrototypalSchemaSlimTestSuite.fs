// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Prototyping.PrototypalSchemaSlimTestSuite

open Calendrie
open Calendrie.Core.Prototyping
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
    inherit PrototypalSchemaFacts<Coptic12DataSet>(new PrototypalSchemaSlim(new Coptic12Schema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage: necessary to cover a branch within GetMonth().
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic13Tests() =
    inherit PrototypalSchemaFacts<Coptic13DataSet>(new PrototypalSchemaSlim(new Coptic13Schema(), 13))

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
    inherit PrototypalSchemaFacts<Egyptian12DataSet>(new PrototypalSchemaSlim(new Egyptian12Schema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian13Tests() =
    inherit PrototypalSchemaFacts<Egyptian13DataSet>(new PrototypalSchemaSlim(new Egyptian13Schema(), 13))

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
    inherit PrototypalSchemaFacts<FrenchRepublican12DataSet>(new PrototypalSchemaSlim(new FrenchRepublican12Schema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican13Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican13DataSet>(new PrototypalSchemaSlim(new FrenchRepublican13Schema(), 13))

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
    inherit PrototypalSchemaFacts<GregorianDataSet>(new PrototypalSchemaSlim(new GregorianSchema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<GregorianPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type InternationalFixedTests() =
    inherit PrototypalSchemaFacts<InternationalFixedDataSet>(new PrototypalSchemaSlim(new InternationalFixedSchema(), 13))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type JulianTests() =
    inherit PrototypalSchemaFacts<JulianDataSet>(new PrototypalSchemaSlim(new JulianSchema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<JulianPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage: necessary to cover a branch within GetMonth().
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type LunisolarTests() =
    inherit PrototypalSchemaFacts<FauxLunisolarDataSet>(new PrototypalSchemaSlim(new FauxLunisolarSchema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Lunisolar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Months
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunisolarPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (false, 0)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PaxTests() =
    inherit PrototypalSchemaFacts<PaxDataSet>(new PrototypalSchemaSlim(new PaxSchema(), 13))

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
    inherit PrototypalSchemaFacts<Persian2820DataSet>(new PrototypalSchemaSlim(new Persian2820Schema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PositivistTests() =
    inherit PrototypalSchemaFacts<PositivistDataSet>(new PrototypalSchemaSlim(new PositivistSchema(), 13))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TabularIslamicTests() =
    inherit PrototypalSchemaFacts<TabularIslamicDataSet>(new PrototypalSchemaSlim(new TabularIslamicSchema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Lunar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunarPreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TropicaliaTests() =
    inherit PrototypalSchemaFacts<TropicaliaDataSet>(new PrototypalSchemaSlim(new TropicaliaSchema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3031Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3031DataSet>(new PrototypalSchemaSlim(new Tropicalia3031Schema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3130Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3130DataSet>(new PrototypalSchemaSlim(new Tropicalia3130Schema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type WorldTests() =
    inherit PrototypalSchemaFacts<WorldDataSet>(new PrototypalSchemaSlim(new WorldSchema(), 12))

    override x.Algorithm_Prop() = x.PrototypeUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.PrototypeUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.PrototypeUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.PrototypeUT.IsRegular() === (true, 12)
