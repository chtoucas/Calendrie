﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Samples.MyGregorianDateFacts

open Calendrie.Core.Intervals
open Calendrie.Testing
open Calendrie.Testing.Data.Bounded
open Calendrie.Testing.Facts.Hemerology

open Samples

// Test IAbsolute static methods.

let domain = Range.Create(MyGregorianDate.MinValue.DayNumber, MyGregorianDate.MaxValue.DayNumber)

[<Sealed>]
[<TestExcludeFrom(TestExcludeFrom.Regular)>]
type DateFacts() =
    inherit IDateFacts<MyGregorianDate, StandardGregorianDataSet>(domain)

    override __.MinDate = MyGregorianDate.MinValue
    override __.MaxDate = MyGregorianDate.MaxValue

    override __.GetDate(y, m, d) = new MyGregorianDate(y, m, d)
