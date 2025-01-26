// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Horology;

using Calendrie.Core;

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
    public DayNumber Today()
    {
        var now = DateTime.Now;
        int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(now.Ticks);

        return new DayNumber(daysSinceZero);
    }

    ///// <inheritdoc/>
    //[Pure]
    //public Moment Now()
    //{
    //    // This method works only because DateTime.Ticks does not account
    //    // for leap seconds!

    //    var now = DateTime.Now;
    //    long daysSinceZero = TemporalArithmetic.DivideByTicksPerDay(now.Ticks);

    //    // NB: the casts should always succeed.
    //    var dayNumber = new DayNumber((int)daysSinceZero);
    //    var instantOfDay = InstantOfDay.FromTickOfDay(now.TimeOfDay.Ticks);

    //    return new Moment(dayNumber, instantOfDay);
    //}
}
