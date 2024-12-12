// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.StandardYearsValidatorTests

open System

open Calendrie.Core.Intervals
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Facts.Systems

open Xunit

let validYearData = StandardScopeFacts.ValidYearData
let invalidYearData = StandardScopeFacts.InvalidYearData

let private validator = new StandardYearsValidator()

[<Fact>]
let ``Property Range`` () =
    validator.Range === Range.Create(StandardScope.MinYear, StandardScope.MaxYear)

[<Theory; MemberData(nameof(invalidYearData))>]
let ``Validate() throws when "year" is out of range`` y =
    outOfRangeExn "year" (fun () -> validator.Validate(y))
    outOfRangeExn "y" (fun () -> validator.Validate(y, nameof(y)))

[<Theory; MemberData(nameof(validYearData))>]
let ``Validate() does not throw when the input is valid`` y =
    validator.Validate(y)
    validator.Validate(y, nameof(y))

[<Theory; MemberData(nameof(invalidYearData))>]
let ``CheckOverflow() overflows when "year" is out of range`` y =
    (fun () -> validator.CheckOverflow(y)) |> overflows

[<Theory; MemberData(nameof(validYearData))>]
let ``CheckOverflow() does not overflow for valid years`` y =
    validator.CheckOverflow(y)

[<Fact>]
let ``CheckLowerBound() overflows when "year" is out of range`` () =
    (fun () -> validator.CheckLowerBound(Int32.MinValue)) |> overflows
    (fun () -> validator.CheckLowerBound(StandardScope.MinYear - 1)) |> overflows

[<Fact>]
let ``CheckLowerBound() does not overflow for valid years`` () =
    validator.CheckLowerBound(StandardScope.MinYear)
    validator.CheckLowerBound(StandardScope.MaxYear)
    validator.CheckLowerBound(StandardScope.MaxYear + 1)
    validator.CheckLowerBound(Int32.MaxValue)

[<Fact>]
let ``CheckUpperBound() overflows when "year" is out of range`` () =
    (fun () -> validator.CheckUpperBound(StandardScope.MaxYear + 1)) |> overflows
    (fun () -> validator.CheckUpperBound(Int32.MaxValue)) |> overflows

[<Fact>]
let ``CheckUpperBound() does not overflow for valid years`` () =
    validator.CheckUpperBound(Int32.MinValue)
    validator.CheckUpperBound(StandardScope.MinYear - 1)
    validator.CheckUpperBound(StandardScope.MinYear)
    validator.CheckUpperBound(StandardScope.MaxYear)
