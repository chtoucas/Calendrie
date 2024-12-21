// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

// Keep this interface internal, it was created only to ease testing.

/// <summary>
/// Defines adjustment methods.
/// </summary>
/// <typeparam name="TDate">The date type that implements this interface.
/// </typeparam>
internal interface IAdjustableDate<TDate> where TDate : IDateable
{
    /// <summary>
    /// Adjusts the current instance using the specified adjuster.
    /// <para>If the adjuster throws, this method will propagate the exception.
    /// </para>
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="adjuster"/> is
    /// <see langword="null"/>.</exception>
    [Pure] TDate Adjust(Func<TDate, TDate> adjuster);

    /// <summary>
    /// Adjusts the year field to the specified value, yielding a new date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure] TDate WithYear(int newYear);

    /// <summary>
    /// Adjusts the month field to the specified value, yielding a new date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure] TDate WithMonth(int newMonth);

    /// <summary>
    /// Adjusts the day of the month field to the specified value, yielding a
    /// new date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure] TDate WithDay(int newDay);

    /// <summary>
    /// Adjusts the day of the year field to the specified value, yielding a new
    /// date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure] TDate WithDayOfYear(int newDayOfYear);
}
