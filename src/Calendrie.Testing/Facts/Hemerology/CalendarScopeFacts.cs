// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Hemerology;
using Calendrie.Testing.Data;
using Calendrie.Testing.Data.Scopes;

// Datasets are tailored to work with a range of years, not a range of
// days, therefore one has to be careful when choosing the dataset, it must be
// tailored to work with the scope under test. For instance, this is what we do
// with the Standard...DataSet.
// Another way to do it would be to add a filter at the start of a test,
// something like SkipMonth() or SkipDate() but it feels wrong. For instance,
// the number of actual tests would be hard to get.

/// <summary>
/// Provides data-driven tests for the <see cref="CalendarScope"/> type.
/// </summary>
public class CalendarScopeFacts<TScope, TDataSet, TScopeDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TScope : CalendarScope
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
    where TScopeDataSet : IScopeDataSet
{
    public CalendarScopeFacts(TScope scope)
    {
        ArgumentNullException.ThrowIfNull(scope);

        ScopeUT = scope;

#if DEBUG
        var (minYear, maxYear) = scope.Segment.SupportedYears.Endpoints;
        Debug.Assert(minYear == TScopeDataSet.MinYear);
        Debug.Assert(maxYear == TScopeDataSet.MaxYear);
#endif
    }

    public static TheoryData<int> InvalidYearData => TScopeDataSet.InvalidYearData;
    public static TheoryData<int> ValidYearData => TScopeDataSet.ValidYearData;

    /// <summary>
    /// Gets the scope under test.
    /// </summary>
    protected TScope ScopeUT { get; }

    #region CheckYear()

    [Theory, MemberData(nameof(InvalidYearData))]
    public void CheckYear_InvalidYear(int y) => Assert.False(ScopeUT.CheckYear(y));

    [Theory, MemberData(nameof(ValidYearData))]
    public void CheckYear(int y) => Assert.True(ScopeUT.CheckYear(y));

    #endregion
    #region ValidateYear()

    [Theory, MemberData(nameof(InvalidYearData))]
    public void ValidateYear_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateYear(y));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateYear(y, nameof(y)));
    }

    [Theory, MemberData(nameof(ValidYearData))]
    public void ValidateYear(int y) => ScopeUT.ValidateYear(y);

    #endregion

    #region CheckYearMonth()

    [Theory, MemberData(nameof(InvalidYearData))]
    public void CheckYearMonth_InvalidYear(int y) =>
        Assert.False(ScopeUT.CheckYearMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void CheckYearMonth_InvalidMonth(int y, int m) =>
        Assert.False(ScopeUT.CheckYearMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CheckYearMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        Assert.True(ScopeUT.CheckYearMonth(y, m));
    }

    #endregion
    #region ValidateYearMonth()

    [Theory, MemberData(nameof(InvalidYearData))]
    public void ValidateYearMonth_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateYearMonth(y, 1));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateYearMonth(y, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void ValidateYearMonth_InvalidMonth(int y, int m)
    {
        AssertEx.ThrowsAoorexn("month", () => ScopeUT.ValidateYearMonth(y, m));
        AssertEx.ThrowsAoorexn("m", () => ScopeUT.ValidateYearMonth(y, m, nameof(m)));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void ValidateYearMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        ScopeUT.ValidateYearMonth(y, m);
    }

    #endregion

    #region CheckYearMonthDay()

    [Theory, MemberData(nameof(InvalidYearData))]
    public void CheckYearMonthDay_InvalidYear(int y) =>
        Assert.False(ScopeUT.CheckYearMonthDay(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void CheckYearMonthDay_InvalidMonth(int y, int m) =>
        Assert.False(ScopeUT.CheckYearMonthDay(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void CheckYearMonthDay_InvalidDay(int y, int m, int d) =>
        Assert.False(ScopeUT.CheckYearMonthDay(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void CheckYearMonthDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        Assert.True(ScopeUT.CheckYearMonthDay(y, m, d));
    }

    #endregion
    #region ValidateYearMonthDay()

    [Theory, MemberData(nameof(InvalidYearData))]
    public void ValidateYearMonthDay_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateYearMonthDay(y, 1, 1));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateYearMonthDay(y, 1, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void ValidateYearMonthDay_InvalidMonth(int y, int m)
    {
        AssertEx.ThrowsAoorexn("month", () => ScopeUT.ValidateYearMonthDay(y, m, 1));
        AssertEx.ThrowsAoorexn("m", () => ScopeUT.ValidateYearMonthDay(y, m, 1, nameof(m)));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void ValidateYearMonthDay_InvalidDay(int y, int m, int d)
    {
        AssertEx.ThrowsAoorexn("day", () => ScopeUT.ValidateYearMonthDay(y, m, d));
        AssertEx.ThrowsAoorexn("d", () => ScopeUT.ValidateYearMonthDay(y, m, d, nameof(d)));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ValidateYearMonthDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        ScopeUT.ValidateYearMonthDay(y, m, d);
    }

    #endregion

    #region CheckOrdinal()

    [Theory, MemberData(nameof(InvalidYearData))]
    public void CheckOrdinal_InvalidYear(int y) =>
        Assert.False(ScopeUT.CheckOrdinal(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void CheckOrdinal_InvalidDayOfYear(int y, int doy) =>
        Assert.False(ScopeUT.CheckOrdinal(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public void CheckOrdinal(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        Assert.True(ScopeUT.CheckOrdinal(y, doy));
    }

    #endregion
    #region ValidateOrdinal()

    [Theory, MemberData(nameof(InvalidYearData))]
    public void ValidateOrdinal_InvalidYear(int y)
    {
        AssertEx.ThrowsAoorexn("year", () => ScopeUT.ValidateOrdinal(y, 1));
        AssertEx.ThrowsAoorexn("y", () => ScopeUT.ValidateOrdinal(y, 1, nameof(y)));
    }

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void ValidateOrdinal_InvalidDayOfYear(int y, int doy)
    {
        AssertEx.ThrowsAoorexn("dayOfYear", () => ScopeUT.ValidateOrdinal(y, doy));
        AssertEx.ThrowsAoorexn("doy", () => ScopeUT.ValidateOrdinal(y, doy, nameof(doy)));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void ValidateOrdinal(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        ScopeUT.ValidateOrdinal(y, doy);
    }

    #endregion
}
