// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianDateTests;

public class DayOfYearTests : JulianDateComparisons
{
    [Benchmark(Description = "DayNumber")]
    public int WithDayNumber() => dayNumber.GetJulianOrdinalParts().DayOfYear;

    [Benchmark(Description = "JulianDate")]
    public int WithJulianDate() => julianDate.DayOfYear;

    [Benchmark(Description = "JulianDate_Plain")]
    public int WithJulianDate_PlainJulianDate() => plainJulianDate.DayOfYear;

    [Benchmark(Description = "LocalDate_NodaTime")]
    public int WithLocalDate() => localDate.DayOfYear;

    [Benchmark(Description = "DateOnly_BCL", Baseline = true)]
    public int WithDateOnly() => dateOnly.DayOfYear;

    [Benchmark(Description = "DateTime_BCL")]
    public int WithDateTime() => dateTime.DayOfYear;
}
