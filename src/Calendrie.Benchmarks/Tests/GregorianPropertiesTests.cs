// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Benchmarks.Tests;

using NodaTime;

using Calendrie;
using Calendrie.Specialized;

// Benchmarks for the core properties.

public class GregorianPropertiesTests : GJTestData
{
    public GregorianPropertiesTests() { Option = BenchmarkOption.Fixed; }

    [Benchmark(Description = "DayNumber")]
    public void WithDayNumber()
    {
        var date = DayNumber.FromGregorianParts(Year, Month, Day);
        var parts = date.GetGregorianParts();
        var oparts = date.GetGregorianOrdinalParts();

        var (y, m, d) = parts;
        var dayOfWeek = date.DayOfWeek;
        int dayOfYear = oparts.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "CivilDate", Baseline = true)]
    public void WithCivilDate()
    {
        CivilDate date = new(Year, Month, Day);

        var (y, m, d) = date;
        var dayOfWeek = date.DayOfWeek;
        int dayOfYear = date.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "GregorianDate")]
    public void WithGregorianDate()
    {
        GregorianDate date = new(Year, Month, Day);

        var (y, m, d) = date;
        var dayOfWeek = date.DayOfWeek;
        int dayOfYear = date.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    //
    // External date types
    //

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public void WithLocalDate()
    {
        LocalDate date = new(Year, Month, Day);

        var (y, m, d) = date;
        var dayOfWeek = date.DayOfWeek;
        int dayOfYear = date.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DateOnly (BCL)")]
    public void WithDateOnly()
    {
        DateOnly date = new(Year, Month, Day);

        int y = date.Year;
        int m = date.Month;
        int d = date.Day;
        var dayOfWeek = date.DayOfWeek;
        int dayOfYear = date.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DateTime (BCL)")]
    public void WithDateTime()
    {
        DateTime date = new(Year, Month, Day);

        int y = date.Year;
        int m = date.Month;
        int d = date.Day;
        var dayOfWeek = date.DayOfWeek;
        int dayOfYear = date.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }
}
