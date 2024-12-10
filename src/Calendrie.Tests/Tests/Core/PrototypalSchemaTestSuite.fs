// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.PrototypalSchemaTestSuite

open Calendrie.Core
open Calendrie.Core.Schemas
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core
open Calendrie.Testing.Faux

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic12Tests() =
    inherit PrototypalSchemaFacts<Coptic12DataSet>(PrototypalSchema.Create(new Coptic12Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic13Tests() =
    inherit PrototypalSchemaFacts<Coptic13DataSet>(PrototypalSchema.Create(new Coptic13Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian12Tests() =
    inherit PrototypalSchemaFacts<Egyptian12DataSet>(PrototypalSchema.Create(new Egyptian12Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian13Tests() =
    inherit PrototypalSchemaFacts<Egyptian13DataSet>(PrototypalSchema.Create(new Egyptian13Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican12Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican12DataSet>(PrototypalSchema.Create(new FrenchRepublican12Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican13Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican13DataSet>(PrototypalSchema.Create(new FrenchRepublican13Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianTests() =
    inherit PrototypalSchemaFacts<GregorianDataSet>(PrototypalSchema.Create(new GregorianSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type InternationalFixedTests() =
    inherit PrototypalSchemaFacts<InternationalFixedDataSet>(PrototypalSchema.Create(new InternationalFixedSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type JulianTests() =
    inherit PrototypalSchemaFacts<JulianDataSet>(PrototypalSchema.Create(new JulianSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type LunisolarTests() =
    inherit PrototypalSchemaFacts<FauxLunisolarDataSet>(PrototypalSchema.Create(new FauxLunisolarSchema()))

//[<Sealed>]
//[<TestPerformance(TestPerformance.SlowBundle)>]
//[<TestExcludeFrom(TestExcludeFrom.Regular)>]
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
//type PaxTests() =
//    inherit PrototypalSchemaFacts<PaxDataSet>(PrototypalSchema.Create(new PaxSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Persian2820Tests() =
    inherit PrototypalSchemaFacts<Persian2820DataSet>(PrototypalSchema.Create(new Persian2820Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PositivistTests() =
    inherit PrototypalSchemaFacts<PositivistDataSet>(PrototypalSchema.Create(new PositivistSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TabularIslamicTests() =
    inherit PrototypalSchemaFacts<TabularIslamicDataSet>(PrototypalSchema.Create(new TabularIslamicSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TropicaliaTests() =
    inherit PrototypalSchemaFacts<TropicaliaDataSet>(PrototypalSchema.Create(new TropicaliaSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3031Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3031DataSet>(PrototypalSchema.Create(new Tropicalia3031Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3130Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3130DataSet>(PrototypalSchema.Create(new Tropicalia3130Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type WorldTests() =
    inherit PrototypalSchemaFacts<WorldDataSet>(PrototypalSchema.Create(new WorldSchema()))
