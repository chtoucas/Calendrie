﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Gregorian;

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

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber7() => _sample.DayNumber.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate7() => _sample.CivilDate.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate7() => _sample.GregorianDate.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate7() => _sample.LocalDate.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly7() => _sample.DateOnly.AddDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime7() => _sample.DateTime.AddDays(7);

    //
    // Change of month
    //

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber31() => _sample.DayNumber.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate31() => _sample.CivilDate.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate31() => _sample.GregorianDate.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate31() => _sample.LocalDate.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly31() => _sample.DateOnly.AddDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime31() => _sample.DateTime.AddDays(31);

    //
    // Slow-track
    //

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber401() => _sample.DayNumber.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate401() => _sample.CivilDate.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate401() => _sample.GregorianDate.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate401() => _sample.LocalDate.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly401() => _sample.DateOnly.AddDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime401() => _sample.DateTime.AddDays(401);
}
