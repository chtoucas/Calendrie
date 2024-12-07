// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.GregorianSchemaTests;

using Calendrie.Core.Schemas;

using static Benchmarks.BenchmarkHelpers;

public class GetYearOrdinalTests : GregorianSchemaComparisons
{
    [Benchmark]
    public int Formulae()
    {
        int y = GregorianFormulae.GetYear(daysSinceEpoch, out int doy);
        Consume(in y);
        return doy;
    }

    [Benchmark(Baseline = true)]
    public int Formulae_Civil()
    {
        int y = CivilFormulae.GetYear(daysSinceEpoch, out int doy);
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

    [Benchmark]
    public int Schema_Civil()
    {
        int y = civilSchema.GetYear(daysSinceEpoch, out int doy);
        Consume(in y);
        return doy;
    }
}
