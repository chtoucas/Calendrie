// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Tests;

using Calendrie.Core.Schemas;

using static Benchmarks.BenchmarkHelpers;

public abstract class GregorianSchemaTests
{
    private protected GregorianSchema schema = new();
    private protected CivilSchema civilSchema = new();

    private protected int year, month, day, daysSinceEpoch;
    private protected long yearL, daysSinceEpochL;

    private protected GregorianSchemaTests(GJDateType type = GJDateType.FixedFast)
    {
        (year, month, day) = CreateGregorianParts(type);
        daysSinceEpoch = DayNumber.FromGregorianParts(year, month, day).DaysSinceZero;
        (yearL, daysSinceEpochL) = (year, daysSinceEpoch);
    }
}

public class GregorianSchema_CountDaysInMonth : GregorianSchemaTests
{
    [Benchmark(Baseline = true)]
    public int Formulae() => GregorianFormulae.CountDaysInMonth(year, month);

    [Benchmark]
    public int Formulae64() => GregorianFormulae.CountDaysInMonth(yearL, month);

    [Benchmark]
    public int Schema() => schema.CountDaysInMonth(year, month);
}

public class GregorianSchema_CountDaysSinceEpoch : GregorianSchemaTests
{
    [Benchmark]
    public int Formulae() => GregorianFormulae.CountDaysSinceEpoch(year, month, day);

    [Benchmark(Baseline = true)]
    public int Formulae_Civil() => CivilFormulae.CountDaysSinceEpoch(year, month, day);

    [Benchmark]
    public long Formulae64() => GregorianFormulae.CountDaysSinceEpoch(yearL, month, day);

    [Benchmark]
    public int Schema() => schema.CountDaysSinceEpoch(year, month, day);

    [Benchmark]
    public int Schema_Civil() => civilSchema.CountDaysSinceEpoch(year, month, day);
}

public class GregorianSchema_GetDateParts : GregorianSchemaTests
{
    [Benchmark]
    public int Formulae()
    {
        GregorianFormulae.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        return d;
    }

    [Benchmark(Baseline = true)]
    public int Formulae_Civil()
    {
        CivilFormulae.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        return d;
    }

    [Benchmark]
    public int Formulae64()
    {
        GregorianFormulae.GetDateParts(daysSinceEpoch, out long y, out int m, out int d);
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

    [Benchmark]
    public int Schema_Civil()
    {
        civilSchema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        Consume(in y);
        Consume(in m);
        return d;
    }
}

public class GregorianSchema_GetYear : GregorianSchemaTests
{
    [Benchmark]
    public int Formulae() => GregorianFormulae.GetYear(daysSinceEpoch);

    [Benchmark(Baseline = true)]
    public int Formulae_Civil() => CivilFormulae.GetYear(daysSinceEpoch);

    [Benchmark]
    public long Formulae64() => GregorianFormulae.GetYear(daysSinceEpochL);

    [Benchmark]
    public int Schema() => schema.GetYear(daysSinceEpoch);

    [Benchmark]
    public int Schema_Civil() => civilSchema.GetYear(daysSinceEpoch);
}

public class GregorianSchema_GetStartOfYear : GregorianSchemaTests
{
    [Benchmark]
    public int Formulae() => GregorianFormulae.GetStartOfYear(year);

    [Benchmark(Baseline = true)]
    public int Formulae_Civil() => CivilFormulae.GetStartOfYear(year);

    [Benchmark]
    public long Formulae64() => GregorianFormulae.GetStartOfYear(yearL);

    [Benchmark]
    public int Schema() => schema.GetStartOfYear(year);

    [Benchmark]
    public int Schema_Civil() => civilSchema.GetStartOfYear(year);
}
