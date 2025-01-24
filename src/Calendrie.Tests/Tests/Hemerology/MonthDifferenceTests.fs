// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.MonthDifferenceTests

open Calendrie.Hemerology
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Static property Zero`` () =
        let zero = MonthDifference.Zero
        // Act & Assert
        zero === MonthDifference.UnsafeCreate(0, 0, 0)
        zero.Years  === 0
        zero.Months === 0
        zero.Sign   === 0

    [<Theory>]
    [<InlineData(1, 2, 1)>]
    [<InlineData(2, 1, 1)>]
    [<InlineData(-1, -2, -1)>]
    [<InlineData(-2, -1, -1)>]
    let ``Properties Years, Months, Weeks, Days and Sign`` (years, months, sign) =
        let x = MonthDifference.UnsafeCreate(years, months, sign)
        // Act & Assert
        x.Years  === years
        x.Months === months
        x.Sign   === sign

module Factories =
    [<Fact>]
    let ``CreatePositive() throws when years = months = days = 0 `` () =
        argExn null (fun () -> MonthDifference.CreatePositive(0, 0))

    [<Fact>]
    let ``CreatePositive() throws when years or months < 0 `` () =
        outOfRangeExn "years"  (fun () -> MonthDifference.CreatePositive(-1, 2))
        outOfRangeExn "months" (fun () -> MonthDifference.CreatePositive(1, -2))

    [<Fact>]
    let ``CreatePositive()`` () =
        // Act
        let x = MonthDifference.CreatePositive(1, 2)
        // Assert
        x === MonthDifference.UnsafeCreate(1, 2, 1)
        x.Years  === 1
        x.Months === 2
        x.Sign   === 1

    [<Fact>]
    let ``CreateNegative() throws when years = months = days = 0 `` () =
        argExn null (fun () -> MonthDifference.CreateNegative(0, 0))

    [<Fact>]
    let ``CreateNegative() throws when years or months < 0 `` () =
        outOfRangeExn "years"  (fun () -> MonthDifference.CreateNegative(-1, 2))
        outOfRangeExn "months" (fun () -> MonthDifference.CreateNegative(1, -2))

    [<Fact>]
    let ``CreateNegative()`` () =
        // Act
        let x = MonthDifference.CreateNegative(1, 2)
        // Assert
        x === MonthDifference.UnsafeCreate(-1, -2, -1)
        x.Years  === -1
        x.Months === -2
        x.Sign   === -1
