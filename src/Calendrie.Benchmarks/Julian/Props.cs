﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Julian;

public class Props : GJComparisons
{
    private readonly GJSample _sample;

    public Props()
    {
        SampleKind = GJSampleKind.Fixed;
        _sample = new GJSample(Year, Month, Day);
    }

    [Benchmark(Description = "DayNumber")]
    public void WithDayNumber()
    {
        var date = _sample.DayNumber;
        var parts = date.GetJulianParts();
        var oparts = date.GetJulianOrdinalParts();

        var (y, m, d) = parts;
        var dayOfWeek = date.DayOfWeek;
        int dayOfYear = oparts.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "JulianDate")]
    public void WithJulianDate()
    {
        var date = _sample.JulianDate;

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
        var date = _sample.LocalDate;

        var (y, m, d) = date;
        var dayOfWeek = date.DayOfWeek;
        int dayOfYear = date.DayOfYear;

        Consume(in y);
        Consume(in m);
        Consume(in d);
        Consume(in dayOfWeek);
        Consume(in dayOfYear);
    }

    [Benchmark(Description = "DateOnly (BCL)", Baseline = true)]
    public void WithDateOnly()
    {
        var date = _sample.DateOnly;

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
        var date = _sample.DateTime;

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
