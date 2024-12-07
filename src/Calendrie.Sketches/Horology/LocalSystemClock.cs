﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Horology;

using Calendrie.Core;

using static Calendrie.Core.TemporalConstants;

// See the comments in UtcSystemClock.

/// <summary>
/// Represents the system clock using the current time zone setting on this
/// machine.
/// <para>See <see cref="SystemClocks.Local"/>.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class LocalSystemClock : IClock
{
    /// <summary>
    /// Represents a singleton instance of the <see cref="LocalSystemClock"/>
    /// class.
    /// <para>This field is read-only.</para>
    /// </summary>
    internal static readonly LocalSystemClock Instance = new();

    private LocalSystemClock() { }

    /// <inheritdoc/>
    [Pure]
    public Moment Now()
    {
        var now = DateTime.Now;
        ulong ticksSinceZero = (ulong)now.Ticks;
        ulong daysSinceZero = TemporalArithmetic.DivideByTicksPerDay(ticksSinceZero, out ulong tickOfDay);
        ulong millisecondOfDay = tickOfDay / TicksPerMillisecond;

        var dayNumber = new DayNumber((int)daysSinceZero);
        var timeOfDay = TimeOfDay.FromMillisecondOfDay((int)millisecondOfDay);

        return new Moment(dayNumber, timeOfDay);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber Today()
    {
        int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(DateTime.Now.Ticks);
        return new DayNumber(daysSinceZero);
    }
}
