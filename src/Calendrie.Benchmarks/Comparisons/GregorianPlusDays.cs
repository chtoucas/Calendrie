// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using Calendrie.Specialized;

using NodaTime;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class GregorianPlusDays : GregorianComparisons
{
    //
    // No change of month
    //

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber7() => dayNumber.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate7() => civilDate.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate7() => gregorianDate.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate7() => localDate.PlusDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly7() => dateOnly.AddDays(7);

    [BenchmarkCategory("+7")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime7() => dateTime.AddDays(7);

    //
    // Change of month
    //

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber31() => dayNumber.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate31() => civilDate.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate31() => gregorianDate.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate31() => localDate.PlusDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly31() => dateOnly.AddDays(31);

    [BenchmarkCategory("+31")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime31() => dateTime.AddDays(31);

    //
    // Slow-track
    //

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber401() => dayNumber.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate401() => civilDate.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate401() => gregorianDate.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate401() => localDate.PlusDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly401() => dateOnly.AddDays(401);

    [BenchmarkCategory("+401")]
    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime401() => dateTime.AddDays(401);
}
