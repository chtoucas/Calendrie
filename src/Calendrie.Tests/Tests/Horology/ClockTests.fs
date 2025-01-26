// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Horology.ClockTests

open Calendrie
open Calendrie.Horology
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Faux

open Xunit

// TODO(fact): Property-based tests. Waiting for migration to the new version of FsCheck
let private daysSinceZero = 123_456

[<Fact>]
let ``FauxClock:Today() sanity checks`` () =
    let clock = FauxClock(daysSinceZero)
    let dayNumber = DayNumber.Zero + daysSinceZero
    // Act & Assert
    clock.Today() === dayNumber

module TropicaliaClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new TropicaliaClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        TropicaliaClock.Local.Clock ==& LocalSystemClock.Instance
        TropicaliaClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new TropicaliaClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = TropicaliaDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === TropicaliaMonth.FromDate(date)
        clock.GetCurrentYear()  === TropicaliaYear.FromDate(date)

module PaxClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new PaxClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        PaxClock.Local.Clock ==& LocalSystemClock.Instance
        PaxClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new PaxClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = PaxDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === PaxMonth.FromDate(date)
        clock.GetCurrentYear()  === PaxYear.FromDate(date)
