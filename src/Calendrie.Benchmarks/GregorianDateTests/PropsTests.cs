﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.GregorianDateTests;

using static Benchmarks.BenchmarkHelpers;

public class PropsTests : GregorianDateComparisons
{
    [Benchmark(Description = "DayNumber")]
    public void WithDayNumber()
    {
        var date = dayNumber;
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

    [Benchmark(Description = "CivilDate")]
    public void WithCivilDate()
    {
        var date = civilDate;

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
        var date = gregorianDate;

        var (y, m, d) = date;
        var dayOfWeek = date.DayOfWeek;
        int dayOfYear = date.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "CivilDate_Plain")]
    public void WithPlainCivilDate()
    {
        var date = plainCivilDate;

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

    [Benchmark(Description = "LocalDate_NodaTime")]
    public void WithLocalDate()
    {
        var date = localDate;

        var (y, m, d) = date;
        var dayOfWeek = date.DayOfWeek;
        int dayOfYear = date.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DateOnly_BCL", Baseline = true)]
    public void WithDateOnly()
    {
        var date = dateOnly;

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

    [Benchmark(Description = "DateTime_BCL")]
    public void WithDateTime()
    {
        var date = dateTime;

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
