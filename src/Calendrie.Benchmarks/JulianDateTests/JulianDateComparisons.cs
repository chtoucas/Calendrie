// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianDateTests;

using Calendrie;
using Calendrie.Systems;

using NodaTime;

public abstract class JulianDateComparisons
{
    protected static readonly System.Globalization.JulianCalendar BclJulianCalendar = new();

    private protected DayNumber dayNumber;
    private protected JulianDate julianDate;
    private protected MyJulianDate plainJulianDate;
    private protected DateOnly dateOnly;
    private protected DateTime dateTime;
    private protected LocalDate localDate;

    private protected JulianDateComparisons(GJDateType type = GJDateType.FixedFast)
    {
        Parts = BenchmarkHelpers.CreateJulianParts(type);
    }

    protected DateParts Parts { get; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        var (y, m, d) = Parts;

        julianDate = new(y, m, d);
        dayNumber = julianDate.DayNumber;

        plainJulianDate = new(y, m, d);

        dateTime = new(y, m, d, BclJulianCalendar);
        dateOnly = new(y, m, d, BclJulianCalendar);
        localDate = new(y, m, d, CalendarSystem.Julian);
    }
}
