// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using Calendrie.Specialized;

using NodaTime;

public class GregorianCtor
{
    private int _year, _month, _day;

    [GlobalSetup]
    public void GlobalSetup() =>
        (_year, _month, _day) = BenchmarkHelpers.CreateGregorianParts();

    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber() => DayNumber.FromGregorianParts(_year, _month, _day);

    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate() => new(_year, _month, _day);

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate() => new(_year, _month, _day);

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate() => new(_year, _month, _day);

    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly() => new(_year, _month, _day);

    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime() => new(_year, _month, _day);
}
