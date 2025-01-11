// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.InternationalFixed

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
    let ``Value of InternationalFixedCalendar.Epoch.DaysZinceZero`` () =
        InternationalFixedCalendar.Instance.Epoch.DaysSinceZero === 0

    [<Fact>]
    let ``default(InternationalFixedDate) is InternationalFixedCalendar.Epoch`` () =
        Unchecked.defaultof<InternationalFixedDate>.DayNumber === InternationalFixedCalendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of InternationalFixedCalendar.MinDaysSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of InternationalFixedCalendar.MaxDaysSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MaxDaysSinceEpoch === 3_652_058

    [<Fact>]
    let ``Value of InternationalFixedCalendar.MinMonthsSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of InternationalFixedCalendar.MaxMonthsSinceEpoch`` () =
        InternationalFixedCalendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

