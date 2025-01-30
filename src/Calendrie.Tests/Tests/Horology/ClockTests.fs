// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Horology.ClockTests

open System

open Calendrie
open Calendrie.Core
open Calendrie.Horology
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Faux

open Xunit

// This value should work for all calendars: 27/1/2025 (Gregorian).
let private daysSinceZero = 739_277;

[<Fact>]
let ``LocalSystemClock:Today()`` () =
    let mid = int(DateTime.Now.Ticks / TemporalConstants.TicksPerDay)
    // Act
    let daysSinceZero = LocalSystemClock.Instance.Today().DaysSinceZero
    // Assert
    daysSinceZero >= mid - 1 |> ok
    daysSinceZero <= mid + 1 |> ok

[<Fact>]
let ``UtcSystemClock:Today()`` () =
    let mid = int(DateTime.Now.Ticks / TemporalConstants.TicksPerDay)
    // Act
    let daysSinceZero = UtcSystemClock.Instance.Today().DaysSinceZero
    // Assert
    daysSinceZero >= mid - 1 |> ok
    daysSinceZero <= mid + 1 |> ok

[<Fact>]
let ``FauxClock:Today() sanity checks`` () =
    let clock = FauxClock(daysSinceZero)
    let dayNumber = DayNumber.Zero + daysSinceZero
    // Act & Assert
    clock.Today() === dayNumber

module CalendarClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new CalendarClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        CalendarClock.Local.Clock ==& LocalSystemClock.Instance
        CalendarClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new CalendarClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = CivilDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()                   === dayNumber
        clock.GetCurrentDate()          === date
        clock.GetCurrentCivilDate()     === date
        clock.GetCurrentGregorianDate() === GregorianDate.FromAbsoluteDate(dayNumber)
        clock.GetCurrentJulianDate()    === JulianDate.FromAbsoluteDate(dayNumber)

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
        clock.GetCurrentMonth() === new TropicaliaMonth(date)
        clock.GetCurrentYear()  === new TropicaliaYear(date)

#if ENABLE_CLOCKS

module ArmenianClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new ArmenianClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        ArmenianClock.Local.Clock ==& LocalSystemClock.Instance
        ArmenianClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new ArmenianClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = ArmenianDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new ArmenianMonth(date)
        clock.GetCurrentYear()  === new ArmenianYear(date)

module Armenian13Clock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new Armenian13Clock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        Armenian13Clock.Local.Clock ==& LocalSystemClock.Instance
        Armenian13Clock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new Armenian13Clock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = Armenian13Date.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new Armenian13Month(date)
        clock.GetCurrentYear()  === new Armenian13Year(date)

module CivilClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new CivilClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        CivilClock.Local.Clock ==& LocalSystemClock.Instance
        CivilClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new CivilClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = CivilDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new CivilMonth(date)
        clock.GetCurrentYear()  === new CivilYear(date)

module CopticClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new CopticClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        CopticClock.Local.Clock ==& LocalSystemClock.Instance
        CopticClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new CopticClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = CopticDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new CopticMonth(date)
        clock.GetCurrentYear()  === new CopticYear(date)

module Coptic13Clock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new Coptic13Clock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        Coptic13Clock.Local.Clock ==& LocalSystemClock.Instance
        Coptic13Clock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new Coptic13Clock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = Coptic13Date.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new Coptic13Month(date)
        clock.GetCurrentYear()  === new Coptic13Year(date)

module EgyptianClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new EgyptianClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        EgyptianClock.Local.Clock ==& LocalSystemClock.Instance
        EgyptianClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new EgyptianClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = EgyptianDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new EgyptianMonth(date)
        clock.GetCurrentYear()  === new EgyptianYear(date)

module Egyptian13Clock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new Egyptian13Clock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        Egyptian13Clock.Local.Clock ==& LocalSystemClock.Instance
        Egyptian13Clock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new Egyptian13Clock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = Egyptian13Date.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new Egyptian13Month(date)
        clock.GetCurrentYear()  === new Egyptian13Year(date)

module EthiopicClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new EthiopicClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        EthiopicClock.Local.Clock ==& LocalSystemClock.Instance
        EthiopicClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new EthiopicClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = EthiopicDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new EthiopicMonth(date)
        clock.GetCurrentYear()  === new EthiopicYear(date)

