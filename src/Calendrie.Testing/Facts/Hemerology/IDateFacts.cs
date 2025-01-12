// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides data-driven tests for the <see cref="IDate{TSelf}"/> type.
/// </summary>
public partial class IDateFacts<TDate, TCalendar, TDataSet> :
    IAbsoluteDateFacts<TDate, TDataSet>
    where TCalendar : Calendar
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    public IDateFacts(TCalendar calendar) : base(GetDomain(calendar))
    {
        Debug.Assert(calendar != null);

        Calendar = calendar;

        var scope = calendar.Scope;
        var supportedYears = scope.Segment.SupportedYears;
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    protected SupportedYearsTester SupportedYearsTester { get; }

    private static Range<DayNumber> GetDomain(Calendar calendar)
    {
        ArgumentNullException.ThrowIfNull(calendar);

        return calendar.Scope.Domain;
    }

    public TCalendar Calendar { get; }

    protected sealed override TDate GetDate(int y, int m, int d) => TDate.Create(y, m, d);

    [Fact]
    public void ToString_InvariantCulture()
    {
        var date = GetDate(1, 1, 1);
        string str = FormattableString.Invariant($"01/01/0001 ({Calendar})");
        // Act & Assert
        Assert.Equal(str, date.ToString());
    }

    // Althought, we do not usually test static methods/props in a fact class,
    // the situation is a bit different here since this is a static method on a
    // __type__.

    //[Fact]
    //public void Today()
    //{
    //    // This test may fail if there is a change of day between the two calls
    //    // to Today().
    //    var today = DayNumber.Today();
    //    // Act & Assert
    //    Assert.Equal(today, TDate.Today().DayNumber);
    //}

    //[Theory, MemberData(nameof(DayNumberInfoData))]
    //public void FromDayNumber(DayNumberInfo info)
    //{
    //    var (dayNumber, y, m, d) = info;
    //    var date = GetDate(y, m, d);
    //    // Act & Assert
    //    Assert.Equal(date, TDate.FromDayNumber(dayNumber));
    //}
}

public partial class IDateFacts<TDate, TCalendar, TDataSet> // Factories & constructors
{
    #region Create(y, m, d)

    [Fact]
    public void Create_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => TDate.Create(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void Create_InvalidMonth(int y, int m) =>
        AssertEx.ThrowsAoorexn("month", () => TDate.Create(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void Create_InvalidDay(int y, int m, int d) =>
        AssertEx.ThrowsAoorexn("day", () => TDate.Create(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Create(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = TDate.Create(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void Create_ViaDayNumber(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var date = TDate.Create(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion
    #region TryCreate(y, m, d)

    [Fact]
    public void TryCreate_InvalidYear() =>
        SupportedYearsTester.TestInvalidYearTryPattern(y => TDate.TryCreate(y, 1, 1, out _));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void TryCreate_InvalidMonth(int y, int m) =>
        Assert.False(TDate.TryCreate(y, m, 1, out _));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void TryCreate_InvalidDay(int y, int m, int d) =>
        Assert.False(TDate.TryCreate(y, m, d, out _));

    [Theory, MemberData(nameof(DateInfoData))]
    public void TryCreate(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        bool result = TDate.TryCreate(y, m, d, out var date);
        // Assert
        Assert.True(result);
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(DayNumberInfoData))]
    public void TryCreate_ViaDayNumber(DayNumberInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        bool result = TDate.TryCreate(y, m, d, out var date);
        // Assert
        Assert.True(result);
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
    }

    #endregion

    #region Create(y, doy)

    [Fact]
    public void Create﹍Ordinal_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => TDate.Create(y, 1));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void Create﹍Ordinal_InvalidDayOfYear(int y, int doy) =>
        AssertEx.ThrowsAoorexn("dayOfYear", () => TDate.Create(y, doy));

    [Theory, MemberData(nameof(DateInfoData))]
    public void Create﹍Ordinal(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        var date = TDate.Create(y, doy);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
        Assert.Equal(doy, date.DayOfYear);
    }

    #endregion
    #region TryCreate(y, doy)

    [Fact]
    public void TryCreate﹍Ordinal_InvalidYear() =>
        SupportedYearsTester.TestInvalidYearTryPattern(y => TDate.TryCreate(y, 1, out _));

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void TryCreate﹍Ordinal_InvalidDayOfYear(int y, int doy) =>
        Assert.False(TDate.TryCreate(y, doy, out _));

    [Theory, MemberData(nameof(DateInfoData))]
    public void TryCreate﹍Ordinal(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        bool result = TDate.TryCreate(y, doy, out var date);
        // Assert
        Assert.True(result);
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
        Assert.Equal(doy, date.DayOfYear);
    }

    #endregion
}
