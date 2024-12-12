// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.DateAdjusterTests

open Calendrie.Systems
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when the calendar is null`` () =
        let chr: CalendarSystem<ArmenianDate> | null = null

        nullExn "calendar" (fun () -> new DateAdjuster<ArmenianDate>(chr))

    //[<Fact>]
    //let ``Property scope`` () =
    //    let range = Range.Create(1, 2)
    //    let scope = MinMaxYearScope.Create(new Egyptian12Schema(), DayZero.Armenian, range)
    //    let adjuster = new FauxDateAdjuster<ArmenianDate>(scope)

    //    adjuster.Scope ==& scope

