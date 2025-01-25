// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.DateDifferenceTests

open System

open Calendrie.Hemerology
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Static property Zero`` () =
        let zero = DateDifference.Zero
        // Act & Assert
        zero === DateDifference.UnsafeCreate(0, 0, 0, 0)
        zero.Years  === 0
        zero.Months === 0
        zero.Days   === 0
        zero.Sign   === 0

    [<Theory>]
    [<InlineData(1, 2, 3, 4, 1)>]
    [<InlineData(-1, -2, -3, -4, -1)>]
    let ``Instance properties`` (years, months, weeks, days, sign) =
        let x = DateDifference.UnsafeCreate(years, months, days + 7 * weeks, sign)
        // Act & Assert
        x.Years  === years
        x.Months === months
        x.Weeks  === weeks
        x.Days   === days
        x.Sign   === sign

    [<Theory>]
    [<InlineData(1, 2, 3, 4, 1)>]
    [<InlineData(-1, -2, -3, -4, -1)>]
    let Deconstructor (years, months, weeks, days, sign) =
        let x = DateDifference.UnsafeCreate(years, months, days + 7 * weeks, sign)
        // Act & Assert
        x.Deconstruct() === (years, months, weeks, days)

module Factories =
    [<Fact>]
    let ``CreatePositive() throws when years = months = days = 0 `` () =
        argExn null (fun () -> DateDifference.CreatePositive(0, 0, 0))

    [<Fact>]
    let ``CreatePositive() throws when years, months or days < 0 `` () =
        outOfRangeExn "years"  (fun () -> DateDifference.CreatePositive(-1, 2, 3))
        outOfRangeExn "months" (fun () -> DateDifference.CreatePositive(1, -2, 3))
        outOfRangeExn "days"   (fun () -> DateDifference.CreatePositive(1, 2, -3))

    [<Theory>]
    [<InlineData(0, 0, 0)>]
    [<InlineData(1, 0, 1)>]
    [<InlineData(6, 0, 6)>]
    [<InlineData(7, 1, 0)>]
    [<InlineData(8, 1, 1)>]
    [<InlineData(14, 2, 0)>]
    [<InlineData(15, 2, 1)>]
    let ``CreatePositive()`` (days, w, d) =
        // Act
        let x = DateDifference.CreatePositive(1, 2, days)
        // Assert
        x === DateDifference.UnsafeCreate(1, 2, days, 1)
        x.Years  === 1
        x.Months === 2
        x.Weeks  === w
        x.Days   === d
        x.Sign   === 1

    [<Fact>]
    let ``CreateNegative() throws when years = months = days = 0 `` () =
        argExn null (fun () -> DateDifference.CreateNegative(0, 0, 0))

    [<Fact>]
    let ``CreateNegative() throws when years, months or days < 0 `` () =
        outOfRangeExn "years"  (fun () -> DateDifference.CreateNegative(-1, 2, 3))
        outOfRangeExn "months" (fun () -> DateDifference.CreateNegative(1, -2, 3))
        outOfRangeExn "days"   (fun () -> DateDifference.CreateNegative(1, 2, -3))

    [<Theory>]
    [<InlineData(0, 0, 0)>]
    [<InlineData(1, 0, 1)>]
    [<InlineData(6, 0, 6)>]
    [<InlineData(7, 1, 0)>]
    [<InlineData(8, 1, 1)>]
    [<InlineData(14, 2, 0)>]
    [<InlineData(15, 2, 1)>]
    let ``CreateNegative()`` (days, w, d) =
        // Act
        let x = DateDifference.CreateNegative(1, 2, days)
        // Assert
        x === DateDifference.UnsafeCreate(-1, -2, -days, -1)
        x.Years  === -1
        x.Months === -2
        x.Weeks  === -w
        x.Days   === -d
        x.Sign   === -1

