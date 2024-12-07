// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianSchemaTests;

using Calendrie.Core.Schemas;

public class CountDaysSinceEpochTests : JulianSchemaComparisons
{
    [Benchmark(Baseline = true)]
    public int Formulae() => JulianFormulae.CountDaysSinceEpoch(year, month, day);

    [Benchmark]
    public long Formulae64() => JulianFormulae.CountDaysSinceEpoch(yearL, month, day);

    [Benchmark]
    public int Schema() => schema.CountDaysSinceEpoch(year, month, day);
}
