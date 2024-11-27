// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Schemas.FeaturetteTestSuite

open Calendrie.Core.Schemas
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core

module BlankDay =
    [<Sealed>]
    type InternationalFixedTests() =
        inherit IBlankDayFeaturetteFacts<InternationalFixedSchema, InternationalFixedDataSet>(new InternationalFixedSchema())

    [<Sealed>]
    type PositivistTests() =
        inherit IBlankDayFeaturetteFacts<PositivistSchema, PositivistDataSet>(new PositivistSchema())

    [<Sealed>]
    type WorldTests() =
        inherit IBlankDayFeaturetteFacts<WorldSchema, WorldDataSet>(new WorldSchema())

module EpagomenalDay =
    [<Sealed>]
    type Coptic12Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Coptic12Schema, Coptic12DataSet>(new Coptic12Schema())

    [<Sealed>]
    type Coptic13Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Coptic13Schema, Coptic13DataSet>(new Coptic13Schema())

    [<Sealed>]
    type Egyptian12Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Egyptian12Schema, Egyptian12DataSet>(new Egyptian12Schema())

    [<Sealed>]
    type Egyptian13Tests() =
        inherit IEpagomenalDayFeaturetteFacts<Egyptian13Schema, Egyptian13DataSet>(new Egyptian13Schema())

    [<Sealed>]
    type FrenchRepublican12Tests() =
        inherit IEpagomenalDayFeaturetteFacts<FrenchRepublican12Schema, FrenchRepublican12DataSet>(new FrenchRepublican12Schema())

    [<Sealed>]
    type FrenchRepublican13Tests() =
        inherit IEpagomenalDayFeaturetteFacts<FrenchRepublican13Schema, FrenchRepublican13DataSet>(new FrenchRepublican13Schema())
