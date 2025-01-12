﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Hemerology;
using Calendrie.Testing.Data;

// TODO(fact): cleanup.
// In addition, one should test WithYear() with valid and invalid results.

internal partial class DateAdjusterFacts<TDate, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected DateAdjusterFacts(Calendar adjuster)
    {
        ArgumentNullException.ThrowIfNull(adjuster);

        CalendarUT = adjuster;

        var supportedYears = adjuster.Scope.Segment.SupportedYears;
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    /// <summary>
    /// Gets the adjuster under test.
    /// </summary>
    protected Calendar CalendarUT { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }
}

#if false

internal partial class DateAdjusterFacts<TDate, TDataSet> // Special dates
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetStartOfYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        var startOfYear = TDate.Create(y, 1, 1);
        // Act & Assert
        Assert.Equal(startOfYear, CalendarUT.GetStartOfYear(date));
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var date = TDate.Create(y, 1, 1);
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
        var date = TDate.Create(y, m, d);
        var startOfMonth = TDate.Create(y, m, 1);
        // Act & Assert
        Assert.Equal(startOfMonth, CalendarUT.GetStartOfMonth(date));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = TDate.Create(y, m, 1);
        var endOfMonth = TDate.Create(y, m, info.DaysInMonth);
        // Act & Assert
        Assert.Equal(endOfMonth, CalendarUT.GetEndOfMonth(date));
    }
}


internal partial class DateAdjusterFacts<TDate, TDataSet> // AdjustYear()
{
    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustYear_InvalidYears(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        SupportedYearsTester.TestInvalidYear(y => AdjusterUT.AdjustYear(date, y), "newYear");
    }

    [Fact]
    public void AdjustYear_ValidYears()
    {
        foreach (int y in SupportedYearsTester.ValidYears)
        {
            var date = TDate.Create(1, 1, 1);
            var exp = TDate.Create(y, 1, 1);
            // Act & Assert
            Assert.Equal(exp, AdjusterUT.AdjustYear(date, y));
        }
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustYear_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
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
    //    var date = TDate.Create(1, m, d);
    //    var exp = TDate.Create(y, m, d);
    //    // Act & Assert
    //    Assert.Equal(exp, AdjusterUT.AdjustYear(date, y));
    //}
}

internal partial class DateAdjusterFacts<TDate, TDataSet> // AdjustMonth()
{
    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void AdjustMonth_InvalidMonth(int y, int newMonth)
    {
        var date = TDate.Create(y, 1, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newMonth", () => AdjusterUT.AdjustMonth(date, newMonth));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustMonth_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustMonth(date, m));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void AdjustMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = TDate.Create(y, 1, 1);
        var exp = TDate.Create(y, m, 1);
        // Act & Assert
        Assert.Equal(exp, AdjusterUT.AdjustMonth(date, m));
    }
}

internal partial class DateAdjusterFacts<TDate, TDataSet> // AdjustDay()
{
    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void AdjustDay_InvalidDay(int y, int m, int newDayOfMonth)
    {
        var date = TDate.Create(y, m, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newDayOfMonth", () => AdjusterUT.AdjustDayOfMonth(date, newDayOfMonth));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDay_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustDayOfMonth(date, d));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = TDate.Create(y, m, 1);
        var exp = TDate.Create(y, m, d);
        // Act & Assert
        Assert.Equal(exp, AdjusterUT.AdjustDayOfMonth(date, d));
    }
}

internal partial class DateAdjusterFacts<TDate, TDataSet> // AdjustDayOfYear()
{
    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void AdjustDayOfYear_InvalidDayOfYear(int y, int newDayOfYear)
    {
        var date = TDate.Create(y, 1);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newDayOfYear", () => AdjusterUT.AdjustDayOfYear(date, newDayOfYear));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDayOfYear_Invariance(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = TDate.Create(y, doy);
        // Act & Assert
        Assert.Equal(date, AdjusterUT.AdjustDayOfYear(date, doy));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void AdjustDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = TDate.Create(y, 1);
        var exp = TDate.Create(y, doy);
        // Act & Assert
        Assert.Equal(exp, AdjusterUT.AdjustDayOfYear(date, doy));
    }
}

#endif

internal partial class DateAdjusterFacts<TDate, TDataSet> // Adjustments
{
    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Adjust(DateInfo info)
    //{
    //    var (y, m, d) = info.Yemoda;
    //    var date = TDate.Create(1, 1, 1);
    //    var adjuster = (DateParts _) => new DateParts(y, m, d);
    //    var exp = TDate.Create(y, m, d);
    //    // Act & Assert
    //    Assert.Equal(exp, date.Adjust(adjuster));
    //}

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Adjust_Invariance(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var date = TDate.Create(y, doy);
    //    // Act & Assert
    //    Assert.Equal(date, date.Adjust(x => x));
    //}

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void Adjust(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var date = TDate.Create(1, 1);
    //    var adjuster = (OrdinalParts _) => new OrdinalParts(y, doy);
    //    var exp = TDate.Create(y, doy);
    //    // Act & Assert
    //    Assert.Equal(exp, date.Adjust(adjuster));
    //}
}
