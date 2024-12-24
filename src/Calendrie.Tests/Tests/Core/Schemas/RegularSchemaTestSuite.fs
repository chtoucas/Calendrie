// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.RegularSchemaTestSuite

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
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic12Tests() =
    inherit CalendricalSchemaFacts<RegularSchema, Coptic12DataSet>(FauxRegularSchema.Create(new Coptic12Schema()))

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYears_Prop() =
        let sch = x.SchemaUT
        if sch.IsProleptic then
            sch.SupportedYears === RegularSchema.ProlepticSupportedYears
        else
            sch.SupportedYears === RegularSchema.StandardSupportedYears

    // TODO(fact): IsLeapYear() overflows as expected but CalendricalSchemaFacts
    // does not understand SupportedYearsCore. See LenientSchemaFacts.
    override _.KernelDoesNotOverflow() = ()

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type GregorianTests() =
    inherit CalendricalSchemaFacts<RegularSchema, GregorianDataSet>(FauxRegularSchema.Create(new GregorianSchema()))

    override x.Algorithm_Prop() = x.SchemaUT.Algorithm === CalendricalAlgorithm.Arithmetical
    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<GregorianPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYears_Prop() =
        let sch = x.SchemaUT
        if sch.IsProleptic then
            sch.SupportedYears === RegularSchema.ProlepticSupportedYears
        else
            sch.SupportedYears === RegularSchema.StandardSupportedYears
