// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Gregorian;

using Calendrie;
using Calendrie.Specialized;

using NodaTime;

// Benchmarks for the addition.

public class Addition : GJComparisons
{
    private const int D7 = 7;        // No change of month.
    private const int D30 = 30;      // Change of month.
    private const int D401 = 401;    // "Slow-track".

    private readonly GJSample _sample;

    public Addition()
    {
        SampleKind = GJSampleKind.Fixed;
        _sample = new GJSample(Year, Month, Day);
    }

    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber() => _sample.DayNumber.PlusDays(D7).PlusDays(D30).PlusDays(D401);

    [Benchmark(Description = "CivilDate", Baseline = true)]
    public CivilDate WithCivilDate() => _sample.CivilDate.PlusDays(D7).PlusDays(D30).PlusDays(D401);

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate() => _sample.GregorianDate.PlusDays(D7).PlusDays(D30).PlusDays(D401);

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate() => _sample.LocalDate.PlusDays(D7).PlusDays(D30).PlusDays(D401);

    [Benchmark(Description = "DateOnly (BCL)")]
    public DateOnly WithDateOnly() => _sample.DateOnly.AddDays(D7).AddDays(D30).AddDays(D401);

    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime() => _sample.DateTime.AddDays(D7).AddDays(D30).AddDays(D401);
}
