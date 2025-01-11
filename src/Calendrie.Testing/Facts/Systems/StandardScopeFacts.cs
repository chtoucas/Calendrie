// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Systems;
using Calendrie.Testing.Data;
using Calendrie.Testing.Facts.Hemerology;

// FIXME(fact): skip negative years.
// Use an ICalendarDataSet? StandardCalendarDataSet?
// What about ProlepticScopeFacts.

public static class StandardScopeFacts
{
    public static readonly TheoryData<int> InvalidYearData =
    [
        int.MinValue,
        StandardScope.MinYear - 1,
        StandardScope.MaxYear + 1,
        int.MaxValue,
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
    CalendarScopeFacts<StandardScope, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected StandardScopeFacts(StandardScope scope) : base(scope)
    {
        Debug.Assert(scope != null);
        Debug.Assert(scope.Segment.SupportedYears.Min == StandardScope.MinYear);
        Debug.Assert(scope.Segment.SupportedYears.Max == StandardScope.MaxYear);
    }

    public static TheoryData<int> InvalidYearData => StandardScopeFacts.InvalidYearData;
    public static TheoryData<int> ValidYearData => StandardScopeFacts.ValidYearData;

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void CheckYear_InvalidYear(int y) => Assert.False(ScopeUT.CheckYear(y));

    [Theory, MemberData(nameof(ValidYearData))]
    public sealed override void CheckYear(int y) => Assert.True(ScopeUT.CheckYear(y));

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYear_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateYear(y));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateYear(y, nameof(y)));
    }

    [Theory, MemberData(nameof(ValidYearData))]
    public sealed override void ValidateYear(int y) => ScopeUT.ValidateYear(y);

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void CheckYearMonth_InvalidYear(int y) =>
        Assert.False(ScopeUT.CheckYearMonth(y, 1));

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYearMonth_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateYearMonth(y, 1));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateYearMonth(y, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void CheckYearMonthDay_InvalidYear(int y) =>
        Assert.False(ScopeUT.CheckYearMonthDay(y, 1, 1));

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateYearMonthDay_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateYearMonthDay(y, 1, 1));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateYearMonthDay(y, 1, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void CheckOrdinal_InvalidYear(int y) =>
        Assert.False(ScopeUT.CheckOrdinal(y, 1));

    [Theory, MemberData(nameof(InvalidYearData))]
    public sealed override void ValidateOrdinal_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateOrdinal(y, 1));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateOrdinal(y, 1, nameof(y)));
    }
}
