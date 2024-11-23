// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Benchmarks.Tests;

using NodaTime;

using Calendrie;
using Calendrie.Specialized;

// Benchmarks for yesterday and tomorrow.

public class GregorianNextPreviousTests : GJTestData
{
    public GregorianNextPreviousTests() { Option = BenchmarkOption.Fixed; }

    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber()
    {
        var start = DayNumber.FromGregorianParts(Year, Month, Day);
        return start.NextDay().PreviousDay();
    }

    [Benchmark(Description = "CivilDate", Baseline = true)]
    public CivilDate WithCivilDate()
    {
        CivilDate start = new(Year, Month, Day);
        return start.NextDay().PreviousDay();
    }

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate()
    {
        GregorianDate start = new(Year, Month, Day);
        return start.NextDay().PreviousDay();
    }

    //
    // External date types
    //

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate()
    {
        LocalDate start = new(Year, Month, Day);
        return start.PlusDays(1).PlusDays(-1);
    }

    [Benchmark(Description = "DateOnly (BCL)")]
    public DateOnly WithDateOnly()
    {
        DateOnly start = new(Year, Month, Day);
        return start.AddDays(1).AddDays(-1);
    }

    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime()
    {
        DateTime start = new(Year, Month, Day);
        return start.AddDays(1).AddDays(-1);
    }
}
