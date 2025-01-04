// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

/// <summary>
/// Defines the mathematical operations on a calendar year.
/// <para>A type implementing this interface SHOULD also implement
/// <see cref="ISubtractionOperators{TSelf, TOther, TResult}"/> where
/// <c>TOther</c> is <typeparamref name="TSelf"/> and
/// <c>TResult</c> is <see cref="int"/>.</para>
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface ICalendarYearArithmetic<TSelf> :
    IAdditionOperators<TSelf, int, TSelf>,
    ISubtractionOperators<TSelf, int, TSelf>,
    IIncrementOperators<TSelf>,
    IDecrementOperators<TSelf>
    where TSelf : ICalendarYearArithmetic<TSelf>
{
    /// <summary>
    /// Counts the number of years elapsed since the specified year.
    /// </summary>
    int CountYearsSince(TSelf other);

    /// <summary>
    /// Adds a number of years to this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    TSelf PlusYears(int years);

    /// <summary>
    /// Obtains the year after this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    TSelf NextYear();

    /// <summary>
    /// Obtains the year before this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    TSelf PreviousYear();
}
