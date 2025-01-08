// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

/// <summary>
/// Defines the base for other absolute date types.
/// <para>A date is said to be <i>absolute</i> if it's attached to a global
/// timeline. In this project, it means that it can be mapped to a
/// <see cref="DayNumber"/>.</para>
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IAbsoluteDateBase<TSelf> :
    IAbsoluteDate,
    // Comparison
    IEqualityOperators<TSelf, TSelf, bool>,
    IEquatable<TSelf>,
    IComparisonOperators<TSelf, TSelf, bool>,
    IComparable<TSelf>,
    IComparable,
    // Arithmetic
    IDayFieldMath<TSelf>,
    //ISubtractionOperators<TSelf, TSelf, int>, // Cannot be added, but see below
    IAdditionOperators<TSelf, int, TSelf>,
    ISubtractionOperators<TSelf, int, TSelf>,
    IIncrementOperators<TSelf>,
    IDecrementOperators<TSelf>
    where TSelf : IAbsoluteDateBase<TSelf>
{
    /// <summary>
    /// Obtains the earliest date between the two specified dates.
    /// </summary>
    [Pure] static abstract TSelf Min(TSelf x, TSelf y);

    /// <summary>
    /// Obtains the latest date between the two specified dates.
    /// </summary>
    [Pure] static abstract TSelf Max(TSelf x, TSelf y);

    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See CountDaysSince()")]
    static abstract int operator -(TSelf left, TSelf right);

    //
    // Find close by day of the week
    //

    /// <summary>
    /// Obtains the day strictly before the current instance that falls on the
    /// specified day of the week.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure] TSelf Previous(DayOfWeek dayOfWeek);

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
    [Pure] TSelf PreviousOrSame(DayOfWeek dayOfWeek);

    /// <summary>
    /// Obtains the nearest day that falls on the specified day of the week.
    /// <para>If the day already falls on the given day of the week, returns the
    /// current instance.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [Pure] TSelf Nearest(DayOfWeek dayOfWeek);

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
    [Pure] TSelf NextOrSame(DayOfWeek dayOfWeek);

    /// <summary>
    /// Obtains the day strictly after the current instance that falls on the
    /// specified day of the week.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfWeek"/>
    /// is not a valid day of the week.</exception>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported days.</exception>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "VB.NET Next statement")]
    [Pure] TSelf Next(DayOfWeek dayOfWeek);
}
