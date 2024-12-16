// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.CalendarScopeTests

open Calendrie
open Calendrie.Core
open Calendrie.Core.Intervals
open Calendrie.Core.Schemas
open Calendrie.Core.Validation
open Calendrie.Testing
open Calendrie.Testing.Faux

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "segment" is null`` () =
        nullExn "segment" (fun () -> new FauxCalendarScope(null))

    [<Fact>]
    let ``Property Epoch`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let scope = new FauxCalendarScope(new FauxCalendricalSchema(), epoch, 1, 4)

        scope.Epoch === epoch

    [<Fact>]
    let ``Property Domain`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let sch = new GregorianSchema()
        let supportedYears = Range.Create(1, 4)
        let scope = new FauxCalendarScope(epoch, CalendricalSegment.Create(sch, supportedYears))

        let minDayNumber = epoch + sch.GetStartOfYear(supportedYears.Min)
        let maxDayNumber = epoch + sch.GetEndOfYear(supportedYears.Max)
        let range = Range.Create(minDayNumber, maxDayNumber)

        scope.Domain === range

    [<Fact>]
    let ``Property Segment and related properties`` () =
        let sch = new GregorianSchema()
        let seg = CalendricalSegment.Create(sch, Range.Create(1, 4))
        let scope = new FauxCalendarScope(seg)

        scope.Segment ==& seg
        // It's enough to check the property Range.
        //scope.YearsValidator.Range === seg.SupportedYears

module DayNumberValidation =
    [<Fact>]
    let ``Validate()`` () =
        let range = Range.Create(DayNumber.Zero, DayNumber.Zero + 2)

        //outOfRangeExn "paramName" (fun () -> range.Validate(DayNumber.Zero - 1, "paramName"))
        outOfRangeExn "dayNumber" (fun () -> range.Validate(DayNumber.Zero - 1))
        range.Validate(DayNumber.Zero)
        range.Validate(DayNumber.Zero + 1)
        range.Validate(DayNumber.Zero + 2)
        outOfRangeExn "dayNumber" (fun () -> range.Validate(DayNumber.Zero + 3))
        //outOfRangeExn "paramName" (fun () -> range.Validate(DayNumber.Zero + 3, "paramName"))

    [<Fact>]
    let ``CheckOverflow()`` () =
        let range = Range.Create(DayNumber.Zero, DayNumber.Zero + 2)

        (fun () -> range.CheckOverflow(DayNumber.Zero - 1)) |> overflows
        range.CheckOverflow(DayNumber.Zero)
        range.CheckOverflow(DayNumber.Zero + 1)
        range.CheckOverflow(DayNumber.Zero + 2)
        (fun () -> range.CheckOverflow(DayNumber.Zero + 3)) |> overflows

    [<Fact>]
    let ``CheckUpperBound()`` () =
        let range = Range.Create(DayNumber.Zero, DayNumber.Zero + 2)

        range.CheckUpperBound(DayNumber.Zero - 1)
        range.CheckUpperBound(DayNumber.Zero)
        range.CheckUpperBound(DayNumber.Zero + 1)
        range.CheckUpperBound(DayNumber.Zero + 2)
        (fun () -> range.CheckUpperBound(DayNumber.Zero + 3)) |> overflows

    [<Fact>]
    let ``CheckLowerBound()`` () =
        let range = Range.Create(DayNumber.Zero, DayNumber.Zero + 2)

        (fun () -> range.CheckLowerBound(DayNumber.Zero - 1)) |> overflows
        range.CheckLowerBound(DayNumber.Zero)
        range.CheckLowerBound(DayNumber.Zero + 1)
        range.CheckLowerBound(DayNumber.Zero + 2)
        range.CheckLowerBound(DayNumber.Zero + 3)
