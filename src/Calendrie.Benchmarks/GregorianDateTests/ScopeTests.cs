// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.GregorianDateTests;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;
using Calendrie.Specialized;

[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ScopeTests
{
    // Range [1..9999], optimized
    private static readonly CivilScope s_CivilScope = new(new CivilSchema());

    // Range [_999_998..999_999], optimized
    private static readonly GregorianScope s_GregorianScope = new(new GregorianSchema());

    // Range [1..9999], only the validation of years is optimized
    private static readonly StandardScope s_StandardScope =
        new(DayZero.NewStyle, new GregorianSchema());

    // Range [1..9999], plain
    private static readonly MinMaxYearScope s_MinMaxYearScope =
        MinMaxYearScope.Create(new GregorianSchema(), DayZero.NewStyle, Range.Create(1, 9999));

    private int _yearFast, _monthFast, _dayFast;
    private int _yearSlow, _monthSlow, _daySlow;

    [GlobalSetup]
    public void GlobalSetup()
    {
        (_yearFast, _monthFast, _dayFast) = BenchmarkHelpers.CreateGregorianParts(GJDateType.FixedFast);
        (_yearSlow, _monthSlow, _daySlow) = BenchmarkHelpers.CreateGregorianParts(GJDateType.FixedSlow);
    }

    //
    // Fast track
    //

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "CivilScope", Baseline = true)]
    public Yemoda WithCivilScopeFast()
    {
        s_CivilScope.ValidateYearMonthDay(_yearFast, _monthFast, _dayFast);
        return new Yemoda(_yearFast, _monthFast, _dayFast);
    }

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "GregorianScope")]
    public Yemoda WithGregorianScopeFast()
    {
        s_GregorianScope.ValidateYearMonthDay(_yearFast, _monthFast, _dayFast);
        return new Yemoda(_yearFast, _monthFast, _dayFast);
    }

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "StandardScope")]
    public Yemoda WithStandardScopeFast()
    {
        s_StandardScope.ValidateYearMonthDay(_yearFast, _monthFast, _dayFast);
        return new Yemoda(_yearFast, _monthFast, _dayFast);
    }

    [BenchmarkCategory("Fast")]
    [Benchmark(Description = "MinMaxYearScope")]
    public Yemoda WithMinMaxYearScopeFast()
    {
        s_MinMaxYearScope.ValidateYearMonthDay(_yearFast, _monthFast, _dayFast);
        return new Yemoda(_yearFast, _monthFast, _dayFast);
    }

    //
    // Slow track
    //

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "CivilScope", Baseline = true)]
    public Yemoda WithCivilScopeSlow()
    {
        s_CivilScope.ValidateYearMonthDay(_yearSlow, _monthSlow, _daySlow);
        return new Yemoda(_yearSlow, _monthSlow, _daySlow);
    }

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "GregorianScope")]
    public Yemoda WithGregorianScopeSlow()
    {
        s_GregorianScope.ValidateYearMonthDay(_yearSlow, _monthSlow, _daySlow);
        return new Yemoda(_yearSlow, _monthSlow, _daySlow);
    }

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "StandardScope")]
    public Yemoda WithStandardScopeSlow()
    {
        s_StandardScope.ValidateYearMonthDay(_yearSlow, _monthSlow, _daySlow);
        return new Yemoda(_yearSlow, _monthSlow, _daySlow);
    }

    [BenchmarkCategory("Slow")]
    [Benchmark(Description = "MinMaxYearScope")]
    public Yemoda WithMinMaxYearScopeSlow()
    {
        s_MinMaxYearScope.ValidateYearMonthDay(_yearSlow, _monthSlow, _daySlow);
        return new Yemoda(_yearSlow, _monthSlow, _daySlow);
    }
}
