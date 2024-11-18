// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Utilities.ReadOnlySetTests

open System.Collections
open System.Collections.Generic
open System.Linq

open Calendrie.Testing

open Calendrie.Core.Utilities

open Xunit
open FsCheck.Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "set" is null`` () =
        nullExn "set" (fun () -> new ReadOnlySet<string>(null))

    [<Fact>]
    let ``Constructor throws when "collection" is null`` () =
        nullExn "collection" (fun () -> new ReadOnlySet<string>(null :> IEnumerable<string>))

    [<Property>]
    let ``Constructor using a seq`` (xs: int list) =
        let set = new ReadOnlySet<int>(xs)

        set.SetEquals(xs) |> ok

    [<Property>]
    let ``Constructor using an hashset`` (xs: int list) =
        let hashset = new HashSet<int>(xs)
        let set = new ReadOnlySet<int>(hashset)

        set.SetEquals(hashset) |> ok

    [<Property>]
    let ``Property Count`` (xs: int list) =
        let hashset = new HashSet<int>(xs)
        let set = new ReadOnlySet<int>(hashset)
        let count = hashset.Count

        set.Count = count

module Enumeration =
    [<Fact>]
    let ``GetEnumerator()`` () =
        let values = [| 1; 3; 2 |]
        let set = new ReadOnlySet<int>(values) :> IEnumerable<int>

        let mutable i = 0
        for n in set do
            n === values[i]
            i <- i + 1

    [<Property>]
    let ``GetEnumerator() generic`` (xs: int list) =
        let hashset = new HashSet<int>(xs)
        let set = new ReadOnlySet<int>(hashset) :> IEnumerable<int>
        let seq = hashset :> IEnumerable<int>

        // We use the Xunit assertion to verify the equality of sequences.
        set === seq

    [<Property>]
    let ``GetEnumerator() non-generic`` (xs: int list) =
        let hashset = new HashSet<int>(xs)
        let set = new ReadOnlySet<int>(hashset) :> IEnumerable
        let seq = hashset :> IEnumerable

        // We use the Xunit assertion to verify the equality of sequences.
        set === seq
