// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

/// <summary>
/// Defines the mathematical operations on a calendar month.
/// <para>A type implementing this interface SHOULD also implement
/// <see cref="ISubtractionOperators{TSelf, TOther, TResult}"/> where
/// <c>TOther</c> is <typeparamref name="TSelf"/> and
/// <c>TResult</c> is <see cref="int"/>.</para>
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface ICalendarMonthArithmetic<TSelf> :
    IAdditionOperators<TSelf, int, TSelf>,
    ISubtractionOperators<TSelf, int, TSelf>,
    IIncrementOperators<TSelf>,
    IDecrementOperators<TSelf>
    where TSelf : ICalendarMonthArithmetic<TSelf>
{
    /// <summary>
    /// Counts the number of months elapsed since the specified month.
    /// </summary>
    int CountMonthsSince(TSelf other);

    /// <summary>
    /// Adds a number of months to this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    TSelf PlusMonths(int months);

    /// <summary>
    /// Obtains the month after this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    TSelf NextMonth();

    /// <summary>
    /// Obtains the month before this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    TSelf PreviousMonth();
}
