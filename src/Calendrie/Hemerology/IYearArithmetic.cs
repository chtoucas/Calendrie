// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines the mathematical operations on the year field of a time-related type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IYearArithmetic<TSelf>
    where TSelf : IYearArithmetic<TSelf>
{
    /// <summary>
    /// Counts the number of years elapsed since the specified value.
    /// </summary>
    [Pure] int CountYearsSince(TSelf other);

    /// <summary>
    /// Adds a number of years to the year field of the current instance,
    /// yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of the year field or the range of supported values.
    /// </exception>
    [Pure] TSelf PlusYears(int years);

    /// <summary>
    /// Returns the value obtained after adding one to the year field of the
    /// current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported value.</exception>
    [Pure] TSelf NextYear() => PlusYears(1);

    /// <summary>
    /// Returns the value obtained after adding minus one to the year field of
    /// the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported value.</exception>
    [Pure] TSelf PreviousYear() => PlusYears(-1);
}
