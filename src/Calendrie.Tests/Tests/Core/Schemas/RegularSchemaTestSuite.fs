// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.RegularSchemaTestSuite

open Calendrie
open Calendrie.Core.Schemas
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core

open Samples

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage.
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type CivilPrototypeTests() =
    inherit RegularSchemaFacts<StandardGregorianDataSet>(new CivilPrototype())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    // TODO(fact): IndexOutOfRangeException. Update also ICalendricalSchemaFacts
    override _.CountDaysInMonth_DoesNotOverflow() = ()
    override _.CountDaysInYearBeforeMonth_DoesNotOverflow() = ()
    override _.CountDaysSinceEpoch﹍DateParts_DoesNotOverflow() = ()
    override _.GetDayOfYear_DoesNotOverflow() = ()
    override _.GetEndOfMonth_DoesNotOverflow() = ()
    override _.GetStartOfMonth_DoesNotOverflow() = ()
    override _.KernelDoesNotOverflow() = ()

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage.
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic12Tests() =
    inherit RegularSchemaFacts<Coptic12DataSet>(new Coptic12Schema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    // TODO(fact): IsLeapYear() overflows as expected but CalendricalSchemaFacts
    // does not understand SupportedYearsCore. See LenientSchemaFacts.
    // Also disabled for Coptic13, Persian2820 and TabularIslamic.
    override _.KernelDoesNotOverflow() = ()

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic13Tests() =
    inherit RegularSchemaFacts<Coptic13DataSet>(new Coptic13Schema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

    override _.KernelDoesNotOverflow() = ()

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian12Tests() =
    inherit RegularSchemaFacts<Egyptian12DataSet>(new Egyptian12Schema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian13Tests() =
    inherit RegularSchemaFacts<Egyptian13DataSet>(new Egyptian13Schema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican12Tests() =
    inherit RegularSchemaFacts<FrenchRepublican12DataSet>(new FrenchRepublican12Schema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican13Tests() =
    inherit RegularSchemaFacts<FrenchRepublican13DataSet>(new FrenchRepublican13Schema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type GregorianTests() =
    inherit RegularSchemaFacts<GregorianDataSet>(new GregorianSchema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<GregorianPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type InternationalFixedTests() =
    inherit RegularSchemaFacts<InternationalFixedDataSet>(new InternationalFixedSchema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type JulianTests() =
    inherit RegularSchemaFacts<JulianDataSet>(new JulianSchema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<JulianPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

// No tests for FauxLunisolarSchema which is not regular.
// No tests for PaxSchema which is not regular.

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Persian2820Tests() =
    inherit RegularSchemaFacts<Persian2820DataSet>(new Persian2820Schema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override _.KernelDoesNotOverflow() = ()

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PositivistTests() =
    inherit RegularSchemaFacts<PositivistDataSet>(new PositivistSchema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TabularIslamicTests() =
    inherit RegularSchemaFacts<TabularIslamicDataSet>(new TabularIslamicSchema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Lunar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunarPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override _.KernelDoesNotOverflow() = ()

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TropicaliaTests() =
    inherit RegularSchemaFacts<TropicaliaDataSet>(new TropicaliaSchema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3031Tests() =
    inherit RegularSchemaFacts<Tropicalia3031DataSet>(new Tropicalia3031Schema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3130Tests() =
    inherit RegularSchemaFacts<Tropicalia3130DataSet>(new Tropicalia3130Schema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type WorldTests() =
    inherit RegularSchemaFacts<WorldDataSet>(new WorldSchema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
