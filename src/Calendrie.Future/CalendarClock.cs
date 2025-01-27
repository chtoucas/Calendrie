// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

using Calendrie.Hemerology;
using Calendrie.Horology;
using Calendrie.Systems;

/// <summary>
/// Represents a clock for the Civil calendar.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class CalendarClock : IClock
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarClock"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="clock"/> is
    /// <see langword="null"/>.</exception>
    public CalendarClock(IClock clock)
    {
        ArgumentNullException.ThrowIfNull(clock);

        Clock = clock;
    }

    /// <summary>
    /// Gets an instance of the <see cref="Clock"/> class for the
    /// system clock using the current time zone setting on this machine.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CalendarClock Local { get; } = new(LocalSystemClock.Instance);

    /// <summary>
    /// Gets an instance of the <see cref="Clock"/> class for the
    /// system clock using the Coordinated Universal Time (UTC).
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static CalendarClock Utc { get; } = new(UtcSystemClock.Instance);

    /// <summary>
    /// Gets the clock used to provide the current day.
    /// </summary>
    public IClock Clock { get; }

    /// <summary>
    /// Obtains a <see cref="DayNumber"/> value representing the current day.
    /// </summary>
    [Pure]
    public DayNumber Today() => Clock.Today();

    /// <summary>
    /// Obtains a <typeparamref name="TDate"/> value representing the current date.
    /// </summary>
    [Pure]
    public TDate GetCurrentDate<TDate>() where TDate : IAbsoluteDate<TDate>
    {
        return TDate.FromDayNumber(Clock.Today());
    }

    /// <summary>
    /// Obtains a <see cref="CivilDate"/> value representing the current date.
    /// </summary>
    [Pure]
    public CivilDate GetCurrentCivilDate() => CivilDate.FromAbsoluteDate(Clock.Today());

    /// <summary>
    /// Obtains a <see cref="GregorianDate"/> value representing the current date.
    /// </summary>
    [Pure]
    public GregorianDate GetCurrentGregorianDate() => GregorianDate.FromAbsoluteDate(Clock.Today());

    /// <summary>
    /// Obtains a <see cref="JulianDate"/> value representing the current date.
    /// </summary>
    [Pure]
    public JulianDate GetCurrentJulianDate() => JulianDate.FromAbsoluteDate(Clock.Today());
}
