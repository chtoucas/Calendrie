// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianSchemaTests;

using Calendrie.Core.Schemas;

using static Benchmarks.BenchmarkHelpers;

public class GetDatePartsTests : JulianSchemaComparisons
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
