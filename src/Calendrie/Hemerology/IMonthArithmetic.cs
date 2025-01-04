// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines the mathematical operations related to the month unit.
/// </summary>
/// <typeparam name="T">The type that implements this interface.</typeparam>
public interface IMonthArithmetic<T>
{
    /// <summary>
    /// Counts the number of months elapsed since the specified month.
    /// </summary>
    [Pure] int CountMonthsSince(T other);

    /// <summary>
    /// Adds a number of months to this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported months.</exception>
    [Pure] T PlusMonths(int months);

    /// <summary>
    /// Obtains the month after this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported month.</exception>
    [Pure] T NextMonth() => PlusMonths(1);

    /// <summary>
    /// Obtains the month before this month instance, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported month.</exception>
    [Pure] T PreviousMonth() => PlusMonths(-1);
}
