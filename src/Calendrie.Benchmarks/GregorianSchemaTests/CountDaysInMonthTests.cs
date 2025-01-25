// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.GregorianSchemaTests;

using Calendrie.Core.Schemas;

public class CountDaysInMonthTests : GregorianSchemaComparisons
{
    [Benchmark(Baseline = true)]
    public int Formulae() => GregorianFormulae.CountDaysInMonth(year, month);

    [Benchmark]
    public int Schema() => schema.CountDaysInMonth(year, month);
}
