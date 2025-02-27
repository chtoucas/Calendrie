﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Defines a date.
/// </summary>
public interface IDate : IDateable, IAbsoluteDate
{
    /// <summary>
    /// Gets the companion calendar.
    /// </summary>
    static abstract Calendar Calendar { get; }
}

/// <summary>
/// Defines a date type.
/// <para>This interface SHOULD NOT be implemented by types participating in a
/// poly-calendar system; see <see cref="IDateBase{TSelf}"/> for a more
/// suitable interface.</para>
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IDate<TSelf> :
    IDateBase<TSelf>,
    IAbsoluteDate<TSelf>
    where TSelf : IDate<TSelf>
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSelf"/> struct
    /// from the specified date components.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the
    /// range of supported years.</exception>
    [Pure] static abstract TSelf Create(int year, int month, int day);

    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSelf"/> struct
    /// from the specified ordinal components.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The specified components
    /// do not form a valid date or <paramref name="year"/> is outside the
    /// range of supported years.</exception>
    [Pure] static abstract TSelf Create(int year, int dayOfYear);

    /// <summary>
    /// Attempts to create a new instance of the <typeparamref name="TSelf"/>
    /// struct from the specified date components.
    /// </summary>
    [Pure]
    static abstract bool TryCreate(
        int year, int month, int day, [NotNullWhen(true)] out TSelf? result);

    /// <summary>
    /// Attempts to create a new instance of the <typeparamref name="TSelf"/>
    /// struct from the specified ordinal components.
    /// </summary>
    [Pure]
    static abstract bool TryCreate(int year, int dayOfYear, [NotNullWhen(true)] out TSelf? result);
}
