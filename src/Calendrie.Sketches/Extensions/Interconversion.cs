// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using Calendrie.Hemerology;

public static class Interconversion
{
    /// <summary>
    /// Converts the specified date to a <typeparamref name="TResult"/> value.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="date"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="date"/>
    /// is outside the range of supported values by <typeparamref name="TResult"/>.
    /// </exception>
    /// <typeparam name="TResult">The type of the absolute date result.
    /// </typeparam>
    [Pure]
    public static TResult ConvertTo<TResult>(this IAbsoluteDate date)
        where TResult : IAbsoluteDate<TResult>
    {
        ArgumentNullException.ThrowIfNull(date);

        return TResult.FromDayNumber(date.DayNumber);
    }
}
