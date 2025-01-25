// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.CSharpTests

open Calendrie.Testing
open Calendrie.Testing.CSharpTests
open Calendrie.Testing.CSharpTests.Geometry

[<Sealed>]
type ApiCSharpTests() =
    inherit ApiTests()

[<Sealed>]
type ArrayHelpersCSharpTests() =
    inherit ArrayHelpersTests()

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type BoundedBelowScopeCSharpTests() =
    inherit BoundedBelowScopeTests()

[<Sealed>]
type CSharpOnlyCSharpTests() =
    inherit CSharpOnlyTests()

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianMinMaxYearCalendarCSharpTests() =
    inherit GregorianMinMaxYearCalendarTests()

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianBoundedBelowCalendarCSharpTests() =
    inherit GregorianBoundedBelowCalendarTests()

[<Sealed>]
type Persian2820SchemaCSharpTests() =
    inherit Persian2820SchemaTests()

//
// Geometry
//

[<Sealed>]
type BoolArrayCSharpTests() =
    inherit BoolArrayTests()

[<Sealed>]
type CodeArrayCSharpTests() =
    inherit CodeArrayTests()

[<Sealed>]
type QuasiAffineFormCSharpTests() =
    inherit QuasiAffineFormTests()

[<Sealed>]
type SliceArrayCSharpTests() =
    inherit SliceArrayTests()

[<Sealed>]
type TroeschAnalyzerCSharpTests() =
    inherit TroeschAnalyzerTests()
