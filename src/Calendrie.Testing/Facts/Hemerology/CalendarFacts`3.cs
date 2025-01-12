// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about <see cref="Calendar"/>.
/// </summary>
[Obsolete("To be removed")]
public abstract partial class CalendarFacts<TDate, TCalendar, TDataSet> :
    CalendarFacts<TCalendar, TDataSet>
    where TDate : IDateable
    where TCalendar : Calendar
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarFacts(TCalendar calendar) : base(calendar) { }

    /// <summary>
    /// Creates a new instance of <typeparamref name="TDate"/> from the specified
    /// components.
    /// </summary>
    protected abstract TDate GetDate(int y, int m, int d);

    /// <summary>
    /// Creates a new instance of <typeparamref name="TDate"/> from the specified
    /// ordinal components.
    /// </summary>
    protected abstract TDate GetDate(int y, int doy);

    /// <summary>
    /// Creates a new instance of <typeparamref name="TDate"/> from the specified
    /// <see cref="DayNumber"/>.
    /// </summary>
    protected abstract TDate GetDate(DayNumber dayNumber);
}

public partial class CalendarFacts<TDate, TCalendar, TDataSet> // Factories
{
    #region Factory(y, m, d)

    [Fact]
    public void Factory_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => GetDate(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void Factory_InvalidMonth(int y, int m) =>
        AssertEx.ThrowsAoorexn("month", () => GetDate(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void Factory_InvalidDay(int y, int m, int d) =>
        AssertEx.ThrowsAoorexn("day", () => GetDate(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Factory(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = GetDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Factory_ViaDayNumber(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = GetDate(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion
    #region Factory(y, doy)

    [Fact]
    public void Factory﹍Ordinal_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => GetDate(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void Factory﹍Ordinal_InvalidDayOfYear(int y, int doy) =>
        AssertEx.ThrowsAoorexn("dayOfYear", () => GetDate(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Factory﹍Ordinal(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var date = GetDate(y, doy);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
        Assert.Equal(doy, date.DayOfYear);
    }

    #endregion
    #region Factory(dayNumber)

    [Fact]
    public void Factory﹍DayNumber_InvalidDayNumber() =>
        DomainTester.TestInvalidDayNumber(GetDate);

    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Factory﹍DayNumber(DayNumberInfo info)
    {
        var (dayNumber, y, m, d) = info;
        // Act
        var date = GetDate(dayNumber);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion
}
