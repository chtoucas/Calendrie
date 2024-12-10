// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.PrototypalSchemaTestSuite

open Calendrie.Core
open Calendrie.Core.Schemas
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core
open Calendrie.Testing.Faux

/// Creates a new instance of the schema archetype of type 'a.
let private prototypeOf(sch: ICalendricalSchema) =
    new PrototypalSchema(sch, sch.MinDaysInYear, sch.MinDaysInMonth)

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic12Tests() =
    inherit PrototypalSchemaFacts<Coptic12DataSet>(prototypeOf(new Coptic12Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic13Tests() =
    inherit PrototypalSchemaFacts<Coptic13DataSet>(prototypeOf(new Coptic13Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian12Tests() =
    inherit PrototypalSchemaFacts<Egyptian12DataSet>(prototypeOf(new Egyptian12Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian13Tests() =
    inherit PrototypalSchemaFacts<Egyptian13DataSet>(prototypeOf(new Egyptian13Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican12Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican12DataSet>(prototypeOf(new FrenchRepublican12Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican13Tests() =
    inherit PrototypalSchemaFacts<FrenchRepublican13DataSet>(prototypeOf(new FrenchRepublican13Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianTests() =
    inherit PrototypalSchemaFacts<GregorianDataSet>(prototypeOf(new GregorianSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type InternationalFixedTests() =
    inherit PrototypalSchemaFacts<InternationalFixedDataSet>(prototypeOf(new InternationalFixedSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type JulianTests() =
    inherit PrototypalSchemaFacts<JulianDataSet>(prototypeOf(new JulianSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type LunisolarTests() =
    inherit PrototypalSchemaFacts<FauxLunisolarDataSet>(prototypeOf(new FauxLunisolarSchema()))

//[<Sealed>]
//[<TestPerformance(TestPerformance.SlowBundle)>]
//[<TestExcludeFrom(TestExcludeFrom.Regular)>]
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
//type PaxTests() =
//    inherit PrototypalSchemaFacts<PaxDataSet>(prototypeOf(new PaxSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Persian2820Tests() =
    inherit PrototypalSchemaFacts<Persian2820DataSet>(prototypeOf(new Persian2820Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PositivistTests() =
    inherit PrototypalSchemaFacts<PositivistDataSet>(prototypeOf(new PositivistSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TabularIslamicTests() =
    inherit PrototypalSchemaFacts<TabularIslamicDataSet>(prototypeOf(new TabularIslamicSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TropicaliaTests() =
    inherit PrototypalSchemaFacts<TropicaliaDataSet>(prototypeOf(new TropicaliaSchema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3031Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3031DataSet>(prototypeOf(new Tropicalia3031Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3130Tests() =
    inherit PrototypalSchemaFacts<Tropicalia3130DataSet>(prototypeOf(new Tropicalia3130Schema()))

[<Sealed>]
[<TestPerformance(TestPerformance.SlowBundle)>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type WorldTests() =
    inherit PrototypalSchemaFacts<WorldDataSet>(prototypeOf(new WorldSchema()))
