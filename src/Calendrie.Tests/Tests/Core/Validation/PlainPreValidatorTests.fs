﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Validation.PlainPreValidatorTests

open System

open Calendrie.Core.Schemas
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Data.Schemas
open Calendrie.Testing.Facts.Core

// PlainPreValidator being quite simple, we do no test it with all types of
// schemas. We use the Copic13 schema because it may overflow when calling
// CountDaysInYear() or CountDaysInMonth(). Both rely on IsLeapYear() which
// overflows at Int32.MaxYear.
let private sch = new Coptic13Schema()
let private supportedYears = sch.SupportedYears

[<Sealed>]
type Copic13Tests() =
    inherit ICalendricalPreValidatorFacts<Coptic13DataSet>(
        new PlainPreValidator(sch),
        supportedYears.Min,
        supportedYears.Max)

    override x.ValidateMonthDay_AtAbsoluteMaxYear() =
        let validator = x.PreValidatorUT
        (fun () -> validator.ValidateMonthDay(Int32.MaxValue, 1, 1)) |> overflows

    override x.ValidateDayOfYear_AtAbsoluteMaxYear() =
        // The base method works fine but we want to show that the method may
        // overflow with other parameters.
        let validator = x.PreValidatorUT
        // At the start of the year, the next method does not overflow because
        // we are below the limit Coptic13Schema.MinDaysInYear.
        validator.ValidateDayOfYear(Int32.MaxValue, 1)
        // We use 366 to be sure that dayOfYear > Coptic13Schema.MinDaysInYear = 365.
        (fun () -> validator.ValidateDayOfYear(Int32.MaxValue, 366)) |> overflows

