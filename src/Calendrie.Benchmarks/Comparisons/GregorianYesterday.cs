// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using Calendrie.Specialized;

using NodaTime;

public class GregorianYesterday
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

    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber() => _dayNumber.PreviousDay();

    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate() => _civilDate.PreviousDay();

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate() => _gregorianDate.PreviousDay();

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate() => _localDate.PlusDays(-1);

    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly() => _dateOnly.AddDays(-1);

    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime() => _dateTime.AddDays(-1);
}
