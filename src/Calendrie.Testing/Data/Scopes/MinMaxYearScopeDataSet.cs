// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Scopes;

public sealed class MinMaxYearScopeDataSet : IScopeDataSet
{
    public static int MinYear => -5;
    public static int MaxYear => 1234;

    public static TheoryData<int> InvalidYearData =>
    [
        int.MinValue,
        MinYear - 1,
        MaxYear + 1,
        int.MaxValue,
    ];

    public static TheoryData<int> ValidYearData =>
    [
        MinYear,
        MinYear + 1,
        -1,
        0,
        1,
        MaxYear - 1,
        MaxYear
    ];
}
