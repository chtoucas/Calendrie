// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

/// <summary>
/// Defines the base for other calendar year types.
/// </summary>
/// <typeparam name="TSelf">The year type that implements this interface.
/// </typeparam>
public interface IYearBase<TSelf> :
    IYear,
    // Comparison
    IEqualityOperators<TSelf, TSelf, bool>,
    IEquatable<TSelf>,
    IComparisonOperators<TSelf, TSelf, bool>,
    IComparable<TSelf>,
    IComparable,
    // Arithmetic
    IYearFieldMath<TSelf>,
    //ISubtractionOperators<TSelf, TSelf, int>, // Cannot be added, but see below
    IAdditionOperators<TSelf, int, TSelf>,
    ISubtractionOperators<TSelf, int, TSelf>,
    IIncrementOperators<TSelf>,
    IDecrementOperators<TSelf>
    where TSelf : IYearBase<TSelf>
{
    /// <summary>
    /// Obtains the earliest year between the two specified years.
    /// </summary>
    [Pure] static abstract TSelf Min(TSelf x, TSelf y);

    /// <summary>
    /// Obtains the latest year between the two specified years.
    /// </summary>
    [Pure] static abstract TSelf Max(TSelf x, TSelf y);

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountYearsSince()")]
    static abstract int operator -(TSelf left, TSelf right);
}
