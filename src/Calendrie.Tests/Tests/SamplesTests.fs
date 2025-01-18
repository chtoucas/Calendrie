// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.SamplesTests

open Calendrie
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Core
open Calendrie.Testing.Facts.Hemerology

open Samples

// Testing a non-proleptic prototype.

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage.
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type CivilPrototypeTests() =
    inherit RegularSchemaPrototypeFacts<StandardGregorianDataSet>(new CivilPrototype())

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

// Testing an affine date.

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type DateableFacts() =
    inherit IDateableFacts<CivilTriple, StandardGregorianDataSet>()

        override __.GetDate(y, m, d) = new CivilTriple(y, m, d)
