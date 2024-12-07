// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.GregorianSchemaTests;

using Calendrie.Core.Schemas;

public class GetYearTests : GregorianSchemaComparisons
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
