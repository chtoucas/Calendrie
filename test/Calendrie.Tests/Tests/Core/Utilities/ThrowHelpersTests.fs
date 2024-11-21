// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Utilities.ThrowHelpersTests

open System

open Calendrie.Testing
open Calendrie
open Calendrie.Core.Utilities

open Xunit

let private paramName = "paramName"

module MiscExns =
    [<Fact>]
    let Unreachable () =
        throws<InvalidOperationException> (fun () -> ThrowHelpers.Unreachable<string>())

module ArgumentOutOfRangeExns =
    [<Fact>]
    let YearOutOfRange () =
        outOfRangeExn "year" (fun () -> ThrowHelpers.YearOutOfRange(1L))
        outOfRangeExn "year" (fun () -> ThrowHelpers.YearOutOfRange(1))
        outOfRangeExn "year" (fun () -> ThrowHelpers.YearOutOfRange(1, null))
        outOfRangeExn paramName (fun () -> ThrowHelpers.YearOutOfRange(1, paramName))

    [<Fact>]
    let MonthOutOfRange () =
        outOfRangeExn "month" (fun () -> ThrowHelpers.MonthOutOfRange(1))
        outOfRangeExn "month" (fun () -> ThrowHelpers.MonthOutOfRange(1, null))
        outOfRangeExn paramName (fun () -> ThrowHelpers.MonthOutOfRange(1, paramName))

    [<Fact>]
    let DayOutOfRange () =
        outOfRangeExn "day" (fun () -> ThrowHelpers.DayOutOfRange(1))
        outOfRangeExn "day" (fun () -> ThrowHelpers.DayOutOfRange(1, null))
        outOfRangeExn paramName (fun () -> ThrowHelpers.DayOutOfRange(1, paramName))

    [<Fact>]
    let DayOfYearOutOfRange () =
        outOfRangeExn "dayOfYear" (fun () -> ThrowHelpers.DayOfYearOutOfRange(1))
        outOfRangeExn "dayOfYear" (fun () -> ThrowHelpers.DayOfYearOutOfRange(1, null))
        outOfRangeExn paramName (fun () -> ThrowHelpers.DayOfYearOutOfRange(1, paramName))

    [<Fact>]
    let DayOfWeekOutOfRange () =
        outOfRangeExn "dayOfWeek" (fun () -> ThrowHelpers.DayOfWeekOutOfRange(DayOfWeek.Monday, null))
        outOfRangeExn paramName (fun () -> ThrowHelpers.DayOfWeekOutOfRange(DayOfWeek.Monday, paramName))

    [<Fact>]
    let IsoWeekdayOutOfRange () =
        outOfRangeExn "weekday" (fun () -> ThrowHelpers.IsoWeekdayOutOfRange(IsoWeekday.Monday, null))
        outOfRangeExn paramName (fun () -> ThrowHelpers.IsoWeekdayOutOfRange(IsoWeekday.Monday, paramName))

    [<Fact>]
    let AdditionRuleOutOfRange () =
        outOfRangeExn "rule" (fun () -> ThrowHelpers.AdditionRuleOutOfRange(AdditionRule.Truncate, null))
        outOfRangeExn paramName (fun () -> ThrowHelpers.AdditionRuleOutOfRange(AdditionRule.Truncate, paramName))

module ArgumentExns =
    [<Fact>]
    let BadBinaryInput () =
        argExn "data" (fun () -> ThrowHelpers.BadBinaryInput())

    [<Fact>]
    let NonComparable () =
        argExn "obj" (fun () -> ThrowHelpers.NonComparable(typeof<string>, 1))

module OverflowExns =
    [<Fact>]
    let DateOverflow () =
        (fun () -> ThrowHelpers.DateOverflow()) |> overflows
        (fun () -> ThrowHelpers.DateOverflow<string>()) |> overflows

    [<Fact>]
    let MonthOverflow () =
        (fun () -> ThrowHelpers.MonthOverflow()) |> overflows

    [<Fact>]
    let DayNumberOverflow () =
        (fun () -> ThrowHelpers.DayNumberOverflow()) |> overflows
        (fun () -> ThrowHelpers.DayNumberOverflow<string>()) |> overflows

    [<Fact>]
    let OrdOverflow () =
        (fun () -> ThrowHelpers.OrdOverflow<string>()) |> overflows
