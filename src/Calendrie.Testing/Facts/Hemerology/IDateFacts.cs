// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

// We also test the static (abstract) methods from the interface.

/// <summary>
/// Provides facts about the <see cref="IDate{TSelf}"/> type.
/// </summary>
public partial class IDateFacts<TDate, TDataSet> :
    IAbsoluteDateFacts<TDate, TDataSet>
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    public IDateFacts() : base(TDate.Calendar.Scope.Domain)
    {
        var supportedYears = TDate.Calendar.Scope.Segment.SupportedYears;
        SupportedYears = supportedYears;
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    protected Range<int> SupportedYears { get; }
    protected SupportedYearsTester SupportedYearsTester { get; }

    protected sealed override TDate GetDate(int y, int m, int d) => TDate.Create(y, m, d);

    [Fact]
    public void Calendar_Prop() => Assert.NotNull(TDate.Calendar);

    [Fact]
    public void ToString_InvariantCulture()
    {
        var date = TDate.FromDayNumber(TDate.Calendar.Epoch);
        string str = FormattableString.Invariant($"01/01/0001 ({TDate.Calendar})");
        // Act & Assert
        Assert.Equal(str, date.ToString());
    }
}

public partial class IDateFacts<TDate, TDataSet> // Factories & constructors
{
    // Althought, we do not usually test static methods/props in a fact class,
    // the situation is a bit different here since this is a static method on a
    // __type__.

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
        var (y, m, d, doy) = info;
        // Act
        var date = TDate.Create(y, m, d);
        // Assert
        Assert.Equal(y, date.Year);
        Assert.Equal(m, date.Month);
        Assert.Equal(d, date.Day);
        Assert.Equal(doy, date.DayOfYear);
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

public partial class IDateFacts<TDate, TDataSet> // Adjustments
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void WithYear_InvalidYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(date.WithYear, "newYear");
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void WithYear(YearInfo info)
    {
        int y = info.Year;
        var date = TDate.Create(1, 1, 1);
        var exp = TDate.Create(y, 1, 1);
        // Act & Assert
        Assert.Equal(exp, date.WithYear(y));
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void WithMonth_InvalidMonth(int y, int newMonth)
    {
        var date = TDate.Create(y, 1, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newMonth", () => date.WithMonth(newMonth));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = TDate.Create(y, 1, 1);
        var exp = TDate.Create(y, m, 1);
        // Act & Assert
        Assert.Equal(exp, date.WithMonth(m));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void WithDay_InvalidDay(int y, int m, int newDay)
    {
        var date = TDate.Create(y, m, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newDay", () => date.WithDay(newDay));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, 1);
        var exp = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(exp, date.WithDay(d));
    }

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void WithDayOfYear_InvalidDayOfYear(int y, int newDayOfYear)
    {
        var date = TDate.Create(y, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newDayOfYear", () => date.WithDayOfYear(newDayOfYear));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void WithDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = TDate.Create(y, 1);
        var exp = TDate.Create(y, doy);
        // Act & Assert
        Assert.Equal(exp, date.WithDayOfYear(doy));
    }
}

public partial class IDateFacts<TDate, TDataSet> // Math
{
    #region PlusYears()

    [Fact]
    public void PlusYears_Overflows_WithMaxYears()
    {
        var date = TDate.FromDayNumber(TDate.Calendar.Epoch);
        // Act & Assert
        AssertEx.Overflows(() => date.PlusYears(int.MinValue));
        AssertEx.Overflows(() => date.PlusYears(int.MaxValue));
    }

    [Fact]
    public void PlusYears_AtMinDate()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MinDate.PlusYears(-1));
        Assert.Equal(MinDate, MinDate.PlusYears(0));
        _ = MinDate.PlusYears(years);
        AssertEx.Overflows(() => MinDate.PlusYears(years + 1));
    }

    [Fact]
    public void PlusYears_AtMaxDate()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MaxDate.PlusYears(-years - 1));
        _ = MaxDate.PlusYears(-years);
        Assert.Equal(MaxDate, MaxDate.PlusYears(0));
        AssertEx.Overflows(() => MaxDate.PlusYears(1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusYears_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.PlusYears(0));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, date.PlusYears(years));
        Assert.Equal(date, other.PlusYears(-years));
    }

    #endregion
    #region CountYearsSince()

    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Equal(years, MaxDate.CountYearsSince(MinDate));
        Assert.Equal(-years, MinDate.CountYearsSince(MaxDate));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountYearsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(0, date.CountYearsSince(date));
    }

    [Theory, MemberData(nameof(CountYearsBetweenData))]
    public void CountYearsSince(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(years, end.CountYearsSince(start));
        Assert.Equal(-years, start.CountYearsSince(end));
    }

    #endregion

    #region PlusMonths()

    [Fact]
    public void PlusMonths_Overflows_WithMaxMonths()
    {
        var date = TDate.FromDayNumber(TDate.Calendar.Epoch);
        // Act & Assert
        AssertEx.Overflows(() => date.PlusMonths(int.MinValue));
        AssertEx.Overflows(() => date.PlusMonths(int.MaxValue));
    }

    [Fact]
    public void PlusMonths_AtMinDate() => Assert.Equal(MinDate, MinDate.PlusMonths(0));

    [Fact]
    public void PlusMonths_AtMaxDate()
    {
        // Act & Assert
        Assert.Equal(MaxDate, MaxDate.PlusMonths(0));
        AssertEx.Overflows(() => MaxDate.PlusMonths(1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusMonths_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.PlusMonths(0));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void PlusMonths(YemodaPairAnd<int> info)
    {
        int months = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, date.PlusMonths(months));
        Assert.Equal(date, other.PlusMonths(-months));
    }

    #endregion
    #region CountMonthsSince()

    [Fact]
    public void CountMonthsSince_DoesNotOverflow()
    {
        _ = MaxDate.CountMonthsSince(MinDate);
        _ = MinDate.CountMonthsSince(MaxDate);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountMonthsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(0, date.CountMonthsSince(date));
    }

    [Theory, MemberData(nameof(CountMonthsBetweenData))]
    public void CountMonthsSince(YemodaPairAnd<int> info)
    {
        int months = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(months, end.CountMonthsSince(start));
        Assert.Equal(-months, start.CountMonthsSince(end));
    }

    #endregion
}
