// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Systems;
using Calendrie.Testing.Data;
using Calendrie.Testing.Facts.Hemerology;

// REVIEW(fact): how to enforce the use of a StandardCalendarDataSet?

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

/// <summary>
/// Provides data-driven tests for <see cref="StandardScope"/>.
/// </summary>
internal abstract class StandardScopeFacts<TDataSet> :
    CalendarScopeFacts<StandardScope, TDataSet, StandardScopeData>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected StandardScopeFacts(StandardScope scope) : base(scope) { }
}

/// <summary>
/// Provides data-driven tests for <see cref="CivilScope"/>.
/// </summary>
internal abstract class CivilScopeFacts<TDataSet> :
    CalendarScopeFacts<CivilScope, TDataSet, StandardScopeData>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected CivilScopeFacts(CivilScope scope) : base(scope) { }
}
