﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

module Calendrie.Tests.Core.PartsTests

open System

open Calendrie.Core
open Calendrie.Testing

open Xunit
open FsCheck
open FsCheck.Xunit

// TODO(fact): binary serialization for Yemo(x).

module TestCommon =
    //
    // Xunit
    //

    [<Sealed>]
    type BadYearData() as self =
        inherit TheoryData<int>()
        do
            self.Add(Int32.MinValue)
            self.Add(Yemoda.MinYear - 1)
            self.Add(Yemoda.MaxYear + 1)
            self.Add(Int32.MaxValue)

    [<Sealed>]
    type BadMonthData() as self =
        inherit TheoryData<int>()
        do
            self.Add(Int32.MinValue)
            self.Add(Yemoda.MinMonth - 1)
            self.Add(Yemoda.MaxMonth + 1)
            self.Add(Int32.MaxValue)

    [<Sealed>]
    type BadDayData() as self =
        inherit TheoryData<int>()
        do
            self.Add(Int32.MinValue)
            self.Add(Yemoda.MinDay - 1)
            self.Add(Yemoda.MaxDay + 1)
            self.Add(Int32.MaxValue)

    [<Sealed>]
    type BadDayOfYearData() as self =
        inherit TheoryData<int>()
        do
            self.Add(Int32.MinValue)
            self.Add(Yedoy.MinDayOfYear - 1)
            self.Add(Yedoy.MaxDayOfYear + 1)
            self.Add(Int32.MaxValue)

    //
    // FsCheck
    //

    /// Single-case DU for the field Year.
    [<Struct>]
    type YearField =
        /// Represents the field Year.
        YearField of int with
        member x.Value = match x with YearField i -> i
        override x.ToString() =
            let s = x.Value
            s.ToString()
        static member op_Explicit(YearField i) = i

    /// Single-case DU for the field Year (short version).
    [<Struct>]
    type ShortYearField =
        /// Represents the field Year (short version).
        ShortYearField of int with
        member x.Value = match x with ShortYearField i -> i
        override x.ToString() =
            let s = x.Value
            s.ToString()
        static member op_Explicit(ShortYearField i) = i

    /// Single-case DU for the field Month.
    [<Struct>]
    type MonthField =
        /// Represents the field Month.
        MonthField of int with
        member x.Value = match x with MonthField i -> i
        override x.ToString() =
            let s = x.Value
            s.ToString()
        static member op_Explicit(MonthField i) = i

    /// Single-case DU for the field Day.
    [<Struct>]
    type DayField =
        /// Represents the field Day.
        DayField of int with
        member x.Value = match x with DayField i -> i
        override x.ToString() =
            let s = x.Value
            s.ToString()
        static member op_Explicit(DayField i) = i

    /// Single-case DU for the field DayOfYear.
    [<Struct>]
    type DayOfYearField =
        /// Represents the field DayOfYear.
        DayOfYearField of int with
        member x.Value = match x with DayOfYearField i -> i
        override x.ToString() =
            let s = x.Value
            s.ToString()
        static member op_Explicit(DayOfYearField i) = i

    /// Single-case DU for the field Extra.
    [<Struct>]
    type ExtraField =
        /// Represents the field Extra.
        ExtraField of int with
        member x.Value = match x with ExtraField i -> i
        override x.ToString() =
            let s = x.Value
            s.ToString()
        static member op_Explicit(ExtraField i) = i

    [<Sealed>]
    type Arbitraries =
        /// Gets an arbitrary for the field Year.
        static member GetYearFieldArbitrary() =
            Gen.choose (Yemoda.MinYear, Yemoda.MaxYear)
            |> Arb.fromGen
            |> Arb.convert YearField int

        /// Gets an arbitrary for the field Month.
        static member GetMonthFieldArbitrary() =
            Gen.choose (Yemoda.MinMonth, Yemoda.MaxMonth)
            |> Arb.fromGen
            |> Arb.convert MonthField int

        /// Gets an arbitrary for the field Day.
        static member GetDayFieldArbitrary() =
            Gen.choose (Yemoda.MinDay, Yemoda.MaxDay)
            |> Arb.fromGen
            |> Arb.convert DayField int

        /// Gets an arbitrary for the field DayOfYear.
        static member GetDayOfYearFieldArbitrary() =
            Gen.choose (Yedoy.MinDayOfYear, Yedoy.MaxDayOfYear)
            |> Arb.fromGen
            |> Arb.convert DayOfYearField int