module Comparison =
    open NonStructuralComparison

    [<Fact>]
    let ``Zero w/ anything`` () =
        // Zero / Zero
        DateDifference.Zero >  DateDifference.Zero |> nok
        DateDifference.Zero >= DateDifference.Zero |> ok
        DateDifference.Zero <  DateDifference.Zero |> nok
        DateDifference.Zero <= DateDifference.Zero |> ok
        // Zero / Positive
        DateDifference.CreatePositive(1, 2, 3) >  DateDifference.Zero |> ok
        DateDifference.CreatePositive(1, 2, 3) >= DateDifference.Zero |> ok
        DateDifference.CreatePositive(1, 2, 3) <  DateDifference.Zero |> nok
        DateDifference.CreatePositive(1, 2, 3) <= DateDifference.Zero |> nok
        // Zero / Negative
        DateDifference.CreateNegative(1, 2, 3) >  DateDifference.Zero |> ok
        DateDifference.CreateNegative(1, 2, 3) >= DateDifference.Zero |> ok
        DateDifference.CreateNegative(1, 2, 3) <  DateDifference.Zero |> nok
        DateDifference.CreateNegative(1, 2, 3) <= DateDifference.Zero |> nok

    //
    // CompareTo()
    //

    [<Theory>]
    [<InlineData(1, 2, 3)>]
    [<InlineData(1, 3, 2)>]
    [<InlineData(2, 1, 3)>]
    [<InlineData(2, 3, 1)>]
    [<InlineData(3, 1, 2)>]
    [<InlineData(3, 2, 1)>]
    let ``CompareTo() returns 0 when both objects are identical`` (years, months, days) =
        let x = DateDifference.CreatePositive(years, months, days)
        // Act & Assert
        x.CompareTo(x) === 0
        (x :> IComparable).CompareTo(x) === 0

    [<Fact>]
    let ``CompareTo() when both objects are distinct`` () =
        let x = DateDifference.CreatePositive(1, 2, 3)
        let y = DateDifference.CreatePositive(2, 0, 1)
        // Act & Assert
        x.CompareTo(y) <= 0                     |> ok
        (x :> IComparable).CompareTo(x) <= 0    |> ok
        // Flipped
        y.CompareTo(x) >= 0                     |> ok
        (y :> IComparable).CompareTo(x) >= 0    |> ok

    [<Theory>]
    [<InlineData(1, 2, 3)>]
    [<InlineData(1, 3, 2)>]
    [<InlineData(2, 1, 3)>]
    [<InlineData(2, 3, 1)>]
    [<InlineData(3, 1, 2)>]
    [<InlineData(3, 2, 1)>]
    let ``CompareTo(obj) returns 1 when "obj" is null`` (years, months, days) =
        let x = DateDifference.CreatePositive(years, months, days)
        // Act & Assert
        (x :> IComparable).CompareTo(null) = 1

    [<Theory>]
    [<InlineData(1, 2, 3)>]
    [<InlineData(1, 3, 2)>]
    [<InlineData(2, 1, 3)>]
    [<InlineData(2, 3, 1)>]
    [<InlineData(3, 1, 2)>]
    [<InlineData(3, 2, 1)>]
    let ``CompareTo(obj) throws when "obj" is a plain object`` (years, months, days) =
        let x = DateDifference.CreatePositive(years, months, days)
        // Act & Assert
        argExn "obj" (fun () -> (x :> IComparable).CompareTo(new obj()))

module Math =
    [<Fact>]
    let ``Unary + and - on Zero`` () =
        let x = DateDifference.Zero
        // Act & Assert
        +x === x
        -x === x

    [<Theory>]
    [<InlineData(1, 2, 3)>]
    [<InlineData(1, 3, 2)>]
    [<InlineData(2, 1, 3)>]
    [<InlineData(2, 3, 1)>]
    [<InlineData(3, 1, 2)>]
    [<InlineData(3, 2, 1)>]
    let ``Unary +`` (years, months, days) =
        let x = DateDifference.CreatePositive(years, months, days)
        let y = DateDifference.CreateNegative(years, months, days)
        // Act & Assert
        +x === x
        +y === y

    [<Theory>]
    [<InlineData(1, 2, 3)>]
    [<InlineData(1, 3, 2)>]
    [<InlineData(2, 1, 3)>]
    [<InlineData(2, 3, 1)>]
    [<InlineData(3, 1, 2)>]
    [<InlineData(3, 2, 1)>]
    let ``Unary -`` (years, months, days) =
        let x = DateDifference.CreatePositive(years, months, days)
        let y = DateDifference.CreateNegative(years, months, days)
        // Act & Assert
        -x === y
        -y === x
        +y === y

    [<Theory>]
    [<InlineData(1, 2, 3)>]
    [<InlineData(1, 3, 2)>]
    [<InlineData(2, 1, 3)>]
    [<InlineData(2, 3, 1)>]
    [<InlineData(3, 1, 2)>]
    [<InlineData(3, 2, 1)>]
    let ``Negate()`` (years, months, days) =
        let x = DateDifference.CreatePositive(years, months, days)
        let y = DateDifference.CreateNegative(years, months, days)
        // Act & Assert
        x.Negate() === y
        y.Negate() === x
