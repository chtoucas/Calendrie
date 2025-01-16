// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about the <see cref="MonthMath{TMonth}"/> type.
/// </summary>
public class DefaultMonthMathFacts<TMonth, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TMonth : struct, IMonth<TMonth>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    public DefaultMonthMathFacts(MonthMath<TMonth> monthMath)
    {
        ArgumentNullException.ThrowIfNull(monthMath);
        if (monthMath.AdditionRule != AdditionRule.Truncate)
            throw new ArgumentException(null, nameof(monthMath));

        MonthMathUT = monthMath;
        SupportedYears = TMonth.Calendar.Scope.Segment.SupportedYears;
        SupportedMonths = TMonth.Calendar.Scope.Segment.SupportedMonths;
    }

    protected MonthMath<TMonth> MonthMathUT { get; }

    protected TMonth MinMonth => TMonth.MinValue;
    protected TMonth MaxMonth => TMonth.MaxValue;

    protected Range<int> SupportedYears { get; }
    protected Range<int> SupportedMonths { get; }

    protected static TMonth GetMonth(Yemo ym)
    {
        var (y, m) = ym;
        return TMonth.Create(y, m);
    }

    /// <summary>
    /// We only use this sample year when its value matters (mathops); otherwise
    /// just use the first month of the year 1. It is initialized to ensure that
    /// the math operations we are going to perform will work.
    /// </summary>
    private static TMonth GetSampleMonth() => TMonth.Create(1234, 2);

    #region AddYears()

    [Fact]
    public void AddYears_Overflows_WithMaxYears()
    {
        var month = TMonth.Create(1, 1);
        // Act & Assert
        AssertEx.Overflows(() => MonthMathUT.AddYears(month, int.MinValue));
        AssertEx.Overflows(() => MonthMathUT.AddYears(month, int.MaxValue));
    }

    [Fact]
    public void AddYears_AtMinMonth()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MonthMathUT.AddYears(MinMonth, -1));
        Assert.Equal(MinMonth, MonthMathUT.AddYears(MinMonth, 0));
        _ = MonthMathUT.AddYears(MinMonth, years);
        AssertEx.Overflows(() => MonthMathUT.AddYears(MinMonth, years + 1));
    }

    [Fact]
    public void AddYears_AtMaxMonth()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MonthMathUT.AddYears(MaxMonth, -years - 1));
        _ = MonthMathUT.AddYears(MaxMonth, -years);
        Assert.Equal(MaxMonth, MonthMathUT.AddYears(MaxMonth, 0));
        AssertEx.Overflows(() => MonthMathUT.AddYears(MaxMonth, 1));
    }

    [Fact]
    public void AddYears_WithLimitYears()
    {
        var month = GetSampleMonth();
        int minYs = MinMonth.Year - month.Year;
        int maxYs = MaxMonth.Year - month.Year;
        // Act & Assert
        AssertEx.Overflows(() => MonthMathUT.AddYears(month, minYs - 1));
        _ = MonthMathUT.AddYears(month, minYs);
        _ = MonthMathUT.AddYears(month, maxYs);
        AssertEx.Overflows(() => MonthMathUT.AddYears(month, maxYs + 1));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AddYears_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(month, MonthMathUT.AddYears(month, 0));
    }

    [Theory, MemberData(nameof(AddYearsMonthData))]
    public void AddYears(YemoPairAnd<int> info)
    {
        int years = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, MonthMathUT.AddYears(month, years));
        Assert.Equal(month, MonthMathUT.AddYears(other, -years));
    }

    #endregion
    #region CountYearsBetween()

    [Fact]
    public void CountYearsBetween_DoesNotOverflow()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Equal(years, MonthMathUT.CountYearsBetween(MinMonth, MaxMonth, out var newStart));
        Assert.Equal(MinMonth.PlusYears(years), newStart);
        Assert.Equal(-years, MonthMathUT.CountYearsBetween(MaxMonth, MinMonth, out newStart));
        Assert.Equal(MaxMonth.PlusYears(-years), newStart);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountYearsBetween_WhenSame_IsZero(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(0, MonthMathUT.CountYearsBetween(month, month, out var newStart));
        Assert.Equal(month, newStart);
    }

    [Theory, MemberData(nameof(CountYearsBetweenMonthData))]
    public void CountYearsBetween(YemoPairAnd<int> info)
    {
        int years = info.Value;
        var start = GetMonth(info.First);
        var end = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(years, MonthMathUT.CountYearsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusYears(years), newStart);
        Assert.Equal(-years, MonthMathUT.CountYearsBetween(end, start, out newStart));
        Assert.Equal(end.PlusYears(-years), newStart);
    }

    #endregion
}