[<Properties(Arbitrary = [| typeof<TestCommon.Arbitraries> |] )>]
module Yemoda =
    open TestCommon

    /// Arbitrary for (x, y) where x and y are Yemoda instances such that x <> y.
    /// Notice that x < y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! ymd =
            Gen.elements [
                // One different element.
                (2, 1, 1); (1, 2, 1); (1, 1, 2);
                // Two different elements.
                (2, 2, 1); (1, 2, 2); (2, 1, 2);
                // Three different elements.
                (2, 2, 2) ]
            |> Gen.map (fun (y, m, d) -> new Yemoda(y, m, d))
        return new Yemoda(1, 1, 1), ymd
    }

    module Prelude =
        [<Fact>]
        let ``Default value`` () =
            let ymd = Unchecked.defaultof<Yemoda>
            let y, m, d = ymd.Deconstruct()

            (y, m, d) === (1, 1, 1)

        [<Property>]
        let Constructor (YearField y) (MonthField m) (DayField d) =
            let ymd = new Yemoda(y, m, d)
            let a = ymd.Year
            let b = ymd.Month
            let c = ymd.Day

            (a, b, c) = (y, m, d)

        [<Property>]
        let Deconstructor (YearField y) (MonthField m) (DayField d) =
            let ymd = Yemoda.Create(y, m, d)
            let a, b, c = ymd.Deconstruct()

            (a, b, c) = (y, m, d)

        [<Property>]
        let ``Unpack(,,)`` (YearField y) (MonthField m) (DayField d) =
            let ymd = Yemoda.Create(y, m, d)
            let a, b, c = ymd.Unpack()

            (a, b, c) = (y, m, d)

        [<Property>]
        let ``Unpack(,)`` (YearField y) (MonthField m) (DayField d) =
            let ymd = Yemoda.Create(y, m, d)
            let a, b = ymd.Unpack()

            (a, b) = (y, m)

        [<Theory>]
        [<InlineData(-2_097_151, 1, 1, "01/01/-2097151")>] // MinValue
        [<InlineData(1, 1, 1, "01/01/0001")>]              // Default
        [<InlineData(2_097_152, 16, 64, "64/16/2097152")>] // MaxValue
        [<InlineData(7, 5, 3, "03/05/0007")>]
        [<InlineData(-7, 5, 3, "03/05/-0007")>]
        [<InlineData(2019, 13, 47, "47/13/2019")>]
        [<InlineData(-2019, 13, 47, "47/13/-2019")>]
        let ``ToString()`` y m d str =
            let ymd = Yemoda.Create(y, m, d)

            ymd.ToString() === str

        //
        // Properties
        //

        [<Fact>]
        let ``Static property MinValue`` () =
            let v = Yemoda.MinValue

            v.Year  === Yemoda.MinYear
            v.Month === Yemoda.MinMonth
            v.Day   === Yemoda.MinDay

        [<Fact>]
        let ``Static property MaxValue`` () =
            let v = Yemoda.MaxValue

            v.Year  === Yemoda.MaxYear
            v.Month === Yemoda.MaxMonth
            v.Day   === Yemoda.MaxDay

        [<Property>]
        let ``Property StarOfYear`` (x: Yemoda) =
            let startOfYear = Yemoda.Create(x.Year, 1, 1)

            x.StartOfYear = startOfYear

        [<Property>]
        let ``Property StartOfMonth`` (x: Yemoda) =
            let y, m, _ = x.Deconstruct()
            let startOfMonth = Yemoda.Create(y, m, 1)

            x.StartOfMonth = startOfMonth

        [<Property>]
        let ``Property Yemo`` (x: Yemoda) =
            let y, m, _ = x.Deconstruct()
            let ym = new Yemo(y, m)

            x.Yemo = ym

    module Factories =
        [<Theory; ClassData(typeof<BadYearData>)>]
        let ``Create() throws when "year" is out of range`` y =
            outOfRangeExn "year" (fun () -> Yemoda.Create(y, 1, 1))

        [<Theory; ClassData(typeof<BadMonthData>)>]
        let ``Create() throws when "month" is out of range`` m =
            outOfRangeExn "month" (fun () -> Yemoda.Create(1, m, 1))

        [<Theory; ClassData(typeof<BadDayData>)>]
        let ``Create() throws when "day" is out of range`` d =
            outOfRangeExn "day" (fun () -> Yemoda.Create(1, 1, d))

        [<Property>]
        let ``Create()`` (YearField y) (MonthField m) (DayField d) =
            let ymd = Yemoda.Create(y, m, d)
            let a, b, c = ymd.Deconstruct()

            (a, b, c) = (y, m, d)

        [<Property>]
        let ``AtStartOfYear()`` (YearField y) =
            let ymd = Yemoda.AtStartOfYear(y)
            let startOfYear = new Yemoda(y, 1, 1)

            ymd = startOfYear

        [<Property>]
        let ``AtStartOfMonth()`` (YearField y) (MonthField m)  =
            let ymd = Yemoda.AtStartOfMonth(y, m)
            let startOfMonth = new Yemoda(y, m, 1)

            ymd = startOfMonth

