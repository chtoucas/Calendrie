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
        scope.YearsValidator.Range === seg.SupportedYears
