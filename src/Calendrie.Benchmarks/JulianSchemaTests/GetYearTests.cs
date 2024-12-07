// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianSchemaTests;

using Calendrie.Core.Schemas;

public class GetYearTests : JulianSchemaComparisons
{
    [Benchmark(Baseline = true)]
    public int Formulae() => JulianFormulae.GetYear(daysSinceEpoch);

    [Benchmark]
    public long Formulae64() => JulianFormulae.GetYear(daysSinceEpochL);

    [Benchmark]
    public int Schema() => schema.GetYear(daysSinceEpoch);
}
