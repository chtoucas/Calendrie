// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.MinMaxYearCalendarTests

open Calendrie
open Calendrie.Core.Intervals
open Calendrie.Core.Schemas
open Calendrie.Hemerology
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "name" is null`` () =
        let scope = MinMaxYearScope.Create(new GregorianSchema(), DayZero.NewStyle, Range.Create(1, 4))

        nullExn "name" (fun () -> new MinMaxYearCalendar(null, scope))
        nullExn "name" (fun () -> new MinMaxYearCalendar(null, scope))

    [<Fact>]
    let ``Constructor throws when "scope" is null`` () =
        nullExn "scope" (fun () -> new MinMaxYearCalendar("Name", (null: MinMaxYearScope)))
        nullExn "scope" (fun () -> new MinMaxYearCalendar("Name", (null: MinMaxYearScope)))
