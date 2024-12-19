// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Collections.Generic;
using System.Linq;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

public sealed partial class MyGregorianCalendar : UserCalendar, IDateProvider<MyGregorianDate>
{
    internal const string DisplayName = "Grégorien";

    public MyGregorianCalendar()
        : base(DisplayName,
            MinMaxYearScope.CreateMaximalOnOrAfterYear1<GregorianSchema>(DayZero.NewStyle))
    {
        (MinYear, MaxYear) = Scope.Segment.SupportedYears.Endpoints;
    }

    public static MyGregorianDate MinDate => MyGregorianDate.MinValue;
    public static MyGregorianDate MaxDate => MyGregorianDate.MaxValue;

    public int MinYear { get; }
    public int MaxYear { get; }
}

public partial class MyGregorianCalendar // IDateProvider<MyGregorianDate>
{
    public IEnumerable<MyGregorianDate> GetDaysInYear(int year)
    {
        Scope.ValidateYear(year);

        int startOfYear = Schema.GetStartOfYear(year);
        int daysInYear = Schema.CountDaysInYear(year);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select new MyGregorianDate(daysSinceEpoch);
    }

    public IEnumerable<MyGregorianDate> GetDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);

        int startOfMonth = Schema.GetStartOfMonth(year, month);
        int daysInMonth = Schema.CountDaysInMonth(year, month);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select new MyGregorianDate(daysSinceEpoch);
    }

    public MyGregorianDate GetStartOfYear(int year)
    {
        Scope.ValidateYear(year);
        int daysSinceEpoch = Schema.GetStartOfYear(year);
        return new MyGregorianDate(daysSinceEpoch);
    }

    public MyGregorianDate GetEndOfYear(int year)
    {
        Scope.ValidateYear(year);
        int daysSinceEpoch = Schema.GetEndOfYear(year);
        return new MyGregorianDate(daysSinceEpoch);
    }

    public MyGregorianDate GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
        return new MyGregorianDate(daysSinceEpoch);
    }

    public MyGregorianDate GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
        return new MyGregorianDate(daysSinceEpoch);
    }
}

public partial class MyGregorianCalendar
{
    internal static MyGregorianCalendar Instance { get; } = new();

    internal int MinDaysSinceEpoch => Scope.Segment.SupportedDays.Min;
    internal int MaxDaysSinceEpoch => Scope.Segment.SupportedDays.Max;

    // This method does not validate its parameters
    internal bool IsIntercalaryDay(int daysSinceEpoch)
    {
        Schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsIntercalaryDay(y, m, d);
    }

    // This method does not validate its parameters
    internal bool IsSupplementaryDay(int daysSinceEpoch)
    {
        Schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsSupplementaryDay(y, m, d);
    }

    // This method does not validate its parameters
    internal void GetDateParts(int daysSinceEpoch, out int year, out int month, out int day) =>
        Schema.GetDateParts(daysSinceEpoch, out year, out month, out day);

    // This method does not validate its parameters
    internal int GetYear(int daysSinceEpoch, out int dayofYear) =>
        Schema.GetYear(daysSinceEpoch, out dayofYear);

    // This method does not validate its parameters
    internal int CountDaysInYearAfter(int daysSinceEpoch)
    {
        int y = GetYear(daysSinceEpoch, out int doy);
        return Schema.CountDaysInYear(y) - doy;
    }

    // This method does not validate its parameters
    internal int CountDaysInMonthAfter(int daysSinceEpoch)
    {
        Schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return Schema.CountDaysInMonth(y, m) - d;
    }
}

public partial class MyGregorianCalendar
{
    public int CountDaysSinceEpoch(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return Schema.CountDaysSinceEpoch(year, month, day);
    }

    public int CountDaysSinceEpoch(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);
        return Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    public MyGregorianDate CreateDate(DayNumber dayNumber)
    {
        Scope.Validate(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    //
    // Adjustments for the core parts
    //

    public MyGregorianDate AdjustYear(MyGregorianDate date, int newYear)
    {
        var (_, m, d) = date;
        Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newYear, m, d);
        return new(daysSinceEpoch);
    }

    public MyGregorianDate AdjustMonth(MyGregorianDate date, int newMonth)
    {
        var (y, _, d) = date;
        Schema.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceEpoch);
    }

    public MyGregorianDate AdjustDay(MyGregorianDate date, int newDay)
    {
        var (y, m, _) = date;
        if (newDay < 1 || newDay > Schema.CountDaysInMonth(y, m))
        {
            throw new ArgumentOutOfRangeException(nameof(newDay));
        }

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, newDay);
        return new(daysSinceEpoch);
    }

    public MyGregorianDate AdjustDayOfYear(MyGregorianDate date, int newDayOfYear)
    {
        int y = date.Year;
        Schema.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceEpoch);
    }

    //
    // Adjusters for the day of the week
    //

    public MyGregorianDate Previous(MyGregorianDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.Previous(dayOfWeek);
        Scope.CheckLowerBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    public MyGregorianDate PreviousOrSame(MyGregorianDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.PreviousOrSame(dayOfWeek);
        Scope.CheckLowerBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    public MyGregorianDate Nearest(MyGregorianDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.Nearest(dayOfWeek);
        Scope.CheckOverflow(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    public MyGregorianDate NextOrSame(MyGregorianDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.NextOrSame(dayOfWeek);
        Scope.CheckUpperBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    public MyGregorianDate Next(MyGregorianDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.Next(dayOfWeek);
        Scope.CheckUpperBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }
}
