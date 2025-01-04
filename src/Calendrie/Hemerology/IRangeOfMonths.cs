// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core.Intervals;

/// <summary>
/// Defines a range of calendar months.
/// </summary>
/// <typeparam name="TMonth">The type of month object.</typeparam>
public interface IRangeOfMonths<TMonth> : ISetMembership<TMonth>
    where TMonth : struct, IEquatable<TMonth>, IComparable<TMonth>
{
    /// <summary>
    /// Gets the earliest month of the current instance.
    /// </summary>
    TMonth MinMonth { get; }

    /// <summary>
    /// Gets the latest month of the current instance.
    /// </summary>
    TMonth MaxMonth { get; }

    /// <summary>
    /// Converts the current instance to a range of months.
    /// </summary>
    [Pure] Range<TMonth> ToRangeOfMonths();

    /// <summary>
    /// Obtains the sequence of all months in the current instance.
    /// </summary>
    [Pure] IEnumerable<TMonth> EnumerateMonths();

    /// <summary>
    /// Obtains the number of months in the current instance.
    /// </summary>
    [Pure] int CountMonths();
}
