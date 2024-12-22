// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

// Keep this interface internal, it was created only to simplify testing.

/// <summary>
/// Defines the operations for finding a close by day of the week.
/// </summary>
/// <typeparam name="TDate">The date type that implements this interface.
/// </typeparam>
internal interface IAdjustableDayOfWeek<out TDate> where TDate : IAbsoluteDate
{
    /// <summary>
    /// Obtains the day strictly before the current instance that falls on the
    /// specified day of the week.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure] TDate Previous(DayOfWeek dayOfWeek);

    /// <summary>
    /// Obtains the day on or before the current instance that falls on the
    /// specified day of the week.
    /// <para>If the day already falls on the given day of the week, returns the
    /// current instance.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure] TDate PreviousOrSame(DayOfWeek dayOfWeek);

    /// <summary>
    /// Obtains the nearest day that falls on the specified day of the week.
    /// <para>If the day already falls on the given day of the week, returns the
    /// current instance.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure] TDate Nearest(DayOfWeek dayOfWeek);

    /// <summary>
    /// Obtains the day on or after the current instance that falls on the
    /// specified day of the week.
    /// <para>If the day already falls on the given day of the week, returns the
    /// current instance.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure] TDate NextOrSame(DayOfWeek dayOfWeek);

    /// <summary>
    /// Obtains the day strictly after the current instance that falls on the
    /// specified day of the week.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "VB.NET Next statement")]
    [Pure] TDate Next(DayOfWeek dayOfWeek);
}
