// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.GregorianSchemaTests;

using Calendrie.Core.Schemas;

using static Benchmarks.BenchmarkHelpers;

public class GetDatePartsTests : GregorianSchemaComparisons
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
        GregorianFormulae.GetDateParts(daysSinceEpochL, out long y, out int m, out int d);
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
