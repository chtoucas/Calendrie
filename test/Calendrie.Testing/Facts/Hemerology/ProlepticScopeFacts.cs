// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Hemerology.Scopes;
using Calendrie.Testing.Data;

public static class ProlepticScopeFacts
{
    public static readonly TheoryData<int> InvalidYearData =
    [
        int.MinValue,
        ProlepticScope.MinYear - 1,
        ProlepticScope.MaxYear + 1,
        int.MaxValue,
    ];

    public static readonly TheoryData<int> ValidYearData =
    [
        ProlepticScope.MinYear,
        ProlepticScope.MinYear + 1,
        -1,
        0,
        1,
        ProlepticScope.MaxYear - 1,
        ProlepticScope.MaxYear
    ];
}

/// <summary>
/// Provides data-driven tests for <see cref="ProlepticScope"/>.
/// </summary>
internal abstract class ProlepticScopeFacts<TDataSet> :
    CalendarScopeFacts<MinMaxYearScope, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ProlepticScopeFacts(MinMaxYearScope scope) : base(scope)
    {
        Debug.Assert(scope != null);
        Debug.Assert(scope.MinYear == ProlepticScope.MinYear);
        Debug.Assert(scope.MaxYear == ProlepticScope.MaxYear);
    }

    public static TheoryData<int> InvalidYearData => ProlepticScopeFacts.InvalidYearData;
    public static TheoryData<int> ValidYearData => ProlepticScopeFacts.ValidYearData;

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
