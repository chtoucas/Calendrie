// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.StandardScopeTestSuite

open Calendrie
open Calendrie.Core.Schemas
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Systems
open Calendrie.Testing.Faux

// TODO(fact): lunisolar (fake).

let private scopeOf(sch) = new StandardScope(sch, DayZero.OldStyle)

// Not an actual StandardScope but close enough.
[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage.
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type CivilTests() =
    inherit CivilScopeFacts<GregorianDataSet>(new CivilScope(new CivilSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic12Tests() =
    inherit StandardScopeFacts<Coptic12DataSet>(scopeOf(new Coptic12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Coptic13Tests() =
    inherit StandardScopeFacts<Coptic13DataSet>(scopeOf(new Coptic13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian12Tests() =
    inherit StandardScopeFacts<Egyptian12DataSet>(scopeOf(new Egyptian12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Egyptian13Tests() =
    inherit StandardScopeFacts<Egyptian13DataSet>(scopeOf(new Egyptian13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican12Tests() =
    inherit StandardScopeFacts<FrenchRepublican12DataSet>(scopeOf(new FrenchRepublican12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FrenchRepublican13Tests() =
    inherit StandardScopeFacts<FrenchRepublican13DataSet>(scopeOf(new FrenchRepublican13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
// We do not exclude this one from CodeCoverage.
//[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type GregorianTests() =
    inherit StandardScopeFacts<GregorianDataSet>(scopeOf(new GregorianSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type InternationalFixedTests() =
    inherit StandardScopeFacts<InternationalFixedDataSet>(scopeOf(new InternationalFixedSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type JulianTests() =
    inherit StandardScopeFacts<JulianDataSet>(scopeOf(new JulianSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type FauxLunisolarTests() =
    inherit StandardScopeFacts<FauxLunisolarDataSet>(scopeOf(new FauxLunisolarSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PaxTests() =
    inherit StandardScopeFacts<PaxDataSet>(scopeOf(new PaxSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type Persian2820Tests() =
    inherit StandardScopeFacts<Persian2820DataSet>(scopeOf(new Persian2820Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type PositivistTests() =
    inherit StandardScopeFacts<PositivistDataSet>(scopeOf(new PositivistSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TabularIslamicTests() =
    inherit StandardScopeFacts<TabularIslamicDataSet>(scopeOf(new TabularIslamicSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
[<TestExcludeFrom(TestExcludeFrom.CodeCoverage)>]
type TropicaliaTests() =
    inherit StandardScopeFacts<TropicaliaDataSet>(scopeOf(new TropicaliaSchema()))

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
    inherit StandardScopeFacts<WorldDataSet>(scopeOf(new WorldSchema()))