#if ENABLE_SERIALIZATION

    module Serialization =
        let extraSeq = seq {
            yield 0u
            yield 1024u
            yield uint(Int32.MaxValue) - 1u
            yield uint(Int32.MaxValue)
        }

        let badExtraSeq = seq {
            yield  uint(Int32.MaxValue) + 1u
            yield  UInt32.MaxValue - 1u
            yield  UInt32.MaxValue
        }

        [<Property>]
        let ``Roundtrip serialization`` (i: int) =
            Yemoda.FromBinary(i).ToBinary() === i

        [<Property>]
        let ``64-bit serialization throws when "extraData" is out of range`` (ymd: Yemoda) =
            for x in badExtraSeq do
                outOfRangeExn "extraData" (fun () -> ymd.ToBinary(x))

        [<Property>]
        let ``Roundtrip 64-bit serialization`` (ymd: Yemoda) =
            for x in extraSeq do
                let bin = ymd.ToBinary(x)
                let parts, extra = Yemoda.FromBinary(bin)

                parts === ymd
                extra === x

        //
        // Sample binary values
        //

        [<Fact>]
        let ``Int32:MinValue -> 01/01/-2_097_151`` () =
            let ymd = Yemoda.FromBinary(Int32.MinValue)

            ymd.ToBinary() === Int32.MinValue
            ymd === Yemoda.MinValue

        [<Fact>]
        let ``Int32:MaxValue -> 64/16/2_097_152`` () =
            let ymd = Yemoda.FromBinary(Int32.MaxValue)

            ymd.ToBinary() === Int32.MaxValue
            ymd === Yemoda.MaxValue

        [<Fact>]
        let ``0 -> 01/01/0001`` () =
            let ymd = Yemoda.FromBinary(0)

            ymd.ToBinary() === 0
            ymd === Unchecked.defaultof<Yemoda>

        [<Fact>]
        let ``-1 -> 64/16/0000, the theoretical end of year 1 BC`` () =
            let ymd = Yemoda.FromBinary(-1)

            ymd.ToBinary() === -1
            ymd.Year  === 0
            ymd.Month === Yemoda.MaxMonth
            ymd.Day   === Yemoda.MaxDay

        [<Fact>]
        let ``1 -> 02/01/0001`` () =
            let ymd = Yemoda.FromBinary(1)

            ymd.ToBinary() === 1
            ymd.Year  === 1
            ymd.Month === 1
            ymd.Day   === 2

        [<Fact>]
        let ``64 -> 01/02/0001`` () =
            let ymd = Yemoda.FromBinary(64)

            ymd.ToBinary() === 64
            ymd.Year  === 1
            ymd.Month === 2
            ymd.Day   === 1

        [<Fact>]
        let ``1024 ->  01/01/0001`` () =
            let ymd = Yemoda.FromBinary(1024)

            ymd.ToBinary() === 1024
            ymd.Year  === 2
            ymd.Month === 1
            ymd.Day   === 1

