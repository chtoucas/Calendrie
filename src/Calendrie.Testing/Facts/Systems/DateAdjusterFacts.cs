// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Hemerology;
using Calendrie.Systems;
using Calendrie.Testing.Data;

// In addition, one should test WithYear() with valid and invalid results.

internal abstract partial class DateAdjusterFacts<TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : struct, IDateable, IAbsoluteDate, IDateFactory<TDate>, IAdjustableDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected DateAdjusterFacts(CalendarSystem<TDate> adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        CalendarUT = adjuster;

        var supportedYears = adjuster.Scope.Segment.SupportedYears;
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    /// <summary>
    /// Gets the adjuster under test.
    /// </summary>
    protected CalendarSystem<TDate> CalendarUT { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }

    protected abstract TDate GetDate(int y, int m, int d);
    protected abstract TDate GetDate(int y, int doy);
}

internal partial class DateAdjusterFacts<TDate, TDataSet> // Special dates
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var startOfYear = GetDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, CalendarUT.GetStartOfYear(date));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var date = GetDate(y, 1, 1);
        // Act
        var endOfYear = CalendarUT.GetEndOfYear(date);
        // Assert
        Assert.Equal(y, endOfYear.Year);
        Assert.Equal(info.DaysInYear, endOfYear.DayOfYear);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfMonth(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        var startOfMonth = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, CalendarUT.GetStartOfMonth(date));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = GetDate(y, m, 1);
        var endOfMonth = GetDate(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, CalendarUT.GetEndOfMonth(date));
    }
}

#if false

internal partial class DateAdjusterFacts<TDate, TDataSet> // AdjustYear()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustYear_InvalidYears(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(y => AdjusterUT.AdjustYear(date, y), "newYear");
    }

    [Fact]
    public void AdjustYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var date = GetDate(1, 1, 1);
            var exp = GetDate(y, 1, 1);
            // Act & Assert
            Assert.Equal(exp, AdjusterUT.AdjustYear(date, y));
        }
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustYear_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustYear(date, y));
    }

    // NB: disabled because this cannot work in case the matching day in year 1
    // is not valid. Nevertheless I keep it around just to remind me that I
    // should not try to create it again.
    //[Theory, MemberData(nameof(DateInfoData))]
    //public void AdjustYear(DateInfo info)
    //{
    //    var (y, m, d) = info.Yemoda;
    //    var date = GetDate(1, m, d);
    //    var exp = GetDate(y, m, d);
    //    // Act & Assert
    //    Assert.Equal(exp, AdjusterUT.AdjustYear(date, y));
    //}
}

internal partial class DateAdjusterFacts<TDate, TDataSet> // AdjustMonth()
{
    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void AdjustMonth_InvalidMonth(int y, int newMonth)
    {
        var date = GetDate(y, 1, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newMonth", () => AdjusterUT.AdjustMonth(date, newMonth));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustMonth_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustMonth(date, m));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AdjustMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = GetDate(y, 1, 1);
        var exp = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(exp, AdjusterUT.AdjustMonth(date, m));
    }
}

internal partial class DateAdjusterFacts<TDate, TDataSet> // AdjustDay()
{
    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void AdjustDay_InvalidDay(int y, int m, int newDayOfMonth)
    {
        var date = GetDate(y, m, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newDayOfMonth", () => AdjusterUT.AdjustDayOfMonth(date, newDayOfMonth));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDay_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustDayOfMonth(date, d));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, 1);
        var exp = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, AdjusterUT.AdjustDayOfMonth(date, d));
    }
}

internal partial class DateAdjusterFacts<TDate, TDataSet> // AdjustDayOfYear()
{
    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void AdjustDayOfYear_InvalidDayOfYear(int y, int newDayOfYear)
    {
        var date = GetDate(y, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newDayOfYear", () => AdjusterUT.AdjustDayOfYear(date, newDayOfYear));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDayOfYear_Invariance(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustDayOfYear(date, doy));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, 1);
        var exp = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(exp, AdjusterUT.AdjustDayOfYear(date, doy));
    }
}

#endif

internal partial class DateAdjusterFacts<TDate, TDataSet> // Adjust()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_InvalidYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        foreach (int invalidYear in SupportedYearsTester.InvalidYears)
        {
            // Act & Assert
            AssertEx.ThrowsAoorexn("newYear", () => date.WithYear(invalidYear));
        }
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void Adjust_InvalidMonth(int y, int newMonth)
    {
        var date = GetDate(y, 1, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newMonth", () => date.WithMonth(newMonth));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void Adjust_InvalidDay(int y, int m, int newDay)
    {
        var date = GetDate(y, m, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newDay", () => date.WithDay(newDay));
    }

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void Adjust_InvalidDayOfYear(int y, int newDayOfYear)
    {
        var date = GetDate(y, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newDayOfYear", () => date.WithDayOfYear(newDayOfYear));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void Adjust_WithYear(YearInfo info)
    {
        var y = info.Year;
        var date = GetDate(1, 1, 1);
        var exp = GetDate(y, 1, 1);
        // Act & Assert
        Assert.Equal(exp, date.WithYear(y));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void Adjust_WithMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = GetDate(y, 1, 1);
        var exp = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(exp, date.WithMonth(m));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_WithDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, 1);
        var exp = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(exp, date.WithDay(d));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_WithDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, 1);
        var exp = GetDate(y, doy);
        // Act & Assert
        Assert.Equal(exp, date.WithDayOfYear(doy));
    }

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Adjust(DateInfo info)
    //{
    //    var (y, m, d) = info.Yemoda;
    //    var date = GetDate(1, 1, 1);
    //    var adjuster = (DateParts _) => new DateParts(y, m, d);
    //    var exp = GetDate(y, m, d);
    //    // Act & Assert
    //    Assert.Equal(exp, date.Adjust(adjuster));
    //}

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Adjust_Invariance(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var date = GetDate(y, doy);
    //    // Act & Assert
    //    Assert.Equal(date, date.Adjust(x => x));
    //}

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Adjust(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var date = GetDate(1, 1);
    //    var adjuster = (OrdinalParts _) => new OrdinalParts(y, doy);
    //    var exp = GetDate(y, doy);
    //    // Act & Assert
    //    Assert.Equal(exp, date.Adjust(adjuster));
    //}
}
