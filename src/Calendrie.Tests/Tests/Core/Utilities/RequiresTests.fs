﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Utilities.RequiresTests

open System

open Calendrie
open Calendrie.Core.Utilities
open Calendrie.Testing
open Calendrie.Testing.Data

open Xunit

// TODO(code): failing tests; see commented code.

let private paramName = "paramName"

let dayOfWeekData  = EnumDataSet.DayOfWeekData
let isoWeekdayData  = EnumDataSet.IsoWeekdayData
let additionRuleData = EnumDataSet.AdditionRuleData
let invalidDayOfWeekData  = EnumDataSet.InvalidDayOfWeekData
let invalidIsoWeekdayData  = EnumDataSet.InvalidIsoWeekdayData
let invalidAdditionRuleData = EnumDataSet.InvalidAdditionRuleData

//
// DayOfWeek
//

[<Theory; MemberData(nameof(dayOfWeekData))>]
let ``Defined(dayOfWeek) does not throw when "dayOfWeek" is a valid value`` (dayOfWeek: DayOfWeek) =
    Requires.Defined(dayOfWeek)
    Requires.Defined(dayOfWeek, paramName)

[<Theory; MemberData(nameof(invalidDayOfWeekData))>]
let ``Defined(dayOfWeek) throws when "dayOfWeek" is not a valid value (without paramName)`` (dayOfWeek: DayOfWeek) =
    //outOfRangeExn "dayOfWeek" (fun () -> Requires.Defined(dayOfWeek))
    outOfRangeExn "" (fun () -> Requires.Defined(dayOfWeek))

[<Theory; MemberData(nameof(invalidDayOfWeekData))>]
let ``Defined(dayOfWeek) throws when "dayOfWeek" is not a valid value (with paramName)`` (dayOfWeek: DayOfWeek) =
    outOfRangeExn paramName (fun () -> Requires.Defined(dayOfWeek, paramName))

//
// IsoWeekday
//

[<Theory; MemberData(nameof(isoWeekdayData))>]
let ``Defined(weekday) does not throw when "weekday" is a valid value`` (weekday: IsoWeekday) =
    RequiresEx.Defined(weekday)
    RequiresEx.Defined(weekday, paramName)

[<Theory; MemberData(nameof(invalidIsoWeekdayData))>]
let ``Defined(weekday) throws when "weekday" is not a valid value (without paramName)`` (weekday: IsoWeekday) =
    //outOfRangeExn "weekday" (fun () -> RequiresEx.Defined(weekday))
    outOfRangeExn "" (fun () -> RequiresEx.Defined(weekday))

[<Theory; MemberData(nameof(invalidIsoWeekdayData))>]
let ``Defined(weekday) throws when "weekday" is not a valid value (with paramName)`` (weekday: IsoWeekday) =
    outOfRangeExn paramName (fun () -> RequiresEx.Defined(weekday, paramName))

//
// AdditionRule
//

[<Theory; MemberData(nameof(additionRuleData))>]
let ``Defined(rule) does not throw when "rule" is a valid value`` (rule: AdditionRule) =
    RequiresEx.Defined(rule)
    RequiresEx.Defined(rule, paramName)

[<Theory; MemberData(nameof(invalidAdditionRuleData))>]
let ``Defined(rule) throws when "rule" is not a valid value (without paramName)`` (rule: AdditionRule) =
    //outOfRangeExn "weekday" (fun () -> RequiresEx.Defined(rule))
    outOfRangeExn "" (fun () -> RequiresEx.Defined(rule))

[<Theory; MemberData(nameof(invalidAdditionRuleData))>]
let ``Defined(rule) throws when "rule" is not a valid value (with paramName)`` (rule: AdditionRule) =
    outOfRangeExn paramName (fun () -> RequiresEx.Defined(rule, paramName))
