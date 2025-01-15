// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie;
using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

// We also test the static (abstract) methods from the interface.

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TYear : IYear<TYear>,
        IMonthSegment<TMonth>, ISetMembership<TMonth>,
        IDaySegment<TDate>, ISetMembership<TDate>
    where TMonth : struct, IMonth<TMonth>
    where TDate : struct, IDate<TDate>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{ }

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> // Prelude
{
    //
    // Properties
    //

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void CenturyOfEra_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var year = TYear.Create(y);
        var centuryOfEra = Ord.Zeroth + century;
        // Act & Assert
        Assert.Equal(centuryOfEra, year.CenturyOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Century_Prop(CenturyInfo info)
    {
        var (y, century, _) = info;
        var year = TYear.Create(y);
        // Act & Assert
        Assert.Equal(century, year.Century);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfEra_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var yearOfEra = Ord.Zeroth + y;
        // Act & Assert
        Assert.Equal(yearOfEra, year.YearOfEra);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void YearOfCentury_Prop(CenturyInfo info)
    {
        var (y, _, yearOfCentury) = info;
        var year = TYear.Create(y);
        // Act & Assert
        Assert.Equal(yearOfCentury, year.YearOfCentury);
    }

    [Theory, MemberData(nameof(CenturyInfoData))]
    public void Year_Prop(CenturyInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        // Act & Assert
        Assert.Equal(y, year.Year);
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void IsLeap_Prop(YearInfo info)
    {
        // Act
        var year = TYear.Create(info.Year);
        // Assert
        Assert.Equal(info.IsLeap, year.IsLeap);
    }
}

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> // IDaySegment
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountMonths(YearInfo info)
    {
        var year = TYear.Create(info.Year);
        // Act & Assert
        Assert.Equal(info.MonthsInYear, year.CountMonths());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void EnumerateMonths(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var exp = from m in Enumerable.Range(1, info.MonthsInYear)
                  select TMonth.Create(y, m);
        // Act
        var actual = year.EnumerateMonths();
        // Assert
        Assert.Equal(exp, actual);
    }

    //[Theory, MemberData(nameof(MonthInfoData))]
    //public void GetMonthOfYear(MonthInfo info)
    //{
    //    var (y, m) = info.Yemo;
    //    var year = TYear.Create(y);
    //    var month = TMonth.Create(y, m);
    //    // Act & Assert
    //    Assert.Equal(month, year.GetMonthOfYear(m));
    //}
}

public partial class IYearFacts<TYear, TMonth, TDate, TDataSet> // IMonthSegment
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDays(YearInfo info)
    {
        var year = TYear.Create(info.Year);
        // Act & Assert
        Assert.Equal(info.DaysInYear, year.CountDays());
    }

    [Theory, MemberData(nameof(YearInfoData))]
    public void EnumerateDays(YearInfo info)
    {
        int y = info.Year;
        var year = TYear.Create(y);
        var exp = from doy in Enumerable.Range(1, info.DaysInYear)
                  select TDate.Create(y, doy);
        // Act
        var actual = year.EnumerateDays();
        // Assert
        Assert.Equal(exp, actual);
    }

    //[Theory, MemberData(nameof(DateInfoData))]
    //public void GetDayOfYear(DateInfo info)
    //{
    //    var (y, doy) = info.Yedoy;
    //    var year = TYear.Create(y);
    //    var date = TDate.Create(y, doy);
    //    // Act & Assert
    //    Assert.Equal(date, year.GetDayOfYear(doy));
    //}
}

