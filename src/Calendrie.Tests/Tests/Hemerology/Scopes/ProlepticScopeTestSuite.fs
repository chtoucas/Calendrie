// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.Scopes.ProlepticScopeTestSuite

open Calendrie
open Calendrie.Core.Schemas
open Calendrie.Hemerology
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Faux

// TODO(code): Hebrew (unfinished, no data), Pax (not proleptic) and lunisolar (fake) schema.

// Returns a ProlepticScope.
let private scopeOf(sch) =
    ProlepticScope.Create(sch, DayZero.OldStyle)

[<Sealed>]
type Coptic12Tests() =
    inherit ProlepticScopeFacts<Coptic12DataSet>(scopeOf(new Coptic12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Coptic13Tests() =
    inherit ProlepticScopeFacts<Coptic13DataSet>(scopeOf(new Coptic13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian12Tests() =
    inherit ProlepticScopeFacts<Egyptian12DataSet>(scopeOf(new Egyptian12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian13Tests() =
    inherit ProlepticScopeFacts<Egyptian13DataSet>(scopeOf(new Egyptian13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican12Tests() =
    inherit ProlepticScopeFacts<FrenchRepublican12DataSet>(scopeOf(new FrenchRepublican12Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican13Tests() =
    inherit ProlepticScopeFacts<FrenchRepublican13DataSet>(scopeOf(new FrenchRepublican13Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianTests() =
    inherit ProlepticScopeFacts<GregorianDataSet>(scopeOf(new GregorianSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type InternationalFixedTests() =
    inherit ProlepticScopeFacts<InternationalFixedDataSet>(scopeOf(new InternationalFixedSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type JulianTests() =
    inherit ProlepticScopeFacts<JulianDataSet>(scopeOf(new JulianSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FauxLunisolarTests() =
    inherit ProlepticScopeFacts<FauxLunisolarDataSet>(scopeOf(new FauxLunisolarSchema()))

//[<Sealed>]
//[<TestExcludeFrom(TestExcludeFrom.Regular)>]
//type PaxTests() =
//    inherit ProlepticScopeFacts<PaxDataSet>(scopeOf2(new PaxSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Persian2820Tests() =
    inherit ProlepticScopeFacts<Persian2820DataSet>(scopeOf(new Persian2820Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type PositivistTests() =
    inherit ProlepticScopeFacts<PositivistDataSet>(scopeOf(new PositivistSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type TabularIslamicTests() =
    inherit ProlepticScopeFacts<TabularIslamicDataSet>(scopeOf(new TabularIslamicSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type TropicaliaTests() =
    inherit ProlepticScopeFacts<TropicaliaDataSet>(scopeOf(new TropicaliaSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Tropicalia3031Tests() =
    inherit ProlepticScopeFacts<Tropicalia3031DataSet>(scopeOf(new Tropicalia3031Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Tropicalia3130Tests() =
    inherit ProlepticScopeFacts<Tropicalia3130DataSet>(scopeOf(new Tropicalia3130Schema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type WorldTests() =
    inherit ProlepticScopeFacts<WorldDataSet>(scopeOf(new WorldSchema()))
