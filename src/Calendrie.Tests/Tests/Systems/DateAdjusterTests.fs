// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.DateAdjusterTests

open Calendrie
open Calendrie.Core.Intervals
open Calendrie.Core.Schemas
open Calendrie.Hemerology
open Calendrie.Systems
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when the scope is null`` () =
        let scope: CalendarScope | null = null

        nullExn "scope" (fun () -> new DateAdjuster<ArmenianDate>(scope))

    [<Fact>]
    let ``Property scope`` () =
        let range = Range.Create(1, 2)
        let scope = MinMaxYearScope.Create(new Egyptian12Schema(), DayZero.Armenian, range)
        let adjuster = new DateAdjuster<ArmenianDate>(scope)

        adjuster.Scope ==& scope

