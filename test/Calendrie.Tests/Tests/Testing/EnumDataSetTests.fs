// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Testing.EnumDataSetTests

open System

open Calendrie
open Calendrie.Testing
open Calendrie.Testing.Data

open Xunit

[<Fact>]
let ``DayOfWeekData is exhaustive`` () =
    let count = Enum.GetValues(typeof<DayOfWeek>).Length

    EnumDataSet.DayOfWeekData.Count === count

[<Fact>]
let ``AdditionRuleData is exhaustive`` () =
    let count = Enum.GetValues(typeof<AdditionRule>).Length

    EnumDataSet.AdditionRuleData.Count === count

[<Fact>]
let ``CalendricalAlgorithmData is exhaustive`` () =
    let count = Enum.GetValues(typeof<CalendricalAlgorithm>).Length

    EnumDataSet.CalendricalAlgorithmData.Count === count

[<Fact>]
let ``CalendricalFamilyData is exhaustive`` () =
    let count = Enum.GetValues(typeof<CalendricalFamily>).Length

    EnumDataSet.CalendricalFamilyData.Count === count
