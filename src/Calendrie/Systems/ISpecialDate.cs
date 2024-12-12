// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

/// <summary>
/// Defines a date type with a companion calendar of fixed type.
/// <para>This interface SHOULD NOT be implemented by date types participating
/// in a poly-calendar system.</para>
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface ISpecialDate<TSelf> : IDate<TSelf>
    where TSelf : IDate<TSelf>
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSelf"/> struct from
    /// the specified count of consecutive days since the epoch.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure] internal static abstract TSelf FromDaysSinceEpochUnchecked(int daysSinceEpoch);
}
