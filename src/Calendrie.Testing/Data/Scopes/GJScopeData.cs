﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Scopes;

using Calendrie.Systems;

public sealed class GJScopeData : IMinMaxYearScopeData
{
    // NB: GregorianScope and JulianScope use the same range of years.
    public static int MinYear => GregorianScope.MinYear;
    public static int MaxYear => GregorianScope.MaxYear;

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
