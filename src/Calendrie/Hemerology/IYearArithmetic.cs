// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines the mathematical operations related to the year unit.
/// </summary>
/// <typeparam name="T">The type that implements this interface.</typeparam>
public interface IYearArithmetic<T>
{
    /// <summary>
    /// Counts the number of years elapsed since the specified year.
    /// </summary>
    [Pure] int CountYearsSince(T other);

    /// <summary>
    /// Adds a number of years to this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    [Pure] T PlusYears(int years);

    /// <summary>
    /// Obtains the year after this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    [Pure] T NextYear() => PlusYears(1);

    /// <summary>
    /// Obtains the year before this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    [Pure] T PreviousYear() => PlusYears(-1);
}
