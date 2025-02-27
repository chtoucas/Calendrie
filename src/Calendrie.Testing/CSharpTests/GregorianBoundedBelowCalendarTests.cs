﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.CSharpTests;

using Calendrie.Core.Schemas;
using Calendrie.Hemerology;
using Calendrie.Testing.Data.Unbounded;
using Calendrie.Testing.Facts.Hemerology;

public static class BoundedBelowCalendarTests
{
    // Exemple du calendrier grégorien qui débute officiellement le 15/10/1582.
    // En 1582, 3 mois, octobre à décembre.
    // En 1582, 78 jours = 17 (oct) + 30 (nov) + 31 (déc).
    private static readonly BoundedBelowCalendar s_GenuineGregorian =
            new("Gregorian",
                BoundedBelowScope.StartingAt(
                    new CivilSchema(), DayZero.NewStyle, new DateParts(1582, 10, 15)));

    [Fact]
    public static void CountMonthsInFirstYear()
    {
        // Act
        var chr = s_GenuineGregorian;
        int minYear = chr.MinDateParts.Year;
        int monthsInFirstYear = 3;
        // Assert
        Assert.Equal(monthsInFirstYear, chr.CountMonthsInYear(minYear));
        Assert.Equal(monthsInFirstYear, chr.CountMonthsInFirstYear());
    }

    [Fact]
    public static void CountDaysInFirstYear()
    {
        // Act
        var chr = s_GenuineGregorian;
        int minYear = chr.MinDateParts.Year;
        int daysInFirstYear = 78;
        // Assert
        Assert.Equal(daysInFirstYear, chr.CountDaysInYear(minYear));
        Assert.Equal(daysInFirstYear, chr.CountDaysInFirstYear());
    }

    [Fact]
    public static void CountDaysInFirstMonth()
    {
        // Act
        var chr = s_GenuineGregorian;
        int daysInFirstMonth = 17;
        var parts = chr.MinDateParts;
        // Assert
        Assert.Equal(daysInFirstMonth, chr.CountDaysInMonth(parts.Year, parts.Month));
        Assert.Equal(daysInFirstMonth, chr.CountDaysInFirstMonth());
    }
}

public class GregorianBoundedBelowCalendarTests :
    CalendarSansFacts<BoundedBelowCalendar, UnboundedGregorianDataSet>
{
    // Datasets are tailored to work with a range of years, not a range of
    // days, therefore these tests are only adapted to MinMaxYearCalendar.
    //
    // On triche un peu, la date de début a été choisie de telle sorte que
    // les tests marchent... (cf. GregorianData).
    private const int FirstYear = -123_456;
    private const int FirstMonth = 4;
    private const int FirstDay = 5;

    public GregorianBoundedBelowCalendarTests() : base(MakeCalendar()) { }

    private static BoundedBelowCalendar MakeCalendar() =>
        new(
            "Gregorian",
            BoundedBelowScope.StartingAt(
                new GregorianSchema(),
                DayZero.NewStyle,
                new DateParts(FirstYear, FirstMonth, FirstDay)));

    [Fact]
    public void MinDateParts_Prop()
    {
        var parts = new DateParts(FirstYear, FirstMonth, FirstDay);
        var seg = CalendarUT.Scope.Segment;
        // Act
        Assert.Equal(parts, seg.MinMaxDateParts.LowerValue);
    }

    [Fact]
    public override void IsRegular()
    {
        Assert.True(CalendarUT.IsRegular(out int monthsIsYear));
        Assert.Equal(monthsIsYear, 12);
    }

    //[Fact]
    //public void GetDaysInYear_FirstYear()
    //{
    //    DayNumber startOfYear = CalendarUT.Scope.Domain.Min;
    //    DayNumber endOfYear = CalendarUT.GetEndOfYear(FirstYear);
    //    int daysInFirstYear = CalendarUT.CountDaysInFirstYear();
    //    IEnumerable<DayNumber> list =
    //        from i in Enumerable.Range(0, daysInFirstYear)
    //        select startOfYear + i;
    //    // Act
    //    IEnumerable<DayNumber> actual = CalendarUT.GetDaysInYear(FirstYear);
    //    var arr = actual.ToArray();
    //    // Assert
    //    Assert.Equal(list, actual);
    //    Assert.Equal(daysInFirstYear, arr.Length);
    //    Assert.Equal(startOfYear, arr.First());
    //    Assert.Equal(endOfYear, arr.Last());
    //}

    //[Fact]
    //public void GetDaysInMonth_FirstMonth()
    //{
    //    DayNumber startofMonth = CalendarUT.Scope.Domain.Min;
    //    DayNumber endOfMonth = CalendarUT.GetEndOfMonth(FirstYear, FirstMonth);
    //    int daysInFirstMonth = CalendarUT.CountDaysInFirstMonth();
    //    IEnumerable<DayNumber> list =
    //        from i in Enumerable.Range(0, daysInFirstMonth)
    //        select startofMonth + i;
    //    // Act
    //    IEnumerable<DayNumber> actual = CalendarUT.GetDaysInMonth(FirstYear, FirstMonth);
    //    var arr = actual.ToArray();
    //    // Assert
    //    Assert.Equal(list, actual);
    //    Assert.Equal(daysInFirstMonth, arr.Length);
    //    Assert.Equal(startofMonth, arr.First());
    //    Assert.Equal(endOfMonth, arr.Last());
    //}

    //[Fact]
    //public void GetStartOfYear_InvalidFirstYear() =>
    //    Assert.Throws<ArgumentOutOfRangeException>(
    //        "year", () => CalendarUT.GetStartOfYear(FirstYear));

    //[Fact]
    //public void GetStartOfMonth_InvalidFirstMonth() =>
    //    Assert.Throws<ArgumentOutOfRangeException>(
    //        "month", () => CalendarUT.GetStartOfMonth(FirstYear, FirstMonth));

    [Fact]
    public void CountMonthsInFirstYear()
    {
        // Act
        int monthsInFirstYear = 12 - (FirstMonth - 1);
        // Assert
        Assert.Equal(monthsInFirstYear, CalendarUT.CountMonthsInYear(FirstYear));
        Assert.Equal(monthsInFirstYear, CalendarUT.CountMonthsInFirstYear());
    }

    [Fact]
    public void CountDaysInFirstYear()
    {
        // Act
        var sch = CalendarUT.Scope.Schema;
        int daysInFirstYear = sch.CountDaysInYear(FirstYear)
            - sch.CountDaysInYearBeforeMonth(FirstYear, FirstMonth)
            - (FirstDay - 1);
        // Assert
        Assert.Equal(daysInFirstYear, CalendarUT.CountDaysInYear(FirstYear));
        Assert.Equal(daysInFirstYear, CalendarUT.CountDaysInFirstYear());
    }

    [Fact]
    public void CountDaysInFirstMonth()
    {
        // Act
        var sch = CalendarUT.Scope.Schema;
        int daysInFirstMonth = sch.CountDaysInMonth(FirstYear, FirstMonth) - (FirstDay - 1);
        // Assert
        Assert.Equal(daysInFirstMonth, CalendarUT.CountDaysInMonth(FirstYear, FirstMonth));
        Assert.Equal(daysInFirstMonth, CalendarUT.CountDaysInFirstMonth());
    }
}
