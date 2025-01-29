// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about the <see cref="DateMath"/> type.
/// </summary>
public class DefaultDateMathFacts<TDate, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    public DefaultDateMathFacts()
    {
        MathUT = new DateMath();
        SupportedYears = TDate.Calendar.Scope.Segment.SupportedYears;
    }

    protected DateMath MathUT { get; }

    protected TDate MinDate => TDate.MinValue;
    protected TDate MaxDate => TDate.MaxValue;

    protected Range<int> SupportedYears { get; }

    protected static TDate GetDate(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return TDate.Create(y, m, d);
    }

    #region AddYears()

    [Fact]
    public void AddYears_Overflows_WithMaxYears()
    {
        var date = TDate.FromDayNumber(TDate.Calendar.Epoch);
        // Act & Assert
        AssertEx.Overflows(() => MathUT.AddYears(date, int.MinValue));
        AssertEx.Overflows(() => MathUT.AddYears(date, int.MaxValue));
    }

    [Fact]
    public void AddYears_AtMinDate()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MathUT.AddYears(MinDate, -1));
        Assert.Equal(MinDate, MathUT.AddYears(MinDate, 0));
        _ = MathUT.AddYears(MinDate, years);
        AssertEx.Overflows(() => MathUT.AddYears(MinDate, years + 1));
    }

    [Fact]
    public void AddYears_AtMaxDate()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MathUT.AddYears(MaxDate, -years - 1));
        _ = MathUT.AddYears(MaxDate, -years);
        Assert.Equal(MaxDate, MathUT.AddYears(MaxDate, 0));
        AssertEx.Overflows(() => MathUT.AddYears(MaxDate, 1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AddYears_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(date, MathUT.AddYears(date, 0));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void AddYears(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddYears(date, years));
        Assert.Equal(date, MathUT.AddYears(other, -years));
    }

    #endregion
    #region CountYearsSince()

    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Equal(years, MathUT.CountYearsBetween(MinDate, MaxDate));
        Assert.Equal(-years, MathUT.CountYearsBetween(MaxDate, MinDate));

        Assert.Equal(years, MathUT.CountYearsBetween(MinDate, MaxDate, out var newStart));
        Assert.Equal(MinDate.PlusYears(years), newStart);
        Assert.Equal(-years, MathUT.CountYearsBetween(MaxDate, MinDate, out newStart));
        Assert.Equal(MaxDate.PlusYears(-years), newStart);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountYearsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(0, MathUT.CountYearsBetween(date, date));
        Assert.Equal(0, MathUT.CountYearsBetween(date, date, out var newStart));
        Assert.Equal(date, newStart);
    }

    [Theory, MemberData(nameof(CountYearsBetweenData))]
    public void CountYearsSince(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
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
    // is an intercalary day or month involved.
    [Fact]
    public void CountYearsSince_SpecialCase()
    {
        // 1/3/2000 - 2/3/1900 = 99 years
        var start = TDate.Create(1900, 3, 2);
        var end = TDate.Create(2000, 3, 1);
        var exp1 = TDate.Create(1999, 3, 2);
        var exp2 = TDate.Create(1901, 3, 1);
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

    #region AddMonths()

    [Fact]
    public void AddMonths_Overflows_WithMaxMonths()
    {
        var date = TDate.FromDayNumber(TDate.Calendar.Epoch);
        // Act & Assert
        AssertEx.Overflows(() => MathUT.AddMonths(date, int.MinValue));
        AssertEx.Overflows(() => MathUT.AddMonths(date, int.MaxValue));
    }

    [Fact]
    public void AddMonths_AtMinDate() => Assert.Equal(MinDate, MathUT.AddMonths(MinDate, 0));

    [Fact]
    public void AddMonths_AtMaxDate()
    {
        // Act & Assert
        Assert.Equal(MaxDate, MathUT.AddMonths(MaxDate, 0));
        AssertEx.Overflows(() => MathUT.AddMonths(MaxDate, 1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AddMonths_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(date, MathUT.AddMonths(date, 0));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void AddMonths(YemodaPairAnd<int> info)
    {
        int months = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, MathUT.AddMonths(date, months));
        Assert.Equal(date, MathUT.AddMonths(other, -months));
    }

    #endregion
    #region CountMonthsSince()

    [Fact]
    public void CountMonthsSince_DoesNotOverflow()
    {
        _ = MathUT.CountMonthsBetween(MinDate, MaxDate);
        _ = MathUT.CountMonthsBetween(MaxDate, MinDate);

        int months = MathUT.CountMonthsBetween(MinDate, MaxDate, out var newStart);
        Assert.Equal(MinDate.PlusMonths(months), newStart);
        Assert.Equal(-months, MathUT.CountMonthsBetween(MaxDate, MinDate, out newStart));
        Assert.Equal(MaxDate.PlusMonths(-months), newStart);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountMonthsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(0, MathUT.CountMonthsBetween(date, date));

        Assert.Equal(0, MathUT.CountMonthsBetween(date, date, out var newStart));
        Assert.Equal(date, newStart);
    }

    [Theory, MemberData(nameof(CountMonthsBetweenData))]
    public void CountMonthsSince(YemodaPairAnd<int> info)
    {
        int months = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(months, MathUT.CountMonthsBetween(start, end));
        // WARNING: this is not true in general. It just happens that
        // CountYearsBetweenData only provides cases where the result is exact.
        // If it changes in the future, we should remove the following two lines.
        Assert.Equal(-months, MathUT.CountMonthsBetween(end, start));

        Assert.Equal(months, MathUT.CountMonthsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusMonths(months), newStart);
        // WARNING: this is not true in general. It just happens that
        // CountYearsBetweenData only provides cases where the result is exact.
        // If it changes in the future, we should remove the following two lines.
        Assert.Equal(-months, MathUT.CountMonthsBetween(end, start, out newStart));
        Assert.Equal(end.PlusMonths(-months), newStart);
    }

    // For each date types, we should add tests which handle the case when there
    // is an intercalary day or month involved.
    [Fact]
    public void CountMonthsSince_SpecialCase()
    {
        // 1/3/2000 - 2/3/1900 = 8 months
        var start = TDate.Create(2000, 3, 2);
        var end = TDate.Create(2000, 12, 1);
        var exp1 = TDate.Create(2000, 11, 2);
        var exp2 = TDate.Create(2000, 4, 1);
        // Act & Assert
        Assert.Equal(8, MathUT.CountMonthsBetween(start, end));
        Assert.Equal(-8, MathUT.CountMonthsBetween(end, start));

        Assert.Equal(8, MathUT.CountMonthsBetween(start, end, out var newStart));
        Assert.Equal(start.PlusMonths(8), newStart);
        Assert.Equal(exp1, newStart);
        Assert.Equal(-8, MathUT.CountMonthsBetween(end, start, out newStart));
        Assert.Equal(end.PlusMonths(-8), newStart);
        Assert.Equal(exp2, newStart);
    }

    #endregion
}
