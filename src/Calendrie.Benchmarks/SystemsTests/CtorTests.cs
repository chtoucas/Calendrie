// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.SystemsTests;

using Calendrie.Systems;

public class CtorTests
{
    private const int
        Year = 2017,
        Month = 11,
        Day = 19;

    [Benchmark(Description = "ArmenianDate")]
    public ArmenianDate WithArmenianDate() => new(Year, Month, Day);

    [Benchmark(Description = "Armenian13Date")]
    public Armenian13Date WithArmenian13Date() => new(Year, Month, Day);

    [Benchmark(Description = "CivilDate")]
    public CivilDate WithCivilDate() => new(Year, Month, Day);

    [Benchmark(Description = "CopticDate")]
    public CopticDate WithCopticDate() => new(Year, Month, Day);

    [Benchmark(Description = "Coptic13Date")]
    public Coptic13Date WithCoptic13Date() => new(Year, Month, Day);

    [Benchmark(Description = "EthiopicDate")]
    public EthiopicDate WithEthiopicDate() => new(Year, Month, Day);

    [Benchmark(Description = "Ethiopic13Date")]
    public Ethiopic13Date WithEthiopic13Date() => new(Year, Month, Day);

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate() => new(Year, Month, Day);

    [Benchmark(Description = "JulianDate")]
    public JulianDate WithJulianDate() => new(Year, Month, Day);

    [Benchmark(Description = "TabularIslamicDate")]
    public TabularIslamicDate WithTabularIslamicDate() => new(Year, Month, Day);

    [Benchmark(Description = "WorldDate")]
    public WorldDate WithWorldDate() => new(Year, Month, Day);

    [Benchmark(Description = "ZoroastrianDate")]
    public ZoroastrianDate WithZoroastrianDate() => new(Year, Month, Day);

    [Benchmark(Description = "Zoroastrian13Date")]
    public Zoroastrian13Date WithZoroastrian13Date() => new(Year, Month, Day);
}
