// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.PreValidatorTestSuite

open System

open Calendrie.Core.Schemas
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core
open Calendrie.Testing.Faux

// TODO(code): Hebrew (unfinished, no data) and lunisolar (fake) schema.

// Solar12PreValidator
[<Sealed>]
type Coptic12Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic12DataSet>(schemaOf<Coptic12Schema>())

    member x.PreValidator() = x.PreValidatorUT |> is<Solar12PreValidator>

// PlainPreValidator
[<Sealed>]
type Coptic13Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic13DataSet>(schemaOf<Coptic13Schema>())

    member x.PreValidator() = x.PreValidatorUT |> is<PlainPreValidator>

    override x.ValidateMonthDay_AtAbsoluteMaxYear() =
        let validator = x.PreValidatorUT
        // PlainPreValidator: no shortcut for short values of the day of the
        // month and Coptic13Schema.CountDaysInMonth() overflows.
        (fun () -> validator.ValidateMonthDay(Int32.MaxValue, 1, 1)) |> overflows

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian12Tests() =
    inherit ICalendricalPreValidatorFacts<Egyptian12DataSet>(schemaOf<Egyptian12Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Egyptian13Tests() =
    inherit ICalendricalPreValidatorFacts<Egyptian13DataSet>(schemaOf<Egyptian13Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican12Tests() =
    inherit ICalendricalPreValidatorFacts<FrenchRepublican12DataSet>(schemaOf<FrenchRepublican12Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type FrenchRepublican13Tests() =
    inherit ICalendricalPreValidatorFacts<FrenchRepublican13DataSet>(schemaOf<FrenchRepublican13Schema>())

// GregorianPreValidator
[<Sealed>]
type GregorianTests() =
    inherit ICalendricalPreValidatorFacts<GregorianDataSet>(schemaOf<GregorianSchema>())

    member x.PreValidator() = x.PreValidatorUT |> is<GregorianPreValidator>

// Solar13PreValidator
[<Sealed>]
type InternationalFixedTests() =
    inherit ICalendricalPreValidatorFacts<InternationalFixedDataSet>(schemaOf<InternationalFixedSchema>())

    member x.PreValidator() = x.PreValidatorUT |> is<Solar13PreValidator>

// JulianPreValidator
[<Sealed>]
type JulianTests() =
    inherit ICalendricalPreValidatorFacts<JulianDataSet>(schemaOf<JulianSchema>())

    member x.PreValidator() = x.PreValidatorUT |> is<JulianPreValidator>

// LunisolarPreValidator
[<Sealed>]
type FauxLunisolarTests() =
    inherit ICalendricalPreValidatorFacts<FauxLunisolarDataSet>(schemaOf<FauxLunisolarSchema>())

    member x.PreValidator() = x.PreValidatorUT |> is<LunisolarPreValidator>

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Persian2820Tests() =
    inherit ICalendricalPreValidatorFacts<Persian2820DataSet>(schemaOf<Persian2820Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type PositivistTests() =
    inherit ICalendricalPreValidatorFacts<PositivistDataSet>(schemaOf<PositivistSchema>())

// LunarPreValidator
[<Sealed>]
type TabularIslamicTests() =
    inherit ICalendricalPreValidatorFacts<TabularIslamicDataSet>(schemaOf<TabularIslamicSchema>())

    member x.PreValidator() = x.PreValidatorUT |> is<LunarPreValidator>

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type TropicaliaTests() =
    inherit ICalendricalPreValidatorFacts<TropicaliaDataSet>(schemaOf<TropicaliaSchema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Tropicalia3031Tests() =
    inherit ICalendricalPreValidatorFacts<Tropicalia3031DataSet>(schemaOf<Tropicalia3031Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type Tropicalia3130Tests() =
    inherit ICalendricalPreValidatorFacts<Tropicalia3130DataSet>(schemaOf<Tropicalia3130Schema>())

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type WorldTests() =
    inherit ICalendricalPreValidatorFacts<WorldDataSet>(schemaOf<WorldSchema>())
