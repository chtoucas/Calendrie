﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.CalendricalSegmentTests

open Calendrie
open Calendrie.Core
open Calendrie.Core.Intervals
open Calendrie.Core.Schemas
open Calendrie.Testing
open Calendrie.Testing.Faux

open Xunit

module Prelude =
    [<Fact>]
    let ``Property Schema`` () =
        let sch = new GregorianSchema()
        let seg = CalendricalSegment.Create(sch, Segment.Create(1, 4))

        seg.Schema ==& sch

    [<Fact>]
    let ``ToString()`` () =
        let min = new DateParts(5, 7, 11)
        let max = new DateParts(13, 5, 3)
        let pair = OrderedPair.Create(min, max)
        let builder = new CalendricalSegmentBuilder(new GregorianSchema())
        builder.MinDateParts <- min
        builder.MaxDateParts <- max
        let seg = builder.BuildSegment()

        seg.ToString() === pair.ToString()

module Factories =

    //
    // Create(range)
    //

    [<Fact>]
    let ``Create(range) throws for null schema`` () =
        nullExn "schema" (fun () -> CalendricalSegment.Create(null, Segment.Create(1, 4)))

    [<Fact>]
    let ``Create(range) throws when range is not a subinterval of schema.SupportedYears`` () =
        let sch = new FauxCalendricalSchema(Segment.Create(5, 10))

        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Segment.Create(3, 4)))
        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Segment.Create(3, 5)))
        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Segment.Create(3, 6)))
        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Segment.Create(9, 12)))
        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Segment.Create(10, 12)))
        argExn "supportedYears" (fun () -> CalendricalSegment.Create(sch, Segment.Create(11, 12)))

    [<Fact>]
    let ``Create(range)`` () =
        let sch = new GregorianSchema()
        let range = Segment.Create(1, 2)
        let seg = CalendricalSegment.Create(sch, range)

        let minMaxDateParts = OrderedPair.Create(
            new DateParts(range.Min, 1, 1),
            new DateParts(range.Max, 12, 31))

        let minMaxOrdinalParts = OrderedPair.Create(
            new OrdinalParts(range.Min, 1),
            new OrdinalParts(range.Max, 365))

        let minMaxMonthParts = OrderedPair.Create(
            new MonthParts(range.Min, 1),
            new MonthParts(range.Max, 12))

        seg.IsComplete |> ok

        seg.SupportedDays   === Segment.Create(0, 729)
        seg.SupportedMonths === Segment.Create(0, 23)
        seg.SupportedYears  === range

        seg.MinMaxDateParts    === minMaxDateParts
        seg.MinMaxOrdinalParts === minMaxOrdinalParts
        seg.MinMaxMonthParts   === minMaxMonthParts

    //
    // CreateMaximal()
    //

    [<Fact>]
    let ``CreateMaximal() throws for null schema`` () =
        nullExn "schema" (fun () -> CalendricalSegment.CreateMaximal(null))

    [<Fact>]
    let ``CreateMaximal()`` () =
        let range = Segment.Create(-10, 10)
        let seg = CalendricalSegment.CreateMaximal(new FauxCalendricalSchema(range))

        seg.IsComplete |> ok
        seg.SupportedYears === range

    [<Fact>]
    let ``CreateMaximal() when the schema only supports years <= 0`` () =
        let range = Segment.Create(-10, 0)
        let seg = CalendricalSegment.CreateMaximal(new FauxCalendricalSchema(range))

        seg.IsComplete |> ok
        seg.SupportedYears === range

    [<Fact>]
    let ``CreateMaximal() when the schema only supports years > 0`` () =
        let range = Segment.Create(5, 10)
        let seg = CalendricalSegment.CreateMaximal(new FauxCalendricalSchema(range))

        seg.IsComplete |> ok
        seg.SupportedYears === range

    //
    // CreateMaximalOnOrAfterYear1()
    //

    [<Fact>]
    let ``CreateMaximalOnOrAfterYear1() throws for null schema`` () =
        nullExn "schema" (fun () -> CalendricalSegment.CreateMaximalOnOrAfterYear1(null))

    [<Fact>]
    let ``CreateMaximalOnOrAfterYear1() throws when the schema only supports years <= 0`` () =
        let range = Segment.Create(-10, 0)

        argExn "schema" (fun () -> CalendricalSegment.CreateMaximalOnOrAfterYear1(new FauxCalendricalSchema(range)))

    [<Fact>]
    let ``CreateMaximalOnOrAfterYear1()`` () =
        let range = Segment.Create(-10, 10)
        let seg = CalendricalSegment.CreateMaximalOnOrAfterYear1(new FauxCalendricalSchema(range))

        seg.IsComplete |> ok
        seg.SupportedYears === Segment.Create(1, 10)

    [<Fact>]
    let ``CreateMaximalOnOrAfterYear1() when the schema only supports years > 0`` () =
        let range = Segment.Create(5, 10)
        let seg = CalendricalSegment.CreateMaximalOnOrAfterYear1(new FauxCalendricalSchema(range))

        seg.IsComplete |> ok
        seg.SupportedYears === range
