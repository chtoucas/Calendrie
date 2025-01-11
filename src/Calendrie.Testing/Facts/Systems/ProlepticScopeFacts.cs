// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Hemerology;
using Calendrie.Testing.Data;
using Calendrie.Testing.Facts.Hemerology;

public static class ProlepticScopeFacts
{
    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to -999_998.</para>
    /// </summary>
    internal const int MinYear = -999_998;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para>
    /// </summary>
    internal const int MaxYear = 999_999;

    ///// <summary>
    ///// Represents the range of supported years.
    ///// </summary>
    //public static readonly Range<int> SupportedYears = Range.Create(MinYear, MaxYear);

    public static readonly TheoryData<int> InvalidYearData =
    [
        int.MinValue,
        MinYear - 1,
        MaxYear + 1,
        int.MaxValue,
    ];

    public static readonly TheoryData<int> ValidYearData =
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
/// Provides data-driven tests for proleptic scopes.
/// </summary>
internal abstract class ProlepticScopeFacts<TDataSet> :
    CalendarScopeFacts<CalendarScope, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ProlepticScopeFacts(CalendarScope scope) : base(scope)
    {
        Debug.Assert(scope != null);

        var (minYear, maxYear) = scope.Segment.SupportedYears.Endpoints;

        Debug.Assert(minYear == ProlepticScopeFacts.MinYear);
        Debug.Assert(maxYear == ProlepticScopeFacts.MaxYear);
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
