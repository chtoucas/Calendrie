﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.RomanKalendarTests

open System

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data

open Xunit

let epiphanyData = RomanKalendarDataSet.EpiphanyData
let easterData = RomanKalendarDataSet.EasterData
let paschalMoonData = RomanKalendarDataSet.PaschalMoonData

[<Theory; MemberData(nameof(epiphanyData))>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
let ``Property EpiphanySunday`` y d =
    let epiphanySunday = new GregorianDate(y, 1, d)
    let kalendar = new RomanKalendar(y)

    epiphanySunday.DayOfWeek === DayOfWeek.Sunday
    kalendar.EpiphanySunday  === epiphanySunday

[<Theory; MemberData(nameof(easterData))>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
let ``Property Easter`` y m d =
    let easter = new GregorianDate(y, m, d)
    let kalendar = new RomanKalendar(y)

    easter.DayOfWeek === DayOfWeek.Sunday
    kalendar.Easter  === easter

[<Theory(Skip = "It seems that the D&R data does not match our definition of the Paschal Moon"); MemberData(nameof(paschalMoonData))>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
let ``Property PaschalMoon`` y m d =
    let moon = new GregorianDate(y, m, d)
    let kalendar = new RomanKalendar(y)

    kalendar.PaschalMoon === moon
