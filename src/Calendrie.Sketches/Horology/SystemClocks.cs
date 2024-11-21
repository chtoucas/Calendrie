// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Horology;

/// <summary>
/// Provides system clocks.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class SystemClocks
{
    /// <summary>
    /// Gets an instance of the system clock using the current time zone setting
    /// on this machine.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static LocalSystemClock Local => LocalSystemClock.Instance;

    /// <summary>
    /// Gets an instance of the system clock using the Coordinated Universal Time
    /// (UTC).
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static UtcSystemClock Utc => UtcSystemClock.Instance;
}
