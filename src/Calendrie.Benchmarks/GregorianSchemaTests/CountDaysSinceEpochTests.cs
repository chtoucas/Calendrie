// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.GregorianSchemaTests;

using Calendrie.Core.Schemas;

public class CountDaysSinceEpochTests : GregorianSchemaComparisons
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
