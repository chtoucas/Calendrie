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
