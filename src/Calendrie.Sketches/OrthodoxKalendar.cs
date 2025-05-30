﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie;

using Calendrie.Systems;

// Calendrier liturgique orthodoxe.
//
// Références :
// - https://fr.wikipedia.org/wiki/Calendrier_chr%C3%A9tien
// - https://fr.wikipedia.org/wiki/Calendrier_liturgique_orthodoxe
// - https://en.wikipedia.org/wiki/Eastern_Orthodox_liturgical_calendar
public sealed class OrthodoxKalendar
{
    private readonly int _year;

    public OrthodoxKalendar(int year)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(year, 0);

        _year = year;
    }

    private JulianDate? _easter;
    public JulianDate Easter => _easter ??= InitEaster(_year);

    private JulianDate? _paschalMoon;
    public JulianDate PaschalMoon => _paschalMoon ??= InitPaschalMoon(_year);

    // GetPaschal ???
    // D.&.R (8.1) p.115, orthodox-easter()
    [Pure]
    private static JulianDate InitEaster(int year)
    {
        var paschalMoon = InitPaschalMoon(year);
        return paschalMoon.Next(DayOfWeek.Sunday);
    }

    [Pure]
    private static JulianDate InitPaschalMoon(int year)
    {
        int epact = getEpact(year);
        return new JulianDate(year, 4, 19) - epact;

        [Pure]
        static int getEpact(int year)
        {
            Debug.Assert(year >= 0);

            return (14 + 11 * (year % 19)) % 30;
        }
    }
}
