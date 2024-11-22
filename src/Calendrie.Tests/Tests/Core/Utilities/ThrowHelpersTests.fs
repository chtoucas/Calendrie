// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Utilities.ThrowHelpersTests

open Calendrie.Testing
open Calendrie.Core.Utilities

open Xunit

module ArgumentOutOfRangeExns =
    [<Fact>]
    let ThrowYearOutOfRange () =
        outOfRangeExn "year" (fun () -> ThrowHelpers.ThrowYearOutOfRange(1))
        outOfRangeExn "year" (fun () -> ThrowHelpers.ThrowYearOutOfRange(1, null))
        outOfRangeExn "paramName" (fun () -> ThrowHelpers.ThrowYearOutOfRange(1, "paramName"))

    [<Fact>]
    let ThrowMonthOutOfRange () =
        outOfRangeExn "month" (fun () -> ThrowHelpers.ThrowMonthOutOfRange(1))
        outOfRangeExn "month" (fun () -> ThrowHelpers.ThrowMonthOutOfRange(1, null))
        outOfRangeExn "paramName" (fun () -> ThrowHelpers.ThrowMonthOutOfRange(1, "paramName"))

    [<Fact>]
    let ThrowDayOutOfRange () =
        outOfRangeExn "day" (fun () -> ThrowHelpers.ThrowDayOutOfRange(1))
        outOfRangeExn "day" (fun () -> ThrowHelpers.ThrowDayOutOfRange(1, null))
        outOfRangeExn "paramName" (fun () -> ThrowHelpers.ThrowDayOutOfRange(1, "paramName"))

    [<Fact>]
    let ThrowDayOfYearOutOfRange () =
        outOfRangeExn "dayOfYear" (fun () -> ThrowHelpers.ThrowDayOfYearOutOfRange(1))
        outOfRangeExn "dayOfYear" (fun () -> ThrowHelpers.ThrowDayOfYearOutOfRange(1, null))
        outOfRangeExn "paramName" (fun () -> ThrowHelpers.ThrowDayOfYearOutOfRange(1, "paramName"))

module ArgumentExns =
    [<Fact>]
    let ThrowBadBinaryInput () =
        argExn "data" (fun () -> ThrowHelpers.ThrowBadBinaryInput())

    [<Fact>]
    let ThrowNonComparable () =
        argExn "obj" (fun () -> ThrowHelpers.ThrowNonComparable(typeof<string>, 1))

module OverflowExns =
    [<Fact>]
    let ThrowDateOverflow () =
        (fun () -> ThrowHelpers.ThrowDateOverflow()) |> overflows

    [<Fact>]
    let ThrowMonthOverflow () =
        (fun () -> ThrowHelpers.ThrowMonthOverflow()) |> overflows

    [<Fact>]
    let ThrowDayNumberOverflow () =
        (fun () -> ThrowHelpers.ThrowDayNumberOverflow()) |> overflows

    [<Fact>]
    let ThrowOrdOverflow () =
        (fun () -> ThrowHelpers.ThrowOrdOverflow()) |> overflows
