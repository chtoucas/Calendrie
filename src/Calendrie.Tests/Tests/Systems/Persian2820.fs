// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.Persian2820

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of Persian2820Calendar.Epoch.DaysZinceZero`` () =
        Persian2820Calendar.Instance.Epoch.DaysSinceZero === 226_895

    [<Fact>]
    let ``default(Persian2820Date) is Persian2820Calendar.Epoch`` () =
        Unchecked.defaultof<Persian2820Date>.DayNumber === Persian2820Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of Persian2820Calendar.MinDaysSinceEpoch`` () =
        Persian2820Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of Persian2820Calendar.MaxDaysSinceEpoch`` () =
        Persian2820Calendar.Instance.MaxDaysSinceEpoch === 3_652_055

    [<Fact>]
    let ``Value of Persian2820Calendar.MinMonthsSinceEpoch`` () =
        Persian2820Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of Persian2820Calendar.MaxMonthsSinceEpoch`` () =
        Persian2820Calendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif


