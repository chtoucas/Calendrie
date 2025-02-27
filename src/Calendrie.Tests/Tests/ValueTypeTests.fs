﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.ValueTypeTests

open System
open System.Runtime.InteropServices

open Calendrie
open Calendrie.Core
open Calendrie.Core.Intervals
open Calendrie.Hemerology
open Calendrie.Systems
open Calendrie.Testing
open Calendrie.Testing.Data

open Xunit

module RuntimeSizes =
    // The runtime size of a struct should stay <= 16 bytes.

    /// Returns the unmanaged size of a (generic) object in bytes.
    let private sizeof<'a when 'a : struct> () = Marshal.SizeOf(Unchecked.defaultof<'a>)

    [<Fact>]
    let ``Generic types`` () =
        // Reminder:
        // byte     1 byte (!)
        // int16    2 bytes
        // int32    4 bytes
        // int64    8 bytes
        // double   8 bytes

        // OrderedPair<T>, two T's.
        sizeof<OrderedPair<byte>>() === 2
        sizeof<OrderedPair<int16>>() === 4
        sizeof<OrderedPair<int32>>() === 8
        sizeof<OrderedPair<int64>>() === 16
        sizeof<OrderedPair<double>>() === 16
        // Segment<T>, two T's.
        sizeof<Segment<byte>>() === 2
        sizeof<Segment<int16>>() === 4
        sizeof<Segment<int32>>() === 8
        sizeof<Segment<int64>>() === 16
        sizeof<Segment<double>>() === 16
        // SegmentSet<T>, two T's and one int32.
        sizeof<SegmentSet<byte>>() === 8
        sizeof<SegmentSet<int16>>() === 8
        sizeof<SegmentSet<int32>>() === 12
        sizeof<SegmentSet<int64>>() === 24   // BIG struct
        sizeof<SegmentSet<double>>() === 24  // BIG struct
        // UpperRay<T>, one T.
        sizeof<UpperRay<byte>>() === 1
        sizeof<UpperRay<int16>>() === 2
        sizeof<UpperRay<int32>>() === 4
        sizeof<UpperRay<int64>>() === 8
        sizeof<UpperRay<double>>() === 8
        // LowerRay<T>, one T.
        sizeof<LowerRay<byte>>() === 1
        sizeof<LowerRay<int16>>() === 2
        sizeof<LowerRay<int32>>() === 4
        sizeof<LowerRay<int64>>() === 8
        sizeof<LowerRay<double>>() === 8

    [<Fact>]
    let ``Types in Calendrie`` () =
        Marshal.SizeOf(typedefof<DateParts>) === 12
        Marshal.SizeOf(typedefof<DayNumber>) === 4
        Marshal.SizeOf(typedefof<MonthParts>) === 8
        Marshal.SizeOf(typedefof<Ord>) === 4
        Marshal.SizeOf(typedefof<OrdinalParts>) === 8

    [<Fact>]
    let ``Types in Calendrie:Core`` () =
        Marshal.SizeOf(typedefof<Yedoy>) === 4
        Marshal.SizeOf(typedefof<Yemo>) === 4
        Marshal.SizeOf(typedefof<Yemoda>) === 4
        Marshal.SizeOf(typedefof<Yewe>) === 4

    [<Fact>]
    let ``Types in Calendrie:Systems`` () =
        Marshal.SizeOf(typedefof<ArmenianDate>) === 4
        Marshal.SizeOf(typedefof<ArmenianMonth>) === 4
        Marshal.SizeOf(typedefof<ArmenianYear>) === 2
        Marshal.SizeOf(typedefof<CivilDate>) === 4
        Marshal.SizeOf(typedefof<CivilMonth>) === 4
        Marshal.SizeOf(typedefof<CivilYear>) === 2
        Marshal.SizeOf(typedefof<CopticDate>) === 4
        Marshal.SizeOf(typedefof<CopticMonth>) === 4
        Marshal.SizeOf(typedefof<CopticYear>) === 2
        Marshal.SizeOf(typedefof<EgyptianDate>) === 4
        Marshal.SizeOf(typedefof<EgyptianMonth>) === 4
        Marshal.SizeOf(typedefof<EgyptianYear>) === 2
        Marshal.SizeOf(typedefof<EthiopicDate>) === 4
        Marshal.SizeOf(typedefof<EthiopicMonth>) === 4
        Marshal.SizeOf(typedefof<EthiopicYear>) === 2
        Marshal.SizeOf(typedefof<FrenchRepublicanDate>) === 4
        Marshal.SizeOf(typedefof<FrenchRepublicanMonth>) === 4
        Marshal.SizeOf(typedefof<FrenchRepublicanYear>) === 2
        Marshal.SizeOf(typedefof<GregorianDate>) === 4
        Marshal.SizeOf(typedefof<GregorianMonth>) === 4
        Marshal.SizeOf(typedefof<GregorianYear>) === 4
        Marshal.SizeOf(typedefof<InternationalFixedDate>) === 4
        Marshal.SizeOf(typedefof<InternationalFixedMonth>) === 4
        Marshal.SizeOf(typedefof<InternationalFixedYear>) === 2
        Marshal.SizeOf(typedefof<JulianDate>) === 4
        Marshal.SizeOf(typedefof<JulianMonth>) === 4
        Marshal.SizeOf(typedefof<JulianYear>) === 4
        Marshal.SizeOf(typedefof<PaxDate>) === 4
        Marshal.SizeOf(typedefof<PaxMonth>) === 4
        Marshal.SizeOf(typedefof<PaxYear>) === 2
        Marshal.SizeOf(typedefof<Persian2820Date>) === 4
        Marshal.SizeOf(typedefof<Persian2820Month>) === 4
        Marshal.SizeOf(typedefof<Persian2820Year>) === 2
        Marshal.SizeOf(typedefof<PositivistDate>) === 4
        Marshal.SizeOf(typedefof<PositivistMonth>) === 4
        Marshal.SizeOf(typedefof<PositivistYear>) === 2
        Marshal.SizeOf(typedefof<TabularIslamicDate>) === 4
        Marshal.SizeOf(typedefof<TabularIslamicMonth>) === 4
        Marshal.SizeOf(typedefof<TabularIslamicYear>) === 2
        Marshal.SizeOf(typedefof<TropicaliaDate>) === 4
        Marshal.SizeOf(typedefof<TropicaliaMonth>) === 4
        Marshal.SizeOf(typedefof<TropicaliaYear>) === 2
        Marshal.SizeOf(typedefof<WorldDate>) === 4
        Marshal.SizeOf(typedefof<WorldMonth>) === 4
        Marshal.SizeOf(typedefof<WorldYear>) === 2
        Marshal.SizeOf(typedefof<ZoroastrianDate>) === 4
        Marshal.SizeOf(typedefof<ZoroastrianMonth>) === 4
        Marshal.SizeOf(typedefof<ZoroastrianYear>) === 2

    // TODO(fact): test all models.
    // TODO(fact): add tests for the data types defined within THIS project.
    [<Fact>]
    let ``Types in Calendrie:Testing:Data`` () =
        Marshal.SizeOf(typedefof<YearMonthsSinceEpoch>) === 8
        Marshal.SizeOf(typedefof<YearDaysSinceEpoch>) === 8
        Marshal.SizeOf(typedefof<YearDayNumber>) === 8
        //
        Marshal.SizeOf(typedefof<MonthsSinceEpochInfo>) === 8
        Marshal.SizeOf(typedefof<DaysSinceEpochInfo>) === 8
        Marshal.SizeOf(typedefof<DaysSinceZeroInfo>) === 8
        Marshal.SizeOf(typedefof<DaysSinceRataDieInfo>) === 8
        Marshal.SizeOf(typedefof<DayNumberInfo>) === 8
        //
        Marshal.SizeOf(typedefof<DateInfo>) === 16
        Marshal.SizeOf(typedefof<MonthInfo>) === 16
        Marshal.SizeOf(typedefof<YearInfo>) === 12
        Marshal.SizeOf(typedefof<DecadeInfo>) === 12
        Marshal.SizeOf(typedefof<CenturyInfo>) === 12
        Marshal.SizeOf(typedefof<MillenniumInfo>) === 12
        Marshal.SizeOf(typedefof<DecadeOfCenturyInfo>) === 12
        //
        Marshal.SizeOf(typedefof<YemodaPair>) === 8
        Marshal.SizeOf(typedefof<YedoyPair>) === 8
        Marshal.SizeOf(typedefof<YemoPair>) === 8
        //
        sizeof<YemodaAnd<int>>() === 8
        sizeof<YemodaAnd<DayOfWeek>>() === 8
        sizeof<YemodaPairAnd<int>>() === 12
        sizeof<YemodaPairAnd<DayOfWeek>>() === 12
        sizeof<YedoyPairAnd<int>>() === 12
        sizeof<YemoPairAnd<int>>() === 12
        //
        Marshal.SizeOf(typedefof<OrdinalPartsPair>) === 16
        Marshal.SizeOf(typedefof<MonthPartsPair>) === 16

module DefaultValues =
    // Date types built upon DayNumber or Yemoda: 01/01/0001 (year 1)
    // For types not attached to a specific calendar, we always default to Gregorian.

    //
    // Types found in Calendrie
    //

    [<Fact>]
    let ``Default value of DayNumber is 01/01/0001 (Gregorian)`` () =
        let dayNumber = Unchecked.defaultof<DayNumber>
        let parts = dayNumber.GetGregorianParts()
        let y, m, d = parts.Deconstruct()

        (y, m, d) === (1, 1, 1)

    [<Fact>]
    let ``Default value of DateParts is 00/00/0000`` () =
        let parts = Unchecked.defaultof<DateParts>
        let y, m, d = parts.Deconstruct()

        (y, m, d) === (0, 0, 0)

    [<Fact>]
    let ``Default value of MonthParts is 00/0000`` () =
        let parts = Unchecked.defaultof<MonthParts>
        let y, m = parts.Deconstruct()

        (y, m) === (0, 0)

    [<Fact>]
    let ``Default value of OrdinalParts is 00/0000`` () =
        let parts = Unchecked.defaultof<OrdinalParts>
        let y, doy = parts.Deconstruct()

        (y, doy) === (0, 0)

    //
    // Calendrical parts found in Calendrie.Core
    //

    [<Fact>]
    let ``Default value of Yemoda is 01/01/0001`` () =
        let parts = Unchecked.defaultof<Yemoda>
        let y, m, d = parts.Deconstruct()

        (y, m, d) === (1, 1, 1)

    [<Fact>]
    let ``Default value of Yemo is 01/0001`` () =
        let parts = Unchecked.defaultof<Yemo>
        let y, m = parts.Deconstruct()

        (y, m) === (1, 1)

    [<Fact>]
    let ``Default value of Yedoy is 001/0001`` () =
        let parts = Unchecked.defaultof<Yedoy>
        let y, doy = parts.Deconstruct()

        (y, doy) === (1, 1)

    //
    // Date types found in Calendrie.Systems
    //

    [<Fact>]
    let ``Default value of CivilDate is 01/01/0001 (Gregorian-only)`` () =
        let date = Unchecked.defaultof<CivilDate>
        let y, m, d = date.Deconstruct()

        (y, m, d) === (1, 1, 1)

    [<Fact>]
    let ``Default value of GregorianDate is 01/01/0001 (Gregorian-only)`` () =
        let date = Unchecked.defaultof<GregorianDate>
        let y, m, d = date.Deconstruct()

        (y, m, d) === (1, 1, 1)

    [<Fact>]
    let ``Default value of JulianDate is 01/01/0001 (Julian-only)`` () =
        let date = Unchecked.defaultof<JulianDate>
        let y, m, d = date.Deconstruct()

        (y, m, d) === (1, 1, 1)
