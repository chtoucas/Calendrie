// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianSchemaTests;

using Calendrie.Core.Schemas;

public class GetStartOfYearTests : JulianSchemaComparisons
{
    [Benchmark(Baseline = true)]
    public int Formulae() => JulianFormulae.GetStartOfYear(year);

    [Benchmark]
    public long Formulae64() => JulianFormulae2.GetStartOfYear(yearL);

    [Benchmark]
    public int Schema() => schema.GetStartOfYear(year);
}
