// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

// Funny interface: it's public but, having a static __internal__ method, it can
// ONLY be implemented from within friend assemblies. Why public? It's use as a
// type constraint by CalendarSystem and DateAdjuster.

/// <summary>
/// Defines an unsafe static factory method for the <typeparamref name="TDate"/>
/// type.
/// <para>This interface can ONLY be implemented from within friend assemblies.
/// </para>
/// </summary>
/// <typeparam name="TDate">The type of date object to create.</typeparam>
public interface IUnsafeDateFactory<out TDate>
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TDate"/> struct from
    /// the specified count of consecutive days since the epoch.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure] internal static abstract TDate UnsafeCreate(int daysSinceEpoch);
}
