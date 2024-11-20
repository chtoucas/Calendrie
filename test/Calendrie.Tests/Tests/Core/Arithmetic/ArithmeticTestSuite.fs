// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Arithmetic.ArithmeticTestSuite

open Calendrie.Core
open Calendrie.Core.Arithmetic
open Calendrie.Core.Schemas
open Calendrie.Core.Utilities
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core

// TODO(code): lunisolar (fake) schema.

let private ariOf<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    let sch = syschemaOf<'a>()
    let seg = SystemSegment.Create(sch, sch.SupportedYears)
    SystemArithmetic.CreateDefault(seg)

// Solar12SystemArithmetic is not the default arithmetic for the Gregorian schema, but
// we still use it because it's the schema has a the most data to offer.
let private solar12Of<'a when 'a :> SystemSchema and 'a :> IBoxable<'a>> () =
    let sch = syschemaOf<'a>()
    let seg = SystemSegment.Create(sch, sch.SupportedYears)
    new Solar12SystemArithmetic(seg)
[<Sealed>]
type Solar12Tests() =
    inherit SystemArithmeticFacts<GregorianDataSet>(solar12Of<GregorianSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<Solar12SystemArithmetic>

//
// Normal test suite.
//

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Coptic12Tests() =
    inherit SystemArithmeticFacts<Coptic12DataSet>(ariOf<Coptic12Schema>())

// PlainSystemArithmetic
[<Sealed>]
type Coptic13Tests() =
    inherit SystemArithmeticFacts<Coptic13DataSet>(ariOf<Coptic13Schema>())

    member x.Arithmetic() = x.Arithmetic |> is<PlainSystemArithmetic>

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian12Tests() =
    inherit SystemArithmeticFacts<Egyptian12DataSet>(ariOf<Egyptian12Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian13Tests() =
    inherit SystemArithmeticFacts<Egyptian13DataSet>(ariOf<Egyptian13Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican12Tests() =
    inherit SystemArithmeticFacts<FrenchRepublican12DataSet>(ariOf<FrenchRepublican12Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican13Tests() =
    inherit SystemArithmeticFacts<FrenchRepublican13DataSet>(ariOf<FrenchRepublican13Schema>())

// GregorianSystemArithmetic
[<Sealed>]
type GregorianTests() =
    inherit SystemArithmeticFacts<GregorianDataSet>(ariOf<GregorianSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<GregorianSystemArithmetic>

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type InternationalFixedTests() =
    inherit SystemArithmeticFacts<InternationalFixedDataSet>(ariOf<InternationalFixedSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type JulianTests() =
    inherit SystemArithmeticFacts<JulianDataSet>(ariOf<JulianSchema>())

// LunisolarSystemArithmetic
[<Sealed>]
type FauxLunisolarTests() =
    inherit SystemArithmeticFacts<FauxLunisolarDataSet>(ariOf<FauxLunisolarSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<LunisolarSystemArithmetic>

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Persian2820Tests() =
    inherit SystemArithmeticFacts<Persian2820DataSet>(ariOf<Persian2820Schema>())

// Solar13SystemArithmetic
[<Sealed>]
type PositivistTests() =
    inherit SystemArithmeticFacts<PositivistDataSet>(ariOf<PositivistSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<Solar13SystemArithmetic>

// LunarSystemArithmetic
[<Sealed>]
type TabularIslamicTests() =
    inherit SystemArithmeticFacts<TabularIslamicDataSet>(ariOf<TabularIslamicSchema>())

    member x.Arithmetic() = x.Arithmetic |> is<LunarSystemArithmetic>

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
