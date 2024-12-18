﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.PartsTests

open System

open Calendrie
open Calendrie.Testing

open Xunit
open FsCheck
open FsCheck.Xunit

module DateParts =
    /// Arbitrary for (x, y) where x and y are DateParts instances such that x <> y.
    /// Notice that x < y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! parts =
            Gen.elements [
                // One distinct element.
                (2, 1, 1); (1, 2, 1); (1, 1, 2);
                // Two distinct elements.
                (2, 2, 1); (1, 2, 2); (2, 1, 2);
                // Three distinct elements.
                (2, 2, 2) ]
            |> Gen.map (fun (y, m, d) -> new DateParts(y, m, d))
        return new DateParts(1, 1, 1), parts
    }

    module Prelude =
        [<Fact>]
        let ``Default value of DateParts is (0, 0, 0)`` () =
            let parts = Unchecked.defaultof<DateParts>

            (parts.Year, parts.Month, parts.Day) === (0, 0, 0)

        [<Property>]
        let Constructor y m d =
            let parts = new DateParts(y, m, d)

            (parts.Year, parts.Month, parts.Day) = (y, m, d)

        [<Property>]
        let Deconstructor y m d =
            let parts = new DateParts(y, m, d)
            let a, b, c = parts.Deconstruct()

            (a, b, c) = (y, m, d)

        [<Property>]
        let ``AtStartOfYear()`` y =
            let parts = new DateParts(y, 1, 1)

            DateParts.AtStartOfYear(y) = parts

        [<Property>]
        let ``AtStartOfMonth()`` y m =
            let parts = new DateParts(y, m, 1)

            DateParts.AtStartOfMonth(y, m) = parts

    module Equality =
        open NonStructuralComparison

        [<Property>]
        let ``Equality when both operands are identical`` (x: DateParts) =
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
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: DateParts) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: DateParts) =
            x.GetHashCode() = x.GetHashCode()

    module Comparison =
        open NonStructuralComparison

        [<Property>]
        let ``Comparisons when both operands are identical`` (x: DateParts) =
            not (x > x)
            .&. not (x < x)
            .&. (x >= x)
            .&. (x <= x)

        [<Property>]
        let ``Comparisons when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x > y)
            .&. not (x >= y)
            .&. (x < y)
            .&. (x <= y)
            // Flipped
            .&. (y > x)
            .&. (y >= x)
            .&. not (y < x)
            .&. not (y <= x)

        //
        // CompareTo()
        //

        [<Property>]
        let ``CompareTo() returns 0 when both operands are identical`` (x: DateParts) =
            (x.CompareTo(x) = 0)
            .&. ((x :> IComparable).CompareTo(x) = 0)

        [<Property>]
        let ``CompareTo() when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            (x.CompareTo(y) <= 0)
            .&. ((x :> IComparable).CompareTo(y) <= 0)
            // Flipped
            .&. (y.CompareTo(x) >= 0)
            .&. ((y :> IComparable).CompareTo(x) >= 0)

        [<Property>]
        let ``CompareTo(obj) returns 1 when "obj" is null`` (x: DateParts) =
             (x :> IComparable).CompareTo(null) = 1

        [<Property>]
        let ``CompareTo(obj) throws when "obj" is a plain object`` (x: DateParts) =
            argExn "obj" (fun () -> (x :> IComparable).CompareTo(new obj()))

