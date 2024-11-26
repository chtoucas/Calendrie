// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using Calendrie.Specialized;

using NodaTime;

public class GregorianYesterday : GregorianComparisons
{
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber() => dayNumber.PreviousDay();

    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate() => civilDate.PreviousDay();

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate() => gregorianDate.PreviousDay();

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate() => localDate.PlusDays(-1);

    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public DateOnly WithDateOnly() => dateOnly.AddDays(-1);

    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime() => dateTime.AddDays(-1);
}
