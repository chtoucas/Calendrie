// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.FeaturetteTestSuite

open Calendrie.Core.Schemas
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core

module BlankDay =
    [<Sealed>]
    type InternationalFixedTests() =
        inherit IBlankDayFeaturetteFacts<InternationalFixedSchema, InternationalFixedDataSet>(schemaOf<InternationalFixedSchema>())

    [<Sealed>]
    type PositivistTests() =
        inherit IBlankDayFeaturetteFacts<PositivistSchema, PositivistDataSet>(schemaOf<PositivistSchema>())

    [<Sealed>]
    type WorldTests() =
        inherit IBlankDayFeaturetteFacts<WorldSchema, WorldDataSet>(schemaOf<WorldSchema>())

module EpagomenalDay =
    [<Sealed>]
    type Coptic12Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Coptic12Schema, Coptic12DataSet>(schemaOf<Coptic12Schema>())

    [<Sealed>]
    type Coptic13Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Coptic13Schema, Coptic13DataSet>(schemaOf<Coptic13Schema>())

    [<Sealed>]
    type Egyptian12Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Egyptian12Schema, Egyptian12DataSet>(schemaOf<Egyptian12Schema>())

    [<Sealed>]
    type Egyptian13Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Egyptian13Schema, Egyptian13DataSet>(schemaOf<Egyptian13Schema>())

    [<Sealed>]
    type FrenchRepublican12Tests() =
        inherit IEpagomenalDayFeaturetteFacts<FrenchRepublican12Schema, FrenchRepublican12DataSet>(schemaOf<FrenchRepublican12Schema>())

    [<Sealed>]
    type FrenchRepublican13Tests() =
        inherit IEpagomenalDayFeaturetteFacts<FrenchRepublican13Schema, FrenchRepublican13DataSet>(schemaOf<FrenchRepublican13Schema>())