#endif

    module Equality =
        open NonStructuralComparison

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: Yemoda) =
            x = x
            .&. not (x <> x)
            .&. x.Equals(x)
            .&. x.Equals(x :> obj)

        [<Property>]
        let ``Equality when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x = y)
            .&. (x <> y)
            .&. not (x.Equals(y))
            .&. not (x.Equals(y :> obj))
            // Flipped
            .&. not (y = x)
            .&. (y <> x)
            .&. not (y.Equals(x))
            .&. not (y.Equals(x :> obj))
        // fsharplint:enable

        [<Property>]
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: Yemoda) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: Yemoda) =
            x.GetHashCode() = x.GetHashCode()

    module Comparison =
        open NonStructuralComparison

        // fsharplint:disable Hints
        [<Property>]
        let ``Comparisons when both operands are identical`` (x: Yemoda) =
            not (x > x)
            .&. not (x < x)
            .&. (x >= x)
            .&. (x <= x)

        [<Property>]
        let ``Comparisons when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x > y)
            .&. not (x >= y)
            .&. (x < y)
            .&. (x <= y)
            // Flipped
            .&. (y > x)
            .&. (y >= x)
            .&. not (y < x)
            .&. not (y <= x)
        // fsharplint:enable

        //
        // CompareTo()
        //

        [<Property>]
        let ``CompareTo() returns 0 when both operands are identical`` (x: Yemoda) =
            (x.CompareTo(x) = 0)
            .&. ((x :> IComparable).CompareTo(x) = 0)

        [<Property>]
        let ``CompareTo() when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            (x.CompareTo(y) <= 0)
            .&. ((x :> IComparable).CompareTo(y) <= 0)
            // Flipped
            .&. (y.CompareTo(x) >= 0)
            .&. ((y :> IComparable).CompareTo(x) >= 0)

        [<Property>]
        let ``CompareTo(obj) returns 1 when "obj" is null`` (x: Yemoda) =
             (x :> IComparable).CompareTo(null) = 1

        [<Property>]
        let ``CompareTo(obj) throws when "obj" is a plain object`` (x: Yemoda) =
            argExn "obj" (fun () -> (x :> IComparable).CompareTo(new obj()))

[<Properties(Arbitrary = [| typeof<TestCommon.Arbitraries> |] )>]
module Yemo =
    open TestCommon

    /// Arbitrary for (x, y) where x and y are Yemo instances such that x <> y.
    /// Notice that x < y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! ym =
            Gen.elements [ (2, 1); (1, 2); (2, 2) ]
            |> Gen.map (fun (y, m) -> new Yemo(y, m))
        return new Yemo(1, 1), ym
    }

    module Prelude =
        [<Fact>]
        let ``Default value`` () =
            let ym = Unchecked.defaultof<Yemo>
            let y, m = ym.Deconstruct()

            (y, m) === (1, 1)

#if DEBUG
        [<Fact>]
        let ``Constructor with ill-formed data`` () =
            // See Yemoda.Pack().
            let y, m, d = 1, 1, 2
            let bin = ((y - 1) <<< Yemoda.YearShift) ||| ((m - 1) <<< Yemoda.MonthShift) ||| (d - 1)

            argExn "data" (fun () -> new Yemo(bin))
