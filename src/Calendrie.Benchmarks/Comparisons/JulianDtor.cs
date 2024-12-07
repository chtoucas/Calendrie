﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using static Benchmarks.BenchmarkHelpers;

public class JulianDtor : JulianComparisons
{
    [Benchmark(Description = "DayNumber")]
    public void WithDayNumber()
    {
        var (y, m, d) = dayNumber.GetJulianParts();

        Consume(in y);
        Consume(in m);
        Consume(in d);
    }

    [Benchmark(Description = "JulianDate")]
    public void WithJulianDate()
    {
        var (y, m, d) = julianDate;

        Consume(in y);
        Consume(in m);
        Consume(in d);
    }

    [Benchmark(Description = "JulianDate_Plain")]
    public void WithJulianDate_PlainJulianDate()
    {
        var (y, m, d) = plainJulianDate;

        Consume(in y);
        Consume(in m);
        Consume(in d);
    }

    //
    // External date types
    //

    [Benchmark(Description = "LocalDate_NodaTime")]
    public void WithLocalDate()
    {
        var (y, m, d) = localDate;

        Consume(in y);
        Consume(in m);
        Consume(in d);
    }

    [Benchmark(Description = "DateOnly_BCL", Baseline = true)]
    public void WithDateOnly()
    {
        var (y, m, d) = dateOnly;

        Consume(in y);
        Consume(in m);
        Consume(in d);
    }

    [Benchmark(Description = "DateTime_BCL")]
    public void WithDateTime()
    {
        var (y, m, d) = dateTime;

        Consume(in y);
        Consume(in m);
        Consume(in d);
    }
}
