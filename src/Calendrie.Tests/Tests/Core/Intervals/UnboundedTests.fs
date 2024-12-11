// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Intervals.UnboundedTests

open System

open Calendrie.Core.Intervals
open Calendrie.Testing

open Xunit
open FsCheck.Xunit

module Prelude =
    [<Fact>]
    let ``Property IsLeftOpen is always true`` () =
        Unbounded<int>.Instance.IsLeftOpen |> ok

    [<Fact>]
    let ``Property IsRightOpen is always true`` () =
        Unbounded<int>.Instance.IsRightOpen |> ok

    [<Fact>]
    let ``Property IsLeftBounded is always false`` () =
        Unbounded<int>.Instance.IsLeftBounded |> nok

    [<Fact>]
    let ``Property IsRightBounded is always false`` () =
        Unbounded<int>.Instance.IsRightBounded |> nok

    [<Fact>]
    let ``ToString()`` () =
        Unbounded<int>.Instance.ToString() === IntervalFormat.Unbounded

module SetOperations =
    //
    // Membership
    //

    [<Fact>]
    let ``Contains() returns true at Int32:Min/MaxValue`` () =
        let x = Unbounded<int>.Instance

        x.Contains(Int32.MinValue)  |> ok
        x.Contains(Int32.MaxValue)  |> ok

    [<Property>]
    let ``Contains() always returns true`` (i: int) =
        Unbounded<int>.Instance.Contains(i) |> ok

    //
    // Set inclusion
    //

    [<Fact>]
    let ``Set inclusion throws for null`` () =
        let x = Unbounded<int>.Instance

        nullExn "other" (fun () -> x.IsSubsetOf(null))
        //nullExn "other" (fun () -> x.IsSupersetOf(null))
        //nullExn "other" (fun () -> x.IsProperSubsetOf(null))
        nullExn "other" (fun () -> x.IsProperSupersetOf(null))

    [<Fact>]
    let ``Set inclusion with itsef`` () =
        let x = Unbounded<int>.Instance

        x.IsSubsetOf(x)         |> ok
        //x.IsSupersetOf(x)       |> ok
        //x.IsProperSubsetOf(x)   |> nok
        x.IsProperSupersetOf(x) |> nok

    [<Property>]
    let ``Set inclusion with a lower ray`` (y: LowerRay<int>) =
        let x = Unbounded<int>.Instance

        x.IsSubsetOf(y)         |> nok
        //x.IsSupersetOf(y)       |> ok
        //x.IsProperSubsetOf(y)   |> nok
        x.IsProperSupersetOf(y) |> ok

    [<Property>]
    let ``Set inclusion with an upper ray`` (y: UpperRay<int>) =
        let x = Unbounded<int>.Instance

        x.IsSubsetOf(y)         |> nok
        //x.IsSupersetOf(y)       |> ok
        //x.IsProperSubsetOf(y)   |> nok
        x.IsProperSupersetOf(y) |> ok

    [<Property>]
    let ``Set inclusion with a range`` (y: Range<int>) =
        let x = Unbounded<int>.Instance

        x.IsSubsetOf(y)         |> nok
        //x.IsSupersetOf(y)       |> ok
        //x.IsProperSubsetOf(y)   |> nok
        x.IsProperSupersetOf(y) |> ok

    //
    // Set equality
    //

    [<Fact>]
    let ``SetEquals() throws for null`` () =
        let x = Unbounded<int>.Instance

        nullExn "other" (fun () -> x.SetEquals(null))

    [<Property>]
    let ``SetEquals() with itself`` () =
        let x = Unbounded<int>.Instance

        x.SetEquals(x)

    [<Property>]
    let ``SetEquals() with a lower ray`` (y: LowerRay<int>) =
        let x = Unbounded<int>.Instance

        not (x.SetEquals(y))

    [<Property>]
    let ``SetEquals() with an upper ray`` (y: UpperRay<int>) =
        let x = Unbounded<int>.Instance

        not (x.SetEquals(y))

    [<Property>]
    let ``SetEquals() with a range`` (y: Range<int>) =
        let x = Unbounded<int>.Instance

        not (x.SetEquals(y))
