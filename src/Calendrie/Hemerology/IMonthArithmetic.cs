// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines the mathematical operations on the month field of a time-related type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IMonthArithmetic<TSelf>
    where TSelf : IMonthArithmetic<TSelf>
{
    /// <summary>
    /// Counts the number of months elapsed since the specified value.
    /// </summary>
    [Pure] int CountMonthsSince(TSelf other);

    /// <summary>
    /// Adds a number of months to the month field of the current instance,
    /// yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of the month field or the range of supported values.
    /// </exception>
    [Pure] TSelf PlusMonths(int months);

    /// <summary>
    /// Returns the value obtained after adding one to the month field of the
    /// current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported value.</exception>
    [Pure] TSelf NextMonth() => PlusMonths(1);

    /// <summary>
    /// Returns the value obtained after adding minus one to the month field of
    /// the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported value.</exception>
    [Pure] TSelf PreviousMonth() => PlusMonths(-1);
}
