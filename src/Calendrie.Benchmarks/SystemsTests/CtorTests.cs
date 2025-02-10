// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.SystemsTests;

using Calendrie.Systems;

public class CtorTests
{
    private int _year, _month, _day;

    [GlobalSetup]
    public void GlobalSetup() =>
        (_year, _month, _day) = BenchmarkHelpers.CreateJulianParts();

    [Benchmark(Description = "Armenian12Date")]
    public Armenian12Date WithArmenian12Date() => new(_year, _month, _day);

    [Benchmark(Description = "ArmenianDate")]
    public ArmenianDate WithArmenianDate() => new(_year, _month, _day);

    [Benchmark(Description = "CivilDate", Baseline = true)]
    public CivilDate WithCivilDate() => new(_year, _month, _day);

    [Benchmark(Description = "Coptic12Date")]
    public Coptic12Date WithCoptic12Date() => new(_year, _month, _day);

    [Benchmark(Description = "CopticDate")]
    public CopticDate WithCopticDate() => new(_year, _month, _day);

    [Benchmark(Description = "Ethiopic12Date")]
    public Ethiopic12Date WithEthiopic12Date() => new(_year, _month, _day);

    [Benchmark(Description = "EthiopicDate")]
    public EthiopicDate WithEthiopicDate() => new(_year, _month, _day);

    [Benchmark(Description = "GregorianDate")]
    public GregorianDate WithGregorianDate() => new(_year, _month, _day);

    [Benchmark(Description = "JulianDate")]
    public JulianDate WithJulianDate() => new(_year, _month, _day);

    [Benchmark(Description = "TabularIslamicDate")]
    public TabularIslamicDate WithTabularIslamicDate() => new(_year, _month, _day);

    [Benchmark(Description = "TropicaliaDate")]
    public TropicaliaDate WithTropicaliaDate() => new(_year, _month, _day);

    [Benchmark(Description = "WorldDate")]
    public WorldDate WithWorldDate() => new(_year, _month, _day);

    [Benchmark(Description = "Zoroastrian12Date")]
    public Zoroastrian12Date WithZoroastrian12Date() => new(_year, _month, _day);

    [Benchmark(Description = "ZoroastrianDate")]
    public ZoroastrianDate WithZoroastrianDate() => new(_year, _month, _day);
}
