// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines the standard mathematical operations related to the day unit.
/// </summary>
/// <typeparam name="T">The type that implements this interface.</typeparam>
public interface IDayArithmetic<T>
{
    /// <summary>
    /// Counts the number of days elapsed since the specified date.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="other"/> does not
    /// belong to the calendar of the current instance.</exception>
    [Pure] int CountDaysSince(T other);

    /// <summary>
    /// Adds a number of days to this date instance, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure] T PlusDays(int days);

    /// <summary>
    /// Obtains the day after this date instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported date.</exception>
    [Pure] T NextDay() => PlusDays(1);

    /// <summary>
    /// Obtains the day before this date instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported date.</exception>
    [Pure] T PreviousDay() => PlusDays(1);
}
