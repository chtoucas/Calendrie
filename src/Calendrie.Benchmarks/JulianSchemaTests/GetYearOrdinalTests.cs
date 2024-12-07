// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianSchemaTests;

using Calendrie.Core.Schemas;

using static Benchmarks.BenchmarkHelpers;

public class GetYearOrdinalTests : JulianSchemaComparisons
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
