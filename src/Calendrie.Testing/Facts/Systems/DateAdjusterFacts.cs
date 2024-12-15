// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Systems;

using Calendrie.Core.Utilities;
using Calendrie.Hemerology;
using Calendrie.Systems;
using Calendrie.Testing.Data;
using Calendrie.Testing.Facts.Hemerology;

public abstract partial class DateAdjusterFacts<TDate, TDataSet> :
    IDateAdjusterFacts<DateAdjuster<TDate>, TDate, TDataSet>
    where TDate : IAdjustable<TDate>, IDate<TDate>, IFixedDateFactory<TDate>
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected DateAdjusterFacts(DateAdjuster<TDate> adjuster) : base(adjuster) { }
}

public partial class DateAdjusterFacts<TDate, TDataSet> // Adjust()
{
    [Fact]
    public void Adjust_InvalidAdjuster()
    {
        var date = GetDate(1, 1, 1);
        // Act & Assert
        AssertEx.ThrowsAnexn("adjuster", () => date.Adjust(null!));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_InvalidYear(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        foreach (int invalidYear in SupportedYearsTester.InvalidYears)
        {
            var adjuster = AdjusterUT.WithYear(invalidYear);
            // Act & Assert
            AssertEx.ThrowsAoorexn("newYear", () => date.Adjust(adjuster));
        }
    }

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void Adjust_InvalidMonth(int y, int newMonth)
    {
        var date = GetDate(y, 1, 1);
        var adjuster = AdjusterUT.WithMonth(newMonth);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newMonth", () => date.Adjust(adjuster));
    }

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void Adjust_InvalidDay(int y, int m, int newDay)
    {
        var date = GetDate(y, m, 1);
        var adjuster = AdjusterUT.WithDay(newDay);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newDay", () => date.Adjust(adjuster));
    }

    [Theory, MemberData(nameof(InvalidDayOfYearFieldData))]
    public void Adjust_InvalidDayOfYear(int y, int newDayOfYear)
    {
        var date = GetDate(y, 1);
        var adjuster = AdjusterUT.WithDayOfYear(newDayOfYear);
        // Act & Assert
        AssertEx.ThrowsAoorexn("newDayOfYear", () => date.Adjust(adjuster));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_Invariance(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, d);
        // Act & Assert
        Assert.Equal(date, date.Adjust(x => x));
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void Adjust_WithMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var date = GetDate(y, 1, 1);
        var adjuster = AdjusterUT.WithMonth(m);
        var exp = GetDate(y, m, 1);
        // Act & Assert
        Assert.Equal(exp, date.Adjust(adjuster));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_WithDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        var date = GetDate(y, m, 1);
        var exp = GetDate(y, m, d);
        var adjuster = AdjusterUT.WithDay(d);
        // Act & Assert
        Assert.Equal(exp, date.Adjust(adjuster));
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void Adjust_WithDayOfYear(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        var date = GetDate(y, 1);
        var exp = GetDate(y, doy);
        var adjuster = AdjusterUT.WithDayOfYear(doy);
        // Act & Assert
        Assert.Equal(exp, date.Adjust(adjuster));
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
