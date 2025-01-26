// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.MomentTests

open Calendrie
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Default value of Moment is Moment.Zero`` () =
        Unchecked.defaultof<Moment> === Moment.Zero

    //
    // Properties
    //

    [<Fact>]
    let ``Static property Zero`` () =
        Moment.Zero.DayNumber === DayNumber.Zero
        Moment.Zero.InstantOfDay === InstantOfDay.Midnight
        Moment.Zero.SecondsSinceZero === 0L
        Moment.Zero.MillisecondsSinceZero === 0L

    [<Fact>]
    let ``Static property MinValue`` () =
        Moment.MinValue.DayNumber === DayNumber.MinValue
        Moment.MinValue.InstantOfDay === InstantOfDay.MinValue
        // Along the way, we also check that the foll. props do not overflow.
        Moment.MinValue.SecondsSinceZero      |> ignore
        Moment.MinValue.MillisecondsSinceZero |> ignore

    [<Fact>]
    let ``Static property MaxValue`` () =
        Moment.MaxValue.DayNumber === DayNumber.MaxValue
        Moment.MaxValue.InstantOfDay === InstantOfDay.MaxValue
        // Along the way, we also check that the foll. props do not overflow.
        Moment.MaxValue.SecondsSinceZero      |> ignore
        Moment.MaxValue.MillisecondsSinceZero |> ignore
