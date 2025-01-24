// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about the <see cref="MonthMath"/> type.
/// </summary>
public class DefaultMonthMathFacts<TMonth, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TMonth : struct, IMonth<TMonth>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    public DefaultMonthMathFacts()
    {
        MathUT = new MonthMath(AdditionRule.Truncate);
        SupportedYears = TMonth.Calendar.Scope.Segment.SupportedYears;
    }

    protected MonthMath MathUT { get; }

    protected TMonth MinMonth => TMonth.MinValue;
    protected TMonth MaxMonth => TMonth.MaxValue;

    protected Range<int> SupportedYears { get; }

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
        AssertEx.Overflows(() => MathUT.AddYears(month, int.MinValue));
        AssertEx.Overflows(() => MathUT.AddYears(month, int.MaxValue));
    }

    [Fact]
    public void AddYears_AtMinMonth()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MathUT.AddYears(MinMonth, -1));
        Assert.Equal(MinMonth, MathUT.AddYears(MinMonth, 0));
        _ = MathUT.AddYears(MinMonth, years);
        AssertEx.Overflows(() => MathUT.AddYears(MinMonth, years + 1));
    }

    [Fact]
    public void AddYears_AtMaxMonth()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MathUT.AddYears(MaxMonth, -years - 1));
        _ = MathUT.AddYears(MaxMonth, -years);
        Assert.Equal(MaxMonth, MathUT.AddYears(MaxMonth, 0));
        AssertEx.Overflows(() => MathUT.AddYears(MaxMonth, 1));
    }

    [Fact]
    public void AddYears_WithLimitYears()
    {
        var month = GetSampleMonth();
        int minYs = MinMonth.Year - month.Year;
        int maxYs = MaxMonth.Year - month.Year;
        // Act & Assert
        AssertEx.Overflows(() => MathUT.AddYears(month, minYs - 1));
        _ = MathUT.AddYears(month, minYs);
        _ = MathUT.AddYears(month, maxYs);
        AssertEx.Overflows(() => MathUT.AddYears(month, maxYs + 1));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AddYears_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(month, MathUT.AddYears(month, 0));
    }

    [Theory, MemberData(nameof(AddYearsMonthData))]
    public void AddYears(YemoPairAnd<int> info)
    {
        int years = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(month, years));
        Assert.Equal(month, MathUT.AddYears(other, -years));
    }

    #endregion
    #region CountYearsBetween()

    [Fact]
    public void CountYearsBetween_DoesNotOverflow()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        _ = MathUT.CountYearsBetween(MinMonth, MaxMonth);
        _ = MathUT.CountYearsBetween(MaxMonth, MinMonth);

        Assert.Equal(years, MathUT.CountYearsBetween(MinMonth, MaxMonth, out var newStart));
        Assert.Equal(MinMonth.PlusYears(years), newStart);
        Assert.Equal(-years, MathUT.CountYearsBetween(MaxMonth, MinMonth, out newStart));
        Assert.Equal(MaxMonth.PlusYears(-years), newStart);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountYearsBetween_WhenSame_IsZero(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(0, MathUT.CountYearsBetween(month, month));

        Assert.Equal(0, MathUT.CountYearsBetween(month, month, out var newStart));
        Assert.Equal(month, newStart);
    }

    [Theory, MemberData(nameof(CountYearsBetweenMonthData))]
    public void CountYearsBetween(YemoPairAnd<int> info)
    {
        int years = info.Value;
        var start = GetMonth(info.First);
        var end = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(years, MathUT.CountYearsBetween(start, end));
        // WARNING: this is not true in general. It just happens that
        // CountYearsBetweenData only provides cases where the result is exact.
        // If it changes in the future, we should remove the following two lines.
        Assert.Equal(-years, MathUT.CountYearsBetween(end, start));

        Assert.Equal(years, MathUT.CountYearsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusYears(years), newStart);
        // WARNING: this is not true in general. It just happens that
        // CountYearsBetweenData only provides cases where the result is exact.
        // If it changes in the future, we should remove the following two lines.
        Assert.Equal(-years, MathUT.CountYearsBetween(end, start, out newStart));
        Assert.Equal(end.PlusYears(-years), newStart);
    }

    // For each date types, we should add tests which handle the case when there
    // is an intercalary month involved.
    [Fact]
    public void CountYearsSince_SpecialCase()
    {
        // 3/2000 - 4/1900 = 99 years
        var start = TMonth.Create(1900, 4);
        var end = TMonth.Create(2000, 3);
        var exp1 = TMonth.Create(1999, 4);
        var exp2 = TMonth.Create(1901, 3);
        // Act & Assert
        Assert.Equal(99, MathUT.CountYearsBetween(start, end));
        Assert.Equal(-99, MathUT.CountYearsBetween(end, start));

        Assert.Equal(99, MathUT.CountYearsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusYears(99), newStart);
        Assert.Equal(exp1, newStart);
        Assert.Equal(-99, MathUT.CountYearsBetween(end, start, out newStart));
        Assert.Equal(end.PlusYears(-99), newStart);
        Assert.Equal(exp2, newStart);
    }

    #endregion
}