#endif

        [<Property>]
        let Constructor (YearField y) (MonthField m)  =
            let ym = new Yemo(y, m)
            let a = ym.Year
            let b = ym.Month

            (a, b) = (y, m)

        [<Property>]
        let Deconstructor (YearField y) (MonthField m) =
            let ym = Yemo.Create(y, m)
            let a, b = ym.Deconstruct()

            (a, b) = (y, m)

        [<Property>]
        let ``Unpack()`` (YearField y) (MonthField m) =
            let ym = Yemo.Create(y, m)
            let a, b = ym.Unpack()

            (a, b) = (y, m)

        [<Theory>]
        [<InlineData(-2_097_151, 1, "01/-2097151")>] // MinValue
        [<InlineData(1, 1, "01/0001")>]              // Default
        [<InlineData(2_097_152, 16, "16/2097152")>]  // MaxValue
        [<InlineData(7, 5, "05/0007")>]
        [<InlineData(-7, 5, "05/-0007")>]
        [<InlineData(2019, 13, "13/2019")>]
        [<InlineData(-2019, 13, "13/-2019")>]
        let ``ToString()`` y m str =
            let ym = Yemo.Create(y, m)

            ym.ToString() === str

        //
        // Properties
        //

        [<Fact>]
        let ``Static property MinValue`` () =
            let v = Yemo.MinValue

            v.Year  === Yemo.MinYear
            v.Month === Yemo.MinMonth

        [<Fact>]
        let ``Static property MaxValue`` () =
            let v = Yemo.MaxValue

            v.Year  === Yemo.MaxYear
            v.Month === Yemo.MaxMonth

        [<Property>]
        let ``Property StarOfYear`` (x: Yemo) =
            let startOfYear = Yemoda.Create(x.Year, 1, 1)

            x.StartOfYear = startOfYear

        [<Property>]
        let ``Property StartOfMonth`` (x: Yemo) =
            let y, m = x.Deconstruct()
            let startOfMonth = Yemoda.Create(y, m, 1)

            x.StartOfMonth = startOfMonth

    module Factories =
        [<Theory; ClassData(typeof<BadYearData>)>]
        let ``Create() throws when "year" is out of range`` y =
            outOfRangeExn "year" (fun () -> Yemo.Create(y, 1))

        [<Theory; ClassData(typeof<BadMonthData>)>]
        let ``Create() throws when "month" is out of range`` m =
            outOfRangeExn "month" (fun () -> Yemo.Create(1, m))

        [<Property>]
        let ``Create()`` (YearField y) (MonthField m) =
            let ym = Yemo.Create(y, m)
            let a, b = ym.Deconstruct()

            (a, b) = (y, m)

        [<Property>]
        let ``AtStartOfYear()`` (YearField y) =
            let ym = Yemo.AtStartOfYear(y)
            let startOfYear = new Yemo(y, 1)

            ym = startOfYear

        [<Theory; ClassData(typeof<BadDayData>)>]
        let ``GetDayOfMonth() throws when "day" is out of range`` d =
            let ym = Yemo.Create(1, 1)

            outOfRangeExn "day" (fun () -> ym.GetDayOfMonth(d))

        [<Property>]
        let ``GetDayOfMonth()`` (YearField y) (MonthField m) (DayField d) =
            let ym = Yemo.Create(y, m)
            let ymd = Yemoda.Create(y, m, d)

            ym.GetDayOfMonth(d) === ymd

        [<Property>]
        let ``GetDayOfMonthUnsafe()`` (YearField y) (MonthField m) (DayField d) =
            let ym = Yemo.Create(y, m)
            let ymd = Yemoda.Create(y, m, d)

            ym.GetDayOfMonthUnsafe(d) === ymd

