﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Data.Scopes;

using Calendrie.Systems;

public sealed class StandardScopeData : IMinMaxYearScopeData
{
    public static int MinYear => StandardScope.MinYear;
    public static int MaxYear => StandardScope.MaxYear;

    public static TheoryData<int> InvalidYearData =>
    [
        int.MinValue,
        StandardScope.MinYear - 1,
        StandardScope.MaxYear + 1,
        int.MaxValue,
    ];

    public static TheoryData<int> ValidYearData =>
    [
        StandardScope.MinYear,
        StandardScope.MinYear + 1,
        StandardScope.MaxYear - 1,
        StandardScope.MaxYear
    ];
}
