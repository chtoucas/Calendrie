// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;

using Calendrie;
using Calendrie.Hemerology;

public sealed partial class MyCalendar : UserCalendar
{
    public MyCalendar(string name, MinMaxYearScope scope) : base(name, scope) { }

    internal int MinDaysSinceEpoch => Scope.Segment.SupportedDays.Min;
    internal int MaxDaysSinceEpoch => Scope.Segment.SupportedDays.Max;

    internal static MyCalendar Instance { get; } = new("My Calendar", null!);

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

// Public methods for MyDate
//
public partial class MyCalendar
{
    //
    // Adjustments for the core parts
    //

    public MyDate AdjustYear(MyDate date, int newYear)
    {
        var (_, m, d) = date;
        Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newYear, m, d);
        return new(daysSinceEpoch);
    }

    public MyDate AdjustMonth(MyDate date, int newMonth)
    {
        var (y, _, d) = date;
        Schema.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceEpoch);
    }

    public MyDate AdjustDay(MyDate date, int newDay)
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

    public MyDate AdjustDayOfYear(MyDate date, int newDayOfYear)
    {
        int y = date.Year;
        Schema.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceEpoch);
    }

    //
    // Adjusters for the day of the week
    //

    public MyDate Previous(MyDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.Previous(dayOfWeek);
        Scope.CheckLowerBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    public MyDate PreviousOrSame(MyDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.PreviousOrSame(dayOfWeek);
        Scope.CheckLowerBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    public MyDate Nearest(MyDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.Nearest(dayOfWeek);
        Scope.CheckOverflow(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    public MyDate NextOrSame(MyDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.NextOrSame(dayOfWeek);
        Scope.CheckUpperBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }

    public MyDate Next(MyDate date, DayOfWeek dayOfWeek)
    {
        var dayNumber = date.DayNumber.Next(dayOfWeek);
        Scope.CheckUpperBound(dayNumber);
        return new(dayNumber.DaysSinceZero - Epoch.DaysSinceZero);
    }
}

// Internal methods for MyDate
//
public partial class MyCalendar
{
    internal MyDate GetDate(DayNumber dayNumber)
    {
        Scope.Validate(dayNumber);

        // We know that the subtraction won't overflow
        // > return new(dayNumber - s_Epoch);
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
