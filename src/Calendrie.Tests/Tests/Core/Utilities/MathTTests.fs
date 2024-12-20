﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Utilities.MathTTests

open Calendrie.Core.Utilities
open Calendrie.Testing

open Xunit

[<Fact>]
let ``Min()`` () =
    MathT.Min(1, 1) === 1
    MathT.Min(2, 2) === 2
    MathT.Min(1, 2) === 1
    MathT.Min(2, 1) === 1

[<Fact>]
let ``Max()`` () =
    MathT.Max(1, 1) === 1
    MathT.Max(2, 2) === 2
    MathT.Max(1, 2) === 2
    MathT.Max(2, 1) === 2
