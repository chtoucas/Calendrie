﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Testing.Data;

// Sync with ICalendricalPartsFactoryFacts.

/// <summary>
/// Provides facts about <see cref="LimitSchema"/>.
/// </summary>
public abstract partial class LimitSchemaFacts<TDataSet> :
    ICalendricalSchemaFacts<LimitSchema, TDataSet>
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected LimitSchemaFacts(LimitSchema schema) : base(schema) { }

    // This property is actually part of CalendricalSchema but being internal
    // it's not publicly testable.
    [Fact] public abstract void Profile_Prop();
}

public partial class LimitSchemaFacts<TDataSet> // Properties
{
    [Fact]
    public sealed override void Algorithm_Prop() =>
        Assert.Equal(CalendricalAlgorithm.Arithmetical, SchemaUT.Algorithm);

    [Fact]
    public override void SupportedYears_Prop() =>
        Assert.Equal(LimitSchema.DefaultSupportedYears, SchemaUT.SupportedYears);

    [Fact]
    public virtual void SupportedYearsCore_Prop() =>
        Assert.Equal(Range.Maximal32, SchemaUT.SupportedYearsCore);
}

public partial class LimitSchemaFacts<TDataSet> // Methods
{
    #region GetMonthParts()

    [Theory, MemberData(nameof(MonthsSinceEpochInfoData))]
    public void GetMonthParts﹍MonthsSinceEpoch(MonthsSinceEpochInfo info)
    {
        // Act
        var actual = SchemaUT.GetMonthParts(info.MonthsSinceEpoch);
        // Assert
        Assert.Equal(info.Yemo, actual);
    }

    #endregion

