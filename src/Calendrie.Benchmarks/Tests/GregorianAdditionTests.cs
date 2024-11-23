// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Benchmarks.Tests;

using NodaTime;

using Calendrie;
using Calendrie.Specialized;

// Benchmarks for the addition.

public class GregorianAdditionTests : GJTestData
{
    private const int D7 = 7;        // No change of month.
    private const int D30 = 30;      // Change of month.
    private const int D401 = 401;    // "Slow-track".

    public GregorianAdditionTests() { Option = BenchmarkOption.Fixed; }

    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber()
    {
        var start = DayNumber.FromGregorianParts(Year, Month, Day);
        return start.PlusDays(D7).PlusDays(D30).PlusDays(D401);
    }

    [Benchmark(Description = "CivilDate", Baseline = true)]
    public CivilDate WithCivilDate()
    {
        CivilDate start = new(Year, Month, Day);
        return start.PlusDays(D7).PlusDays(D30).PlusDays(D401);
    }

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate()
    {
        GregorianDate start = new(Year, Month, Day);
        return start.PlusDays(D7).PlusDays(D30).PlusDays(D401);
    }

    //
    // External date types
    //

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate()
    {
        LocalDate start = new(Year, Month, Day);
        return start.PlusDays(D7).PlusDays(D30).PlusDays(D401);
    }

    [Benchmark(Description = "DateOnly (BCL)")]
    public DateOnly WithDateOnly()
    {
        DateOnly start = new(Year, Month, Day);
        return start.AddDays(D7).AddDays(D30).AddDays(D401);
    }

    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime()
    {
        DateTime start = new(Year, Month, Day);
        return start.AddDays(D7).AddDays(D30).AddDays(D401);
    }
}
