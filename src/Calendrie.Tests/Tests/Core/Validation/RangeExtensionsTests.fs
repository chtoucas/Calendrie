// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Validation.RangeExtensionsTests

open Calendrie.Core.Intervals
open Calendrie.Core.Validation
open Calendrie.Testing

open Xunit

// TODO(code): pourquoi ça ne marche en tant que méthodes d'extension ?

[<Fact>]
let ``Validate()`` () =
    let range : Range<int> = Range.Create(0, 2)

    outOfRangeExn "paramName" (fun () -> RangeExtensions.Validate(range, -1, "paramName"))
    outOfRangeExn "value" (fun () -> RangeExtensions.Validate(range, -1))
    RangeExtensions.Validate(range, 0)
    RangeExtensions.Validate(range, 1)
    RangeExtensions.Validate(range, 2)
    outOfRangeExn "value" (fun () -> RangeExtensions.Validate(range, 3))
    outOfRangeExn "paramName" (fun () -> RangeExtensions.Validate(range, 3, "paramName"))

