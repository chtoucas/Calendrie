// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.Scopes.StandardScopeTestSuite

open Calendrie
open Calendrie.Core.Schemas
open Calendrie.Specialized
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Faux

// Returns a StandardScope.
let private scopeOf(sch) =
    StandardScope.Create(sch, DayZero.OldStyle)

[<Sealed>]
type Coptic12Tests() =
    inherit StandardScopeFacts<Coptic12DataSet>(scopeOf(new Coptic12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Coptic13Tests() =
    inherit StandardScopeFacts<Coptic13DataSet>(scopeOf(new Coptic13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian12Tests() =
    inherit StandardScopeFacts<Egyptian12DataSet>(scopeOf(new Egyptian12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian13Tests() =
    inherit StandardScopeFacts<Egyptian13DataSet>(scopeOf(new Egyptian13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican12Tests() =
    inherit StandardScopeFacts<FrenchRepublican12DataSet>(scopeOf(new FrenchRepublican12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican13Tests() =
    inherit StandardScopeFacts<FrenchRepublican13DataSet>(scopeOf(new FrenchRepublican13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianTests() =
    inherit StandardScopeFacts<GregorianDataSet>(scopeOf(new GregorianSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type InternationalFixedTests() =
    inherit StandardScopeFacts<InternationalFixedDataSet>(scopeOf(new InternationalFixedSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type JulianTests() =
    inherit StandardScopeFacts<JulianDataSet>(scopeOf(new JulianSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FauxLunisolarTests() =
    inherit StandardScopeFacts<FauxLunisolarDataSet>(scopeOf(new FauxLunisolarSchema()))

//[<Sealed>]
//[<TestExcludeFrom(TestExcludeFrom.Regular)>]
//type PaxTests() =
//    inherit StandardScopeFacts<PaxDataSet>(scopeOf(new PaxSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Persian2820Tests() =
    inherit StandardScopeFacts<Persian2820DataSet>(scopeOf(new Persian2820Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type PositivistTests() =
    inherit StandardScopeFacts<PositivistDataSet>(scopeOf(new PositivistSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type TabularIslamicTests() =
    inherit StandardScopeFacts<TabularIslamicDataSet>(scopeOf(new TabularIslamicSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type TropicaliaTests() =
    inherit StandardScopeFacts<TropicaliaDataSet>(scopeOf(new TropicaliaSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Tropicalia3031Tests() =
    inherit StandardScopeFacts<Tropicalia3031DataSet>(scopeOf(new Tropicalia3031Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Tropicalia3130Tests() =
    inherit StandardScopeFacts<Tropicalia3130DataSet>(scopeOf(new Tropicalia3130Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type WorldTests() =
    inherit StandardScopeFacts<WorldDataSet>(scopeOf(new WorldSchema()))
