// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Systems.StandardScopeTests

open Calendrie
open Calendrie.Core.Intervals
open Calendrie.Core.Schemas
open Calendrie.Systems
open Calendrie.Testing
// Only required, if we re-enable the tests for the static methods.
//open Calendrie.Testing.Data
//open Calendrie.Testing.Data.Schemas
//open Calendrie.Testing.Facts.Systems
open Calendrie.Testing.Faux

open Xunit

module Prelude =
    [<Fact>]
    let ``Constructor throws when "schema" is null`` () =
        nullExn "schema" (fun () -> new StandardScope(null, DayZero.OldStyle))

    [<Fact>]
    let ``Constructor throws when schema.MinYear > 1`` () =
        let range = Range.Create(StandardScope.MinYear + 1, StandardScope.MaxYear)
        let sch = new FauxCalendricalSchema(range)

        argExn "supportedYears" (fun () -> new StandardScope(sch, DayZero.OldStyle))

    [<Fact>]
    let ``Constructor throws when schema.MaxYear < 9999`` () =
        let range = Range.Create(1, StandardScope.MaxYear - 1)
        let sch = new FauxCalendricalSchema(range)

        argExn "supportedYears" (fun () -> new StandardScope(sch, DayZero.OldStyle))

    [<Fact>]
    let ``Property Epoch`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let scope = new StandardScope(new FauxCalendricalSchema(), epoch)

        scope.Epoch === epoch

    [<Fact>]
    let ``Property Domain`` () =
        let epoch = DayZero.NewStyle + 123_456_789
        let sch = new GregorianSchema()
        let scope = new StandardScope(sch, epoch)
        let minDayNumber = epoch + sch.GetStartOfYear(StandardScope.MinYear)
        let maxDayNumber = epoch + sch.GetEndOfYear(StandardScope.MaxYear)
        let range = Range.Create(minDayNumber, maxDayNumber)

        scope.Domain === range

    [<Fact>]
    let ``Property SupportedYears`` () =
        let scope = new StandardScope(new FauxCalendricalSchema(), DayZero.OldStyle)
        let range = Range.Create(StandardScope.MinYear, StandardScope.MaxYear)

        scope.Segment.SupportedYears === range

#if true
// These tests are no longer necessary but only because the non-static methods
// call them to do their work.

module CivilCase =
    let private dataSet = GregorianDataSet.Instance

    let validYearData = StandardScopeFacts.ValidYearData
    let invalidYearData = StandardScopeFacts.InvalidYearData

    let dateInfoData = dataSet.DateInfoData
    let monthInfoData = dataSet.MonthInfoData
    let invalidMonthFieldData = dataSet.InvalidMonthFieldData
    let invalidDayOfYearFieldData = dataSet.InvalidDayOfYearFieldData
    let invalidDayFieldData = dataSet.InvalidDayFieldData

    // ValidateYearMonthImpl() --- this method is now private.

    //[<Theory; MemberData(nameof(invalidYearData))>]
    //let ``ValidateYearMonthImpl() throws when "year" is out of range`` y =
    //    outOfRangeExn "year" (fun () -> CivilScope.ValidateYearMonthImpl(y, 1))
    //    outOfRangeExn "y" (fun () -> CivilScope.ValidateYearMonthImpl(y, 1, nameof(y)))

    //[<Theory; MemberData(nameof(invalidMonthFieldData))>]
    //let ``ValidateYearMonthImpl() throws when "month" is out of range`` y m =
    //    outOfRangeExn "month" (fun () -> CivilScope.ValidateYearMonthImpl(y, m))
    //    outOfRangeExn "m" (fun () -> CivilScope.ValidateYearMonthImpl(y, m, nameof(m)))

    //[<Theory; MemberData(nameof(monthInfoData))>]
    //let ``ValidateYearMonth() does not throw when the input is valid`` (x: MonthInfo) =
    //    let y, m = x.Yemo.Deconstruct()
    //    CivilScope.ValidateYearMonthImpl(y, m)

    // ValidateYearMonthDayImpl()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateYearMonthDayImpl() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> CivilScope.ValidateYearMonthDayImpl(y, 1, 1))
        outOfRangeExn "y" (fun () -> CivilScope.ValidateYearMonthDayImpl(y, 1, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidMonthFieldData))>]
    let ``ValidateYearMonthDayImpl() throws when "month" is out of range`` y m =
        outOfRangeExn "month" (fun () -> CivilScope.ValidateYearMonthDayImpl(y, m, 1))
        outOfRangeExn "m" (fun () -> CivilScope.ValidateYearMonthDayImpl(y, m, 1, nameof(m)))

    [<Theory; MemberData(nameof(invalidDayFieldData))>]
    let ``ValidateYearMonthDayImpl() throws when "day" is out of range`` y m d =
        outOfRangeExn "day" (fun () -> CivilScope.ValidateYearMonthDayImpl(y, m, d))
        outOfRangeExn "d" (fun () -> CivilScope.ValidateYearMonthDayImpl(y, m, d, nameof(d)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateYearMonthDayImpl() does not throw when the input is valid`` (x: DateInfo) =
        let y, m, d = x.Yemoda.Deconstruct()
        CivilScope.ValidateYearMonthDayImpl(y, m, d)

    // ValidateOrdinalImpl()

    [<Theory; MemberData(nameof(invalidYearData))>]
    let ``ValidateOrdinalImpl() throws when "year" is out of range`` y =
        outOfRangeExn "year" (fun () -> CivilScope.ValidateOrdinalImpl(y,  1))
        outOfRangeExn "y" (fun () -> CivilScope.ValidateOrdinalImpl(y, 1, nameof(y)))

    [<Theory; MemberData(nameof(invalidDayOfYearFieldData))>]
    let ``ValidateOrdinalImpl() throws when "dayOfYear" is out of range`` y doy =
        outOfRangeExn "dayOfYear" (fun () -> CivilScope.ValidateOrdinalImpl(y, doy))
        outOfRangeExn "doy" (fun () -> CivilScope.ValidateOrdinalImpl(y, doy, nameof(doy)))

    [<Theory; MemberData(nameof(dateInfoData))>]
    let ``ValidateOrdinalImpl() does not throw when the input is valid`` (x: DateInfo) =
        let y, doy = x.Yedoy.Deconstruct()
        CivilScope.ValidateOrdinalImpl(y, doy)

#endif
