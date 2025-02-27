﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.Intervals.IntervalTests

open Calendrie
open Calendrie.Core.Intervals
open Calendrie.Testing
open Calendrie.Testing.Data

open Xunit
open FsCheck.Xunit

// We also test IntervalExtra and Lavretni.

let private maprange (range: Segment<int>) =
    let endpoints = range.Endpoints.Select(fun i -> DayNumber.Zero + i)
    Segment.FromEndpoints(endpoints)
let private maprangeset (set: SegmentSet<int>) =
    if set.IsEmpty then
        SegmentSet<DayNumber>.Empty
    else
        let endpoints = set.Segment.Endpoints.Select(fun i -> DayNumber.Zero + i)
        SegmentSet.FromEndpoints(endpoints)
let private maplowerray (ray: LowerRay<int>) =
    LowerRay.EndingAt(DayNumber.Zero + ray.Max)
let private mapupperray (ray: UpperRay<int>) =
    UpperRay.StartingAt(DayNumber.Zero + ray.Min)

module Prelude =
    [<Property>]
    let ``Segment with itself`` (x: Pair<int>) =
        let v = Segment.Create(x.Min, x.Max)

        Interval.Intersect(v, v) === SegmentSet.Create(x.Min, x.Max)

        let coalesce = Interval.Coalesce(v, v)
        coalesce.HasValue |> ok
        coalesce.Value === v

        Interval.Span(v, v) === v
        Interval.Gap(v, v).IsEmpty  |> ok

        Interval.Disjoint(v, v)     |> nok
        Interval.Adjacent(v, v)     |> nok
        Interval.Connected(v, v)    |> ok

    [<Property>]
    let ``Segment with itself when singleton`` (i: int) =
        let v = Segment.Singleton(i)

        Interval.Intersect(v, v) === SegmentSet.Create(i, i)

        let coalesce = Interval.Coalesce(v, v)
        coalesce.HasValue |> ok
        coalesce.Value === v

        Interval.Span(v, v) === v
        Interval.Gap(v, v).IsEmpty  |> ok

        Interval.Disjoint(v, v)     |> nok
        Interval.Adjacent(v, v)     |> nok
        Interval.Connected(v, v)    |> ok

    [<Property>]
    let ``LowerRay with itself`` (i: int) =
        let v = LowerRay.EndingAt(i)

        Interval.Intersect(v, v) === v
        Interval.Union(v, v) === v

        IntervalExtra.Coalesce(v, v) === v
        IntervalExtra.Span(v, v) === v
        IntervalExtra.Gap(v, v).IsEmpty  |> ok

        IntervalExtra.Disjoint(v, v)    |> nok
        IntervalExtra.Adjacent(v, v)    |> nok
        IntervalExtra.Connected(v, v)   |> ok

    [<Property>]
    let ``UpperRay with itself`` (i: int) =
        let v = UpperRay.StartingAt(i)

        Interval.Intersect(v, v) === v
        Interval.Union(v, v) === v

        IntervalExtra.Coalesce(v, v) === v
        IntervalExtra.Span(v, v) === v
        IntervalExtra.Gap(v, v).IsEmpty  |> ok

        IntervalExtra.Disjoint(v, v)    |> nok
        IntervalExtra.Adjacent(v, v)    |> nok
        IntervalExtra.Connected(v, v)   |> ok

