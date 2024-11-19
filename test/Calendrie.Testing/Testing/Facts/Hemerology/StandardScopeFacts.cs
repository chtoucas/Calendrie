﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Hemerology.Scopes;
using Calendrie.Testing.Data;

// FIXME(fact): skip negative years.
// Use an ICalendarDataSet? StandardCalendarDataSet?
// What about ProlepticScopeFacts.

public static class StandardScopeFacts
{
    public static readonly TheoryData<int> InvalidYearData =
    [
        Int32.MinValue,
        StandardScope.MinYear - 1,
        StandardScope.MaxYear + 1,
        Int32.MaxValue,
    ];

    public static readonly TheoryData<int> ValidYearData =
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
    CalendarScopeFacts<MinMaxYearScope, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected StandardScopeFacts(MinMaxYearScope scope) : base(scope)
    {
        Debug.Assert(scope != null);
        Debug.Assert(scope.MinYear == StandardScope.MinYear);
        Debug.Assert(scope.MaxYear == StandardScope.MaxYear);
    }

    public static TheoryData<int> InvalidYearData => StandardScopeFacts.InvalidYearData;
    public static TheoryData<int> ValidYearData => StandardScopeFacts.ValidYearData;

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYearMonth_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateYearMonth(y, 1));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateYearMonth(y, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYearMonthDay_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateYearMonthDay(y, 1, 1));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateYearMonthDay(y, 1, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateOrdinal_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateOrdinal(y, 1));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateOrdinal(y, 1, nameof(y)));
    }
}