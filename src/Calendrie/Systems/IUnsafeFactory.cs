// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

// Funny interface: it's public but, having a static __internal__ method, it can
// ONLY be implemented from within friend assemblies. Why public? It's use as a
// type constraint by CalendarSystem and others.

/// <summary>
/// Defines an unsafe static factory method for the <typeparamref name="T"/>
/// type.
/// <para>This interface can ONLY be implemented from within friend assemblies.
/// </para>
/// </summary>
/// <typeparam name="T">The type of date object to create.</typeparam>
public interface IUnsafeFactory<out T>
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="T"/> struct from
    /// the specified count of consecutive units since the epoch.
    /// <para>This method does NOT validate its parameter.</para>
    /// </summary>
    [Pure] internal static abstract T UnsafeCreate(int unitsSinceEpoch);
}
