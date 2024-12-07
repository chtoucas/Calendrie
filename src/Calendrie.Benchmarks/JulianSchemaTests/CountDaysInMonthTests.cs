// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.JulianSchemaTests;

using Calendrie.Core.Schemas;

public class CountDaysInMonthTests : JulianSchemaComparisons
{
    [Benchmark(Baseline = true)]
    public int Formulae() => JulianFormulae.CountDaysInMonth(year, month);

    [Benchmark]
    public int Formulae64() => JulianFormulae.CountDaysInMonth(yearL, month);

    [Benchmark]
    public int Schema() => schema.CountDaysInMonth(year, month);
}
