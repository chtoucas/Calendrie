// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.PrototypalSchemaSlimTests

open Calendrie.Core
open Calendrie.Core.Schemas
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "kernel" is null`` () =
        nullExn "kernel" (fun () -> new PrototypalSchemaSlim(null, 1, 1))

    [<Fact>]
    let ``Constructor throws when "minDaysInYear" is <= 0`` () =
        outOfRangeExn "minDaysInYear" (fun () -> new PrototypalSchemaSlim(new GregorianSchema(), 0, 1))
        outOfRangeExn "minDaysInYear" (fun () -> new PrototypalSchemaSlim(new GregorianSchema(), -1, 1))

    [<Fact>]
    let ``Constructor throws when "minDaysInMonth" is <= 0`` () =
        outOfRangeExn "minDaysInMonth" (fun () -> new PrototypalSchemaSlim(new GregorianSchema(), 1, 0))
        outOfRangeExn "minDaysInMonth" (fun () -> new PrototypalSchemaSlim(new GregorianSchema(), 1, -1))

    [<Fact>]
    let ``Constructor throws when "minDaysInYear" and "minDaysInMonth" are > 0`` () =
        new PrototypalSchemaSlim(new GregorianSchema(), 1, 1) |> ignore

    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new PrototypalSchemaSlim(null))

    [<Fact>]
    let ``Properties from constructor`` () =
        let proto = new PrototypalSchemaSlim(new GregorianSchema())
        let sch = proto :> ICalendricalSchema

        sch.MinDaysInYear === 365
        sch.MinDaysInMonth === 28
