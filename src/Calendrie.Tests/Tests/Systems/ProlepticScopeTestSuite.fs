// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.ProlepticScopeTestSuite

open Calendrie.Core.Schemas
open Calendrie.Systems
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Systems

[<Sealed>]
type GregorianTests() =
    inherit ProlepticScopeFacts<GregorianDataSet>(new GregorianScope(new GregorianSchema()))

[<Sealed>]
type JulianTests() =
    inherit ProlepticScopeFacts<JulianDataSet>(new JulianScope(new JulianSchema()))

