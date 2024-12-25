// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.CSharpTests

open Calendrie.Testing
open Calendrie.Testing.CSharpTests

[<Sealed>]
type CSharpOnlyCSharpTests() =
    inherit CSharpOnlyTests()

[<Sealed>]
type ApiCSharpTests() =
    inherit ApiTests()

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianMinMaxYearCalendarCSharpTests() =
    inherit GregorianMinMaxYearCalendarTests()

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type BoundedBelowScopeCSharpTests() =
    inherit BoundedBelowScopeTests()

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianBoundedBelowCalendarCSharpTests() =
    inherit GregorianBoundedBelowCalendarTests()