// What we test in SetOps:
// - Span / Union
// - Intersect
// - Gap (specialized)
// - Disjoint
// - Adjacent (specialized)
// - Connected (specialized)
// - Coalesce (specialized)
module SetOps =
    let segmentSegmentInfoData = IntervalDataSet.SegmentSegmentInfoData
    let lowerRaySegmentInfoData = IntervalDataSet.LowerRaySegmentInfoData
    let upperRaySegmentInfoData = IntervalDataSet.UpperRaySegmentInfoData
    let lowerRayUpperRayInfoData = IntervalDataSet.LowerRayUpperRayInfoData

    //
    // Segment and Segment
    //

    [<Theory; MemberData(nameof(segmentSegmentInfoData))>]
    let ``(Segment, Segment)`` (data: SegmentSegmentInfo) =
        let v = data.First
        let w = data.Second
        let span  = data.Span
        let inter = data.Intersection
        let gap   = data.Gap

        Interval.Span(v, w) === span
        Interval.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Interval.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Interval.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Interval.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Interval.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Interval.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> Assert.Null
            Interval.Coalesce(w, v) |> Assert.Null

    [<Theory; MemberData(nameof(segmentSegmentInfoData))>]
    let ``(Segment, Segment) for DayNumber's`` (data: SegmentSegmentInfo) =
        let v = data.First              |> maprange
        let w = data.Second             |> maprange
        let span  = data.Span           |> maprange
        let inter = data.Intersection   |> maprangeset
        let gap   = data.Gap            |> maprangeset

        Interval.Span(v, w) === span
        Interval.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Interval.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Interval.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Interval.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Interval.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Interval.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(w, v) |> Assert.Null
            Interval.Coalesce(v, w) |> Assert.Null

    //
    // Segment and LowerRay
    //

    [<Theory; MemberData(nameof(lowerRaySegmentInfoData))>]
    let ``(Segment, LowerRay)`` (data: LowerRaySegmentInfo) =
        let w = data.First
        let v = data.Second
        let span  = data.Span
        let inter = data.Intersection
        let gap   = data.Gap

        Interval.Span(v, w) === span
        Lavretni.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Lavretni.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> Assert.Null
            Lavretni.Coalesce(w, v) |> Assert.Null

    [<Theory; MemberData(nameof(lowerRaySegmentInfoData))>]
    let ``(Segment, LowerRay) for DayNumber's`` (data: LowerRaySegmentInfo) =
        let w = data.First              |> maplowerray
        let v = data.Second             |> maprange
        let span  = data.Span           |> maplowerray
        let inter = data.Intersection   |> maprangeset
        let gap   = data.Gap            |> maprangeset

        Interval.Span(v, w) === span
        Lavretni.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Lavretni.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> Assert.Null
            Lavretni.Coalesce(w, v) |> Assert.Null

    //
    // Segment and UpperRay
    //

    [<Theory; MemberData(nameof(upperRaySegmentInfoData))>]
    let ``(Segment, UpperRay)`` (data: UpperRaySegmentInfo) =
        let w = data.First
        let v = data.Second
        let span  = data.Span
        let inter = data.Intersection
        let gap   = data.Gap

        Interval.Span(v, w) === span
        Lavretni.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Lavretni.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> Assert.Null
            Lavretni.Coalesce(w, v) |> Assert.Null

    [<Theory; MemberData(nameof(upperRaySegmentInfoData))>]
    let ``(Segment, UpperRay) for DayNumber's`` (data: UpperRaySegmentInfo) =
        let w = data.First              |> mapupperray
        let v = data.Second             |> maprange
        let span  = data.Span           |> mapupperray
        let inter = data.Intersection   |> maprangeset
        let gap   = data.Gap            |> maprangeset

        Interval.Span(v, w) === span
        Lavretni.Span(w, v) === span

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

        if data.Connected then
            Interval.Coalesce(v, w).Value === span
            Lavretni.Coalesce(w, v).Value === span
        else
            Interval.Coalesce(v, w) |> Assert.Null
            Lavretni.Coalesce(w, v) |> Assert.Null

    //
    // LowerRay and UpperRay
    //

    [<Theory; MemberData(nameof(lowerRayUpperRayInfoData))>]
    let ``(LowerRay, UpperRay)`` (data: LowerRayUpperRayInfo) =
        let v = data.First
        let w = data.Second
        let inter = data.Intersection
        let gap   = data.Gap

        IntervalExtra.Span(v, w) === Unbounded<int>.Instance
        Lavretni.Span(w, v)      === Unbounded<int>.Instance

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

        if data.Connected then
            IntervalExtra.Coalesce(v, w) === Unbounded<int>.Instance
            Lavretni.Coalesce(w, v)      === Unbounded<int>.Instance
        else
            IntervalExtra.Coalesce(v, w) |> isnull
            Lavretni.Coalesce(w, v)      |> isnull

    [<Theory; MemberData(nameof(lowerRayUpperRayInfoData))>]
    let ``(LowerRay, UpperRay) for DayNumber's`` (data: LowerRayUpperRayInfo) =
        let v = data.First              |> maplowerray
        let w = data.Second             |> mapupperray
        let inter = data.Intersection   |> maprangeset
        let gap   = data.Gap            |> maprangeset

        IntervalExtra.Span(v, w) === Unbounded<DayNumber>.Instance
        Lavretni.Span(w, v)      === Unbounded<DayNumber>.Instance

        Interval.Intersect(v, w) === inter
        Lavretni.Intersect(w, v) === inter

        Interval.Gap(v, w) === gap
        Lavretni.Gap(w, v) === gap

        Interval.Disjoint(v, w) === data.Disjoint
        Lavretni.Disjoint(w, v) === data.Disjoint

        Interval.Adjacent(v, w) === data.Adjacent
        Lavretni.Adjacent(w, v) === data.Adjacent

        Interval.Connected(v, w) === data.Connected
        Lavretni.Connected(w, v) === data.Connected

        if data.Connected then
            IntervalExtra.Coalesce(v, w) === Unbounded<DayNumber>.Instance
            Lavretni.Coalesce(w, v)      === Unbounded<DayNumber>.Instance
        else
            IntervalExtra.Coalesce(v, w) |> isnull
            Lavretni.Coalesce(w, v)      |> isnull

    //
    // LowerRay and LowerRay
    //

    [<Theory>]
    [<InlineData(1, 1, 1, 1)>]
    [<InlineData(1, 4, 4, 1)>]
    let ``(LowerRay, LowerRay)`` (i: int) j m n =
        let v = LowerRay.EndingAt(i)
        let w = LowerRay.EndingAt(j)
        let union = LowerRay.EndingAt(m)
        let inter = LowerRay.EndingAt(n)

        Interval.Union(v, w) === union
        Interval.Union(w, v) === union

        IntervalExtra.Span(v, w) === union
        IntervalExtra.Span(w, v) === union

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

        IntervalExtra.Gap(v, w).IsEmpty |> ok
        IntervalExtra.Gap(w, v).IsEmpty |> ok

        IntervalExtra.Disjoint(v, w) |> nok
        IntervalExtra.Disjoint(w, v) |> nok

        IntervalExtra.Adjacent(v, w) |> nok
        IntervalExtra.Adjacent(w, v) |> nok

        IntervalExtra.Connected(v, w) |> ok
        IntervalExtra.Connected(w, v) |> ok

        IntervalExtra.Coalesce(v, w) === union
        IntervalExtra.Coalesce(w, v) === union

    [<Theory>]
    [<InlineData(1, 1, 1, 1)>]
    [<InlineData(1, 4, 4, 1)>]
    let ``(LowerRay, LowerRay)  for DayNumber's`` (i: int) j m n =
        let v = LowerRay.EndingAt(i)        |> maplowerray
        let w = LowerRay.EndingAt(j)        |> maplowerray
        let union = LowerRay.EndingAt(m)    |> maplowerray
        let inter = LowerRay.EndingAt(n)    |> maplowerray

        Interval.Union(v, w) === union
        Interval.Union(w, v) === union

        IntervalExtra.Span(v, w) === union
        IntervalExtra.Span(w, v) === union

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

        IntervalExtra.Gap(v, w).IsEmpty |> ok
        IntervalExtra.Gap(w, v).IsEmpty |> ok

        IntervalExtra.Disjoint(v, w) |> nok
        IntervalExtra.Disjoint(w, v) |> nok

        IntervalExtra.Adjacent(v, w) |> nok
        IntervalExtra.Adjacent(w, v) |> nok

        IntervalExtra.Connected(v, w) |> ok
        IntervalExtra.Connected(w, v) |> ok

        IntervalExtra.Coalesce(v, w) === union
        IntervalExtra.Coalesce(w, v) === union

    //
    // UpperRay and UpperRay
    //

    [<Theory>]
    [<InlineData(1, 1, 1, 1)>]
    [<InlineData(1, 4, 1, 4)>]
    let ``(UpperRay, UpperRay)`` (i: int) j m n =
        let v = UpperRay.StartingAt(i)
        let w = UpperRay.StartingAt(j)
        let union = UpperRay.StartingAt(m)
        let inter = UpperRay.StartingAt(n)

        Interval.Union(v, w) === union
        Interval.Union(w, v) === union

        IntervalExtra.Span(v, w) === union
        IntervalExtra.Span(w, v) === union

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

        IntervalExtra.Gap(v, w).IsEmpty |> ok
        IntervalExtra.Gap(w, v).IsEmpty |> ok

        IntervalExtra.Disjoint(v, w) |> nok
        IntervalExtra.Disjoint(w, v) |> nok

        IntervalExtra.Adjacent(v, w) |> nok
        IntervalExtra.Adjacent(w, v) |> nok

        IntervalExtra.Connected(v, w) |> ok
        IntervalExtra.Connected(w, v) |> ok

        IntervalExtra.Coalesce(v, w) === union
        IntervalExtra.Coalesce(w, v) === union

    [<Theory>]
    [<InlineData(1, 1, 1, 1)>]
    [<InlineData(1, 4, 1, 4)>]
    let ``(UpperRay, UpperRay) for DayNumber's`` (i: int) j m n =
        let v = UpperRay.StartingAt(i)      |> mapupperray
        let w = UpperRay.StartingAt(j)      |> mapupperray
        let union = UpperRay.StartingAt(m)  |> mapupperray
        let inter = UpperRay.StartingAt(n)  |> mapupperray

        Interval.Union(v, w) === union
        Interval.Union(w, v) === union

        IntervalExtra.Span(v, w) === union
        IntervalExtra.Span(w, v) === union

        Interval.Intersect(v, w) === inter
        Interval.Intersect(w, v) === inter

        IntervalExtra.Gap(v, w).IsEmpty |> ok
        IntervalExtra.Gap(w, v).IsEmpty |> ok

        IntervalExtra.Disjoint(v, w) |> nok
        IntervalExtra.Disjoint(w, v) |> nok

        IntervalExtra.Adjacent(v, w) |> nok
        IntervalExtra.Adjacent(w, v) |> nok

        IntervalExtra.Connected(v, w) |> ok
        IntervalExtra.Connected(w, v) |> ok

        IntervalExtra.Coalesce(v, w) === union
        IntervalExtra.Coalesce(w, v) === union
