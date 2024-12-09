// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.SystemSchemaTestSuite

open System

open Calendrie
open Calendrie.Core
open Calendrie.Core.Intervals
open Calendrie.Core.Schemas
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core
open Calendrie.Testing.Faux

open Xunit

// TODO(code): Hebrew (unfinished, no data), Pax (unfinished) and lunisolar (fake) schema.

//let private verifyThatPreValidatorIs<'a> (sch: ICalendricalSchema) =
//    sch.PreValidator |> is<'a>

[<Sealed>]
type CivilTests() =
    inherit SystemSchemaFacts<StandardGregorianDataSet>(new CivilSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYears_Prop() =
        let range = SystemSchema.DefaultSupportedYears.WithMin(1)
        x.SchemaUT.SupportedYears === range

[<Sealed>]
type Coptic12Tests() =
    inherit SystemSchemaFacts<Coptic12DataSet>(new Coptic12Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.EndingAt(Int32.MaxValue - 1)

    [<Fact>]
    member x.``CountDaysInMonth() overflows when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        for m in 1 .. x.MaxMonth do
            (fun () -> sch.CountDaysInMonth(Int32.MaxValue, m)) |> overflows

[<Sealed>]
type Coptic13Tests() =
    inherit SystemSchemaFacts<Coptic13DataSet>(new Coptic13Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.EndingAt(Int32.MaxValue - 1)

    [<Fact>]
    member x.``CountDaysInMonth() overflows when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        for m in 1 .. x.MaxMonth do
            (fun () -> sch.CountDaysInMonth(Int32.MaxValue, m)) |> overflows

[<Sealed>]
type Egyptian12Tests() =
    inherit SystemSchemaFacts<Egyptian12DataSet>(new Egyptian12Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
type Egyptian13Tests() =
    inherit SystemSchemaFacts<Egyptian13DataSet>(new Egyptian13Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.AnnusVagus
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.None
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
type FrenchRepublican12Tests() =
    inherit SystemSchemaFacts<FrenchRepublican12DataSet>(new FrenchRepublican12Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
type FrenchRepublican13Tests() =
    inherit SystemSchemaFacts<FrenchRepublican13DataSet>(new FrenchRepublican13Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Other
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<PlainPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
type GregorianTests() =
    inherit SystemSchemaFacts<GregorianDataSet>(new GregorianSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<GregorianPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
type InternationalFixedTests() =
    inherit SystemSchemaFacts<InternationalFixedDataSet>(new InternationalFixedSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar13
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
type JulianTests() =
    inherit SystemSchemaFacts<JulianDataSet>(new JulianSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<JulianPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
type FauxLunisolarTests() =
    inherit SystemSchemaFacts<FauxLunisolarDataSet>(new FauxLunisolarSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Lunisolar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Months
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Lunisolar
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunisolarPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (false, 0)

[<Sealed>]
type Persian2820Tests() =
    inherit SystemSchemaFacts<Persian2820DataSet>(new Persian2820Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.StartingAt(Int32.MinValue + Persian2820Schema.Year0)

    [<Fact>]
    member x.``CountDaysInMonth() may overflow when y = Int32.MinValue``() =
        let sch = x.SchemaUT

        // No overflow if y >= Int32.MinValue + Persian2820Schema.Year0.
        // It is a bit more complicated, it depends on the value of month.

        // No underflow if m < 12.
        [for m in 1 .. 11 -> sch.CountDaysInMonth(Int32.MinValue, m)] |> ignore

        for y0 in 0 .. (Persian2820Schema.Year0 - 1) do
            for m in 12 .. x.MaxMonth do
                (fun () -> sch.CountDaysInMonth(Int32.MinValue + y0, m)) |> overflows

[<Sealed>]
type PositivistTests() =
    inherit SystemSchemaFacts<PositivistDataSet>(new PositivistSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar13
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar13PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 13)

[<Sealed>]
type TabularIslamicTests() =
    inherit SystemSchemaFacts<TabularIslamicDataSet>(new TabularIslamicSchema()) with

    let isnot12 m = m <> 12

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Lunar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Lunar
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<LunarPreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

    override x.SupportedYears_Prop() =
        x.SchemaUT.SupportedYears === Range.Create(-199_999, 200_000)

    override x.SupportedYearsCore_Prop() =
        x.SchemaUT.SupportedYearsCore === Range.Create(-199_999, 200_000)

    [<Fact>]
    member x.``CountDaysInMonth() may overflow when y = Int32.MinValue``() =
        let sch = x.SchemaUT
        let countDaysInMonth m = sch.CountDaysInMonth(Int32.MinValue, m)
        let maxMonth = x.MaxMonth

        (fun () -> countDaysInMonth 12) |> overflows
        // No overflow if m != 12.
        [1 .. maxMonth] |> List.filter isnot12 |> List.map countDaysInMonth

    [<Fact>]
    member x.``CountDaysInMonth() may overflow when y = Int32.MaxValue``() =
        let sch = x.SchemaUT
        let countDaysInMonth m = sch.CountDaysInMonth(Int32.MaxValue, m)
        let maxMonth = x.MaxMonth

        (fun () -> countDaysInMonth 12) |> overflows
        // No overflow if m != 12.
        [1 .. maxMonth] |> List.filter isnot12 |> List.map countDaysInMonth

[<Sealed>]
type TropicaliaTests() =
    inherit SystemSchemaFacts<TropicaliaDataSet>(new TropicaliaSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
type Tropicalia3031Tests() =
    inherit SystemSchemaFacts<Tropicalia3031DataSet>(new Tropicalia3031Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
type Tropicalia3130Tests() =
    inherit SystemSchemaFacts<Tropicalia3130DataSet>(new Tropicalia3130Schema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)

[<Sealed>]
type WorldTests() =
    inherit SystemSchemaFacts<WorldDataSet>(new WorldSchema())

    override x.Family_Prop() = x.SchemaUT.Family === CalendricalFamily.Solar
    override x.PeriodicAdjustments_Prop() = x.SchemaUT.PeriodicAdjustments === CalendricalAdjustments.Days
    override x.Profile_Prop() = x.SchemaUT.Profile === CalendricalProfile.Solar12
    override x.PreValidator_Prop() = x.VerifyThatPreValidatorIs<Solar12PreValidator>()
    override x.IsRegular() = x.SchemaUT.IsRegular() === (true, 12)