#if ENABLE_SERIALIZATION

    module Serialization =
        // We rely on the fact that the prop Yemo unset the day bits.

        let minBin = Yemoda.MinValue.Yemo.ToBinary()
        let maxBin = Yemoda.MaxValue.Yemo.ToBinary()

        let private badBinArbitrary = Arb.fromGen <| gen {
            let! b = Arb.generate<int>
            // A bad binary has its day bits not set to zero (d > 1).
            let! d = Gen.choose (2, Yemoda.MaxDay)
            let ymd = new Yemoda(b)
            return ymd.Yemo.ToBinary() ||| (d - 1)
        }

        let private binArbitrary = Arb.fromGen <| gen {
            let! b = Arb.generate<int>
            let ymd = new Yemoda(b)
            return ymd.Yemo.ToBinary()
        }

        [<Fact>]
        let ``Self-check`` () =
            minBin === Int32.MinValue
            maxBin === 2_147_483_584
            maxBin !== Int32.MaxValue

        [<Fact>]
        let ``FromBinary() throws when data > 2_147_483_584 (max)`` () =
            let mutable bin = maxBin

            while bin < Int32.MaxValue do
                bin <- bin + 1
                argExn "data" (fun () -> Yemo.FromBinary(bin))

        [<Property>]
        let ``FromBinary() throws when "data" has its day bits not set to zero`` () = badBinArbitrary @@@@ fun bin ->
            argExn "data" (fun () -> Yemo.FromBinary(bin))

        [<Property>]
        let ``Roundtrip serialization`` () = binArbitrary @@@@ fun bin ->
            Yemo.FromBinary(bin).ToBinary() = bin

        //
        // Sample binary values
        //

        [<Fact>]
        let ``Int32:MinValue -> 01/-2_097_151`` () =
            let ym = Yemo.FromBinary(Int32.MinValue)

            ym.ToBinary() === Int32.MinValue
            ym === Yemo.MinValue

        [<Fact>]
        let ``2_147_483_584 (max) -> 16/2_097_152`` () =
            let ym = Yemo.FromBinary(maxBin)

            ym.ToBinary() === maxBin
            ym === Yemo.MaxValue

        [<Fact>]
        let ``0 -> 01/0001`` () =
            let ym = Yemo.FromBinary(0)

            ym.ToBinary() === 0
            ym === Unchecked.defaultof<Yemo>

#endif

    module Equality =
        open NonStructuralComparison

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: Yemo) =
            x = x
            .&. not (x <> x)
            .&. x.Equals(x)
            .&. x.Equals(x :> obj)

        [<Property>]
        let ``Equality when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x = y)
            .&. (x <> y)
            .&. not (x.Equals(y))
            .&. not (x.Equals(y :> obj))
            // Flipped
            .&. not (y = x)
            .&. (y <> x)
            .&. not (y.Equals(x))
            .&. not (y.Equals(x :> obj))
        // fsharplint:enable

        [<Property>]
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: Yemo) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: Yemo) =
            x.GetHashCode() = x.GetHashCode()

    module Comparison =
        open NonStructuralComparison

        // fsharplint:disable Hints
        [<Property>]
        let ``Comparisons when both operands are identical`` (x: Yemo) =
            not (x > x)
            .&. not (x < x)
            .&. (x >= x)
            .&. (x <= x)

        [<Property>]
        let ``Comparisons when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x > y)
            .&. not (x >= y)
            .&. (x < y)
            .&. (x <= y)
            // Flipped
            .&. (y > x)
            .&. (y >= x)
            .&. not (y < x)
            .&. not (y <= x)
        // fsharplint:enable

        //
        // CompareTo()
        //

        [<Property>]
        let ``CompareTo() returns 0 when both operands are identical`` (x: Yemo) =
            (x.CompareTo(x) = 0)
            .&. ((x :> IComparable).CompareTo(x) = 0)

        [<Property>]
        let ``CompareTo() when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            (x.CompareTo(y) <= 0)
            .&. ((x :> IComparable).CompareTo(y) <= 0)
            // Flipped
            .&. (y.CompareTo(x) >= 0)
            .&. ((y :> IComparable).CompareTo(x) >= 0)

        [<Property>]
        let ``CompareTo(obj) returns 1 when "obj" is null`` (x: Yemo) =
             (x :> IComparable).CompareTo(null) = 1

        [<Property>]
        let ``CompareTo(obj) throws when "obj" is a plain object`` (x: Yemo) =
            argExn "obj" (fun () -> (x :> IComparable).CompareTo(new obj()))

