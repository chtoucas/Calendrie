// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.GregorianDateTests;

#if TEMP_BCL_CODE
using Calendrie.Systems;
#else
using Calendrie;
using Calendrie.Systems;

using NodaTime;
#endif

[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class CtorTests
{
    private int _yearFast, _monthFast, _dayFast;
    private int _yearSlow, _monthSlow, _daySlow;

    [GlobalSetup]
    public void GlobalSetup()
    {
        (_yearFast, _monthFast, _dayFast) = BenchmarkHelpers.CreateGregorianParts(GJDateType.FixedFast);
        (_yearSlow, _monthSlow, _daySlow) = BenchmarkHelpers.CreateGregorianParts(GJDateType.FixedSlow);
    }

    //
    // Fast track
    //

#if !TEMP_BCL_CODE
    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumberFast() => DayNumber.FromGregorianParts(_yearFast, _monthFast, _dayFast);
#endif

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDateFast() => new(_yearFast, _monthFast, _dayFast);

#if !TEMP_BCL_CODE
    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "CivilDate_Plain")]
    public PlainCivilDate WithPlainCivilDateFast() => new(_yearFast, _monthFast, _dayFast);

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDateFast() => new(_yearFast, _monthFast, _dayFast);

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "LocalDate_NodaTime")]
    public LocalDate WithLocalDateFast() => new(_yearFast, _monthFast, _dayFast);

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "DateOnly_BCL", Baseline = true)]
    public DateOnly WithDateOnlyFast() => new(_yearFast, _monthFast, _dayFast);
#endif

    [BenchmarkCategory("Fast")]
#if TEMP_BCL_CODE
    [Benchmark(Description = "DateTime_BCL", Baseline = true)]
#else
    [Benchmark(Description = "DateTime_BCL")]
#endif
    public DateTime WithDateTimeFast() => new(_yearFast, _monthFast, _dayFast);

    //
    // Slow track
    //

#if !TEMP_BCL_CODE
    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumberSlow() => DayNumber.FromGregorianParts(_yearSlow, _monthSlow, _daySlow);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDateSlow() => new(_yearSlow, _monthSlow, _daySlow);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "CivilDate_Plain")]
    public PlainCivilDate WithPlainCivilDateSlow() => new(_yearSlow, _monthSlow, _daySlow);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDateSlow() => new(_yearSlow, _monthSlow, _daySlow);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "LocalDate_NodaTime")]
    public LocalDate WithLocalDateSlow() => new(_yearSlow, _monthSlow, _daySlow);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "DateOnly_BCL", Baseline = true)]
    public DateOnly WithDateOnlySlow() => new(_yearSlow, _monthSlow, _daySlow);

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "DateTime_BCL")]
    public DateTime WithDateTimeSlow() => new(_yearSlow, _monthSlow, _daySlow);
#endif
}
