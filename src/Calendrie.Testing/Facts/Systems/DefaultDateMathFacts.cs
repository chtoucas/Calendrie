// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

// TODO(fact): test newStart

/// <summary>
/// Provides facts about the <see cref="IDate{TSelf}"/> type.
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

        DateMath = dateMath;
        SupportedYears = TDate.Calendar.Scope.Segment.SupportedYears;
    }

    protected DateMath<TDate> DateMath { get; }

    protected TDate MinDate => TDate.MinValue;
    protected TDate MaxDate => TDate.MaxValue;

    protected Range<int> SupportedYears { get; }

    protected TDate GetDate(Yemoda ymd)
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
        AssertEx.Overflows(() => DateMath.AddYears(date, int.MinValue));
        AssertEx.Overflows(() => DateMath.AddYears(date, int.MaxValue));
    }

    [Fact]
    public void PlusYears_AtMinDate()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => DateMath.AddYears(MinDate, -1));
        Assert.Equal(MinDate, DateMath.AddYears(MinDate, 0));
        _ = DateMath.AddYears(MinDate, years);
        AssertEx.Overflows(() => DateMath.AddYears(MinDate, years + 1));
    }

    [Fact]
    public void PlusYears_AtMaxDate()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => DateMath.AddYears(MaxDate, -years - 1));
        _ = DateMath.AddYears(MaxDate, -years);
        Assert.Equal(MaxDate, DateMath.AddYears(MaxDate, 0));
        AssertEx.Overflows(() => DateMath.AddYears(MaxDate, 1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusYears_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(date, DateMath.AddYears(date, 0));
    }

    [Theory, MemberData(nameof(AddYearsData))]
    public void PlusYears(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, DateMath.AddYears(date, years));
        Assert.Equal(date, DateMath.AddYears(other, -years));
    }

    #endregion
    #region CountYearsSince()

    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Equal(years, DateMath.CountYearsBetween(MinDate, MaxDate, out _));
        Assert.Equal(-years, DateMath.CountYearsBetween(MaxDate, MinDate, out _));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountYearsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(0, DateMath.CountYearsBetween(date, date, out _));
    }

    [Theory, MemberData(nameof(CountYearsBetweenData))]
    public void CountYearsSince(YemodaPairAnd<int> info)
    {
        int years = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(years, DateMath.CountYearsBetween(start, end, out _));
        Assert.Equal(-years, DateMath.CountYearsBetween(end, start, out _));
    }

    #endregion

    #region PlusMonths()

    [Fact]
    public void PlusMonths_Overflows_WithMaxMonths()
    {
        var date = TDate.FromDayNumber(TDate.Calendar.Epoch);
        // Act & Assert
        AssertEx.Overflows(() => DateMath.AddMonths(date, int.MinValue));
        AssertEx.Overflows(() => DateMath.AddMonths(date, int.MaxValue));
    }

    [Fact]
    public void PlusMonths_AtMinDate() => Assert.Equal(MinDate, DateMath.AddMonths(MinDate, 0));

    [Fact]
    public void PlusMonths_AtMaxDate()
    {
        // Act & Assert
        Assert.Equal(MaxDate, DateMath.AddMonths(MaxDate, 0));
        AssertEx.Overflows(() => DateMath.AddMonths(MaxDate, 1));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void PlusMonths_Zero_IsNeutral(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(date, DateMath.AddMonths(date, 0));
    }

    [Theory, MemberData(nameof(AddMonthsData))]
    public void PlusMonths(YemodaPairAnd<int> info)
    {
        int months = info.Value;
        var date = GetDate(info.First);
        var other = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(other, DateMath.AddMonths(date, months));
        Assert.Equal(date, DateMath.AddMonths(other, -months));
    }

    #endregion
    #region CountMonthsSince()

    [Fact]
    public void CountMonthsSince_DoesNotOverflow()
    {
        _ = DateMath.CountMonthsBetween(MinDate, MaxDate, out _);
        _ = DateMath.CountMonthsBetween(MaxDate, MinDate, out _);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountMonthsSince_WhenSame_IsZero(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(0, DateMath.CountMonthsBetween(date, date, out _));
    }

    [Theory, MemberData(nameof(CountMonthsBetweenData))]
    public void CountMonthsSince(YemodaPairAnd<int> info)
    {
        int months = info.Value;
        var start = GetDate(info.First);
        var end = GetDate(info.Second);
        // Act & Assert
        Assert.Equal(months, DateMath.CountMonthsBetween(start, end, out _));
        Assert.Equal(-months, DateMath.CountMonthsBetween(end, start, out _));
    }

    #endregion
}
