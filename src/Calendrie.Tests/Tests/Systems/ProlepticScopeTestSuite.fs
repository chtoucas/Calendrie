// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.ProlepticScopeTestSuite

open Calendrie.Core.Schemas
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Systems

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type GregorianTests() =
    inherit ProlepticScopeFacts<GregorianDataSet>(new GregorianScope(new GregorianSchema()))

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type JulianTests() =
    inherit ProlepticScopeFacts<JulianDataSet>(new JulianScope(new JulianSchema()))

