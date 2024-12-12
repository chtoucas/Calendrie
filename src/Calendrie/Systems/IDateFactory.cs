// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

/// <summary>
/// Defines a factory method to create a date from the count of consecutive days
/// since the epoch of the companion calendar.
/// <para>This interface SHOULD NOT be implemented by date types participating
/// in a poly-calendar system.</para>
/// </summary>
/// <typeparam name="TDate">The date type.</typeparam>
public interface IDateFactory<TDate> where TDate : IFixedDate
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TDate"/> struct from
    /// the specified count of consecutive days since the epoch.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure] internal static abstract TDate FromDaysSinceEpochUnchecked(int daysSinceEpoch);
}
