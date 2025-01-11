// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.RevisedWorld

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
    let ``Value of RevisedWorldCalendar.Epoch.DaysZinceZero`` () =
        RevisedWorldCalendar.Instance.Epoch.DaysSinceZero === 0

    [<Fact>]
    let ``default(RevisedWorldDate) is RevisedWorldCalendar.Epoch`` () =
        Unchecked.defaultof<RevisedWorldDate>.DayNumber === RevisedWorldCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of RevisedWorldCalendar.MinDaysSinceEpoch`` () =
        RevisedWorldCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of RevisedWorldCalendar.MaxDaysSinceEpoch`` () =
        RevisedWorldCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of RevisedWorldCalendar.MinMonthsSinceEpoch`` () =
        RevisedWorldCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of RevisedWorldCalendar.MaxMonthsSinceEpoch`` () =
        RevisedWorldCalendar.Instance.MaxMonthsSinceEpoch === 119_987
#endif
