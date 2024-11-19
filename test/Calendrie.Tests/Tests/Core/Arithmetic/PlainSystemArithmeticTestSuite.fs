// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Arithmetic.PlainSystemArithmeticTestSuite

open Calendrie.Core
open Calendrie.Core.Arithmetic
open Calendrie.Core.Schemas
open Calendrie.Core.Utilities
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished) and lunisolar (fake) schema.

// Since the Gregorian schema has the richest dataset, we use it as a default
// model for testing.

let private ariOf<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    let sch = syschemaOf<'a>()
    let seg = SystemSegment.Create(sch, sch.SupportedYears)
    new PlainSystemArithmetic(seg)

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Coptic12Tests() =
    inherit SystemArithmeticFacts<Coptic12DataSet>(ariOf<Coptic12Schema>())

// Coptic13Schema -> already tested in ArithmeticTestSuite.

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian12Tests() =
    inherit SystemArithmeticFacts<Egyptian12DataSet>(ariOf<Egyptian12Schema>())

// Egyptian13Schema -> already tested in ArithmeticTestSuite.

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican12Tests() =
    inherit SystemArithmeticFacts<FrenchRepublican12DataSet>(ariOf<FrenchRepublican12Schema>())

// FrenchRepublican13Schema -> already tested in ArithmeticTestSuite.

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

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type LunisolarTests() =
    inherit SystemArithmeticFacts<LunisolarDataSet>(ariOf<LunisolarSchema>())

// PaxSchema -> already tested in ArithmeticTestSuite.

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
