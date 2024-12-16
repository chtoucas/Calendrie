// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Hemerology;

using Calendrie.Core.Intervals;
using Calendrie.Hemerology;
using Calendrie.Testing.Data;

// TODO(fact): ICalendar and Calendar props and methods.
// - IsRegular()
// - Epoch
// - Domain
// - Scope
// This class no longer derives from CalendarTesting, but is it the right move?

// We do not use ICalendarFacts which is tailored to work with schemas
// (unbounded calendars).

/// <summary>
/// Provides facts about <see cref="Calendar"/>.
/// </summary>
public abstract partial class CalendarFacts<TCalendar, TDataSet> :
    CalendarDataConsumer<TDataSet>
    where TCalendar : Calendar
    where TDataSet : ICalendarDataSet, ISingleton<TDataSet>
{
    protected CalendarFacts(TCalendar calendar)
    {
        ArgumentNullException.ThrowIfNull(calendar);

        CalendarUT = calendar;

        var scope = calendar.Scope;
        var supportedYears = scope.Segment.SupportedYears;
        SupportedYearsTester = new SupportedYearsTester(supportedYears);

        var domain = scope.Domain;
        Domain = domain;
        DomainTester = new DomainTester(domain);
    }

    /// <summary>
    /// Gets the calendar under test.
    /// </summary>
    protected TCalendar CalendarUT { get; }

    protected Range<DayNumber> Domain { get; }

    protected SupportedYearsTester SupportedYearsTester { get; }
    protected DomainTester DomainTester { get; }
}

public partial class CalendarFacts<TCalendar, TDataSet> // Abstract
{
    [Fact] public abstract void Algorithm_Prop();
    [Fact] public abstract void Family_Prop();
    [Fact] public abstract void PeriodicAdjustments_Prop();

    //[Fact] public abstract void IsRegular();
}

public partial class CalendarFacts<TCalendar, TDataSet> // ICalendar
{
    //[Fact]
    //public void Epoch_Prop() => Assert.Equal(Epoch, CalendarUT.Epoch);

    //
    // Characteristics
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
    #region IsIntercalaryDay()

    [Fact]
    public void IsIntercalaryDay_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.IsIntercalaryDay(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void IsIntercalaryDay_InvalidMonth(int y, int m) =>
        AssertEx.ThrowsAoorexn("month", () => CalendarUT.IsIntercalaryDay(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void IsIntercalaryDay_InvalidDay(int y, int m, int d) =>
        AssertEx.ThrowsAoorexn("day", () => CalendarUT.IsIntercalaryDay(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsIntercalaryDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        bool actual = CalendarUT.IsIntercalaryDay(y, m, d);
        // Assert
        Assert.Equal(info.IsIntercalary, actual);
    }

    #endregion
    #region IsSupplementaryDay()

    [Fact]
    public void IsSupplementaryDay_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(y => CalendarUT.IsSupplementaryDay(y, 1, 1));

    [Theory, MemberData(nameof(InvalidMonthFieldData))]
    public void IsSupplementaryDay_InvalidMonth(int y, int m) =>
        AssertEx.ThrowsAoorexn("month", () => CalendarUT.IsSupplementaryDay(y, m, 1));

    [Theory, MemberData(nameof(InvalidDayFieldData))]
    public void IsSupplementaryDay_InvalidDay(int y, int m, int d) =>
        AssertEx.ThrowsAoorexn("day", () => CalendarUT.IsSupplementaryDay(y, m, d));

    [Theory, MemberData(nameof(DateInfoData))]
    public void IsSupplementaryDay(DateInfo info)
    {
        var (y, m, d) = info.Yemoda;
        // Act
        bool actual = CalendarUT.IsSupplementaryDay(y, m, d);
        // Assert
        Assert.Equal(info.IsSupplementary, actual);
    }

    #endregion

    //
    // Counting
    //

    #region CountMonthsInYear()

    [Fact]
    public void CountMonthsInYear_InvalidYear() =>
        SupportedYearsTester.TestInvalidYear(CalendarUT.CountMonthsInYear);

    [Theory, MemberData(nameof(YearInfoData))]
    public void CountMonthsInYear(YearInfo info)
    {
        // Act
        int actual = CalendarUT.CountMonthsInYear(info.Year);
        // Assert
        Assert.Equal(info.MonthsInYear, actual);
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
