// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Gregorian;

using Calendrie;
using Calendrie.Specialized;

using NodaTime;

// Benchmarks for the ctor.

public class Ctor : GJSampleData
{
    public Ctor() { Option = BenchmarkOption.Fixed; }

    [Benchmark(Description = "DayNumber")]
    public DayNumber WithDayNumber() => DayNumber.FromGregorianParts(Year, Month, Day);

    [Benchmark(Description = "CivilDate", Baseline = true)]
    public CivilDate WithCivilDate() => new(Year, Month, Day);

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate() => new(Year, Month, Day);

    [Benchmark(Description = "LocalDate (NodaTime)")]
    public LocalDate WithLocalDate() => new(Year, Month, Day);

    [Benchmark(Description = "DateOnly (BCL)")]
    public DateOnly WithDateOnly() => new(Year, Month, Day);

    [Benchmark(Description = "DateTime (BCL)")]
    public DateTime WithDateTime() => new(Year, Month, Day);
}
