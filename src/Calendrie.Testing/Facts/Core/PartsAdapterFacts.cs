﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Testing.Data;

// Sync with LimitSchemaFacts.

/// <summary>
/// Provides facts about the <see cref="PartsAdapter"/> type.
/// </summary>
public partial class PartsAdapterFacts<TDataSet> :
    CalendricalDataConsumer<TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected PartsAdapterFacts(PartsAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);

        AdapterUT = adapter;
    }

    /// <summary>
    /// Gets the adapter under test.
    /// </summary>
    protected PartsAdapter AdapterUT { get; }
}

public partial class PartsAdapterFacts<TDataSet> // Conversions
{
    #region GetMonthParts()

    [Theory, MemberData(nameof(MonthsSinceEpochInfoData))]
    public void GetMonthParts﹍MonthsSinceEpoch(MonthsSinceEpochInfo info)
    {
        var (monthsSinceEpoch, y, m) = info;
        var parts = new MonthParts(y, m);
        // Act
        var actual = AdapterUT.GetMonthParts(monthsSinceEpoch);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion
    #region GetDateParts﹍DaysSinceEpoch()

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetDateParts﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        var parts = new DateParts(y, m, d);
        // Act
        var actual = AdapterUT.GetDateParts(daysSinceEpoch);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion
    #region GetOrdinalParts﹍DaysSinceEpoch()

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetOrdinalParts﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        var parts = AdapterUT.GetOrdinalParts(y, m, d);
        // Act
        var actual = AdapterUT.GetOrdinalParts(daysSinceEpoch);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion
    #region GetOrdinalParts﹍DateParts()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalParts﹍DateParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var parts = new OrdinalParts(y, doy);
        // Act
        var actual = AdapterUT.GetOrdinalParts(y, m, d);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion
    #region GetDateParts﹍OrdinalParts()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDateParts﹍OrdinalParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        var parts = new DateParts(y, m, d);
        // Act
        var actual = AdapterUT.GetDateParts(y, doy);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion
}

public partial class PartsAdapterFacts<TDataSet> // Dates in a given year or month
{
#if DEBUG
    // No need to be fancy here, these methods delegate to MonthParts & co
    // which are tested separately and only in mode DEBUG.

    [Fact]
    public void GetMonthPartsAtStartOfYear() =>
        Assert.Equal(new MonthParts(3, 1), PartsAdapter.GetMonthPartsAtStartOfYear(3));

    [Fact]
    public void GetDatePartsAtStartOfYear() =>
        Assert.Equal(new DateParts(3, 1, 1), PartsAdapter.GetDatePartsAtStartOfYear(3));

    [Fact]
    public void GetOrdinalPartsAtStartOfYear() =>
        Assert.Equal(new OrdinalParts(3, 1), PartsAdapter.GetOrdinalPartsAtStartOfYear(3));

    [Fact]
    public void GetDatePartsAtStartOfMonth() =>
        Assert.Equal(new DateParts(3, 2, 1), PartsAdapter.GetDatePartsAtStartOfMonth(3, 2));

#endif

    #region GetMonthPartsAtEndOfYear()

    [Theory, MemberData(nameof(EndOfYearPartsData))]
    public void GetMonthPartsAtEndOfYear(Yemoda ymd)
    {
        var (y, m) = ymd.Yemo;
        var parts = new MonthParts(y, m);
        // Act
        var actual = AdapterUT.GetMonthPartsAtEndOfYear(y);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion
    #region GetDatePartsAtEndOfYear()

    [Theory, MemberData(nameof(EndOfYearPartsData))]
    public void GetDatePartsAtEndOfYear(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        var parts = new DateParts(y, m, d);
        // Act
        var actual = AdapterUT.GetDatePartsAtEndOfYear(y);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion
    #region GetOrdinalPartsAtEndOfYear()

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetOrdinalPartsAtEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var parts = new OrdinalParts(y, info.DaysInYear);
        // Act
        var actual = AdapterUT.GetOrdinalPartsAtEndOfYear(y);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion

    #region GetOrdinalPartsAtStartOfMonth()

    [Fact]
    public void GetOrdinalPartsAtStartOfMonth_AtFirstMonthOfYear1()
    {
        var parts = new OrdinalParts(1, 1);
        // Act
        var actual = AdapterUT.GetOrdinalPartsAtStartOfMonth(1, 1);
        // Assert
        Assert.Equal(parts, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetOrdinalPartsAtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var parts = new OrdinalParts(y, info.DaysInYearBeforeMonth + 1);
        // Act
        var actual = AdapterUT.GetOrdinalPartsAtStartOfMonth(y, m);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion

    #region GetDatePartsAtEndOfMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDatePartsAtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var parts = new DateParts(y, m, info.DaysInMonth);
        // Act
        var actual = AdapterUT.GetDatePartsAtEndOfMonth(y, m);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion
    #region GetOrdinalPartsAtEndOfMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetOrdinalPartsAtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var parts = new OrdinalParts(y, info.DaysInYearBeforeMonth + info.DaysInMonth);
        // Act
        var actual = AdapterUT.GetOrdinalPartsAtEndOfMonth(y, m);
        // Assert
        Assert.Equal(parts, actual);
    }

    #endregion
}