module MonthParts =
    /// Arbitrary for (x, y) where x and y are MonthParts instances such that x <> y.
    /// Notice that x < y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! parts =
            Gen.elements [ (2, 1); (1, 2); (2, 2) ]
            |> Gen.map (fun (y, m) -> new MonthParts(y, m))
        return new MonthParts(1, 1), parts
    }

    module Prelude =
        [<Fact>]
        let ``Default value of MonthParts is (0, 0)`` () =
            let parts = Unchecked.defaultof<MonthParts>

            (parts.Year, parts.Month) === (0, 0)

        [<Property>]
        let Constructor y m =
            let parts = new MonthParts(y, m)

            (parts.Year, parts.Month) = (y, m)

        [<Property>]
        let Deconstructor y m =
            let parts = new MonthParts(y, m)
            let a, b = parts.Deconstruct()

            (a, b) = (y, m)

        [<Property>]
        let ``AtStartOfYear()`` y =
            let parts = new MonthParts(y, 1)

            MonthParts.AtStartOfYear(y) = parts

    module Equality =
        open NonStructuralComparison

        [<Property>]
        let ``Equality when both operands are identical`` (x: MonthParts) =
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
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: MonthParts) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: MonthParts) =
            x.GetHashCode() = x.GetHashCode()

    module Comparison =
        open NonStructuralComparison

        [<Property>]
        let ``Comparisons when both operands are identical`` (x: MonthParts) =
            not (x > x)
            .&. not (x < x)
            .&. (x >= x)
            .&. (x <= x)

        [<Property>]
        let ``Comparisons when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x > y)
            .&. not (x >= y)
            .&. (x < y)
            .&. (x <= y)
            // Flipped
            .&. (y > x)
            .&. (y >= x)
            .&. not (y < x)
            .&. not (y <= x)

        //
        // CompareTo()
        //

        [<Property>]
        let ``CompareTo() returns 0 when both operands are identical`` (x: MonthParts) =
            (x.CompareTo(x) = 0)
            .&. ((x :> IComparable).CompareTo(x) = 0)

        [<Property>]
        let ``CompareTo() when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            (x.CompareTo(y) <= 0)
            .&. ((x :> IComparable).CompareTo(y) <= 0)
            // Flipped
            .&. (y.CompareTo(x) >= 0)
            .&. ((y :> IComparable).CompareTo(x) >= 0)

        [<Property>]
        let ``CompareTo(obj) returns 1 when "obj" is null`` (x: MonthParts) =
             (x :> IComparable).CompareTo(null) = 1

        [<Property>]
        let ``CompareTo(obj) throws when "obj" is a plain object`` (x: MonthParts) =
            argExn "obj" (fun () -> (x :> IComparable).CompareTo(new obj()))

module OrdinalParts =
    /// Arbitrary for (x, y) where x and y are OrdinalParts instances such that x <> y.
    /// Notice that x < y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! parts =
            Gen.elements [ (2, 1); (1, 2); (2, 2) ]
            |> Gen.map (fun (y, doy) -> new OrdinalParts(y, doy))
        return new OrdinalParts(1, 1), parts
    }

    module Prelude =
        [<Fact>]
        let ``Default value of OrdinalParts is (0, 0)`` () =
            let parts = Unchecked.defaultof<OrdinalParts>

            (parts.Year, parts.DayOfYear) === (0, 0)

        [<Property>]
        let Constructor y doy =
            let parts = new OrdinalParts(y, doy)

            (parts.Year, parts.DayOfYear) = (y, doy)

        [<Property>]
        let Deconstructor y doy =
            let parts = new OrdinalParts(y, doy)
            let a, b = parts.Deconstruct()

            (a, b) = (y, doy)

        [<Property>]
        let ``AtStartOfYear()`` y =
            let parts = new OrdinalParts(y, 1)

            OrdinalParts.AtStartOfYear(y) = parts

    module Equality =
        open NonStructuralComparison

        [<Property>]
        let ``Equality when both operands are identical`` (x: OrdinalParts) =
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
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: OrdinalParts) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: OrdinalParts) =
            x.GetHashCode() = x.GetHashCode()

    module Comparison =
        open NonStructuralComparison

        [<Property>]
        let ``Comparisons when both operands are identical`` (x: OrdinalParts) =
            not (x > x)
            .&. not (x < x)
            .&. (x >= x)
            .&. (x <= x)

        [<Property>]
        let ``Comparisons when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x > y)
            .&. not (x >= y)
            .&. (x < y)
            .&. (x <= y)
            // Flipped
            .&. (y > x)
            .&. (y >= x)
            .&. not (y < x)
            .&. not (y <= x)

        //
        // CompareTo()
        //

        [<Property>]
        let ``CompareTo() returns 0 when both operands are identical`` (x: OrdinalParts) =
            (x.CompareTo(x) = 0)
            .&. ((x :> IComparable).CompareTo(x) = 0)

        [<Property>]
        let ``CompareTo() when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            (x.CompareTo(y) <= 0)
            .&. ((x :> IComparable).CompareTo(y) <= 0)
            // Flipped
            .&. (y.CompareTo(x) >= 0)
            .&. ((y :> IComparable).CompareTo(x) >= 0)

        [<Property>]
        let ``CompareTo(obj) returns 1 when "obj" is null`` (x: OrdinalParts) =
             (x :> IComparable).CompareTo(null) = 1

        [<Property>]
        let ``CompareTo(obj) throws when "obj" is a plain object`` (x: OrdinalParts) =
            argExn "obj" (fun () -> (x :> IComparable).CompareTo(new obj()))
