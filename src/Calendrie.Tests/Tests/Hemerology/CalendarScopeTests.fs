// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Hemerology.CalendarScopeTests

open Calendrie
open Calendrie.Core
open Calendrie.Core.Intervals
open Calendrie.Core.Schemas
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
        let supportedYears = Segment.Create(1, 4)
        let scope = new FauxCalendarScope(epoch, CalendricalSegment.Create(sch, supportedYears))

        let minDayNumber = epoch + sch.GetStartOfYear(supportedYears.Min)
        let maxDayNumber = epoch + sch.GetEndOfYear(supportedYears.Max)
        let range = Segment.Create(minDayNumber, maxDayNumber)

        scope.Domain === range

    [<Fact>]
    let ``Property Segment`` () =
        let sch = new GregorianSchema()
        let seg = CalendricalSegment.Create(sch, Segment.Create(1, 4))
        let scope = new FauxCalendarScope(seg)

        scope.Segment ==& seg

module DayNumberValidation =
    let seg = CalendricalSegment.Create(new GregorianSchema(), Segment.Create(1, 2))
    let scope = new FauxCalendarScope(DayZero.NewStyle, seg)

    [<Fact>]
    let ``Check test values`` () =
        seg.SupportedDays.Min === 0
        seg.SupportedDays.Max === 729

    [<Fact>]
    let ``Validate()`` () =
        outOfRangeExn "dayNumber" (fun () -> scope.Validate(DayNumber.Zero - 1))
        scope.Validate(DayNumber.Zero)
        scope.Validate(DayNumber.Zero + 729)
        outOfRangeExn "dayNumber" (fun () -> scope.Validate(DayNumber.Zero + 730))

    [<Fact>]
    let ``CheckOverflow()`` () =
        (fun () -> scope.CheckOverflow(DayNumber.Zero - 1)) |> overflows
        scope.CheckOverflow(DayNumber.Zero)
        scope.CheckOverflow(DayNumber.Zero + 729)
        (fun () -> scope.CheckOverflow(DayNumber.Zero + 730)) |> overflows

    [<Fact>]
    let ``CheckUpperBound()`` () =
        scope.CheckUpperBound(DayNumber.Zero - 1)
        scope.CheckUpperBound(DayNumber.Zero)
        scope.CheckUpperBound(DayNumber.Zero + 729)
        (fun () -> scope.CheckUpperBound(DayNumber.Zero + 730)) |> overflows

    [<Fact>]
    let ``CheckLowerBound()`` () =
        (fun () -> scope.CheckLowerBound(DayNumber.Zero - 1)) |> overflows
        scope.CheckLowerBound(DayNumber.Zero)
        scope.CheckLowerBound(DayNumber.Zero + 729)
        scope.CheckLowerBound(DayNumber.Zero + 730)
