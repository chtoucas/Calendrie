// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using Calendrie.Specialized;

using NodaTime;

public class JulianCtor
{
    private int _year, _month, _day;

    [GlobalSetup]
    public void GlobalSetup() =>
        (_year, _month, _day) = BenchmarkHelpers.CreateJulianParts();

    [Benchmark(Description = "DayNumber", Baseline = true)]
    public DayNumber WithDayNumber() => DayNumber.FromJulianParts(_year, _month, _day);

    [Benchmark(Description = "JulianDate")]
    public JulianDate WithJulianDate() => new(_year, _month, _day);

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate() => new(_year, _month, _day, CalendarSystem.Julian);

    [Benchmark(Description = "DateOnly (BCL)")]
    public DateOnly WithDateOnly() => new(_year, _month, _day, new System.Globalization.JulianCalendar());

    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime() => new(_year, _month, _day, new System.Globalization.JulianCalendar());
}
