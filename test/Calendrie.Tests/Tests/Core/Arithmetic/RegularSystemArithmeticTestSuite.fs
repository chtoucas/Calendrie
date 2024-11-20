// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Arithmetic.RegularSystemArithmeticTestSuite

open Calendrie.Core
open Calendrie.Core.Schemas
open Calendrie.Core.Utilities
open Calendrie.Sketches.Core.Arithmetic
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

// Since the Gregorian schema has the richest dataset, we use it as a default
// model for testing.

let private ariOf<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    let sch = syschemaOf<'a>()
    let seg = SystemSegment.Create(sch, sch.SupportedYears)
    new RegularSystemArithmetic(seg)

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Coptic12Tests() =
    inherit SystemArithmeticFacts<Coptic12DataSet>(ariOf<Coptic12Schema>())

// Coptic13 -> not compatible with RegularSystemArithmetic.

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian12Tests() =
    inherit SystemArithmeticFacts<Egyptian12DataSet>(ariOf<Egyptian12Schema>())

// Egyptian13 -> not compatible with RegularSystemArithmetic.

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican12Tests() =
    inherit SystemArithmeticFacts<FrenchRepublican12DataSet>(ariOf<FrenchRepublican12Schema>())

// FrenchRepublican13 -> not compatible with RegularSystemArithmetic.

[<Sealed>]
type GregorianTests() =
    inherit SystemArithmeticFacts<GregorianDataSet>(ariOf<GregorianSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type InternationalFixedTests() =
    inherit SystemArithmeticFacts<InternationalFixedDataSet>(ariOf<InternationalFixedSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type JulianTests() =
    inherit SystemArithmeticFacts<JulianDataSet>(ariOf<JulianSchema>())

// Lunisolar -> not compatible with RegularSystemArithmetic.

// Pax -> not compatible with RegularSystemArithmetic.

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Persian2820Tests() =
    inherit SystemArithmeticFacts<Persian2820DataSet>(ariOf<Persian2820Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type PositivistTests() =
    inherit SystemArithmeticFacts<PositivistDataSet>(ariOf<PositivistSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type TabularIslamicTests() =
    inherit SystemArithmeticFacts<TabularIslamicDataSet>(ariOf<TabularIslamicSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type TropicaliaTests() =
    inherit SystemArithmeticFacts<TropicaliaDataSet>(ariOf<TropicaliaSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Tropicalia3031Tests() =
    inherit SystemArithmeticFacts<Tropicalia3031DataSet>(ariOf<Tropicalia3031Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Tropicalia3130Tests() =
    inherit SystemArithmeticFacts<Tropicalia3130DataSet>(ariOf<Tropicalia3130Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type WorldTests() =
    inherit SystemArithmeticFacts<WorldDataSet>(ariOf<WorldSchema>())
