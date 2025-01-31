// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Intervals.SegmentSetTests

open System

open Calendrie.Core.Intervals
open Calendrie.Testing

open Xunit
open FsCheck
open FsCheck.Xunit

module Prelude =
    [<Fact>]
    let ``Default value`` () =
        Unchecked.defaultof<SegmentSet<int>> === SegmentSet<int>.Empty

    [<Fact>]
    let ``Static property Empty`` () =
        let v = SegmentSet<int>.Empty

        v.IsEmpty |> ok
        throws<InvalidOperationException> (fun () -> v.Segment)
        v.ToString() === "[]"

module Factories =
    [<Fact>]
    let ``SegmentSet:Empty()`` () =
        let v = SegmentSet.Empty<int>()

        v.IsEmpty |> ok
        throws<InvalidOperationException> (fun () -> v.Segment)
        v.ToString() === "[]"

    [<Property>]
    let ``SegmentSet:Create() throws when max < min`` (x: Pair<int>) =
        outOfRangeExn "max" (fun () -> SegmentSet.Create(x.Max, x.Min))

    [<Property>]
    let ``SegmentSet:Create()`` (x: Pair<int>) =
        let v = SegmentSet.Create(x.Min, x.Max)
        let range = Segment.Create(x.Min, x.Max)

        v.IsEmpty |> nok
        v.Segment === range
        v.ToString() === sprintf "[%i..%i]" x.Min x.Max

        // Constructor
        let other = new SegmentSet<int>(x.Min, x.Max)
        v === other

    [<Property>]
    let ``SegmentSet:Create() when singleton`` (i: int) =
        let v = SegmentSet.Create(i, i)
        let range = Segment.Singleton(i)

        v.IsEmpty |> nok
        v.Segment === range
        v.ToString() === sprintf "[%i]" i

        // Constructor
        let other = new SegmentSet<int>(i, i)
        v === other

    [<Property>]
    let ``SegmentSet:FromEndpoints()`` (x: OrderedPair<int>) =
        let v = SegmentSet.FromEndpoints(x)
        let range = new Segment<int>(x.LowerValue, x.UpperValue)

        let isSingleton = x.LowerValue = x.UpperValue

        v.IsEmpty |> nok
        v.Segment === range
        v.ToString() ===
            if isSingleton then
                sprintf "[%i]" x.LowerValue
            else
                sprintf "[%i..%i]" x.LowerValue x.UpperValue

        // Constructor
        let other = new SegmentSet<int>(x.LowerValue, x.UpperValue)
        v === other

module Equality =
    open NonStructuralComparison

    /// Arbitrary for (x, y) where x and y are Segment<int> instances such that x <> y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! range =
            Gen.elements [ (0, 1); (1, 2); (2, 3) ]
            |> Gen.map (fun (i, j) -> new SegmentSet<int>(i, j))
        return new SegmentSet<int>(1, 1), range
    }

    // fsharplint:disable Hints
    [<Property>]
    let ``Equality when both operands are identical`` (x: SegmentSet<int>) =
        x = x
        .&. not (x <> x)
        .&. x.Equals(x)
        .&. x.Equals(x :> obj)

    [<Property>]
    let ``Equality when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
        not (x = y)
        .&. (x <> y)
        .&. not (x.Equals(y))
        .&. not (x.Equals(y :> obj))
        // Flipped
        .&. not (y = x)
        .&. (y <> x)
        .&. not (y.Equals(x))
        .&. not (y.Equals(x :> obj))

    [<Property>]
    let ``Equality when only one operand is empty`` (x: SegmentSet<int>) =
        let empty = SegmentSet<int>.Empty

        not (x = empty)
        .&. (x <> empty)
        .&. not (x.Equals(empty))
        .&. not (x.Equals(empty :> obj))
        // Flipped
        .&. not (empty = x)
        .&. (empty <> x)
        .&. not (empty.Equals(x))
        .&. not (empty.Equals(x :> obj))

    [<Fact>]
    let ``Equality when both operands are empty`` () =
        let v = SegmentSet<int>.Empty

        v = v               |> ok
        not (v <> v)        |> ok
        v.Equals(v)         |> ok
        v.Equals(v :> obj)  |> ok
    // fsharplint:enable

    [<Property>]
    let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: SegmentSet<int>) =
        not (x.Equals(null))
        .&. not (x.Equals(new obj()))

    [<Property>]
    let ``GetHashCode() is invariant`` (x: SegmentSet<int>) =
        x.GetHashCode() = x.GetHashCode()


