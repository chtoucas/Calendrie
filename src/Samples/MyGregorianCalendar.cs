// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

public sealed partial class MyGregorianCalendar : Calendar
{
    public MyGregorianCalendar()
        : base("Grégorien", MinMaxYearScope.CreateMaximal<GregorianSchema>(DayZero.NewStyle)) { }

    internal static MyGregorianCalendar Instance { get; } = new();
    internal int MinDaysSinceEpoch => Scope.Segment.SupportedDays.Min;
    internal int MaxDaysSinceEpoch => Scope.Segment.SupportedDays.Max;

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

// Internal methods used to build MyDate
// These methods may not validate their parameters
public partial class MyGregorianCalendar
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

    internal MyGregorianDate GetDate(DayNumber dayNumber)
    {
        Scope.Validate(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

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

// internal (optional) methods used to build MyDate
//
public partial class MyGregorianCalendar
{
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
        if (newDay < 1
            || (newDay > Schema.MinDaysInMonth
                && newDay > Schema.CountDaysInMonth(y, m)))
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
