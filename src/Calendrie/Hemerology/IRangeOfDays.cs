// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core.Intervals;

/// <summary>
/// Defines a range of calendar dates.
/// </summary>
/// <typeparam name="TDate">The type of date object.</typeparam>
public interface IRangeOfDays<TDate>
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
    [Pure] Range<TDate> ToRangeOfDays();

    /// <summary>
    /// Obtains the sequence of all days in the current instance.
    /// </summary>
    [Pure] IEnumerable<TDate> EnumerateDays();

    /// <summary>
    /// Obtains the number of days in the current instance.
    /// </summary>
    [Pure] int CountDays();

    /// <summary>
    /// Determines whether the current instance contains the specified date or
    /// not.
    /// </summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
    [Pure] bool Contains(TDate date);
}
