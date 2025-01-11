// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.FrenchRepublican

open Calendrie
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Facts.Systems

open Xunit

module Prelude =
    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.Epoch.DaysZinceZero`` () =
        FrenchRepublicanCalendar.Instance.Epoch.DaysSinceZero === 654_414
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.Epoch.DaysZinceZero`` () =
        FrenchRepublican13Calendar.Instance.Epoch.DaysSinceZero === 654_414

    [<Fact>]
    let ``default(FrenchRepublicanDate) is FrenchRepublicanCalendar.Epoch`` () =
        Unchecked.defaultof<FrenchRepublicanDate>.DayNumber === FrenchRepublicanCalendar.Instance.Epoch
    [<Fact>]
    let ``default(FrenchRepublican13Date) is FrenchRepublican13Calendar.Epoch`` () =
        Unchecked.defaultof<FrenchRepublican13Date>.DayNumber === FrenchRepublican13Calendar.Instance.Epoch

#if DEBUG
    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MinDaysSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MinDaysSinceEpoch === 0
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MinDaysSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MinDaysSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MaxDaysSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MaxDaysSinceEpoch === 3_652_056
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MaxDaysSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MaxDaysSinceEpoch === 3_652_056

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MinMonthsSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MinMonthsSinceEpoch === 0
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MinMonthsSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MinMonthsSinceEpoch === 0

    [<Fact>]
    let ``Value of FrenchRepublicanCalendar.MaxMonthsSinceEpoch`` () =
        FrenchRepublicanCalendar.Instance.MaxMonthsSinceEpoch === 119_987
    [<Fact>]
    let ``Value of FrenchRepublican13Calendar.MaxMonthsSinceEpoch`` () =
        FrenchRepublican13Calendar.Instance.MaxMonthsSinceEpoch === 129_986
#endif

