// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.BasicCalendarTests

open Calendrie
open Calendrie.Core.Schemas
open Calendrie.Hemerology.Scopes
open Calendrie.Testing
open Calendrie.Testing.Faux

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "name" is null`` () =
        let name: string = null
        let scope = StandardScope.Create(new GregorianSchema(), DayZero.NewStyle)

        nullExn "name" (fun () -> new FauxBasicCalendar(name, scope))

    [<Fact>]
    let ``Constructor throws when "scope" is null`` () =
        let scope: CalendarScope = null

        nullExn "scope" (fun () -> new FauxBasicCalendar("Name", scope))

    [<Fact>]
    let ``Properties from constructor`` () =
        let name = "My Name"
        let scope = StandardScope.Create(new GregorianSchema(), DayZero.NewStyle)
        let chr = new FauxBasicCalendar(name, scope)

        chr.Name  === name
        chr.Scope ==& scope

    [<Fact>]
    let ``IsRegular() when the calendar is regular`` () =
        let name = "My Name"
        let scope = StandardScope.Create(new GregorianSchema(), DayZero.NewStyle)
        let chr = new FauxBasicCalendar(name, scope)

        let (regular, monthsInYear) = chr.IsRegular()

        regular |> ok
        monthsInYear === 12

    [<Fact>]
    let ``IsRegular() when the calendar is not regular`` () =
        let name = "My Name"
        let scope = StandardScope.Create(new FauxLunisolarSchema(), DayZero.NewStyle)
        let chr = new FauxBasicCalendar(name, scope)

        let (regular, monthsInYear) = chr.IsRegular()

        regular |> nok
        monthsInYear === 0
