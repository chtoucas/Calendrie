// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Validation.PreValidatorTests

open Calendrie.Core
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Faux

open Xunit

module Prelude =
    let badLunarProfile = FauxCalendricalSchema.NotLunar
    let badLunisolarProfile = FauxCalendricalSchema.NotLunisolar
    let badSolar12Profile = FauxCalendricalSchema.NotSolar12
    let badSolar13Profile = FauxCalendricalSchema.NotSolar13

    [<Fact>]
    let ``ICalendricalPreValidator.CreateDefault() "unreachable" case`` () =
        let sch = FauxCalendricalSchema.WithBadProfile

        ICalendricalPreValidator.CreateDefault(sch) |> is<PlainPreValidator>

    [<Fact>]
    let ``Constructor throws for null schema`` () =
        // GregorianPreValidator: singleton, no public ctor
        // JulianPreValidator:    singleton, no public ctor
        nullExn "schema" (fun () -> new LunarPreValidator(null))
        nullExn "schema" (fun () -> new LunisolarPreValidator(null))
        nullExn "schema" (fun () -> new PaxPreValidator(null))
        nullExn "schema" (fun () -> new PlainPreValidator(null))
        nullExn "schema" (fun () -> new Solar12PreValidator(null))
        nullExn "schema" (fun () -> new Solar13PreValidator(null))

    [<Theory; MemberData(nameof(badLunarProfile))>]
    let ``LunarPreValidator constructor throws for non-lunar schema`` (sch) =
        argExn "schema" (fun () -> new LunarPreValidator(sch))

    [<Theory; MemberData(nameof(badLunisolarProfile))>]
    let ``LunisolarPreValidator constructor throws for non-lunisolar schema`` (sch) =
        argExn "schema" (fun () -> new LunisolarPreValidator(sch))

    [<Theory; MemberData(nameof(badSolar12Profile))>]
    let ``Solar12PreValidator constructor throws for non-solar12 schema`` (sch) =
        argExn "schema" (fun () -> new Solar12PreValidator(sch))

    [<Theory; MemberData(nameof(badSolar13Profile))>]
    let ``Solar13PreValidator constructor throws for non-solar13 schema`` (sch) =
        argExn "schema" (fun () -> new Solar13PreValidator(sch))