module Ethiopic13Clock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new Ethiopic13Clock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        Ethiopic13Clock.Local.Clock ==& LocalSystemClock.Instance
        Ethiopic13Clock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new Ethiopic13Clock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = Ethiopic13Date.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new Ethiopic13Month(date)
        clock.GetCurrentYear()  === new Ethiopic13Year(date)

module FrenchRepublicanClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new FrenchRepublicanClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        FrenchRepublicanClock.Local.Clock ==& LocalSystemClock.Instance
        FrenchRepublicanClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new FrenchRepublicanClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = FrenchRepublicanDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new FrenchRepublicanMonth(date)
        clock.GetCurrentYear()  === new FrenchRepublicanYear(date)

module FrenchRepublican13Clock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new FrenchRepublican13Clock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        FrenchRepublican13Clock.Local.Clock ==& LocalSystemClock.Instance
        FrenchRepublican13Clock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new FrenchRepublican13Clock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = FrenchRepublican13Date.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new FrenchRepublican13Month(date)
        clock.GetCurrentYear()  === new FrenchRepublican13Year(date)

module GregorianClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new GregorianClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        GregorianClock.Local.Clock ==& LocalSystemClock.Instance
        GregorianClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new GregorianClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = GregorianDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new GregorianMonth(date)
        clock.GetCurrentYear()  === new GregorianYear(date)

module InternationalFixedClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new InternationalFixedClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        InternationalFixedClock.Local.Clock ==& LocalSystemClock.Instance
        InternationalFixedClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new InternationalFixedClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = InternationalFixedDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new InternationalFixedMonth(date)
        clock.GetCurrentYear()  === new InternationalFixedYear(date)

module JulianClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new JulianClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        JulianClock.Local.Clock ==& LocalSystemClock.Instance
        JulianClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new JulianClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = JulianDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new JulianMonth(date)
        clock.GetCurrentYear()  === new JulianYear(date)

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
        clock.GetCurrentMonth() === new PaxMonth(date)
        clock.GetCurrentYear()  === new PaxYear(date)

module Persian2820Clock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new Persian2820Clock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        Persian2820Clock.Local.Clock ==& LocalSystemClock.Instance
        Persian2820Clock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new Persian2820Clock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = Persian2820Date.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new Persian2820Month(date)
        clock.GetCurrentYear()  === new Persian2820Year(date)

module PositivistClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new PositivistClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        PositivistClock.Local.Clock ==& LocalSystemClock.Instance
        PositivistClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new PositivistClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = PositivistDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new PositivistMonth(date)
        clock.GetCurrentYear()  === new PositivistYear(date)

module TabularIslamicClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new TabularIslamicClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        TabularIslamicClock.Local.Clock ==& LocalSystemClock.Instance
        TabularIslamicClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new TabularIslamicClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = TabularIslamicDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new TabularIslamicMonth(date)
        clock.GetCurrentYear()  === new TabularIslamicYear(date)

module WorldClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new WorldClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        WorldClock.Local.Clock ==& LocalSystemClock.Instance
        WorldClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new WorldClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = WorldDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new WorldMonth(date)
        clock.GetCurrentYear()  === new WorldYear(date)

module ZoroastrianClock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new ZoroastrianClock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        ZoroastrianClock.Local.Clock ==& LocalSystemClock.Instance
        ZoroastrianClock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new ZoroastrianClock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = ZoroastrianDate.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new ZoroastrianMonth(date)
        clock.GetCurrentYear()  === new ZoroastrianYear(date)

module Zoroastrian13Clock =
    [<Fact>]
    let ``Constructor throws when clock is null`` () =
        nullExn "clock" (fun () -> new Zoroastrian13Clock(null))

    [<Fact>]
    let ``Static properties Local and Utc`` () =
        Zoroastrian13Clock.Local.Clock ==& LocalSystemClock.Instance
        Zoroastrian13Clock.Utc.Clock   ==& UtcSystemClock.Instance

    [<Fact>]
    let ``GetCurrentXXX() and Today()`` () =
        let clock = new Zoroastrian13Clock(FauxClock(daysSinceZero))
        let dayNumber = DayNumber.Zero + daysSinceZero
        let date = Zoroastrian13Date.FromAbsoluteDate(dayNumber)
        // Act & Assert
        clock.Today()           === dayNumber
        clock.GetCurrentDate()  === date
        clock.GetCurrentMonth() === new Zoroastrian13Month(date)
        clock.GetCurrentYear()  === new Zoroastrian13Year(date)

#endif
