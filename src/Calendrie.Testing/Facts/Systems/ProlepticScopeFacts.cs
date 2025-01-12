// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Hemerology;
using Calendrie.Systems;
using Calendrie.Testing.Data;
using Calendrie.Testing.Facts.Hemerology;

public sealed class ProlepticScopeData : IMinMaxYearScopeData
{
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

/// <summary>
/// Provides data-driven tests for <see cref="GregorianScope"/> and
/// <see cref="JulianScope"/>.
/// </summary>
public abstract class ProlepticScopeFacts<TDataSet> :
    CalendarScopeFacts<CalendarScope, TDataSet, ProlepticScopeData>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ProlepticScopeFacts(CalendarScope scope) : base(scope) { }
}
