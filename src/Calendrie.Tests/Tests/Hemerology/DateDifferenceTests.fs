// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.DateDifferenceTests

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
    let ``Properties Years, Months, Weeks, Days and Sign`` (years, months, weeks, days, sign) =
        let x = DateDifference.UnsafeCreate(years, months, days + 7 * weeks, sign)
        // Act & Assert
        x.Years  === years
        x.Months === months
        x.Weeks  === weeks
        x.Days   === days
        x.Sign   === sign

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
