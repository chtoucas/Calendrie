// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

// La réforme grégorienne entraîne une rupture dans la suite des jours
// ou des mois. Le passage officiel du calendrier julien au calendrier
// grégorien se fait le lendemain du jeudi 4 octobre 1582 (julien), on
// est alors le vendredi 15 octobre 1582 (grégorien), c-à-d le vendredi
// 5 octobre 1582 (julien).

public sealed record GregorianReform
{
    // Introduction officielle de la réforme grégorienne.
    public static readonly GregorianReform Official = new();

    private GregorianReform()
        : this(
            new JulianDate(1582, 10, 4),
            new GregorianDate(1582, 10, 15),
            null)
    { }

    private GregorianReform(
        JulianDate lastJulianDate,
        GregorianDate firstGregorianDate,
        DayNumber? switchover)
    {
        LastJulianDate = lastJulianDate;
        FirstGregorianDate = firstGregorianDate;
        Switchover = switchover ?? FirstGregorianDate.DayNumber;
        SecularShift = InitSecularShift();
    }

    /// <summary>
    /// Gets the last Julian date.
    /// </summary>
    public JulianDate LastJulianDate { get; }

    /// <summary>
    /// Gets the first Gregorian date.
    /// </summary>
    public GregorianDate FirstGregorianDate { get; }

    /// <summary>
    /// Gets the first Gregorian <see cref="DayNumber"/>.
    /// </summary>
    public DayNumber Switchover { get; }

    /// <summary>
    /// Gets the initial secular shift.
    /// </summary>
    public int SecularShift { get; }

    [Pure]
    public static GregorianReform FromLastJulianDate(JulianDate date)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(date, Official.LastJulianDate);

        var switchover = date.DayNumber + 1;
        var firstGregorianDate = GregorianDate.FromDayNumber(switchover);

        return new GregorianReform(date, firstGregorianDate, switchover);
    }

    [Pure]
    public static GregorianReform FromFirstGregorianDate(GregorianDate date)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(date, Official.FirstGregorianDate);

        var switchover = date.DayNumber;
        var lastJulianDate = JulianDate.FromDayNumber(switchover - 1);

        return new GregorianReform(lastJulianDate, date, switchover);
    }

    [Pure]
    private int InitSecularShift()
    {
        var (y, m, d) = FirstGregorianDate;
        var dayNumber = new JulianDate(y, m, d).DayNumber;
        return dayNumber - Switchover;
    }
}
