// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Utilities.ThrowHelpersTests

open Calendrie
open Calendrie.Core.Utilities
open Calendrie.Testing

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
    let ThrowWeekOfYearOutOfRange () =
        outOfRangeExn "weekOfYear" (fun () -> ThrowHelpers.ThrowWeekOfYearOutOfRange(1))
        outOfRangeExn "weekOfYear" (fun () -> ThrowHelpers.ThrowWeekOfYearOutOfRange(1, null))
        outOfRangeExn "paramName" (fun () -> ThrowHelpers.ThrowWeekOfYearOutOfRange(1, "paramName"))

    [<Fact>]
    let ThrowDayOfYearOutOfRange () =
        outOfRangeExn "dayOfYear" (fun () -> ThrowHelpers.ThrowDayOfYearOutOfRange(1))
        outOfRangeExn "dayOfYear" (fun () -> ThrowHelpers.ThrowDayOfYearOutOfRange(1, null))
        outOfRangeExn "paramName" (fun () -> ThrowHelpers.ThrowDayOfYearOutOfRange(1, "paramName"))

    [<Fact>]
    let ThrowDayNumberOutOfRange () =
        outOfRangeExn "dayNumber" (fun () -> ThrowHelpers.ThrowDayNumberOutOfRange(DayNumber.Zero))
        outOfRangeExn "dayNumber" (fun () -> ThrowHelpers.ThrowDayNumberOutOfRange(DayNumber.Zero, null))
        outOfRangeExn "paramName" (fun () -> ThrowHelpers.ThrowDayNumberOutOfRange(DayNumber.Zero, "paramName"))

    [<Fact>]
    let ThrowRankOutOfRange () =
        outOfRangeExn "rank" (fun () -> ThrowHelpers.ThrowRankOutOfRange(1))
        outOfRangeExn "rank" (fun () -> ThrowHelpers.ThrowRankOutOfRange(1, null))
        outOfRangeExn "paramName" (fun () -> ThrowHelpers.ThrowRankOutOfRange(1, "paramName"))

    [<Fact>]
    let ThrowDaysSinceEpochOutOfRange () =
        outOfRangeExn "daysSinceEpoch" (fun () -> ThrowHelpers.ThrowDaysSinceEpochOutOfRange(1))
        outOfRangeExn "daysSinceEpoch" (fun () -> ThrowHelpers.ThrowDaysSinceEpochOutOfRange(1, null))
        outOfRangeExn "paramName" (fun () -> ThrowHelpers.ThrowDaysSinceEpochOutOfRange(1, "paramName"))

    [<Fact>]
    let ThrowMonthsSinceEpochOutOfRange () =
        outOfRangeExn "monthsSinceEpoch" (fun () -> ThrowHelpers.ThrowMonthsSinceEpochOutOfRange(1))
        outOfRangeExn "monthsSinceEpoch" (fun () -> ThrowHelpers.ThrowMonthsSinceEpochOutOfRange(1, null))
        outOfRangeExn "paramName" (fun () -> ThrowHelpers.ThrowMonthsSinceEpochOutOfRange(1, "paramName"))

module ArgumentExns =
    [<Fact>]
    let ThrowBadBinaryInput () =
        argExn "data" (fun () -> ThrowHelpers.ThrowBadBinaryInput())
        argExn "paramName" (fun () -> ThrowHelpers.ThrowBadBinaryInput("paramName"))

    [<Fact>]
    let ThrowNonComparable () =
        argExn "obj" (fun () -> ThrowHelpers.ThrowNonComparable(typeof<string>, 1))
        argExn "paramName" (fun () -> ThrowHelpers.ThrowNonComparable(typeof<string>, 1, "paramName"))

module OverflowExns =
    [<Fact>]
    let ThrowDateOverflow () =
        (fun () -> ThrowHelpers.ThrowDateOverflow()) |> overflows

    [<Fact>]
    let ThrowMonthOverflow () =
        (fun () -> ThrowHelpers.ThrowMonthOverflow()) |> overflows

    [<Fact>]
    let ThrowYearOverflow () =
        (fun () -> ThrowHelpers.ThrowYearOverflow()) |> overflows

    [<Fact>]
    let ThrowDayNumberOverflow () =
        (fun () -> ThrowHelpers.ThrowDayNumberOverflow()) |> overflows

    [<Fact>]
    let ThrowOrdOverflow () =
        (fun () -> ThrowHelpers.ThrowOrdOverflow()) |> overflows
