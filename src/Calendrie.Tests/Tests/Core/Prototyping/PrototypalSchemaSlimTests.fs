// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Prototyping.PrototypalSchemaSlimTests

open Calendrie.Core
open Calendrie.Core.Schemas
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "kernel" is null`` () =
        nullExn "kernel" (fun () -> new PrototypalSchemaSlim(null, true, 12, 1, 1))

    [<Fact>]
    let ``Constructor throws when "minMonthsInYear" is <= 0`` () =
        outOfRangeExn "minMonthsInYear" (fun () -> new PrototypalSchemaSlim(new GregorianSchema(), true, 0, 2, 1))
        outOfRangeExn "minMonthsInYear" (fun () -> new PrototypalSchemaSlim(new GregorianSchema(), true, -1, 2, 1))

    [<Fact>]
    let ``Constructor throws when "minDaysInYear" is <= 0`` () =
        outOfRangeExn "minDaysInYear" (fun () -> new PrototypalSchemaSlim(new GregorianSchema(), true, 12, 0, 1))
        outOfRangeExn "minDaysInYear" (fun () -> new PrototypalSchemaSlim(new GregorianSchema(), true, 12, -1, 1))

    [<Fact>]
    let ``Constructor throws when "minDaysInMonth" is <= 0`` () =
        outOfRangeExn "minDaysInMonth" (fun () -> new PrototypalSchemaSlim(new GregorianSchema(), true, 12, 1, 0))
        outOfRangeExn "minDaysInMonth" (fun () -> new PrototypalSchemaSlim(new GregorianSchema(), true, 12, 1, -1))

    [<Fact>]
    let ``Constructor does not throw when "minMonthsInYear", "minDaysInYear" and "minDaysInMonth" are > 0`` () =
        new PrototypalSchemaSlim(new GregorianSchema(), true, 12, 2, 1) |> ignore

    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new PrototypalSchemaSlim(null, 12))

    [<Fact>]
    let ``Properties from constructor`` () =
        let proto = new PrototypalSchemaSlim(new GregorianSchema(), 12)
        let sch = proto :> ICalendricalSchema

        proto.MinMonthsInYear === 12
        sch.MinDaysInYear   === 365
        sch.MinDaysInMonth  === 28
