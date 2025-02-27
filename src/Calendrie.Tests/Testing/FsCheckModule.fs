﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

[<AutoOpen>]
module Calendrie.Testing.FsCheckModule

open System
open System.Runtime.CompilerServices

open Calendrie
open Calendrie.Core
open Calendrie.Core.Intervals
open Calendrie.Core.Utilities

open FsCheck
open FsCheck.Xunit

// Custom operators.
// Adapted from https://github.com/fscheck/FsCheck/blob/master/src/FsCheck/FSharp.Prop.fs

/// Conditional property combinator.
/// Resulting property holds if the property after &&& holds whenever the condition does.
/// Replacement for the FsCheck operator ==>.
let ( &&&& ) condition (assertion: 'Testable) = Prop.filter condition assertion

/// Quantified property combinator. Provide a custom test data generator to a property.
/// Moyen mnémotechnique : "four @all".
let ( @@@@ ) (arb: Arbitrary<'Value>) (body: 'Value -> 'Testable) = Prop.forAll arb body

/// Represents a pair of distinct values.
[<Struct; IsReadOnly>]
type Pair<'T when 'T: struct and 'T :> IComparable<'T>> = { Min: 'T; Max: 'T; Delta: 'T }

/// Generators for int.
module IntGenerators =
    /// Generator for integers > 0.
    let greaterThanZero = Arb.generate<int> |> Gen.filter (fun i -> i > 0)

    /// Generator for integers between -1_000_000 and 1_000_000, inclusive.
    let private ``[-10^6..10^6]`` = Gen.choose (-1_000_000, 1_000_000)

    /// Generator for integers between 1 and 1_000_000, inclusive.
    let private ``[1..10^6]`` = Gen.choose (1, 1_000_000)

    /// Generator for (i, j, j - i) where i and j are integers such that i < j.
    let orderedPair =
        gen {
            let! i = ``[-10^6..10^6]``
            let! n = ``[1..10^6]``
            return i, i + n, n
        }

/// Arbitraries for int.
module IntArbitraries =
    /// Arbitrary for an int <> 0.
    /// See also NonZeroInt from FsCheck.
    let nonZero =
        Arb.Default.Int32()
        |> Arb.filter ((<>) 0)

    /// Arbitrary for an int < 0.
    /// See also NegativeInt from FsCheck.
    let lessThanZero =
        Arb.Default.Int32()
        |> Arb.mapFilter (fun i -> -abs i) (fun i -> i < 0)

    /// Arbitrary for an int <= 0.
    let lessThanOrEqualToZero =
        Arb.Default.Int32()
        |> Arb.mapFilter (fun i -> -abs i) (fun i -> i <= 0)

    /// Arbitrary for an int > 0.
    /// See also PositiveInt from FsCheck.
    let greaterThanZero =
        Arb.Default.Int32()
        |> Arb.mapFilter abs (fun i -> i > 0)

    /// Arbitrary for an int >= 0.
    /// See also NonNegativeInt from FsCheck.
    let greaterThanOrEqualToZero =
        Arb.Default.Int32()
        |> Arb.mapFilter abs (fun i -> i >= 0)

/// Domain-specific arbitraries.
module DomainArbitraries =
    /// Arbitrary for the "daysSinceZero" of a DayNumber.
    let daysSinceZero =
        Arb.Default.Int32()
        |> Arb.filter (fun i -> DayNumber.MinDaysSinceZero <= i && i <= DayNumber.MaxDaysSinceZero)

    ///// Arbitrary for the "daysSinceZero" of a DayNumber64.
    //let daysSinceZero64 =
    //    Arb.Default.Int64()
    //    |> Arb.filter (fun i -> DayNumber64.MinDaysSinceZero <= i && i <= DayNumber64.MaxDaysSinceZero)

    /// Arbitrary for the algebraic value of an Ord.
    let algebraicOrd =
        Arb.Default.Int32()
        // Formally, we we should also filter i with i <= Ord.MaxAlgebraicValue,
        // but it's useless since Ord.MaxAlgebraicValue is equal to Int32.MaxValue.
        |> Arb.filter (fun i -> i >= Ord.MinAlgebraicValue)

    ///// Arbitrary for the algebraic value of an Ord64.
    //let algebraicOrd64 =
    //    Arb.Default.Int64()
    //    |> Arb.filter (fun i -> i >= Ord64.MinAlgebraicValue)

/// Global arbitraries.
[<Sealed>]
type GlobalArbitraries =
    //
    // Calendrie.Testing
    //

    /// Obtains an arbitrary for Pair<int>.
    static member GetPairOfIntArbitrary() =
        IntGenerators.orderedPair
        |> Gen.map (fun (i, j, n) -> { Pair.Min = i; Max = j; Delta = n })
        |> Arb.fromGen

    //
    // Calendrie
    //

    /// Obtains an arbitrary for DayNumber.
    static member GetDayNumberArbitrary() =
        DomainArbitraries.daysSinceZero
        |> Arb.convert (fun i -> DayNumber.Zero + i) (fun x -> x - DayNumber.Zero)

    ///// Obtains an arbitrary for DayNumber64.
    //static member GetDayNumber64Arbitrary() =
    //    DomainArbitraries.daysSinceZero64
    //    |> Arb.convert (fun i -> DayNumber64.Zero + i) (fun x -> x - DayNumber64.Zero)

    /// Obtains an arbitrary for Ord.
    static member GetOrdArbitrary() =
        DomainArbitraries.algebraicOrd
        |> Arb.convert Ord.FromInt32 int

    ///// Obtains an arbitrary for Ord64.
    //static member GetOrd64Arbitrary() =
    //    DomainArbitraries.algebraicOrd64
    //    |> Arb.convert Ord64.FromInt64 int64

    /// Obtains an arbitrary for DateParts.
    static member GetDatePartsArbitrary() =
        Arb.fromGen <| gen {
            let! y = Arb.generate<int>
            let! m = Arb.generate<int>
            let! d = Arb.generate<int>
            return new DateParts(y, m, d)
        }

    /// Obtains an arbitrary for MonthParts.
    static member GetMonthPartsArbitrary() =
        Arb.fromGen <| gen {
            let! y = Arb.generate<int>
            let! m = Arb.generate<int>
            return new MonthParts(y, m)
        }

    /// Obtains an arbitrary for OrdinalParts.
    static member GetOrdinalPartsArbitrary() =
        Arb.fromGen <| gen {
            let! y = Arb.generate<int>
            let! doy = Arb.generate<int>
            return new OrdinalParts(y, doy)
        }

    ///// Obtains an arbitrary for DateFields.
    //static member GetDateFieldsArbitrary() =
    //    Arb.fromGen <| gen {
    //        let! y = Arb.generate<int>
    //        let! m = IntGenerators.greaterThanZero
    //        let! d = IntGenerators.greaterThanZero
    //        return new DateFields(y, m, d)
    //    }

    ///// Obtains an arbitrary for MonthFields.
    //static member GetMonthFieldsArbitrary() =
    //    Arb.fromGen <| gen {
    //        let! y = Arb.generate<int>
    //        let! m = IntGenerators.greaterThanZero
    //        return new MonthFields(y, m)
    //    }

    ///// Obtains an arbitrary for OrdinalFields.
    //static member GetOrdinalFieldsArbitrary() =
    //    Arb.fromGen <| gen {
    //        let! y = Arb.generate<int>
    //        let! doy = IntGenerators.greaterThanZero
    //        return new OrdinalFields(y, doy)
    //    }

    //
    // Calendrie.Core
    //
    // For Yemoda(x) and Yedoy(x), we use de-serialization, which is a trivial
    // operation. Notice also that serialization is a one-to-one mapping between
    // Int32 and the type.
    // For Yemo(x), we have to do things manually as de-serialization is not
    // always valid.

    /// Obtains an arbitrary for Yemoda.
    static member GetYemodaArbitrary() =
        Arb.generate<int>
#if ENABLE_SERIALIZATION
        |> Gen.map Yemoda.FromBinary
#endif
        |> Gen.map Yemoda.FromInt32
        |> Arb.fromGen

    /// Obtains an arbitrary for Yemo.
    static member GetYemoArbitrary() =
        Arb.fromGen <| gen {
            let! y = Gen.choose (Yemo.MinYear, Yemo.MaxYear)
            let! m = Gen.choose (Yemo.MinMonth, Yemo.MaxMonth)
            return Yemo.Create(y, m)
        }

   /// Obtains an arbitrary for Yedoy.
    static member GetYedoyArbitrary() =
        Arb.generate<int>
#if ENABLE_SERIALIZATION
        |> Gen.map Yedoy.FromBinary
#endif
        |> Gen.map Yedoy.FromInt32
        |> Arb.fromGen

    //
    // Calendrie.Core.Intervals
    //

    /// Obtains an arbitrary for Segment<int>.
    static member GetRangeArbitrary() =
        Arb.fromGen <| gen {
            let! i = Arb.generate<int>
            let! j = Arb.generate<int>
            return if i < j then new Segment<int>(i, j) else new Segment<int>(j, i)
        }

    /// Obtains an arbitrary for SegmentSet<int>.
    static member GetSegmentSetArbitrary() =
        Arb.fromGen <| gen {
            let! i = Arb.generate<int>
            let! j = Arb.generate<int>
            return if i < j then new SegmentSet<int>(i, j) else new SegmentSet<int>(j, i)
        }

    /// Obtains an arbitrary for LowerRay<int>.
    static member GetLowerRayArbitrary() =
        Arb.generate<int>
        |> Gen.map (fun i -> new LowerRay<int>(i))
        |> Arb.fromGen

    /// Obtains an arbitrary for UpperRay<int>.
    static member GetUpperRayAbitrary() =
        Arb.generate<int>
        |> Gen.map (fun i -> new UpperRay<int>(i))
        |> Arb.fromGen

    //
    // Calendrie.Core.Utilities
    //

    /// Obtains an arbitrary for OrderedPair<int>.
    static member GetOrderedPairArbitrary() =
        Arb.fromGen <| gen {
            let! i = Arb.generate<int>
            let! j = Arb.generate<int>
            return new OrderedPair<int>(i, j)
        }

    ////
    //// Calendrie.Horology
    ////

    ///// Obtains an arbitrary for Duration32.
    //static member GetDuration32Abitrary() =
    //    Arb.fromGen <| gen {
    //        let! i = Arb.generate<uint16>
    //        let! j = Arb.generate<uint16>
    //        return new Duration32(i, j)
    //    }

    ///// Obtains an arbitrary for Duration64.
    //static member GetDuration64Abitrary() =
    //    Arb.generate<int64>
    //    |> Gen.map (fun i -> new Duration64(i))
    //    |> Arb.fromGen

    ///// Obtains an arbitrary for ReferenceId.
    //static member GetReferenceIdAbitrary() =
    //    Arb.generate<uint>
    //    |> Gen.map (fun i -> new ReferenceId(i))
    //    |> Arb.fromGen

[<assembly: Properties( Arbitrary = [| typeof<GlobalArbitraries> |] )>] do()
