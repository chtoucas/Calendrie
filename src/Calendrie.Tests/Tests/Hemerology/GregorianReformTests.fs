// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.GregorianReformTests

open Calendrie.Hemerology
open Calendrie.Specialized
open Calendrie.Testing

open Xunit

[<Fact>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
let ``Property Official`` () =
    let official = GregorianReform.Official

    official.FirstGregorianDate === new GregorianDate(1582, 10, 15)
    official.LastJulianDate     === new JulianDate(1582, 10, 4)
    official.SecularShift       === 10

[<Fact>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
let ``Property Official (from last Julian date)`` () =
    let date = new JulianDate(1582, 10, 4)
    let reform = GregorianReform.FromLastJulianDate(date)

    reform === GregorianReform.Official

[<Fact>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
let ``Property Official (from first Gregorian date)`` () =
    let date = new GregorianDate(1582, 10, 15)
    let reform = GregorianReform.FromFirstGregorianDate(date)

    reform === GregorianReform.Official