[<Properties(Arbitrary = [| typeof<TestCommon.Arbitraries> |] )>]
module Yedoy =
    open TestCommon

    /// Arbitrary for (x, y) where x and y are Yedoy instances such that x <> y.
    /// Notice that x < y.
    let private xyArbitrary = Arb.fromGen <| gen {
        let! ydoy =
            Gen.elements [ (2, 1); (1, 2); (2, 2) ]
            |> Gen.map (fun (y, doy) -> new Yedoy(y, doy))
        return new Yedoy(1, 1), ydoy
    }

    module Prelude =
        [<Fact>]
        let ``Default value`` () =
            let ydoy = Unchecked.defaultof<Yedoy>
            let y, doy = ydoy.Deconstruct()

            (y, doy) === (1, 1)

        [<Property>]
        let Constructor (YearField y) (DayOfYearField doy)  =
            let ydoy = new Yedoy(y, doy)
            let a = ydoy.Year
            let b = ydoy.DayOfYear

            (a, b) = (y, doy)

        [<Property>]
        let Deconstructor (YearField y) (DayOfYearField doy) =
            let ydoy = Yedoy.Create(y, doy)
            let a, b = ydoy.Deconstruct()

            (a, b) = (y, doy)

        [<Property>]
        let ``Unpack()`` (YearField y) (DayOfYearField doy) =
            let ydoy = Yedoy.Create(y, doy)
            let a, b = ydoy.Unpack()

            (a, b) = (y, doy)

        [<Theory>]
        [<InlineData(-2_097_151, 1, "001/-2097151")>]   // MinValue
        [<InlineData(1, 1, "001/0001")>]                // Default
        [<InlineData(2_097_152, 1024, "1024/2097152")>] // MaxValue
        [<InlineData(7, 5, "005/0007")>]
        [<InlineData(-7, 5, "005/-0007")>]
        [<InlineData(2019, 133, "133/2019")>]
        [<InlineData(-2019, 133, "133/-2019")>]
        let ``ToString()`` y doy str =
            let ydoy = Yedoy.Create(y, doy)

            ydoy.ToString() === str

        //
        // Properties
        //

        [<Fact>]
        let ``Static property MinValue`` () =
            let v = Yedoy.MinValue

            v.Year      === Yedoy.MinYear
            v.DayOfYear === Yedoy.MinDayOfYear

        [<Fact>]
        let ``Static property MaxValue`` () =
            let v = Yedoy.MaxValue

            v.Year      === Yedoy.MaxYear
            v.DayOfYear === Yedoy.MaxDayOfYear

        [<Property>]
        let ``Property StarOfYear`` (x: Yedoy) =
            let startOfYear = Yedoy.Create(x.Year, 1)

            x.StartOfYear = startOfYear

    module Factories =
        [<Theory; ClassData(typeof<BadYearData>)>]
        let ``Create() throws when year is out of range`` y =
            outOfRangeExn "year" (fun () -> Yedoy.Create(y, 1))

        [<Theory; ClassData(typeof<BadDayOfYearData>)>]
        let ``Create() throws when day of the year is out of range`` doy =
            outOfRangeExn "dayOfYear" (fun () -> Yedoy.Create(1, doy))

        [<Property>]
        let ``Create()`` (YearField y) (DayOfYearField doy) =
            let ydoy = Yedoy.Create(y, doy)
            let a, b = ydoy.Deconstruct()

            (a, b) = (y, doy)

        [<Property>]
        let ``AtStartOfYear()`` (YearField y) =
            let ymd = Yedoy.AtStartOfYear(y)
            let startOfYear = new Yedoy(y, 1)

            ymd = startOfYear

