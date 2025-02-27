﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.DayZeroTests

open System

open Calendrie
open Calendrie.Systems
open Calendrie.Testing

open Xunit

[<Fact>]
let ``Static property OldStyle`` () =
    let epoch = DayZero.OldStyle
    let date = JulianDate.FromAbsoluteDate(epoch)
    let y, m, d = date.Deconstruct()

    epoch === DayNumber.Zero - 2
    epoch.Ordinal === Ord.First - 2
    epoch.DayOfWeek === DayOfWeek.Saturday
    (y, m, d) === (1, 1, 1)
    date.YearOfEra === Ord.First

[<Fact>]
let ``Static property RataDie`` () =
    let epoch = DayZero.RataDie
    let date = GregorianDate.FromAbsoluteDate(epoch)
    let y, m, d = date.Deconstruct()

    epoch === DayNumber.Zero - 1
    epoch.Ordinal === Ord.First - 1
    epoch.DayOfWeek === DayOfWeek.Sunday
    (y, m, d) === (0, 12, 31)
    date.YearOfEra === Ord.Zeroth

[<Fact>]
let ``Static property NewStyle`` () =
    let epoch = DayZero.NewStyle
    let date = CivilDate.FromAbsoluteDate(epoch)
    let y, m, d = date.Deconstruct()

    epoch === DayNumber.Zero
    epoch.Ordinal === Ord.First
    epoch.DayOfWeek === DayOfWeek.Monday
    (y, m, d) === (1, 1, 1)
    date.YearOfEra === Ord.First

[<Fact>]
let ``Static property Julian is an alias for OldStyle`` () =
    DayZero.Julian === DayZero.OldStyle

[<Fact>]
let ``Static property SundayBeforeGregorian is an alias for RataDie`` () =
    DayZero.SundayBeforeGregorian === DayZero.RataDie

[<Fact>]
let ``Static property Gregorian is an alias for NewStyle`` () =
    DayZero.Gregorian === DayZero.NewStyle

// See D.&R., p.15.

[<Fact>]
let ``Static property Holocene`` () =
    DayZero.Holocene === DayZero.NewStyle - 3_652_425

[<Fact>]
let ``Static property Egyptian`` () =
    DayZero.Egyptian === DayZero.NewStyle - 272_788

[<Fact>]
let ``Static property Ethiopic`` () =
    DayZero.Ethiopic === DayZero.NewStyle + 2795

[<Fact>]
let ``Static property Coptic`` () =
    DayZero.Coptic === DayZero.NewStyle + 103_604

[<Fact>]
let ``Static property Armenian`` () =
    DayZero.Armenian === DayZero.NewStyle + 201_442

[<Fact>]
let ``Static property Persian`` () =
    DayZero.Persian === DayZero.NewStyle + 226_895

[<Fact>]
let ``Static property TabularIslamic`` () =
    DayZero.TabularIslamic === DayZero.NewStyle + 227_014

[<Fact>]
let ``Static property Zoroastrian`` () =
    DayZero.Zoroastrian === DayZero.NewStyle + 230_637

[<Fact>]
let ``Static property Positivist`` () =
    DayZero.Positivist === DayZero.NewStyle + 653_054

[<Fact>]
let ``Static property FrenchRepublican`` () =
    DayZero.FrenchRepublican === DayZero.NewStyle + 654_414

[<Fact>]
let ``Static property Minguo`` () =
    let date = CivilDate.FromAbsoluteDate(DayZero.Minguo)
    let year = new CivilYear(1912)
    let startOfYear = year.MinDay

    date === startOfYear

[<Fact>]
let ``Static property Tropicalia`` () =
    let date = CivilDate.FromAbsoluteDate(DayZero.Tropicalia)
    let year = new CivilYear(1968)
    let startOfYear = year.MinDay

    date === startOfYear
