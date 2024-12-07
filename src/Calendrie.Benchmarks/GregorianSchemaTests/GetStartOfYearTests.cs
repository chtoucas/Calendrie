// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.GregorianSchemaTests;

using Calendrie.Core.Schemas;

public class GetStartOfYearTests : GregorianSchemaComparisons
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
