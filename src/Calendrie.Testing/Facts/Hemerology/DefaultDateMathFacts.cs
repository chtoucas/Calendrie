// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about the <see cref="DateMath{TDate}"/> type.
/// </summary>
public class DefaultDateMathFacts<TDate, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    public DefaultDateMathFacts(DateMath<TDate> dateMath)
    {
        ArgumentNullException.ThrowIfNull(dateMath);
        if (dateMath.AdditionRule != AdditionRule.Truncate)
            throw new ArgumentException(null, nameof(dateMath));

        DateMathUT = dateMath;
        SupportedYears = TDate.Calendar.Scope.Segment.SupportedYears;
    }

    protected DateMath<TDate> DateMathUT { get; }

    protected TDate MinDate => TDate.MinValue;
    protected TDate MaxDate => TDate.MaxValue;

    protected Range<int> SupportedYears { get; }

    protected static TDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return TDate.Create(y, m, d);
    }

    #region PlusYears()

    [Fact]
    public void PlusYears_Overflows_WithMaxYears()
    {
        var date = TDate.FromDayNumber(TDate.Calendar.Epoch);
        // Act & Assert
        AssertEx.Overflows(() => DateMathUT.AddYears(date, int.MinValue));
        AssertEx.Overflows(() => DateMathUT.AddYears(date, int.MaxValue));
    }

    [Fact]
    public void PlusYears_AtMinDate()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => DateMathUT.AddYears(MinDate, -1));
        Assert.Equal(MinDate, DateMathUT.AddYears(MinDate, 0));
        _ = DateMathUT.AddYears(MinDate, years);
        AssertEx.Overflows(() => DateMathUT.AddYears(MinDate, years + 1));
    }

    [Fact]
    public void PlusYears_AtMaxDate()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => DateMathUT.AddYears(MaxDate, -years - 1));
        _ = DateMathUT.AddYears(MaxDate, -years);
        Assert.Equal(MaxDate, DateMathUT.AddYears(MaxDate, 0));
        AssertEx.Overflows(() => DateMathUT.AddYears(MaxDate, 1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusYears_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(date, DateMathUT.AddYears(date, 0));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, DateMathUT.AddYears(date, years));
        Assert.Equal(date, DateMathUT.AddYears(other, -years));
    }

    #endregion
    #region CountYearsSince()

    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Equal(years, DateMathUT.CountYearsBetween(MinDate, MaxDate, out var newStart));
        Assert.Equal(MinDate.PlusYears(years), newStart);
        Assert.Equal(-years, DateMathUT.CountYearsBetween(MaxDate, MinDate, out newStart));
        Assert.Equal(MaxDate.PlusYears(-years), newStart);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountYearsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(0, DateMathUT.CountYearsBetween(date, date, out var newStart));
        Assert.Equal(date, newStart);
    }

    [Theory, MemberData(nameof(CountYearsBetweenData))]
    public void CountYearsSince(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(years, DateMathUT.CountYearsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusYears(years), newStart);
        Assert.Equal(-years, DateMathUT.CountYearsBetween(end, start, out newStart));
        Assert.Equal(end.PlusYears(-years), newStart);
    }

    [Fact]
    public void CountYearsSince_SpecialCases()
    {
        // 1/3/2000 - 2/3/1900 = 99 years
        var start = TDate.Create(1900, 3, 2);
        var end = TDate.Create(2000, 3, 1);
        var exp1 = TDate.Create(1999, 3, 2);
        var exp2 = TDate.Create(1901, 3, 1);
        // Act & Assert
        Assert.Equal(99, DateMathUT.CountYearsBetween(start, end, out var newStart));
        Assert.Equal(exp1, newStart);
        Assert.Equal(-99, DateMathUT.CountYearsBetween(end, start, out newStart));
        Assert.Equal(exp2, newStart);
    }

    #endregion

    #region PlusMonths()

    [Fact]
    public void PlusMonths_Overflows_WithMaxMonths()
    {
        var date = TDate.FromDayNumber(TDate.Calendar.Epoch);
        // Act & Assert
        AssertEx.Overflows(() => DateMathUT.AddMonths(date, int.MinValue));
        AssertEx.Overflows(() => DateMathUT.AddMonths(date, int.MaxValue));
    }

    [Fact]
    public void PlusMonths_AtMinDate() => Assert.Equal(MinDate, DateMathUT.AddMonths(MinDate, 0));

    [Fact]
    public void PlusMonths_AtMaxDate()
    {
        // Act & Assert
        Assert.Equal(MaxDate, DateMathUT.AddMonths(MaxDate, 0));
        AssertEx.Overflows(() => DateMathUT.AddMonths(MaxDate, 1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusMonths_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(date, DateMathUT.AddMonths(date, 0));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void PlusMonths(YemodaPairAnd<int> info)
    {
        int months = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, DateMathUT.AddMonths(date, months));
        Assert.Equal(date, DateMathUT.AddMonths(other, -months));
    }

    #endregion
    #region CountMonthsSince()

    [Fact]
    public void CountMonthsSince_DoesNotOverflow()
    {
        int months = DateMathUT.CountMonthsBetween(MinDate, MaxDate, out var newStart);
        Assert.Equal(MinDate.PlusMonths(months), newStart);
        Assert.Equal(-months, DateMathUT.CountMonthsBetween(MaxDate, MinDate, out newStart));
        Assert.Equal(MaxDate.PlusMonths(-months), newStart);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountMonthsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(0, DateMathUT.CountMonthsBetween(date, date, out var newStart));
        Assert.Equal(date, newStart);
    }

    [Theory, MemberData(nameof(CountMonthsBetweenData))]
    public void CountMonthsSince(YemodaPairAnd<int> info)
    {
        int months = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(months, DateMathUT.CountMonthsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusMonths(months), newStart);
        Assert.Equal(-months, DateMathUT.CountMonthsBetween(end, start, out newStart));
        Assert.Equal(end.PlusMonths(-months), newStart);
    }

    [Fact]
    public void CountMonthsSince_SpecialCases()
    {
        // 1/3/2000 - 2/3/1900 = 8 months
        var start = TDate.Create(2000, 3, 2);
        var end = TDate.Create(2000, 12, 1);
        var exp1 = TDate.Create(2000, 11, 2);
        var exp2 = TDate.Create(2000, 4, 1);
        // Act & Assert
        Assert.Equal(8, DateMathUT.CountMonthsBetween(start, end, out var newStart));
        Assert.Equal(exp1, newStart);
        Assert.Equal(-8, DateMathUT.CountMonthsBetween(end, start, out newStart));
        Assert.Equal(exp2, newStart);
    }

    #endregion
}
