// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using Calendrie.Specialized;

using NodaTime;

public abstract class JulianComparisons
{
    private protected DayNumber dayNumber;
    private protected JulianDate julianDate;
    private protected DateOnly dateOnly;
    private protected DateTime dateTime;
    private protected LocalDate localDate;

    private protected JulianComparisons(GJDateType type = GJDateType.FixedFast)
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

        dateTime = new(y, m, d, new System.Globalization.JulianCalendar());
        dateOnly = new(y, m, d, new System.Globalization.JulianCalendar());
        localDate = new(y, m, d, CalendarSystem.Julian);
    }
}