#if ENABLE_SERIALIZATION

    module Serialization =
        [<Property>]
        let ``Roundtrip serialization`` (i: int) =
            Yedoy.FromBinary(i).ToBinary() === i

        //
        // Sample binary values
        //

        [<Fact>]
        let ``Int32:MinValue -> 001/-2_097_151`` () =
            let ydoy = Yedoy.FromBinary(Int32.MinValue)

            ydoy.ToBinary() === Int32.MinValue
            ydoy === Yedoy.MinValue

        [<Fact>]
        let ``Int32:MaxValue -> 1024/2_097_152`` () =
            let ydoy = Yedoy.FromBinary(Int32.MaxValue)

            ydoy.ToBinary() === Int32.MaxValue
            ydoy === Yedoy.MaxValue

        [<Fact>]
        let ``0 -> 001/0001`` () =
            let ydoy = Yedoy.FromBinary(0)

            ydoy.ToBinary() === 0
            ydoy === Unchecked.defaultof<Yedoy>

#endif

    module Equality =
        open NonStructuralComparison

        // fsharplint:disable Hints
        [<Property>]
        let ``Equality when both operands are identical`` (x: Yedoy) =
            x = x
            .&. not (x <> x)
            .&. x.Equals(x)
            .&. x.Equals(x :> obj)

        [<Property>]
        let ``Equality when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x = y)
            .&. (x <> y)
            .&. not (x.Equals(y))
            .&. not (x.Equals(y :> obj))
            // Flipped
            .&. not (y = x)
            .&. (y <> x)
            .&. not (y.Equals(x))
            .&. not (y.Equals(x :> obj))
        // fsharplint:enable

        [<Property>]
        let ``Equals(obj) returns false when "obj" is null or is a plain object`` (x: Yedoy) =
            not (x.Equals(null))
            .&. not (x.Equals(new obj()))

        [<Property>]
        let ``GetHashCode() is invariant`` (x: Yedoy) =
            x.GetHashCode() = x.GetHashCode()

    module Comparison =
        open NonStructuralComparison

        // fsharplint:disable Hints
        [<Property>]
        let ``Comparisons when both operands are identical`` (x: Yedoy) =
            not (x > x)
            .&. not (x < x)
            .&. (x >= x)
            .&. (x <= x)

        [<Property>]
        let ``Comparisons when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            not (x > y)
            .&. not (x >= y)
            .&. (x < y)
            .&. (x <= y)
            // Flipped
            .&. (y > x)
            .&. (y >= x)
            .&. not (y < x)
            .&. not (y <= x)
        // fsharplint:enable

        //
        // CompareTo()
        //

        [<Property>]
        let ``CompareTo() returns 0 when both operands are identical`` (x: Yedoy) =
            (x.CompareTo(x) = 0)
            .&. ((x :> IComparable).CompareTo(x) = 0)

        [<Property>]
        let ``CompareTo() when both operands are distinct`` () = xyArbitrary @@@@ fun (x, y) ->
            (x.CompareTo(y) <= 0)
            .&. ((x :> IComparable).CompareTo(y) <= 0)
            // Flipped
            .&. (y.CompareTo(x) >= 0)
            .&. ((y :> IComparable).CompareTo(x) >= 0)

        [<Property>]
        let ``CompareTo(obj) returns 1 when "obj" is null`` (x: Yedoy) =
             (x :> IComparable).CompareTo(null) = 1

        [<Property>]
        let ``CompareTo(obj) throws when "obj" is a plain object`` (x: Yedoy) =
            argExn "obj" (fun () -> (x :> IComparable).CompareTo(new obj()))
