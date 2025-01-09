// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Prototyping.NonRegularSchemaTestSuite

open Calendrie
open Calendrie.Core.Schemas
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core
open Calendrie.Testing.Faux

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage: necessary to fully cover GetMonthParts().
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type LunisolarTests() =
    inherit NonRegularSchemaPrototypeFacts<FauxLunisolarDataSet>(new FauxLunisolarSchema(), 12)

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Lunisolar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Months
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunisolarPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (false, 0)

// We add this temporary test for full test coverage. We lie about the value of
// MinMonthsInYear and use 12 instead of the actual value 13.
[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage: necessary to fully cover
// GetMonthParts() when monthsSinceEpoch >= 0.
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PaxTmpTests() =
    inherit NonRegularSchemaPrototypeFacts<PaxDataSet>(new PaxSchema(), 12)

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Other
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Weeks
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PaxPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (false, 0)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PaxTests() =
    inherit NonRegularSchemaPrototypeFacts<PaxDataSet>(new PaxSchema())

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Other
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Weeks
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PaxPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (false, 0)
