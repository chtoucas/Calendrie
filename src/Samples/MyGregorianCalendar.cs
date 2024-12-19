// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Collections.Generic;
using System.Linq;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

public sealed partial class MyGregorianCalendar : Calendar, IDateProvider<MyGregorianDate>
{
    internal const string DisplayName = "Gregorian";

    public MyGregorianCalendar()
        : base(DisplayName,
            MinMaxYearScope.CreateMaximalOnOrAfterYear1<GregorianSchema>(DayZero.NewStyle))
    {
        (MinYear, MaxYear) = Scope.Segment.SupportedYears.Endpoints;
        (MinDaysSinceEpoch, MaxDaysSinceEpoch) = Scope.Segment.SupportedDays.Endpoints;
    }

    public static MyGregorianDate MinDate => MyGregorianDate.MinValue;
    public static MyGregorianDate MaxDate => MyGregorianDate.MaxValue;

    internal static MyGregorianCalendar Instance { get; } = new();

    public int MinYear { get; }
    public int MaxYear { get; }

    internal int MinDaysSinceEpoch { get; }
    internal int MaxDaysSinceEpoch { get; }

    public sealed override int CountDaysInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountDaysInYear(year);
    }

    public sealed override int CountDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Schema.CountDaysInMonth(year, month);
    }
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

public partial class MyGregorianCalendar // Date constructors helpers
{
    internal int CountDaysSinceEpoch(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return Schema.CountDaysSinceEpoch(year, month, day);
    }

    internal int CountDaysSinceEpoch(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);
        return Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    internal int CountDaysSinceEpoch(DayNumber dayNumber)
    {
        Scope.Validate(dayNumber);
        return dayNumber.DaysSinceZero - Epoch.DaysSinceZero;
    }
}

// These methods do not validate their parameters
public partial class MyGregorianCalendar // Date helpers
{
    internal bool IsIntercalaryDay(int daysSinceEpoch)
    {
        Schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsIntercalaryDay(y, m, d);
    }

    internal bool IsSupplementaryDay(int daysSinceEpoch)
    {
        Schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsSupplementaryDay(y, m, d);
    }

    internal void GetDateParts(int daysSinceEpoch, out int year, out int month, out int day) =>
        Schema.GetDateParts(daysSinceEpoch, out year, out month, out day);

    internal int GetYear(int daysSinceEpoch, out int dayofYear) =>
        Schema.GetYear(daysSinceEpoch, out dayofYear);
}

// These methods do not validate their parameters
public partial class MyGregorianCalendar // Date helpers (counting)
{
    internal int CountDaysInYearAfter(int daysSinceEpoch)
    {
        int y = Schema.GetYear(daysSinceEpoch, out int doy);
        return Schema.CountDaysInYear(y) - doy;
    }

    internal int CountDaysInMonthAfter(int daysSinceEpoch)
    {
        Schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return Schema.CountDaysInMonth(y, m) - d;
    }
}

public partial class MyGregorianCalendar // Date helpers (adjustments)
{
    //
    // Adjustments for the core parts
    //

    internal MyGregorianDate AdjustYear(MyGregorianDate date, int newYear)
    {
        var (_, m, d) = date;
        Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newYear, m, d);
        return new(daysSinceEpoch);
    }

    internal MyGregorianDate AdjustMonth(MyGregorianDate date, int newMonth)
    {
        var (y, _, d) = date;
        Schema.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceEpoch);
    }

    internal MyGregorianDate AdjustDay(MyGregorianDate date, int newDay)
    {
        var (y, m, _) = date;
        if (newDay < 1
            || (newDay > Schema.MinDaysInMonth
                && newDay > Schema.CountDaysInMonth(y, m)))
        {
            throw new ArgumentOutOfRangeException(nameof(newDay));
        }

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, newDay);
        return new(daysSinceEpoch);
    }

    internal MyGregorianDate AdjustDayOfYear(MyGregorianDate date, int newDayOfYear)
    {
        int y = date.Year;
        Schema.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceEpoch);
    }

    //
    // Adjusters for the day of the week
    //

    internal MyGregorianDate Previous(MyGregorianDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.Previous(dayOfWeek);
        Scope.CheckLowerBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    internal MyGregorianDate PreviousOrSame(MyGregorianDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.PreviousOrSame(dayOfWeek);
        Scope.CheckLowerBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    internal MyGregorianDate Nearest(MyGregorianDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.Nearest(dayOfWeek);
        Scope.CheckOverflow(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    internal MyGregorianDate NextOrSame(MyGregorianDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.NextOrSame(dayOfWeek);
        Scope.CheckUpperBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    internal MyGregorianDate Next(MyGregorianDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.Next(dayOfWeek);
        Scope.CheckUpperBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }
}
