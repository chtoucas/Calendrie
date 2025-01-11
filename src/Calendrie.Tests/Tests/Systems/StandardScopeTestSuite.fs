// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.StandardScopeTestSuite

open Calendrie
open Calendrie.Core.Schemas
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Systems
open Calendrie.Testing.Faux

// TODO(fact): lunisolar (fake, not standard dataset), Tropicalia3xxx (not standard).

let private scopeOf(sch) = new StandardScope(sch, DayZero.OldStyle)

// Not an actual StandardScope but close enough.
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage.
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type CivilTests() =
    inherit CivilScopeFacts<StandardGregorianDataSet>(new CivilScope(new CivilSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic12Tests() =
    inherit StandardScopeFacts<StandardCoptic12DataSet>(scopeOf(new Coptic12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic13Tests() =
    inherit StandardScopeFacts<StandardCoptic13DataSet>(scopeOf(new Coptic13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian12Tests() =
    inherit StandardScopeFacts<StandardEgyptian12DataSet>(scopeOf(new Egyptian12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian13Tests() =
    inherit StandardScopeFacts<StandardEgyptian13DataSet>(scopeOf(new Egyptian13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican12Tests() =
    inherit StandardScopeFacts<StandardFrenchRepublican12DataSet>(scopeOf(new FrenchRepublican12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican13Tests() =
    inherit StandardScopeFacts<StandardFrenchRepublican13DataSet>(scopeOf(new FrenchRepublican13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage.
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type GregorianTests() =
    inherit StandardScopeFacts<StandardGregorianDataSet>(scopeOf(new GregorianSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type InternationalFixedTests() =
    inherit StandardScopeFacts<StandardInternationalFixedDataSet>(scopeOf(new InternationalFixedSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type JulianTests() =
    inherit StandardScopeFacts<StandardJulianDataSet>(scopeOf(new JulianSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FauxLunisolarTests() =
    inherit StandardScopeFacts<FauxLunisolarDataSet>(scopeOf(new FauxLunisolarSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PaxTests() =
    inherit StandardScopeFacts<StandardPaxDataSet>(scopeOf(new PaxSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Persian2820Tests() =
    inherit StandardScopeFacts<StandardPersian2820DataSet>(scopeOf(new Persian2820Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PositivistTests() =
    inherit StandardScopeFacts<StandardPositivistDataSet>(scopeOf(new PositivistSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TabularIslamicTests() =
    inherit StandardScopeFacts<StandardTabularIslamicDataSet>(scopeOf(new TabularIslamicSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TropicaliaTests() =
    inherit StandardScopeFacts<StandardTropicaliaDataSet>(scopeOf(new TropicaliaSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3031Tests() =
    inherit StandardScopeFacts<Tropicalia3031DataSet>(scopeOf(new Tropicalia3031Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Tropicalia3130Tests() =
    inherit StandardScopeFacts<Tropicalia3130DataSet>(scopeOf(new Tropicalia3130Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type WorldTests() =
    inherit StandardScopeFacts<StandardWorldDataSet>(scopeOf(new WorldSchema()))
