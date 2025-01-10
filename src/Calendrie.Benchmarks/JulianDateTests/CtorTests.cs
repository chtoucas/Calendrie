// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianDateTests;

using Calendrie;
using Calendrie.Systems;

using NodaTime;

public class CtorTests
{
    private int _year, _month, _day;

    [GlobalSetup]
    public void GlobalSetup() =>
        (_year, _month, _day) = BenchmarkHelpers.CreateJulianParts();

    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber() => DayNumber.FromJulianParts(_year, _month, _day);

    [Benchmark(Description = "JulianDate")]
    public JulianDate WithJulianDate() => new(_year, _month, _day);

    [Benchmark(Description = "JulianDate_Plain")]
    public MyJulianDate WithPlainJulianDate() => new(_year, _month, _day);

    [Benchmark(Description = "LocalDate_NodaTime")]
    public LocalDate WithLocalDate() => new(_year, _month, _day, CalendarSystem.Julian);

    [Benchmark(Description = "DateOnly_BCL", Baseline = true)]
    public DateOnly WithDateOnly() => new(_year, _month, _day, new System.Globalization.JulianCalendar());

    [Benchmark(Description = "DateTime_BCL")]
    public DateTime WithDateTime() => new(_year, _month, _day, new System.Globalization.JulianCalendar());
}
