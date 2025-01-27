// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.MathTests

open Calendrie
open Calendrie.Hemerology
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data

open Xunit

let additionRuleData = EnumDataSet.AdditionRuleData
let invalidAdditionRuleData = EnumDataSet.InvalidAdditionRuleData

[<Theory; MemberData(nameof(additionRuleData))>]
let ``Adjust(date) does not throw when "rule" is a valid value`` (rule: AdditionRule) =
    DateMath.Adjust(CivilDate.MinValue, 1, rule)

[<Theory; MemberData(nameof(invalidAdditionRuleData))>]
let ``Adjust(date) throws when "rule" is not a valid value`` (rule: AdditionRule) =
    notSupportedExn (fun () -> DateMath.Adjust(CivilDate.MinValue, 1, rule))

[<Theory; MemberData(nameof(additionRuleData))>]
let ``Adjust(month) does not throw when "rule" is a valid value`` (rule: AdditionRule) =
    MonthMath.Adjust(CivilMonth.MinValue, 1, rule)

[<Theory; MemberData(nameof(invalidAdditionRuleData))>]
let ``Adjust(month) throws when "rule" is not a valid value`` (rule: AdditionRule) =
    notSupportedExn (fun () -> MonthMath.Adjust(CivilMonth.MinValue, 1, rule))
