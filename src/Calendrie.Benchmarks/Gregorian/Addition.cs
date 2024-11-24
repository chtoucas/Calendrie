// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Gregorian;

using Calendrie;
using Calendrie.Specialized;

using NodaTime;

// Benchmarks for the addition.

public class Addition : GJSampleData
{
    private const int D7 = 7;        // No change of month.
    private const int D30 = 30;      // Change of month.
    private const int D401 = 401;    // "Slow-track".

    public Addition() { Option = BenchmarkOption.Fixed; }

    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber() => SampleDayNumber.PlusDays(D7).PlusDays(D30).PlusDays(D401);

    [Benchmark(Description = "CivilDate", Baseline = true)]
    public CivilDate WithCivilDate() => SampleCivilDate.PlusDays(D7).PlusDays(D30).PlusDays(D401);

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate() => SampleGregorianDate.PlusDays(D7).PlusDays(D30).PlusDays(D401);

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate() => SampleLocalDate.PlusDays(D7).PlusDays(D30).PlusDays(D401);

    [Benchmark(Description = "DateOnly (BCL)")]
    public DateOnly WithDateOnly() => SampleDateOnly.AddDays(D7).AddDays(D30).AddDays(D401);

    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime() => SampleDateTime.AddDays(D7).AddDays(D30).AddDays(D401);
}
