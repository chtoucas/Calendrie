// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

/// <summary>
/// Defines the base for other calendar month types.
/// </summary>
/// <typeparam name="TSelf">The month type that implements this interface.
/// </typeparam>
public interface ICalendarMonthBase<TSelf> :
    ICalendarMonth,
    // Comparison
    IEqualityOperators<TSelf, TSelf, bool>,
    IEquatable<TSelf>,
    IComparisonOperators<TSelf, TSelf, bool>,
    IComparable<TSelf>,
    IComparable,
    // Arithmetic
    IMonthArithmetic<TSelf>,
    //ISubtractionOperators<TSelf, TSelf, int>, // Cannot be added, but see below
    IAdditionOperators<TSelf, int, TSelf>,
    ISubtractionOperators<TSelf, int, TSelf>,
    IIncrementOperators<TSelf>,
    IDecrementOperators<TSelf>,
    // Non-standard math ops
    IYearArithmetic<TSelf>
    where TSelf : ICalendarMonthBase<TSelf>
{
    /// <summary>
    /// Obtains the earliest month between the two specified months.
    /// </summary>
    [Pure] static abstract TSelf Min(TSelf x, TSelf y);

    /// <summary>
    /// Obtains the latest month between the two specified months.
    /// </summary>
    [Pure] static abstract TSelf Max(TSelf x, TSelf y);

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountMonthsSince()")]
    static abstract int operator -(TSelf left, TSelf right);

    /// <summary>
    /// Adjusts the year field to the specified value, yielding a new month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified month cannot
    /// be converted into the new calendar, the resulting year would be outside
    /// its range of years.</exception>
    [Pure] TSelf WithYear(int newYear);

    /// <summary>
    /// Adjusts the month field to the specified value, yielding a new month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting month would
    /// be invalid.</exception>
    [Pure] TSelf WithMonth(int newMonth);
}