    #region GetDateParts﹍DaysSinceEpoch()

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetDateParts﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        // Act
        var actual = SchemaUT.GetDateParts(info.DaysSinceEpoch);
        // Assert
        Assert.Equal(info.Yemoda, actual);
    }

    #endregion
    #region GetDateParts﹍OrdinalParts()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDateParts﹍OrdinalParts(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        // Act
        var actual = SchemaUT.GetDateParts(y, doy);
        // Assert
        Assert.Equal(info.Yemoda, actual);
    }

    #endregion
    #region GetDateParts﹍DateParts()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDateParts﹍DateParts(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var actual = SchemaUT.GetDateParts(y, m, d);
        // Assert
        Assert.Equal(info.Yemoda, actual);
    }

    #endregion

    #region GetOrdinalParts﹍DaysSinceEpoch()

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetOrdinalParts﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        int doy = SchemaUT.GetDayOfYear(y, m, d);
        var ydoy = new Yedoy(y, doy);
        // Act
        var actual = SchemaUT.GetOrdinalParts(daysSinceEpoch);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalParts﹍DaysSinceEpoch_DateParts(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        var actual = SchemaUT.GetOrdinalParts(daysSinceEpoch);
        // Assert
        Assert.Equal(info.Yedoy, actual);
    }

    #endregion
    #region GetOrdinalParts﹍DateParts()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetOrdinalParts﹍DateParts(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        var actual = SchemaUT.GetOrdinalParts(y, m, d);
        // Assert
        Assert.Equal(info.Yedoy, actual);
    }

    #endregion

    #region GetMonthPartsAtStartOfYear()

    [Fact]
    public void GetMonthPartsAtStartOfYear_AtYear1()
    {
        var ym = new Yemo(1, 1);
        // Act
        var actual = SchemaUT.GetMonthPartsAtStartOfYear(1);
        // Assert
        Assert.Equal(ym, actual);
    }

    [Theory, MemberData(nameof(StartOfYearPartsData))]
    public void GetMonthPartsAtStartOfYear(Yemoda ymd)
    {
        // Act
        var actual = SchemaUT.GetMonthPartsAtStartOfYear(ymd.Year);
        // Assert
        Assert.Equal(ymd.Yemo, actual);
    }

    #endregion
    #region GetDatePartsAtStartOfYear()

    [Fact]
    public void GetDatePartsAtStartOfYear_AtYear1()
    {
        var ymd = new Yemoda(1, 1, 1);
        // Act
        var actual = SchemaUT.GetDatePartsAtStartOfYear(1);
        // Assert
        Assert.Equal(ymd, actual);
    }

    [Theory, MemberData(nameof(StartOfYearPartsData))]
    public void GetDatePartsAtStartOfYear(Yemoda ymd)
    {
        // Act
        var actual = SchemaUT.GetDatePartsAtStartOfYear(ymd.Year);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetOrdinalPartsAtStartOfYear()

    [Fact]
    public void GetOrdinalPartsAtStartOfYear_AtYear1()
    {
        var ydoy = new Yedoy(1, 1);
        // Act
        var actual = SchemaUT.GetOrdinalPartsAtStartOfYear(1);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    [Theory, MemberData(nameof(StartOfYearDaysSinceEpochData))]
    public void GetOrdinalPartsAtStartOfYear(YearDaysSinceEpoch info)
    {
        int y = info.Year;
        var ydoy = new Yedoy(y, 1);
        // Act
        var actual = SchemaUT.GetOrdinalPartsAtStartOfYear(y);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion

    #region GetMonthPartsAtEndOfYear()

    [Theory, MemberData(nameof(EndOfYearPartsData))]
    public void GetMonthPartsAtEndOfYear(Yemoda ymd)
    {
        // Act
        var actual = SchemaUT.GetMonthPartsAtEndOfYear(ymd.Year);
        // Assert
        Assert.Equal(ymd.Yemo, actual);
    }

    #endregion
    #region GetDatePartsAtEndOfYear()

    [Theory, MemberData(nameof(EndOfYearPartsData))]
    public void GetDatePartsAtEndOfYear(Yemoda ymd)
    {
        // Act
        var actual = SchemaUT.GetDatePartsAtEndOfYear(ymd.Year);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetOrdinalPartsAtEndOfYear()

    [Theory, MemberData(nameof(YearInfoData))]
    public void GetOrdinalPartsAtEndOfYear(YearInfo info)
    {
        int y = info.Year;
        var ydoy = new Yedoy(y, info.DaysInYear);
        // Act
        var actual = SchemaUT.GetOrdinalPartsAtEndOfYear(y);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion

    #region GetDatePartsAtStartOfMonth()

    [Fact]
    public void GetDatePartsAtStartOfMonth_AtFirstMonthOfYear1()
    {
        var ymd = new Yemoda(1, 1, 1);
        // Act
        var actual = SchemaUT.GetDatePartsAtStartOfMonth(1, 1);
        // Assert
        Assert.Equal(ymd, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDatePartsAtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ymd = new Yemoda(y, m, 1);
        // Act
        var actual = SchemaUT.GetDatePartsAtStartOfMonth(y, m);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetOrdinalPartsAtStartOfMonth()

    [Fact]
    public void GetOrdinalPartsAtStartOfMonth_AtFirstMonthOfYear1()
    {
        var ydoy = new Yedoy(1, 1);
        // Act
        var actual = SchemaUT.GetOrdinalPartsAtStartOfMonth(1, 1);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetOrdinalPartsAtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ydoy = new Yedoy(y, info.DaysInYearBeforeMonth + 1);
        // Act
        var actual = SchemaUT.GetOrdinalPartsAtStartOfMonth(y, m);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion

    #region GetDatePartsAtEndOfMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetDatePartsAtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ymd = new Yemoda(y, m, info.DaysInMonth);
        // Act
        var actual = SchemaUT.GetDatePartsAtEndOfMonth(y, m);
        // Assert
        Assert.Equal(ymd, actual);
    }

    #endregion
    #region GetOrdinalPartsAtEndOfMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetOrdinalPartsAtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        var ydoy = new Yedoy(y, info.DaysInYearBeforeMonth + info.DaysInMonth);
        // Act
        var actual = SchemaUT.GetOrdinalPartsAtEndOfMonth(y, m);
        // Assert
        Assert.Equal(ydoy, actual);
    }

    #endregion
}

public partial class LimitSchemaFacts<TDataSet> // Overflows
{
    [Fact]
    public sealed override void KernelDoesNotOverflow()
    {
        var (minYearCore, maxYearCore) = SchemaUT.SupportedYearsCore.Endpoints;

        _ = SchemaUT.IsLeapYear(minYearCore);
        _ = SchemaUT.IsLeapYear(maxYearCore);

        // NB: right now, it works w/ Int32.Min(Max)Year but it might change
        // in the future with new lunisolar schemas.
        _ = SchemaUT.CountMonthsInYear(int.MinValue);
        _ = SchemaUT.CountMonthsInYear(int.MaxValue);

        _ = SchemaUT.CountDaysInYear(minYearCore);
        _ = SchemaUT.CountDaysInYear(maxYearCore);

        for (int m = 1; m <= MaxMonth; m++)
        {
            _ = SchemaUT.IsIntercalaryMonth(int.MinValue, m);
            _ = SchemaUT.IsIntercalaryMonth(int.MaxValue, m);

            _ = SchemaUT.CountDaysInMonth(minYearCore, m);
            _ = SchemaUT.CountDaysInMonth(maxYearCore, m);

            for (int d = 1; d <= MaxDay; d++)
            {
                _ = SchemaUT.IsIntercalaryDay(int.MinValue, m, d);
                _ = SchemaUT.IsIntercalaryDay(int.MaxValue, m, d);

                _ = SchemaUT.IsSupplementaryDay(int.MinValue, m, d);
                _ = SchemaUT.IsSupplementaryDay(int.MaxValue, m, d);
            }
        }

        if (minYearCore != int.MinValue)
        {
            AssertEx.Overflows(() => SchemaUT.IsLeapYear(int.MinValue));
            AssertEx.Overflows(() => SchemaUT.CountDaysInYear(int.MinValue));
        }
        if (maxYearCore != int.MaxValue)
        {
            AssertEx.Overflows(() => SchemaUT.IsLeapYear(int.MaxValue));
            AssertEx.Overflows(() => SchemaUT.CountDaysInYear(int.MaxValue));
        }
    }

    [Fact] public void GetYear﹍Plain_DoesNotUnderflow() => _ = SchemaUT.GetYear(MinDaysSinceEpoch);
    [Fact] public void GetYear﹍Plain_DoesNotOverflow() => _ = SchemaUT.GetYear(MaxDaysSinceEpoch);

    [Fact] public void GetDateParts﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.GetDateParts(MinDaysSinceEpoch);
    [Fact] public void GetDateParts﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.GetDateParts(MaxDaysSinceEpoch);

    [Fact] public void GetDateParts﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.GetDateParts(MinYear, 1);
    // Just for this test, we choose a custom value for MaxDayOfYear.
    [Fact] public void GetDateParts﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.GetDateParts(MaxYear, 400);

    [Fact] public void GetOrdinalParts﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.GetOrdinalParts(MinDaysSinceEpoch);
    [Fact] public void GetOrdinalParts﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.GetOrdinalParts(MaxDaysSinceEpoch);

    [Fact] public void GetOrdinalParts﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.GetOrdinalParts(MinYear, 1, 1);
    [Fact] public void GetOrdinalParts﹍DateParts_DoesNotOverflow() => _ = SchemaUT.GetOrdinalParts(MaxYear, MaxMonth, MaxDay);

    [Fact] public void GetDatePartsAtStartOfYear_DoesNotUnderflow() => _ = SchemaUT.GetDatePartsAtStartOfYear(MinYear);
    [Fact] public void GetDatePartsAtStartOfYear_DoesNotOverflow() => _ = SchemaUT.GetDatePartsAtStartOfYear(MaxYear);
    [Fact] public void GetOrdinalPartsAtStartOfYear_DoesNotUnderflow() => _ = SchemaUT.GetOrdinalPartsAtStartOfYear(MinYear);
    [Fact] public void GetOrdinalPartsAtStartOfYear_DoesNotOverflow() => _ = SchemaUT.GetOrdinalPartsAtStartOfYear(MaxYear);

    [Fact] public void GetDatePartsAtEndOfYear_DoesNotUnderflow() => _ = SchemaUT.GetDatePartsAtEndOfYear(MinYear);
    [Fact] public void GetDatePartsAtEndOfYear_DoesNotOverflow() => _ = SchemaUT.GetDatePartsAtEndOfYear(MaxYear);
    [Fact] public void GetOrdinalPartsAtEndOfYear_DoesNotUnderflow() => _ = SchemaUT.GetOrdinalPartsAtEndOfYear(MinYear);
    [Fact] public void GetOrdinalPartsAtEndOfYear_DoesNotOverflow() => _ = SchemaUT.GetOrdinalPartsAtEndOfYear(MaxYear);

    [Fact] public void GetDatePartsAtStartOfMonth_DoesNotUnderflow() => _ = SchemaUT.GetDatePartsAtStartOfMonth(MinYear, 1);
    [Fact] public void GetDatePartsAtStartOfMonth_DoesNotOverflow() => _ = SchemaUT.GetDatePartsAtStartOfMonth(MaxYear, MaxMonth);
    [Fact] public void GetOrdinalPartsAtStartOfMonth_DoesNotUnderflow() => _ = SchemaUT.GetOrdinalPartsAtStartOfMonth(MinYear, 1);
    [Fact] public void GetOrdinalPartsAtStartOfMonth_DoesNotOverflow() => _ = SchemaUT.GetOrdinalPartsAtStartOfMonth(MaxYear, MaxMonth);

    [Fact] public void GetDatePartsAtEndOfMonth_DoesNotUnderflow() => _ = SchemaUT.GetDatePartsAtEndOfMonth(MinYear, 1);
    [Fact] public void GetDatePartsAtEndOfMonth_DoesNotOverflow() => _ = SchemaUT.GetDatePartsAtEndOfMonth(MaxYear, MaxMonth);
    [Fact] public void GetOrdinalPartsAtEndOfMonth_DoesNotUnderflow() => _ = SchemaUT.GetOrdinalPartsAtEndOfMonth(MinYear, 1);
    [Fact] public void GetOrdinalPartsAtEndOfMonth_DoesNotOverflow() => _ = SchemaUT.GetOrdinalPartsAtEndOfMonth(MaxYear, MaxMonth);
}
