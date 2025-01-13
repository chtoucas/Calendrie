// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about the <see cref="ICalendricalSchema"/> type.
/// </summary>
public abstract partial class ICalendricalSchemaFacts<TSchema, TDataSet> :
    ICalendricalCoreFacts<TSchema, TDataSet>
    where TSchema : ICalendricalSchema
    where TDataSet : ICalendricalDataSet, ISingleton<TDataSet>
{
    protected ICalendricalSchemaFacts(TSchema schema) : base(schema)
    {
        (MinYear, MaxYear) = schema.SupportedYears.Endpoints;

        MinDaysSinceEpoch = schema.GetStartOfYear(MinYear);
        MaxDaysSinceEpoch = schema.GetEndOfYear(MaxYear);

        MinMonthsSinceEpoch = schema.GetStartOfYearInMonths(MinYear);
        MaxMonthsSinceEpoch = schema.GetEndOfYearInMonths(MaxYear);
    }

    /// <summary>
    /// Gets a minimum value of the year for which the methods are known not to overflow.
    /// </summary>
    protected int MinYear { get; }

    /// <summary>
    /// Gets a maximum value of the year for which the methods are known not to overflow.
    /// </summary>
    protected int MaxYear { get; }

    protected int MaxDay { get; init; } = Yemoda.MaxDay;
    protected int MaxMonth { get; init; } = Yemoda.MaxMonth;
    protected int MaxDayOfYear { get; init; } = Yedoy.MaxDayOfYear;

    protected int MinDaysSinceEpoch { get; }
    protected int MaxDaysSinceEpoch { get; }

    protected int MinMonthsSinceEpoch { get; }
    protected int MaxMonthsSinceEpoch { get; }

    [Fact] public abstract void PreValidator_Prop();

    [Fact] public abstract void SupportedYears_Prop();

    protected void VerifyThatPreValidatorIs<T>() => Assert.IsType<T>(SchemaUT.PreValidator);
}

public partial class ICalendricalSchemaFacts<TSchema, TDataSet> // Properties
{
    [Theory, MemberData(nameof(YearInfoData))]
    public void MinDaysInYear_Prop_IsLessThanOrEqualTo_DaysInYear(YearInfo info) =>
        Assert.True(SchemaUT.MinDaysInYear <= info.DaysInYear);

    [Theory, MemberData(nameof(MonthInfoData))]
    public void MinDaysInMonth_Prop_IsLessThanOrEqualTo_DaysInMonth(MonthInfo info) =>
        Assert.True(SchemaUT.MinDaysInMonth <= info.DaysInMonth);

    [Fact]
    public void SupportedDays_Prop()
    {
        var supportedDays = new Range<int>(MinDaysSinceEpoch, MaxDaysSinceEpoch);
        // Act & Assert
        Assert.Equal(supportedDays, SchemaUT.SupportedDays);
        // Lazy prop: we duplicate the test to ensure full test coverage.
        Assert.Equal(supportedDays, SchemaUT.SupportedDays);
    }

    [Fact]
    public void SupportedMonths_Prop()
    {
        var domain = new Range<int>(MinMonthsSinceEpoch, MaxMonthsSinceEpoch);
        // Act & Assert
        Assert.Equal(domain, SchemaUT.SupportedMonths);
        // Lazy prop: we duplicate the test to ensure full test coverage.
        Assert.Equal(domain, SchemaUT.SupportedMonths);
    }
}

public partial class ICalendricalSchemaFacts<TSchema, TDataSet> // Methods
{
    #region CountDaysInYearBeforeMonth()

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBeforeMonth_AtStartOfYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.CountDaysInYearBeforeMonth(info.Year, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBeforeMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = SchemaUT.CountDaysInYearBeforeMonth(y, m);
        // Assert
        Assert.Equal(info.DaysInYearBeforeMonth, actual);
    }

    #endregion

    #region CountMonthsSinceEpoch()

    [Fact]
    public void CountMonthsSinceEpoch_AtEpoch_IsZero()
    {
        // Act
        int actual = SchemaUT.CountMonthsSinceEpoch(1, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(MonthsSinceEpochInfoData))]
    public void CountMonthsSinceEpoch(MonthsSinceEpochInfo info)
    {
        var (monthsSinceEpoch, y, m) = info;
        // Act
        int actual = SchemaUT.CountMonthsSinceEpoch(y, m);
        // Assert
        Assert.Equal(monthsSinceEpoch, actual);
    }

