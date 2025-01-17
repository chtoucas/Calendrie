﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Samples.MyGregorianDateFacts

open Calendrie.Core.Intervals
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology
open Calendrie.Testing.Faux

// Test IDateable and IAbsoluteDate default implementation methods.

let domain = Range.Create(SimpleGregorianDate.MinValue.DayNumber, SimpleGregorianDate.MaxValue.DayNumber)

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type DateFacts() =
    inherit IDateFacts<SimpleGregorianDate, StandardGregorianDataSet>()

    // TDate.FromDayNumber() throws an OverflowException here but only because
    // the base test uses the explicit implementation of FromDayNumber().
    override x.FromDayNumber_InvalidDayNumber () =
        x.DomainTester.TestInvalidDayNumber(SimpleGregorianDate.FromDayNumber);

