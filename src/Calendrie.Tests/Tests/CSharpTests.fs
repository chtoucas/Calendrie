// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.CSharpTests

open Calendrie.Testing.CSharpTests

[<Sealed>]
type GregorianMinMaxYearCalendarCSharpTests() =
    inherit GregorianMinMaxYearCalendarTests()

[<Sealed>]
type BoundedBelowScopeCSharpTests() =
    inherit BoundedBelowScopeTests()

[<Sealed>]
type GregorianBoundedBelowCalendarCSharpTests() =
    inherit GregorianBoundedBelowCalendarTests()
