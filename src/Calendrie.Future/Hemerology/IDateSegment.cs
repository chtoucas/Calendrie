// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core.Intervals;

/// <summary>
/// Defines a finite sequence of consecutive days.
/// </summary>
/// <typeparam name="TDate">The type of date object.</typeparam>
public interface IDateSegment<TDate>
    where TDate : struct, IEquatable<TDate>, IComparable<TDate>
{
    /// <summary>
    /// Gets the earliest day of the current instance.
    /// </summary>
    TDate MinDay { get; }

    /// <summary>
    /// Gets the latest day of the current instance.
    /// </summary>
    TDate MaxDay { get; }

    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    [Pure] Range<TDate> ToDayRange();

    /// <summary>
    /// Obtains the sequence of all days in the current instance.
    /// </summary>
    [Pure] IEnumerable<TDate> EnumerateDays();

    /// <summary>
    /// Obtains the number of days in the current instance.
    /// </summary>
    [Pure] int CountDays();
}
