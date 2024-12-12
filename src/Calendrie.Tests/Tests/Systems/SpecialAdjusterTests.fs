// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.SpecialAdjusterTests

open Calendrie.Systems
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when the calendar is null`` () =
        let chr: CalendarSystem<ArmenianDate> | null = null

        nullExn "calendar" (fun () -> new SpecialAdjuster<ArmenianDate>(chr))

    //[<Fact>]
    //let ``Property scope`` () =
    //    let range = Range.Create(1, 2)
    //    let scope = MinMaxYearScope.Create(new Egyptian12Schema(), DayZero.Armenian, range)
    //    let adjuster = new FauxSpecialAdjuster<ArmenianDate>(scope)

    //    adjuster.Scope ==& scope