    #endregion
    #region CountDaysSinceEpoch﹍DateParts()

    [Fact]
    public void CountDaysSinceEpoch﹍DateParts_AtEpoch_IsZero()
    {
        // Act
        int actual = SchemaUT.CountDaysSinceEpoch(1, 1, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysSinceEpoch﹍DateParts(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        // Act
        int actual = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Assert
        Assert.Equal(daysSinceEpoch, actual);
    }

    #endregion
    #region CountDaysSinceEpoch﹍OrdinalParts()

    [Fact]
    public void CountDaysSinceEpoch﹍OrdinalParts_AtEpoch_IsZero()
    {
        // Act
        int actual = SchemaUT.CountDaysSinceEpoch(1, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysSinceEpoch﹍OrdinalParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int actual = SchemaUT.CountDaysSinceEpoch(y, doy);
        // Assert
        Assert.Equal(daysSinceEpoch, actual);
    }

    #endregion
    #region GetMonthParts()

    [Theory, MemberData(nameof(MonthsSinceEpochInfoData))]
    public void GetMonthParts(MonthsSinceEpochInfo info)
    {
        var (monthsSinceEpoch, y, m) = info;
        // Act
        SchemaUT.GetMonthParts(monthsSinceEpoch, out int yA, out int mA);
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(m, mA);
    }

    #endregion
    #region GetDateParts()

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetDateParts(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        // Act
        SchemaUT.GetDateParts(daysSinceEpoch, out int yA, out int mA, out int dA);
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(m, mA);
        Assert.Equal(d, dA);
    }

    #endregion
    #region GetYear()

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(DateInfoData))]
    public void GetYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int yA = SchemaUT.GetYear(daysSinceEpoch, out int doyA);
        // Assert
        Assert.Equal(y, yA);
        Assert.Equal(doy, doyA);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetYear_DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        // Act
        int yA = SchemaUT.GetYear(info.DaysSinceEpoch, out _);
        // Assert
        Assert.Equal(info.Yemoda.Year, yA);
    }

