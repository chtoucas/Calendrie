// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.PartsAdapterTests

open Calendrie.Core
open Calendrie.Core.Schemas
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core

open Xunit

module Bundles =
    [<Sealed>]
    type PartsAdapterTests() =
        inherit PartsAdapterFacts<GregorianDataSet>(
            new PartsAdapter(new GregorianSchema()))

module Prelude =
    [<Fact>]
    let ``Constructor throws for null schema`` () =
        nullExn "schema" (fun () -> new PartsAdapter(null))

