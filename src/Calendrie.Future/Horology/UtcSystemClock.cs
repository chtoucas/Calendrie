// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Horology;

using Calendrie.Core;

// Beware, we use DateTime.Ticks but
// > "It does not include the number of ticks that are attributable to leap seconds."
// See
// - https://docs.microsoft.com/en-us/dotnet/api/system.datetime.ticks
// - https://github.com/dotnet/dotnet-api-docs/issues/966
//
// Consequences?
// Regarding DateTime,
// - In case of a positive leap second, 23:59:59 is repeated.
// - In case of a negative leap second, 23:59:59 is not valid but I don't
//   know how DateTime actually handles this case. Notice that no negative
//   leap second has ever occured so far.
//
// Being based on the OS clock, this clock is NOT monotonic.
// In fact, it does not matter, one SHOULD NOT use this clock for timing.
// See also
// - https://github.com/dotnet/runtime/issues/15207
// - https://github.com/dotnet/runtime/issues/5883

/// <summary>
/// Represents the system clock using the Coordinated Universal Time (UTC).
/// <para>See <see cref="SystemClocks.Utc"/>.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class UtcSystemClock : IClock
{
    /// <summary>
    /// Represents a singleton instance of the <see cref="UtcSystemClock"/> class.
    /// <para>This field is read-only.</para>
    /// </summary>
    internal static readonly UtcSystemClock Instance = new();

    private UtcSystemClock() { }

    /// <inheritdoc/>
    [Pure]
    public DayNumber Today()
    {
        // NB: the cast should always succeed.
        int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(DateTime.UtcNow.Ticks);
        return new DayNumber(daysSinceZero);
    }

    ///// <inheritdoc/>
    //[Pure]
    //public Moment Now()
    //{
    //    // This method works only because DateTime.Ticks does not account
    //    // for leap seconds!
    //    var now = DateTime.UtcNow;
    //    long daysSinceZero = TemporalArithmetic.DivideByTicksPerDay(now.Ticks);

    //    // NB: the casts should always succeed.
    //    var dayNumber = new DayNumber((int)daysSinceZero);
    //    var instantOfDay = InstantOfDay.FromTickOfDay(now.TimeOfDay.Ticks);

    //    return new Moment(dayNumber, instantOfDay);
    //}
}
