// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines the mathematical operations on the day field of a time-related type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IDayArithmetic<TSelf>
    where TSelf : IDayArithmetic<TSelf>
{
    /// <summary>
    /// Counts the number of days elapsed since the specified value.
    /// </summary>
    [Pure] int CountDaysSince(TSelf other);

    /// <summary>
    /// Adds a number of days to the day field of the current instance,
    /// yielding a new value.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow either
    /// the capacity of the day field or the range of supported values.
    /// </exception>
    [Pure] TSelf PlusDays(int days);

    /// <summary>
    /// Returns the value obtained after adding one to the day field of the
    /// current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported value.</exception>
    [Pure] TSelf NextDay() => PlusDays(1);

    /// <summary>
    /// Returns the value obtained after adding minus one to the day field of
    /// the current instance.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported value.</exception>
    [Pure] TSelf PreviousDay() => PlusDays(1);
}
