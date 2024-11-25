// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using Calendrie.Specialized;

using NodaTime;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class GregorianPlusDays
{
    private DayNumber _dayNumber;
    private CivilDate _civilDate;
    private DateOnly _dateOnly;
    private DateTime _dateTime;
    private GregorianDate _gregorianDate;
    private LocalDate _localDate;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var sample = new GJSample { SampleKind = GJSampleKind.Fixed };

        _dayNumber = sample.DayNumber;
        _civilDate = sample.CivilDate;
        _dateTime = sample.DateTime;
        _dateOnly = sample.DateOnly;
        _gregorianDate = sample.GregorianDate;
        _localDate = sample.LocalDate;
    }

    //
    // No change of month
    //

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber7() => _dayNumber.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate7() => _civilDate.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate7() => _gregorianDate.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate7() => _localDate.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly7() => _dateOnly.AddDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime7() => _dateTime.AddDays(7);

    //
    // Change of month
    //

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber31() => _dayNumber.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate31() => _civilDate.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate31() => _gregorianDate.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate31() => _localDate.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly31() => _dateOnly.AddDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime31() => _dateTime.AddDays(31);

    //
    // Slow-track
    //

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber401() => _dayNumber.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate401() => _civilDate.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate401() => _gregorianDate.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate401() => _localDate.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly401() => _dateOnly.AddDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime401() => _dateTime.AddDays(401);
}
