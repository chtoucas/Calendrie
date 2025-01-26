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
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class UtcSystemClock : IClock
{
    private UtcSystemClock() { }

    /// <summary>
    /// Gets a singleton instance of the <see cref="UtcSystemClock"/> class.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static UtcSystemClock Instance { get; } = new();

    /// <inheritdoc/>
    [Pure]
    public DayNumber Today()
    {
        var now = DateTime.UtcNow;
        // NB: the cast should always succeed.
        int daysSinceZero = (int)TemporalArithmetic.DivideByTicksPerDay(now.Ticks);
        return new DayNumber(daysSinceZero);
    }
}
