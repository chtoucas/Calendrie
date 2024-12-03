// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks.Comparisons;

using Calendrie.Core.Schemas;
using Calendrie.Hemerology;
using Calendrie.Specialized;

public class GregorianValidation
{
    // Range [1..9999], optimized
    private static readonly CivilScope s_CivilScope = new(new CivilSchema());

    // Range [_999_998..999_999], optimized
    private static readonly GregorianScope s_GregorianScope = new(new GregorianSchema());

    // Range [1..9999], only the validation of years is optimized
    private static readonly StandardScope s_StandardScope =
        new(new GregorianSchema(), DayZero.NewStyle);

    // Range [_999_998..999_999]
    private static readonly MinMaxYearScope s_MinMaxYearScope =
        MinMaxYearScope.CreateMaximal(new GregorianSchema(), DayZero.NewStyle);

    private int _year, _month, _day;

    [GlobalSetup]
    public void GlobalSetup() =>
        (_year, _month, _day) = BenchmarkHelpers.CreateGregorianParts();

    [Benchmark(Description = "CivilScope", Baseline = true)]
    public Yemoda WithCivilScope()
    {
        s_CivilScope.ValidateYearMonthDay(_year, _month, _day);
        return new Yemoda(_year, _month, _day);
    }

    [Benchmark(Description = "GregorianScope")]
    public Yemoda WithGregorianScope()
    {
        s_GregorianScope.ValidateYearMonthDay(_year, _month, _day);
        return new Yemoda(_year, _month, _day);
    }

    [Benchmark(Description = "StandardScope")]
    public Yemoda WithStandardScope()
    {
        s_StandardScope.ValidateYearMonthDay(_year, _month, _day);
        return new Yemoda(_year, _month, _day);
    }

    [Benchmark(Description = "MinMaxYearScope")]
    public Yemoda WithMinMaxYearScope()
    {
        s_MinMaxYearScope.ValidateYearMonthDay(_year, _month, _day);
        return new Yemoda(_year, _month, _day);
    }
}
