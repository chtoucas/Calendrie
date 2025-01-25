// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.MonthDifferenceTests

open System

open Calendrie.Hemerology
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Static property Zero`` () =
        let zero = MonthDifference.Zero
        // Act & Assert
        zero === MonthDifference.UnsafeCreate(0, 0)
        zero.Years  === 0
        zero.Months === 0

        MonthDifference.Abs(zero) === zero
        MonthDifference.IsZero(zero)     |> ok
        MonthDifference.IsPositive(zero) |> ok
        MonthDifference.IsNegative(zero) |> ok

    [<Theory>]
    [<InlineData(1, 2, 1)>]
    [<InlineData(2, 1, 1)>]
    [<InlineData(-1, -2, -1)>]
    [<InlineData(-2, -1, -1)>]
    let ``Instance properties`` (years, months, sign) =
        let x = MonthDifference.UnsafeCreate(years, months)
        // Act & Assert
        x.Years  === years
        x.Months === months

        MonthDifference.IsZero(x) |> nok
        MonthDifference.IsPositive(x) === (sign >= 0)
        MonthDifference.IsNegative(x) === (sign <= 0)

    [<Theory>]
    [<InlineData(1, 2)>]
    [<InlineData(2, 1)>]
    [<InlineData(-1, -2)>]
    [<InlineData(-2, -1)>]
    let Deconstructor (years, months) =
        let x = MonthDifference.UnsafeCreate(years, months)
        // Act & Assert
        x.Deconstruct() === (years, months)

module Factories =
    [<Fact>]
    let ``CreatePositive(0, 0) returns Zero `` () =
        MonthDifference.CreatePositive(0, 0) === MonthDifference.Zero

    [<Fact>]
    let ``CreatePositive() throws when years or months < 0 `` () =
        outOfRangeExn "years"  (fun () -> MonthDifference.CreatePositive(-1, 2))
        outOfRangeExn "months" (fun () -> MonthDifference.CreatePositive(1, -2))

    [<Fact>]
    let ``CreatePositive()`` () =
        // Act
        let x = MonthDifference.CreatePositive(1, 2)
        // Assert
        x === MonthDifference.UnsafeCreate(1, 2)
        x.Years  === 1
        x.Months === 2

        MonthDifference.Abs(x) === x
        MonthDifference.IsZero(x)     |> nok
        MonthDifference.IsPositive(x) |> ok
        MonthDifference.IsNegative(x) |> nok

    [<Fact>]
    let ``CreateNegative(0, 0) returns Zero `` () =
        MonthDifference.CreatePositive(0, 0) === MonthDifference.Zero

    [<Fact>]
    let ``CreateNegative() throws when years or months < 0 `` () =
        outOfRangeExn "years"  (fun () -> MonthDifference.CreateNegative(-1, 2))
        outOfRangeExn "months" (fun () -> MonthDifference.CreateNegative(1, -2))

    [<Fact>]
    let ``CreateNegative()`` () =
        // Act
        let x = MonthDifference.CreateNegative(1, 2)
        // Assert
        x === MonthDifference.UnsafeCreate(-1, -2)
        x.Years  === -1
        x.Months === -2

        MonthDifference.Abs(x) === -x
        MonthDifference.IsZero(x)     |> nok
        MonthDifference.IsPositive(x) |> nok
        MonthDifference.IsNegative(x) |> ok

