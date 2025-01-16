// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

// We also test the static (abstract) methods from the interface.

public partial class IMonthFacts<TMonth, TDate, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TMonth : IMonth<TMonth>, IDaySegment<TDate>, ISetMembership<TDate>
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    public IMonthFacts()
    {
        var supportedYears = TMonth.Calendar.Scope.Segment.SupportedYears;
        SupportedYears = supportedYears;
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    protected TMonth MinMonth => TMonth.MinValue;
    protected TMonth MaxMonth => TMonth.MaxValue;

    protected Range<int> SupportedYears { get; }
    protected SupportedYearsTester SupportedYearsTester { get; }

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
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // Prelude
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void Deconstructor(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act
        var (yA, mA) = month;
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(m, mA);
    }

    //
    // Properties
    //

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void CenturyOfEra_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var month = TMonth.Create(y, 1);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, month.CenturyOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Century_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var month = TMonth.Create(y, 1);
        // Act & Assert
        Assert.Equal(century, month.Century);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfEra_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var month = TMonth.Create(y, 1);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, month.YearOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfCentury_Prop(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        var month = TMonth.Create(y, 1);
        // Act & Assert
        Assert.Equal(yearOfCentury, month.YearOfCentury);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void IsIntercalary_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        var month = TMonth.Create(y, m);
        // Assert
        Assert.Equal(info.IsIntercalary, month.IsIntercalary);
    }
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // Adjustments
{
    #region Year adjustment

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithYear_InvalidYears(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(month.WithYear, "newYear");
    }

    [Fact]
    public void WithYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var month = TMonth.Create(1, 1);
            var exp = TMonth.Create(y, 1);
            // Act & Assert
            Assert.Equal(exp, month.WithYear(y));
        }
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithYear_Invariance(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(month, month.WithYear(y));
    }

    // FIXME(fact): case of intercalary months
    //[Theory, MemberData(nameof(MonthInfoData))]
    //public void WithYear(MonthInfo info)
    //{
    //    var (y, m) = info.Yemo;
    //    var month = TMonth.Create(1, m);
    //    var exp = TMonth.Create(y, m);
    //    // Act & Assert
    //    Assert.Equal(exp, month.WithYear(y));
    //}

    #endregion
    #region Month adjustment

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void WithMonth_InvalidMonth(int y, int newMonth)
    {
        var month = TMonth.Create(y, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newMonth", () => month.WithMonth(newMonth));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonth_Invariance(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(month, month.WithMonth(m));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void WithMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, 1);
        var exp = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(exp, month.WithMonth(m));
    }

    #endregion
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // IDaySegment
{
    [Theory, MemberData(nameof(MonthInfoData))]
    public void MinDay_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        var startOfMonth = TDate.Create(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, month.MinDay);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void MaxDay_Prop(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        byte daysInMonth = info.DaysInMonth;
        var month = TMonth.Create(y, m);
        var endOfMonth = TDate.Create(y, m, daysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, month.MaxDay);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDays(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act
        int actual = month.CountDays();
        // Assert
        Assert.Equal(info.DaysInMonth, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void ToDayRange(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        var min = TDate.Create(y, m, 1);
        var max = TDate.Create(y, m, info.DaysInMonth);
        // Act
        var range = month.ToDayRange();
        // Assert
        Assert.Equal(min, range.Min);
        Assert.Equal(max, range.Max);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void EnumerateDays(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        var exp = from d in Enumerable.Range(1, info.DaysInMonth)
                  select TDate.Create(y, m, d);
        // Act
        var actual = month.EnumerateDays();
        // Assert
        Assert.Equal(exp, actual);
    }
}

public partial class IMonthFacts<TMonth, TDate, TDataSet> // Math
{
    #region PlusYears()

    [Fact]
    public void PlusYears_Overflows_WithMaxYears()
    {
        var month = TMonth.Create(1, 1);
        // Act & Assert
        AssertEx.Overflows(() => month.PlusYears(int.MinValue));
        AssertEx.Overflows(() => month.PlusYears(int.MaxValue));
    }

    [Fact]
    public void PlusYears_AtMinMonth()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MinMonth.PlusYears(-1));
        Assert.Equal(MinMonth, MinMonth.PlusYears(0));
        _ = MinMonth.PlusYears(years);
        AssertEx.Overflows(() => MinMonth.PlusYears(years + 1));
    }

    [Fact]
    public void PlusYears_AtMaxMonth()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        AssertEx.Overflows(() => MaxMonth.PlusYears(-years - 1));
        _ = MaxMonth.PlusYears(-years);
        Assert.Equal(MaxMonth, MaxMonth.PlusYears(0));
        AssertEx.Overflows(() => MaxMonth.PlusYears(1));
    }

    [Fact]
    public void PlusYears_WithLimitYears()
    {
        var month = GetSampleMonth();
        int minYs = MinMonth.Year - month.Year;
        int maxYs = MaxMonth.Year - month.Year;
        // Act & Assert
        AssertEx.Overflows(() => month.PlusYears(minYs - 1));
        _ = month.PlusYears(minYs);
        _ = month.PlusYears(maxYs);
        AssertEx.Overflows(() => month.PlusYears(maxYs + 1));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void PlusYears_Zero_IsNeutral(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(month, month.PlusYears(0));
    }

    [Theory, MemberData(nameof(AddYearsMonthData))]
    public void PlusYears(YemoPairAnd<int> info)
    {
        int years = info.Value;
        var month = GetMonth(info.First);
        var other = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(other, month.PlusYears(years));
        Assert.Equal(month, other.PlusYears(-years));
    }

    #endregion
    #region CountYearsSince()

    [Fact]
    public void CountYearsSince_DoesNotOverflow()
    {
        int years = SupportedYears.Count() - 1;
        // Act & Assert
        Assert.Equal(years, MaxMonth.CountYearsSince(MinMonth));
        Assert.Equal(-years, MinMonth.CountYearsSince(MaxMonth));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountYearsSince_WhenSame_IsZero(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var month = TMonth.Create(y, m);
        // Act & Assert
        Assert.Equal(0, month.CountYearsSince(month));
    }

    [Theory, MemberData(nameof(CountYearsBetweenMonthData))]
    public void CountYearsSince(YemoPairAnd<int> info)
    {
        int years = info.Value;
        var start = GetMonth(info.First);
        var end = GetMonth(info.Second);
        // Act & Assert
        Assert.Equal(years, end.CountYearsSince(start));
        Assert.Equal(-years, start.CountYearsSince(end));
    }

    #endregion
}
