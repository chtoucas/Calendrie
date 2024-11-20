// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.Scopes.ProlepticScopeTestSuite

open Calendrie
open Calendrie.Core
open Calendrie.Core.Schemas
open Calendrie.Core.Utilities
open Calendrie.Hemerology.Scopes
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology

// TODO(code): Hebrew (unfinished, no data), Pax (not proleptic) and lunisolar (fake) schema.

// Returns a ProlepticScope.
let private scopeOf<'a when 'a :> CalendricalSchema and 'a :> IBoxable<'a>> () =
    let sch = SchemaActivator.CreateInstance<'a>()
    ProlepticScope.Create(sch, DayZero.OldStyle)

[<Sealed>]
type Coptic12Tests() =
    inherit ProlepticScopeFacts<Coptic12DataSet>(scopeOf<Coptic12Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Coptic13Tests() =
    inherit ProlepticScopeFacts<Coptic13DataSet>(scopeOf<Coptic13Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian12Tests() =
    inherit ProlepticScopeFacts<Egyptian12DataSet>(scopeOf<Egyptian12Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian13Tests() =
    inherit ProlepticScopeFacts<Egyptian13DataSet>(scopeOf<Egyptian13Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican12Tests() =
    inherit ProlepticScopeFacts<FrenchRepublican12DataSet>(scopeOf<FrenchRepublican12Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican13Tests() =
    inherit ProlepticScopeFacts<FrenchRepublican13DataSet>(scopeOf<FrenchRepublican13Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianTests() =
    inherit ProlepticScopeFacts<GregorianDataSet>(scopeOf<GregorianSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type InternationalFixedTests() =
    inherit ProlepticScopeFacts<InternationalFixedDataSet>(scopeOf<InternationalFixedSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type JulianTests() =
    inherit ProlepticScopeFacts<JulianDataSet>(scopeOf<JulianSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FauxLunisolarTests() =
    inherit ProlepticScopeFacts<FauxLunisolarDataSet>(scopeOf<FauxLunisolarSchema>())

//[<Sealed>]
//[<TestExcludeFrom(TestExcludeFrom.Regular)>]
//type PaxTests() =
//    inherit ProlepticScopeFacts<PaxDataSet>(scopeOf<PaxSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Persian2820Tests() =
    inherit ProlepticScopeFacts<Persian2820DataSet>(scopeOf<Persian2820Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type PositivistTests() =
    inherit ProlepticScopeFacts<PositivistDataSet>(scopeOf<PositivistSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type TabularIslamicTests() =
    inherit ProlepticScopeFacts<TabularIslamicDataSet>(scopeOf<TabularIslamicSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type TropicaliaTests() =
    inherit ProlepticScopeFacts<TropicaliaDataSet>(scopeOf<TropicaliaSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Tropicalia3031Tests() =
    inherit ProlepticScopeFacts<Tropicalia3031DataSet>(scopeOf<Tropicalia3031Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Tropicalia3130Tests() =
    inherit ProlepticScopeFacts<Tropicalia3130DataSet>(scopeOf<Tropicalia3130Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type WorldTests() =
    inherit ProlepticScopeFacts<WorldDataSet>(scopeOf<WorldSchema>())
