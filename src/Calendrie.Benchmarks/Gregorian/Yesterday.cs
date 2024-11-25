// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Gregorian;

using Calendrie.Specialized;

using NodaTime;

public class Yesterday : GJComparisons
{
    private readonly GJSample _sample;

    public Yesterday()
    {
        SampleKind = GJSampleKind.Fixed;
        _sample = new GJSample(Year, Month, Day);
    }

    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber() => _sample.DayNumber.PreviousDay();

    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate() => _sample.CivilDate.PreviousDay();

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate() => _sample.GregorianDate.PreviousDay();

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate() => _sample.LocalDate.PlusDays(-1);

    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly() => _sample.DateOnly.AddDays(-1);

    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime() => _sample.DateTime.AddDays(-1);
}
