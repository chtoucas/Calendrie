// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.ValueTypeTests

open System
open System.Runtime.InteropServices

open Calendrie
open Calendrie.Core
open Calendrie.Core.Intervals
open Calendrie.Core.Utilities
open Calendrie.Specialized
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
        // Range<T>, two T's.
        sizeof<Range<byte>>() === 2
        sizeof<Range<int16>>() === 4
        sizeof<Range<int32>>() === 8
        sizeof<Range<int64>>() === 16
        sizeof<Range<double>>() === 16
        // RangeSet<T>, two T's and one int32.
        sizeof<RangeSet<byte>>() === 8
        sizeof<RangeSet<int16>>() === 8
        sizeof<RangeSet<int32>>() === 12
        sizeof<RangeSet<int64>>() === 24   // BIG struct
        sizeof<RangeSet<double>>() === 24  // BIG struct
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
        Marshal.SizeOf(typedefof<AdditionRuleset>) === 12
        Marshal.SizeOf(typedefof<DateParts>) === 12
        Marshal.SizeOf(typedefof<DayNumber>) === 4
        Marshal.SizeOf(typedefof<MonthParts>) === 8
        Marshal.SizeOf(typedefof<Ord>) === 4
        Marshal.SizeOf(typedefof<OrdinalParts>) === 8

    [<Fact>]
    let ``Types in Calendrie:Core`` () =
        Marshal.SizeOf(typedefof<Yedoy>) === 4
        Marshal.SizeOf(typedefof<Yedoyx>) === 4
        Marshal.SizeOf(typedefof<Yemo>) === 4
        Marshal.SizeOf(typedefof<Yemoda>) === 4
        Marshal.SizeOf(typedefof<Yemodax>) === 4
        Marshal.SizeOf(typedefof<Yemox>) === 4

    [<Fact>]
    let ``Types in Calendrie:Specialized`` () =
        Marshal.SizeOf(typedefof<Armenian13Date>) === 4
        Marshal.SizeOf(typedefof<ArmenianDate>) === 4
        Marshal.SizeOf(typedefof<CivilDate>) === 4
        Marshal.SizeOf(typedefof<Coptic13Date>) === 4
        Marshal.SizeOf(typedefof<CopticDate>) === 4
        Marshal.SizeOf(typedefof<Ethiopic13Date>) === 4
        Marshal.SizeOf(typedefof<EthiopicDate>) === 4
        Marshal.SizeOf(typedefof<GregorianDate>) === 4
        Marshal.SizeOf(typedefof<JulianDate>) === 4
        Marshal.SizeOf(typedefof<TabularIslamicDate>) === 4
        Marshal.SizeOf(typedefof<WorldDate>) === 4
        Marshal.SizeOf(typedefof<Zoroastrian13Date>) === 4
        Marshal.SizeOf(typedefof<ZoroastrianDate>) === 4

    // TODO(code): test all models.
    // TODO(code): add tests for the data types defined within THIS project.
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
    let ``Default value of Yemodax is 01/01/0001 (0)`` () =
        let parts = Unchecked.defaultof<Yemodax>
        let y, m, d = parts.Deconstruct()

        (y, m, d) === (1, 1, 1)
        parts.Extra === 0

    [<Fact>]
    let ``Default value of Yemo is 01/0001`` () =
        let parts = Unchecked.defaultof<Yemo>
        let y, m = parts.Deconstruct()

        (y, m) === (1, 1)

    [<Fact>]
    let ``Default value of Yemox is 01/0001 (0)`` () =
        let parts = Unchecked.defaultof<Yemox>
        let y, m = parts.Deconstruct()

        (y, m) === (1, 1)
        parts.Extra === 0

    [<Fact>]
    let ``Default value of Yedoy is 001/0001`` () =
        let parts = Unchecked.defaultof<Yedoy>
        let y, doy = parts.Deconstruct()

        (y, doy) === (1, 1)

    [<Fact>]
    let ``Default value of Yedoyx is 001/0001 (0)`` () =
        let parts = Unchecked.defaultof<Yedoyx>
        let y, doy = parts.Deconstruct()

        (y, doy) === (1, 1)
        parts.Extra === 0

    //
    // Date types found in Calendrie.Specialized
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
