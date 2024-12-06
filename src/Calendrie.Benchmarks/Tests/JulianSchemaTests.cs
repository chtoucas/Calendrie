// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Tests;

using Calendrie.Core.Schemas;

using static Benchmarks.BenchmarkHelpers;

public abstract class JulianSchemaTests
{
    private protected JulianSchema schema = new();

    private protected int year, month, day, daysSinceEpoch;
    private protected long yearL, daysSinceEpochL;

    private protected JulianSchemaTests(GJDateType type = GJDateType.FixedFast)
    {
        (year, month, day) = CreateJulianParts(type);
        daysSinceEpoch = DayNumber.FromJulianParts(year, month, day) - DayZero.OldStyle;
        (yearL, daysSinceEpochL) = (year, daysSinceEpoch);
    }
}

public class JulianSchema_CountDaysInMonth : JulianSchemaTests
{
    [Benchmark(Baseline = true)]
    public int Formulae() => JulianFormulae.CountDaysInMonth(year, month);

    [Benchmark]
    public int Formulae64() => JulianFormulae.CountDaysInMonth(yearL, month);

    [Benchmark]
    public int Schema() => schema.CountDaysInMonth(year, month);
}

public class JulianSchema_CountDaysSinceEpoch : JulianSchemaTests
{
    [Benchmark(Baseline = true)]
    public int Formulae() => JulianFormulae.CountDaysSinceEpoch(year, month, day);

    [Benchmark]
    public long Formulae64() => JulianFormulae.CountDaysSinceEpoch(yearL, month, day);

    [Benchmark]
    public int Schema() => schema.CountDaysSinceEpoch(year, month, day);
}

public class JulianSchema_GetDateParts : JulianSchemaTests
{
    [Benchmark(Baseline = true)]
    public int Formulae()
    {
        JulianFormulae.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        return d;
    }

    [Benchmark]
    public int Formulae64()
    {
        JulianFormulae.GetDateParts(daysSinceEpochL, out long y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        return d;
    }

    [Benchmark]
    public int Schema()
    {
        schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        return d;
    }
}

public class JulianSchema_GetYearOrdinal : JulianSchemaTests
{
    [Benchmark(Baseline = true)]
    public int Formulae()
    {
        int y = JulianFormulae.GetYear(daysSinceEpoch, out int doy);
        Consume(in y);
        return doy;
    }

    [Benchmark]
    public int Schema()
    {
        int y = schema.GetYear(daysSinceEpoch, out int doy);
        Consume(in y);
        return doy;
    }
}

public class JulianSchema_GetYear : JulianSchemaTests
{
    [Benchmark(Baseline = true)]
    public int Formulae() => JulianFormulae.GetYear(daysSinceEpoch);

    [Benchmark]
    public long Formulae64() => JulianFormulae.GetYear(daysSinceEpochL);

    [Benchmark]
    public int Schema() => schema.GetYear(daysSinceEpoch);
}

public class JulianSchema_GetStartOfYear : JulianSchemaTests
{
    [Benchmark(Baseline = true)]
    public int Formulae() => JulianFormulae.GetStartOfYear(year);

    [Benchmark]
    public long Formulae64() => JulianFormulae.GetStartOfYear(yearL);

    [Benchmark]
    public int Schema() => schema.GetStartOfYear(year);
}
