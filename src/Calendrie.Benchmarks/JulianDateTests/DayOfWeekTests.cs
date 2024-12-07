// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianDateTests;

using NodaTime;

public class DayOfWeekTests : JulianDateComparisons
{
    [Benchmark(Description = "DayNumber")]
    public DayOfWeek WithDayNumber() => dayNumber.DayOfWeek;

    [Benchmark(Description = "JulianDate")]
    public DayOfWeek WithJulianDate() => julianDate.DayOfWeek;

    [Benchmark(Description = "JulianDate_Plain")]
    public DayOfWeek WithPlainJulianDate() => plainJulianDate.DayOfWeek;

    [Benchmark(Description = "LocalDate_NodaTime")]
    public IsoDayOfWeek WithLocalDate() => localDate.DayOfWeek;

    [Benchmark(Description = "DateOnly_BCL", Baseline = true)]
    public DayOfWeek WithDateOnly() => dateOnly.DayOfWeek;

    [Benchmark(Description = "DateTime_BCL")]
    public DayOfWeek WithDateTime() => dateTime.DayOfWeek;
}
