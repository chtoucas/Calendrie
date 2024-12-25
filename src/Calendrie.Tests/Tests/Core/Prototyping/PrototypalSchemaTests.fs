// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Prototyping.PrototypalSchemaTests

open Calendrie.Core
open Calendrie.Core.Prototyping
open Calendrie.Core.Schemas
open Calendrie.Testing

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "kernel" is null`` () =
        nullExn "kernel" (fun () -> new PrototypalSchema(null, true, 1, 1))

    [<Fact>]
    let ``Constructor throws when "minDaysInYear" is <= 0`` () =
        outOfRangeExn "minDaysInYear" (fun () -> new PrototypalSchema(new GregorianSchema(), true, 0, 1))
        outOfRangeExn "minDaysInYear" (fun () -> new PrototypalSchema(new GregorianSchema(), true, -1, 1))

    [<Fact>]
    let ``Constructor throws when "minDaysInMonth" is <= 0`` () =
        outOfRangeExn "minDaysInMonth" (fun () -> new PrototypalSchema(new GregorianSchema(), true, 1, 0))
        outOfRangeExn "minDaysInMonth" (fun () -> new PrototypalSchema(new GregorianSchema(), true, 1, -1))

    [<Fact>]
    let ``Constructor does not throw when "minDaysInYear" and "minDaysInMonth" are > 0`` () =
        new PrototypalSchema(new GregorianSchema(), true, 2, 1) |> ignore

    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new PrototypalSchema(null))

    [<Fact>]
    let ``Properties from constructor`` () =
        let proto = new PrototypalSchema(new GregorianSchema())
        let sch = proto :> ICalendricalSchema

        sch.MinDaysInYear === 365
        sch.MinDaysInMonth === 28