module Comparison =
    open NonStructuralComparison

    [<Fact>]
    let ``Zero w/ anything`` () =
        // Zero / Zero
        MonthDifference.Zero >  MonthDifference.Zero |> nok
        MonthDifference.Zero >= MonthDifference.Zero |> ok
        MonthDifference.Zero <  MonthDifference.Zero |> nok
        MonthDifference.Zero <= MonthDifference.Zero |> ok
        // Zero / Positive
        MonthDifference.CreatePositive(1, 2) >  MonthDifference.Zero |> ok
        MonthDifference.CreatePositive(1, 2) >= MonthDifference.Zero |> ok
        MonthDifference.CreatePositive(1, 2) <  MonthDifference.Zero |> nok
        MonthDifference.CreatePositive(1, 2) <= MonthDifference.Zero |> nok
        // Zero / Negative
        MonthDifference.CreateNegative(1, 2) >  MonthDifference.Zero |> ok
        MonthDifference.CreateNegative(1, 2) >= MonthDifference.Zero |> ok
        MonthDifference.CreateNegative(1, 2) <  MonthDifference.Zero |> nok
        MonthDifference.CreateNegative(1, 2) <= MonthDifference.Zero |> nok

    [<Fact>]
    let ``Positive or Negative, same length`` () =
        // Positive / Positive
        MonthDifference.CreatePositive(1, 2) >  MonthDifference.CreatePositive(1, 2) |> nok
        MonthDifference.CreatePositive(1, 2) >= MonthDifference.CreatePositive(1, 2) |> ok
        MonthDifference.CreatePositive(1, 2) <  MonthDifference.CreatePositive(1, 2) |> nok
        MonthDifference.CreatePositive(1, 2) <= MonthDifference.CreatePositive(1, 2) |> ok
        // Positive / Negative
        MonthDifference.CreatePositive(1, 2) >  MonthDifference.CreateNegative(1, 2) |> nok
        MonthDifference.CreatePositive(1, 2) >= MonthDifference.CreateNegative(1, 2) |> ok
        MonthDifference.CreatePositive(1, 2) <  MonthDifference.CreateNegative(1, 2) |> nok
        MonthDifference.CreatePositive(1, 2) <= MonthDifference.CreateNegative(1, 2) |> ok
        // Negative / Negative
        MonthDifference.CreateNegative(1, 2) >  MonthDifference.CreateNegative(1, 2) |> nok
        MonthDifference.CreateNegative(1, 2) >= MonthDifference.CreateNegative(1, 2) |> ok
        MonthDifference.CreateNegative(1, 2) <  MonthDifference.CreateNegative(1, 2) |> nok
        MonthDifference.CreateNegative(1, 2) <= MonthDifference.CreateNegative(1, 2) |> ok

    [<Fact>]
    let ``Positive or Negative, diff length`` () =
        // Positive / Positive
        MonthDifference.CreatePositive(1, 3) >  MonthDifference.CreatePositive(1, 2) |> ok
        MonthDifference.CreatePositive(1, 3) >= MonthDifference.CreatePositive(1, 2) |> ok
        MonthDifference.CreatePositive(1, 3) <  MonthDifference.CreatePositive(1, 2) |> nok
        MonthDifference.CreatePositive(1, 3) <= MonthDifference.CreatePositive(1, 2) |> nok
        // Positive / Negative
        MonthDifference.CreatePositive(1, 3) >  MonthDifference.CreateNegative(1, 2) |> ok
        MonthDifference.CreatePositive(1, 3) >= MonthDifference.CreateNegative(1, 2) |> ok
        MonthDifference.CreatePositive(1, 3) <  MonthDifference.CreateNegative(1, 2) |> nok
        MonthDifference.CreatePositive(1, 3) <= MonthDifference.CreateNegative(1, 2) |> nok
        // Negative / Negative
        MonthDifference.CreateNegative(1, 3) >  MonthDifference.CreateNegative(1, 2) |> ok
        MonthDifference.CreateNegative(1, 3) >= MonthDifference.CreateNegative(1, 2) |> ok
        MonthDifference.CreateNegative(1, 3) <  MonthDifference.CreateNegative(1, 2) |> nok
        MonthDifference.CreateNegative(1, 3) <= MonthDifference.CreateNegative(1, 2) |> nok

    //
    // CompareTo()
    //

    [<Theory>]
    [<InlineData(1, 2)>]
    [<InlineData(2, 1)>]
    let ``CompareTo() returns 0 when both objects are identical`` (years, months) =
        let x = MonthDifference.CreatePositive(years, months)
        // Act & Assert
        x.CompareTo(x) === 0
        (x :> IComparable).CompareTo(x) === 0

    [<Fact>]
    let ``CompareTo() when both objects are distinct`` () =
        let x = MonthDifference.CreatePositive(1, 2)
        let y = MonthDifference.CreatePositive(2, 0)
        // Act & Assert
        x.CompareTo(y) <= 0                     |> ok
        (x :> IComparable).CompareTo(x) <= 0    |> ok
        // Flipped
        y.CompareTo(x) >= 0                     |> ok
        (y :> IComparable).CompareTo(x) >= 0    |> ok

    [<Theory>]
    [<InlineData(1, 2)>]
    [<InlineData(2, 1)>]
    let ``CompareTo(obj) returns 1 when "obj" is null`` (years, months) =
        let x = MonthDifference.CreatePositive(years, months)
        // Act & Assert
        (x :> IComparable).CompareTo(null) = 1

    [<Theory>]
    [<InlineData(1, 2)>]
    [<InlineData(2, 1)>]
    let ``CompareTo(obj) throws when "obj" is a plain object`` (years, months) =
        let x = MonthDifference.CreatePositive(years, months)
        // Act & Assert
        argExn "obj" (fun () -> (x :> IComparable).CompareTo(new obj()))

module Math =
    [<Fact>]
    let ``Unary + and - on Zero`` () =
        let x = MonthDifference.Zero
        // Act & Assert
        +x === x
        -x === x

    [<Theory>]
    [<InlineData(1, 2)>]
    [<InlineData(2, 1)>]
    let ``Unary +`` (years, months) =
        let x = MonthDifference.CreatePositive(years, months)
        let y = MonthDifference.CreateNegative(years, months)
        // Act & Assert
        +x === x
        +y === y

    [<Theory>]
    [<InlineData(1, 2)>]
    [<InlineData(2, 1)>]
    let ``Unary -`` (years, months) =
        let x = MonthDifference.CreatePositive(years, months)
        let y = MonthDifference.CreateNegative(years, months)
        // Act & Assert
        -x === y
        -y === x
        +y === y

    [<Theory>]
    [<InlineData(1, 2)>]
    [<InlineData(2, 1)>]
    let ``Negate()`` (years, months) =
        let x = MonthDifference.CreatePositive(years, months)
        let y = MonthDifference.CreateNegative(years, months)
        // Act & Assert
        x.Negate() === y
        y.Negate() === x
