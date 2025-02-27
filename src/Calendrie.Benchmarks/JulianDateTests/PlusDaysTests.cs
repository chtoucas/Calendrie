﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianDateTests;

using Calendrie;
using Calendrie.Systems;

using NodaTime;

public class PlusDaysTests : JulianDateComparisons
{
    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber31() => dayNumber.PlusDays(31);

    [Benchmark(Description = "JulianDate")]
    public JulianDate WithJulianDate31() => julianDate.PlusDays(31);

    [Benchmark(Description = "JulianDate_Plain")]
    public MyJulianDate WithPlainJulianDate31() => plainJulianDate.PlusDays(31);

    [Benchmark(Description = "LocalDate_NodaTime")]
    public LocalDate WithLocalDate31() => localDate.PlusDays(31);

    [Benchmark(Description = "DateOnly_BCL", Baseline = true)]
    public DateOnly WithDateOnly31() => dateOnly.AddDays(31);

    [Benchmark(Description = "DateTime_BCL")]
    public DateTime WithDateTime31() => dateTime.AddDays(31);
}