    #endregion
    #region GetYear()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetYear﹍Plain(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int yA = SchemaUT.GetYear(daysSinceEpoch);
        // Assert
        Assert.Equal(y, yA);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void GetYear﹍Plain_DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        // Act
        int yA = SchemaUT.GetYear(info.DaysSinceEpoch);
        // Assert
        Assert.Equal(info.Yemoda.Year, yA);
    }

    #endregion
    #region GetMonth()

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetMonth(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        int mA = SchemaUT.GetMonth(y, doy, out int dA);
        // Assert
        Assert.Equal(m, mA);
        Assert.Equal(d, dA);
    }

    #endregion
    #region GetDayOfYear()

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetDayOfYear_AtStartOfYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.GetDayOfYear(info.Year, 1, 1);
        // Assert
        Assert.Equal(1, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void GetDayOfYear(DateInfo info)
    {
        var (y, m, d, doy) = info;
        // Act
        int actual = SchemaUT.GetDayOfYear(y, m, d);
        // Assert
        Assert.Equal(doy, actual);
    }

    #endregion

    #region GetStartOfYearInMonths()

    [Fact]
    public void GetStartOfYearInMonths_AtYear1_IsZero()
    {
        // Act
        int actual = SchemaUT.GetStartOfYearInMonths(1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(StartOfYearMonthsSinceEpochData))]
    public void GetStartOfYearInMonths(YearMonthsSinceEpoch info)
    {
        // Act
        int actual = SchemaUT.GetStartOfYearInMonths(info.Year);
        // Assert
        Assert.Equal(info.MonthsSinceEpoch, actual);
    }

    #endregion
    #region GetEndOfYearInMonths()

    [Theory, MemberData(nameof(EndOfYearMonthsSinceEpochData))]
    public void GetEndOfYearInMonths(YearMonthsSinceEpoch info)
    {
        // Act
        int actual = SchemaUT.GetEndOfYearInMonths(info.Year);
        // Assert
        Assert.Equal(info.MonthsSinceEpoch, actual);
    }

    #endregion
    #region GetStartOfYear()

    [Fact]
    public void GetStartOfYear_AtYear1_IsZero()
    {
        // Act
        int actual = SchemaUT.GetStartOfYear(1);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(StartOfYearDaysSinceEpochData))]
    public void GetStartOfYear(YearDaysSinceEpoch info)
    {
        // Act
        int actual = SchemaUT.GetStartOfYear(info.Year);
        // Assert
        Assert.Equal(info.DaysSinceEpoch, actual);
    }

    #endregion
    #region GetEndOfYear()

    [Theory, MemberData(nameof(EndOfYearDaysSinceEpochData))]
    public void GetEndOfYear(YearDaysSinceEpoch info)
    {
        // Act
        int actual = SchemaUT.GetEndOfYear(info.Year);
        // Assert
        Assert.Equal(info.DaysSinceEpoch, actual);
    }

    // When we don't have any test data, we check that the method returns a
    // result in agreement with what we know.

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(EndOfYearPartsData))]
    public void GetEndOfYear_CountDaysSinceEpoch(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        int endOfYear = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int actual = SchemaUT.GetEndOfYear(y);
        // Assert
        Assert.Equal(endOfYear, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfYear_EndOfYear_Equals_StartOfNextYearMinusOne(YearInfo info)
    {
        int y = info.Year;
        if (y >= MaxYear) { return; }
        int startOfNextYearMinusOne = SchemaUT.GetStartOfYear(y + 1) - 1;
        // Act
        int endOfYear = SchemaUT.GetEndOfYear(y);
        // Assert
        Assert.Equal(startOfNextYearMinusOne, endOfYear);
    }

    #endregion
    #region GetStartOfMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int startOfMonth = SchemaUT.GetStartOfYear(y) + info.DaysInYearBeforeMonth;
        // Act
        int actual = SchemaUT.GetStartOfMonth(y, m);
        // Assert
        Assert.Equal(startOfMonth, actual);
    }

    [Fact]
    public void GetStartOfMonth_AtFirstMonthOfYear1_IsZero()
    {
        // Act
        int actual = SchemaUT.GetStartOfMonth(1, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    #endregion
    #region GetEndOfMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void GetEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int endOfMonth = SchemaUT.GetStartOfYear(y) + info.DaysInYearBeforeMonth + info.DaysInMonth - 1;
        // Act
        int actual = SchemaUT.GetEndOfMonth(y, m);
        // Assert
        Assert.Equal(endOfMonth, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void GetEndOfMonth_EndOfMonth_Equals_StartOfNextMonthMinusOne(YearInfo info)
    {
        int y = info.Year;
        int monthsInYear = SchemaUT.CountMonthsInYear(y);
        for (int m = 1; m < monthsInYear; m++)
        {
            int startOfNextMonthMinusOne = SchemaUT.CountDaysSinceEpoch(y, m + 1, 1) - 1;
            // Act
            int endOfMonth = SchemaUT.GetEndOfMonth(y, m);
            // Assert
            Assert.Equal(startOfNextMonthMinusOne, endOfMonth);
        }
    }

    #endregion
}

public partial class ICalendricalSchemaFacts<TSchema, TDataSet> // Overflows (Kernel)
{
    [Fact]
    public virtual void KernelDoesNotOverflow()
    {
        _ = SchemaUT.IsLeapYear(int.MinValue);
        _ = SchemaUT.IsLeapYear(int.MaxValue);

        _ = SchemaUT.CountMonthsInYear(int.MinValue);
        _ = SchemaUT.CountMonthsInYear(int.MaxValue);

        _ = SchemaUT.CountDaysInYear(int.MinValue);
        _ = SchemaUT.CountDaysInYear(int.MaxValue);

        for (int m = 1; m <= MaxMonth; m++)
        {
            _ = SchemaUT.IsIntercalaryMonth(int.MinValue, m);
            _ = SchemaUT.IsIntercalaryMonth(int.MaxValue, m);

            _ = SchemaUT.CountDaysInMonth(int.MinValue, m);
            _ = SchemaUT.CountDaysInMonth(int.MaxValue, m);

            for (int d = 1; d <= MaxDay; d++)
            {
                _ = SchemaUT.IsIntercalaryDay(int.MinValue, m, d);
                _ = SchemaUT.IsIntercalaryDay(int.MaxValue, m, d);

                _ = SchemaUT.IsSupplementaryDay(int.MinValue, m, d);
                _ = SchemaUT.IsSupplementaryDay(int.MaxValue, m, d);
            }
        }
    }

    // Virtual methods: for those schemas that use an array for CountDaysInMonth() & co

    [Fact] public void IsLeapYear_DoesNotUnderflow() => _ = SchemaUT.IsLeapYear(MinYear);
    [Fact] public void IsLeapYear_DoesNotOverflow() => _ = SchemaUT.IsLeapYear(MaxYear);

    [Fact] public void IsIntercalaryMonth_DoesNotUnderflow() => _ = SchemaUT.IsIntercalaryMonth(MinYear, 1);
    [Fact] public void IsIntercalaryMonth_DoesNotOverflow() => _ = SchemaUT.IsIntercalaryMonth(MaxYear, MaxMonth);

    [Fact] public void IsIntercalaryDay_DoesNotUnderflow() => _ = SchemaUT.IsIntercalaryDay(MinYear, 1, 1);
    [Fact] public void IsIntercalaryDay_DoesNotOverflow() => _ = SchemaUT.IsIntercalaryDay(MaxYear, MaxMonth, MaxDay);

    [Fact] public void IsSupplementaryDay_DoesNotUnderflow() => _ = SchemaUT.IsSupplementaryDay(MinYear, 1, 1);
    [Fact] public void IsSupplementaryDay_DoesNotOverflow() => _ = SchemaUT.IsSupplementaryDay(MaxYear, MaxMonth, MaxDay);

    [Fact] public void CountMonthsInYear_DoesNotUnderflow() => _ = SchemaUT.CountMonthsInYear(MinYear);
    [Fact] public void CountMonthsInYear_DoesNotOverflow() => _ = SchemaUT.CountMonthsInYear(MaxYear);

    [Fact] public void CountDaysInYear_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYear(MinYear);
    [Fact] public void CountDaysInYear_DoesNotOverflow() => _ = SchemaUT.CountDaysInYear(MaxYear);

    [Fact] public void CountDaysInMonth_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonth(MinYear, 1);
    [Fact] public virtual void CountDaysInMonth_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonth(MaxYear, MaxMonth);
}

public partial class ICalendricalSchemaFacts<TSchema, TDataSet> // Overflows
{
    [Fact] public void CountDaysInYearBeforeMonth_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearBeforeMonth(MinYear, 1);
    [Fact] public virtual void CountDaysInYearBeforeMonth_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearBeforeMonth(MaxYear, MaxMonth);

    [Fact] public void CountDaysSinceEpoch﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysSinceEpoch(MinYear, 1, 1);
    [Fact] public virtual void CountDaysSinceEpoch﹍DateParts_DoesNotOverflow() => _ = SchemaUT.CountDaysSinceEpoch(MaxYear, MaxMonth, MaxDay);

    [Fact] public void CountDaysSinceEpoch﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysSinceEpoch(MinYear, 1);
    [Fact] public void CountDaysSinceEpoch﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.CountDaysSinceEpoch(MaxYear, MaxDayOfYear);

    [Fact] public void GetDateParts_DoesNotUnderflow() => SchemaUT.GetDateParts(MinDaysSinceEpoch, out _, out _, out _);
    [Fact] public void GetDateParts_DoesNotOverflow() => SchemaUT.GetDateParts(MaxDaysSinceEpoch, out _, out _, out _);

    [Fact] public void GetYear_DoesNotUnderflow() => _ = SchemaUT.GetYear(MinDaysSinceEpoch, out _);
    [Fact] public void GetYear_DoesNotOverflow() => _ = SchemaUT.GetYear(MaxDaysSinceEpoch, out _);

    [Fact] public void GetMonth_DoesNotUnderflow() => _ = SchemaUT.GetMonth(MinYear, 1, out _);
    [Fact] public void GetMonth_DoesNotOverflow() => _ = SchemaUT.GetMonth(MaxYear, MaxDayOfYear, out _);

    [Fact] public void GetDayOfYear_DoesNotUnderflow() => _ = SchemaUT.GetDayOfYear(MinYear, 1, 1);
    [Fact] public virtual void GetDayOfYear_DoesNotOverflow() => _ = SchemaUT.GetDayOfYear(MaxYear, MaxMonth, MaxDay);

    [Fact] public void GetStartOfYearInMonths_DoesNotUnderflow() => _ = SchemaUT.GetStartOfYearInMonths(MinYear);
    [Fact] public void GetStartOfYearInMonths_DoesNotOverflow() => _ = SchemaUT.GetStartOfYearInMonths(MaxYear);

    [Fact] public void GetEndOfYearInMonths_DoesNotUnderflow() => _ = SchemaUT.GetEndOfYearInMonths(MinYear);
    [Fact] public void GetEndOfYearInMonths_DoesNotOverflow() => _ = SchemaUT.GetEndOfYearInMonths(MaxYear);

    [Fact] public void GetStartOfYear_DoesNotUnderflow() => _ = SchemaUT.GetStartOfYear(MinYear);
    [Fact] public void GetStartOfYear_DoesNotOverflow() => _ = SchemaUT.GetStartOfYear(MaxYear);

    [Fact] public void GetEndOfYear_DoesNotUnderflow() => _ = SchemaUT.GetEndOfYear(MinYear);
    [Fact] public void GetEndOfYear_DoesNotOverflow() => _ = SchemaUT.GetEndOfYear(MaxYear);

    [Fact] public void GetStartOfMonth_DoesNotUnderflow() => _ = SchemaUT.GetStartOfMonth(MinYear, 1);
    [Fact] public virtual void GetStartOfMonth_DoesNotOverflow() => _ = SchemaUT.GetStartOfMonth(MaxYear, MaxMonth);

    [Fact] public void GetEndOfMonth_DoesNotUnderflow() => _ = SchemaUT.GetEndOfMonth(MinYear, 1);
    [Fact] public virtual void GetEndOfMonth_DoesNotOverflow() => _ = SchemaUT.GetEndOfMonth(MaxYear, MaxMonth);
}

public partial class ICalendricalSchemaFacts<TSchema, TDataSet> // Methods
{
    #region CountDaysInYearAfterMonth()

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearAfterMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = SchemaUT.CountDaysInYearAfterMonth(y, m);
        // Assert
        Assert.Equal(info.DaysInYearAfterMonth, actual);
    }

    #endregion

    #region CountDaysInYearBefore()

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBefore﹍DateParts_AtStartOfYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(info.Year, 1, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBefore﹍OrdinalParts_AtStartOfYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(info.Year, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBefore﹍DaysSinceEpoch_AtStartOfYear(YearInfo info)
    {
        int daysSinceEpoch = SchemaUT.GetStartOfYear(info.Year);
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(0, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBefore﹍OrdinalParts_AtEndOfYear(YearInfo info)
    {
        int doy = info.DaysInYear;
        int daysInYearBefore = doy - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(info.Year, doy);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearBefore﹍DaysSinceEpoch_AtEndOfYear(YearInfo info)
    {
        int daysSinceEpoch = SchemaUT.GetEndOfYear(info.Year);
        int daysInYearBefore = info.DaysInYear - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBefore﹍DateParts_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysInYearBefore = info.DaysInYearBeforeMonth;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(y, m, 1);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBefore﹍DaysSinceEpoch_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetStartOfMonth(y, m);
        int daysInYearBefore = info.DaysInYearBeforeMonth;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBefore﹍DateParts_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int d = info.DaysInMonth;
        int daysInYearBefore = info.DaysInYearBeforeMonth + d - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(y, m, d);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInYearBefore﹍DaysSinceEpoch_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetEndOfMonth(y, m);
        int daysInYearBefore = info.DaysInYearBeforeMonth + info.DaysInMonth - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInYearBefore﹍DateParts(DateInfo info)
    {
        var (y, m, d, doy) = info;
        int daysInYearBefore = doy - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(y, m, d);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInYearBefore﹍OrdinalParts(DateInfo info)
    {
        var (y, doy) = info.Yedoy;
        int daysInYearBefore = doy - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(y, doy);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysInYearBefore﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, y, m, d) = info;
        int daysInYearBefore = SchemaUT.GetDayOfYear(y, m, d) - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearBefore, actual);
    }

    #endregion
    #region CountDaysInYearAfter()

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearAfter﹍DateParts_AtStartOfYear(YearInfo info)
    {
        int daysInYearAfter = info.DaysInYear - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(info.Year, 1, 1);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearAfter﹍OrdinalParts_AtStartOfYear(YearInfo info)
    {
        int daysInYearAfter = info.DaysInYear - 1;
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(info.Year, 1);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearAfter﹍DaysSinceEpoch_AtStartOfYear(YearInfo info)
    {
        int daysInYearAfter = info.DaysInYear - 1;
        int daysSinceEpoch = SchemaUT.GetStartOfYear(info.Year);
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearAfter﹍OrdinalParts_AtEndOfYear(YearInfo info)
    {
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(info.Year, info.DaysInYear);
        // Assert
        Assert.Equal(0, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYearAfter﹍DaysSinceEpoch_AtEndOfYear(YearInfo info)
    {
        int daysSinceEpoch = SchemaUT.GetEndOfYear(info.Year);
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(DaysInYearAfterDateData))]
    public void CountDaysInYearAfter﹍DateParts(YemodaAnd<int> info)
    {
        var (y, m, d, daysInYearAfter) = info;
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(y, m, d);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [Theory, MemberData(nameof(DaysInYearAfterDateData))]
    public void CountDaysInYearAfter﹍OrdinalParts(YemodaAnd<int> info)
    {
        var (y, m, d, daysInYearAfter) = info;
        int doy = SchemaUT.GetDayOfYear(y, m, d);
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(y, doy);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    [Theory, MemberData(nameof(DaysInYearAfterDateData))]
    public void CountDaysInYearAfter﹍DaysSinceEpoch(YemodaAnd<int> info)
    {
        var (y, m, d, daysInYearAfter) = info;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int actual = SchemaUT.CountDaysInYearAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInYearAfter, actual);
    }

    #endregion
    #region CountDaysInMonthBefore()

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthBefore﹍DateParts_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(y, m, 1);
        // Assert
        Assert.Equal(0, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthBefore﹍DaysSinceEpoch_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetStartOfMonth(y, m);
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(0, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthBefore﹍DateParts_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int d = info.DaysInMonth;
        int daysInMonthBefore = d - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(y, m, d);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthBefore﹍DaysSinceEpoch_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetEndOfMonth(y, m);
        int daysInMonthBefore = info.DaysInMonth - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInMonthBefore﹍DateParts(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        int daysInMonthBefore = d - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(y, m, d);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    [Theory, MemberData(nameof(DateInfoData))]
    public void CountDaysInMonthBefore﹍OrdinalParts(DateInfo info)
    {
        var (y, _, d, doy) = info;
        int daysInMonthBefore = d - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(y, doy);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    [Theory, MemberData(nameof(DaysSinceEpochInfoData))]
    public void CountDaysInMonthBefore﹍DaysSinceEpoch(DaysSinceEpochInfo info)
    {
        var (daysSinceEpoch, _, _, d) = info;
        int daysInMonthBefore = d - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthBefore(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInMonthBefore, actual);
    }

    #endregion
    #region CountDaysInMonthAfter()

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthAfter﹍DateParts_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysInMonthAfter = info.DaysInMonth - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(y, m, 1);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthAfter﹍DaysSinceEpoch_AtStartOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetStartOfMonth(y, m);
        int daysInMonthAfter = info.DaysInMonth - 1;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthAfter﹍DateParts_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int d = info.DaysInMonth;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(y, m, d);
        // Assert
        Assert.Equal(0, actual);
    }

    [TestExcludeFrom(TestExcludeFrom.Regular)]
    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonthAfter﹍DaysSinceEpoch_AtEndOfMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        int daysSinceEpoch = SchemaUT.GetEndOfMonth(y, m);
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(0, actual);
    }

    [Theory, MemberData(nameof(DaysInMonthAfterDateData))]
    public void CountDaysInMonthAfter﹍DateParts(YemodaAnd<int> info)
    {
        var (y, m, d, daysInMonthAfter) = info;
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(y, m, d);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    [Theory, MemberData(nameof(DaysInMonthAfterDateData))]
    public void CountDaysInMonthAfter﹍OrdinalParts(YemodaAnd<int> info)
    {
        var (y, m, d, daysInMonthAfter) = info;
        int doy = SchemaUT.GetDayOfYear(y, m, d);
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(y, doy);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    [Theory, MemberData(nameof(DaysInMonthAfterDateData))]
    public void CountDaysInMonthAfter﹍DaysSinceEpoch(YemodaAnd<int> info)
    {
        var (y, m, d, daysInMonthAfter) = info;
        int daysSinceEpoch = SchemaUT.CountDaysSinceEpoch(y, m, d);
        // Act
        int actual = SchemaUT.CountDaysInMonthAfter(daysSinceEpoch);
        // Assert
        Assert.Equal(daysInMonthAfter, actual);
    }

    #endregion
}

public partial class ICalendricalSchemaFacts<TSchema, TDataSet> // Overflows
{
    [Fact] public void CountDaysInYearAfterMonth_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearAfterMonth(MinYear, 1);
    [Fact] public void CountDaysInYearAfterMonth_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearAfterMonth(MaxYear, MaxMonth);

    [Fact] public void CountDaysInYearBefore﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearBefore(MinYear, 1, 1);
    [Fact] public void CountDaysInYearBefore﹍DateParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearBefore(MaxYear, 1, 1);
    [Fact] public void CountDaysInYearBefore﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearBefore(MinYear, 1);
    [Fact] public void CountDaysInYearBefore﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearBefore(MaxYear, MaxDayOfYear);
    [Fact] public void CountDaysInYearBefore﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearBefore(MinDaysSinceEpoch);
    [Fact] public void CountDaysInYearBefore﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearBefore(MaxDaysSinceEpoch);

    [Fact] public void CountDaysInYearAfter﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearAfter(MinYear, 1, 1);
    [Fact] public void CountDaysInYearAfter﹍DateParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearAfter(MaxYear, 1, 1);
    [Fact] public void CountDaysInYearAfter﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearAfter(MinYear, 1);
    [Fact] public void CountDaysInYearAfter﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearAfter(MaxYear, MaxDayOfYear);
    [Fact] public void CountDaysInYearAfter﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.CountDaysInYearAfter(MinDaysSinceEpoch);
    [Fact] public void CountDaysInYearAfter﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.CountDaysInYearAfter(MaxDaysSinceEpoch);

    [Fact] public void CountDaysInMonthBefore﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthBefore(MinYear, 1, 1);
    [Fact] public void CountDaysInMonthBefore﹍DateParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthBefore(MaxYear, 1, 1);
    [Fact] public void CountDaysInMonthBefore﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthBefore(MinYear, 1);
    [Fact] public void CountDaysInMonthBefore﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthBefore(MaxYear, MaxDayOfYear);
    [Fact] public void CountDaysInMonthBefore﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthBefore(MinDaysSinceEpoch);
    [Fact] public void CountDaysInMonthBefore﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthBefore(MaxDaysSinceEpoch);

    [Fact] public void CountDaysInMonthAfter﹍DateParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthAfter(MinYear, 1, 1);
    [Fact] public void CountDaysInMonthAfter﹍DateParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthAfter(MaxYear, 1, 1);
    [Fact] public void CountDaysInMonthAfter﹍OrdinalParts_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthAfter(MinYear, 1);
    [Fact] public void CountDaysInMonthAfter﹍OrdinalParts_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthAfter(MaxYear, MaxDayOfYear);
    [Fact] public void CountDaysInMonthAfter﹍DaysSinceEpoch_DoesNotUnderflow() => _ = SchemaUT.CountDaysInMonthAfter(MinDaysSinceEpoch);
    [Fact] public void CountDaysInMonthAfter﹍DaysSinceEpoch_DoesNotOverflow() => _ = SchemaUT.CountDaysInMonthAfter(MaxDaysSinceEpoch);
}
