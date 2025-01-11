// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides data-driven tests for <see cref="CalendarScope"/>.
/// </summary>
internal abstract class CalendarScopeFacts<TScope, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TScope : CalendarScope
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected CalendarScopeFacts(TScope scope)
    {
        ArgumentNullException.ThrowIfNull(scope);

        // Right now, datasets only work for a range of years.
        if (!scope.Segment.IsComplete) throw new ArgumentException("", nameof(scope));

        ScopeUT = scope;
    }

    /// <summary>
    /// Gets the scope under test.
    /// </summary>
    protected TScope ScopeUT { get; }

    [Theory] public abstract void CheckYear_InvalidYear(int y);
    [Theory] public abstract void CheckYear(int y);

    [Theory] public abstract void ValidateYear_InvalidYear(int y);
    [Theory] public abstract void ValidateYear(int y);

    #region CheckYearMonth()

    [Theory] public abstract void CheckYearMonth_InvalidYear(int y);

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

    [Theory] public abstract void ValidateYearMonth_InvalidYear(int y);

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

    [Theory] public abstract void CheckYearMonthDay_InvalidYear(int y);

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

    [Theory] public abstract void ValidateYearMonthDay_InvalidYear(int y);

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

    [Theory] public abstract void CheckOrdinal_InvalidYear(int y);

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

    [Theory] public abstract void ValidateOrdinal_InvalidYear(int y);

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
