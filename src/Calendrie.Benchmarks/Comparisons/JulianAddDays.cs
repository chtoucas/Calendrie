// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using Calendrie.Specialized;

using NodaTime;

public class JulianAddDays : JulianComparisons
{
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber31() => dayNumber.AddDays(31);

    [Benchmark(Description = "JulianDate")]
    public JulianDate WithGregorianDate31() => julianDate.AddDays(31);

    [Benchmark(Description = "LocalDate_NodaTime")]
    public LocalDate WithLocalDate31() => localDate.PlusDays(31);

    [Benchmark(Description = "DateOnly_BCL", Baseline = true)]
    public DateOnly WithDateOnly31() => dateOnly.AddDays(31);

    [Benchmark(Description = "DateTime_BCL")]
    public DateTime WithDateTime31() => dateTime.AddDays(31);
}
