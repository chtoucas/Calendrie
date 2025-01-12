// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Hemerology;
using Calendrie.Testing.Data;

/// <summary>
/// Provides facts about <see cref="UserCalendar"/>.
/// </summary>
public abstract class UserCalendarFacts<TCalendar, TDataSet> :
    CalendarFacts<TCalendar, TDataSet>
    where TCalendar : UserCalendar
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected UserCalendarFacts(TCalendar calendar) : base(calendar)
    {
        var supportedYears = calendar.Scope.Segment.SupportedYears;
        SupportedYearsTester = new SupportedYearsTester(supportedYears);
    }

    protected SupportedYearsTester SupportedYearsTester { get; }

    //
    // Properties
    //

    [Fact]
    public sealed override void Algorithm_Prop() =>
        Assert.Equal(CalendarUT.Algorithm, CalendarUT.Scope.Schema.Algorithm);

    [Fact]
    public sealed override void Family_Prop() =>
        Assert.Equal(CalendarUT.Family, CalendarUT.Scope.Schema.Family);

    [Fact]
    public sealed override void PeriodicAdjustments_Prop() =>
        Assert.Equal(CalendarUT.PeriodicAdjustments, CalendarUT.Scope.Schema.PeriodicAdjustments);

    [Fact]
    public void ToString_ReturnsName() => Assert.Equal(CalendarUT.Name, CalendarUT.ToString());

    //
    // Characteristics
    //

    //[Fact] public abstract void IsRegular();

    //
    // Year infos
    //

    #region IsLeapYear()

    [Fact]
    public void IsLeapYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.IsLeapYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void IsLeapYear(YearInfo info)
    {
        // Act
        bool actual = CalendarUT.IsLeapYear(info.Year);
        // Assert
        Assert.Equal(info.IsLeap, actual);
    }

    #endregion

    #region CountDaysInYear()

    [Fact]
    public void CountDaysInYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.CountDaysInYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountDaysInYear(YearInfo info)
    {
        // Act
        int actual = CalendarUT.CountDaysInYear(info.Year);
        // Assert
        Assert.Equal(info.DaysInYear, actual);
    }

    #endregion

    //
    // Month infos
    //

    #region IsIntercalaryMonth()

    [Fact]
    public void IsIntercalaryMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.IsIntercalaryMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void IsIntercalaryMonth_InvalidMonth(int y, int m) =>
        AssertEx.ThrowsAoorexn("month", () => CalendarUT.IsIntercalaryMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void IsIntercalaryMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        bool actual = CalendarUT.IsIntercalaryMonth(y, m);
        // Assert
        Assert.Equal(info.IsIntercalary, actual);
    }

    #endregion

    #region CountDaysInMonth()

    [Fact]
    public void CountDaysInMonth_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.CountDaysInMonth(y, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void CountDaysInMonth_InvalidMonth(int y, int m) =>
        AssertEx.ThrowsAoorexn("month", () => CalendarUT.CountDaysInMonth(y, m));

    [Theory, MemberData(nameof(MonthInfoData))]
    public void CountDaysInMonth(MonthInfo info)
    {
        var (y, m) = info.Yemo;
        // Act
        int actual = CalendarUT.CountDaysInMonth(y, m);
        // Assert
        Assert.Equal(info.DaysInMonth, actual);
    }

    #endregion
}
