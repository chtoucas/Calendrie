// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Gregorian;

using BenchmarkDotNet.Configs;

using Calendrie.Specialized;

using NodaTime;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class PlusDays : GJComparisons
{
    private readonly GJSample _sample;

    public PlusDays()
    {
        SampleKind = GJSampleKind.Fixed;
        _sample = new GJSample(Year, Month, Day);
    }

    //
    // No change of month
    //

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber7() => _sample.DayNumber.PlusDays(7);

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate7() => _sample.CivilDate.PlusDays(7);

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate7() => _sample.GregorianDate.PlusDays(7);

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate7() => _sample.LocalDate.PlusDays(7);

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly7() => _sample.DateOnly.AddDays(7);

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime7() => _sample.DateTime.AddDays(7);

    //
    // Change of month
    //

    [BenchmarkCategory("Medium")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber30() => _sample.DayNumber.PlusDays(30);

    [BenchmarkCategory("Medium")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate30() => _sample.CivilDate.PlusDays(30);

    [BenchmarkCategory("Medium")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate30() => _sample.GregorianDate.PlusDays(30);

    [BenchmarkCategory("Medium")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate30() => _sample.LocalDate.PlusDays(30);

    [BenchmarkCategory("Medium")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly30() => _sample.DateOnly.AddDays(30);

    [BenchmarkCategory("Medium")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime30() => _sample.DateTime.AddDays(30);

    //
    // Slow-track
    //

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber401() => _sample.DayNumber.PlusDays(401);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate401() => _sample.CivilDate.PlusDays(401);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate401() => _sample.GregorianDate.PlusDays(401);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate401() => _sample.LocalDate.PlusDays(401);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly401() => _sample.DateOnly.AddDays(401);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime401() => _sample.DateTime.AddDays(401);
}
