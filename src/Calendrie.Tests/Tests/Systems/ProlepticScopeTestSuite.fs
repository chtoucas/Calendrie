// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.ProlepticScopeTestSuite

open Calendrie.Core.Schemas
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Data.Scopes
open Calendrie.Testing.Facts.Hemerology

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianTests() =
    inherit CalendarScopeFacts<GregorianScope, GregorianDataSet, GJScopeDataSet>(
        new GregorianScope(new GregorianSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type JulianTests() =
    inherit CalendarScopeFacts<JulianScope, JulianDataSet, GJScopeDataSet>(
        new JulianScope(new JulianSchema()))
