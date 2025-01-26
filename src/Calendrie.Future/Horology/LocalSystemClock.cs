// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Horology;

using Calendrie.Core;

/// <summary>
/// Represents the system clock using the current time zone setting on this
/// machine.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class LocalSystemClock : IClock
{
    private LocalSystemClock() { }

    /// <summary>
    /// Gets a singleton instance of the <see cref="LocalSystemClock"/> class.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static LocalSystemClock Instance { get; } = new();

    /// <inheritdoc/>
    [Pure]
    public DayNumber Today()
    {
        var now = DateTime.Now;
        // NB: the cast should always succeed.
        int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(now.Ticks);
        return new DayNumber(daysSinceZero);
    }
}
