// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Calendrie;
using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

// This is a plain implementation, there is plenty room left to apply various optimizations.

public sealed partial class MyGregorianCalendar : CalendarSystem2, IDateProvider<MyGregorianDate>
{
    internal const string DisplayName = "Gregorian";

    public const int MonthsInYear = 12;

    public MyGregorianCalendar()
        : base(DisplayName,
            MinMaxYearScope.CreateMaximalOnOrAfterYear1<GregorianSchema>(DayZero.NewStyle))
    {
        Debug.Assert(Scope != null);

        var seg = Scope.Segment;
        (MinYear, MaxYear) = seg.SupportedYears.Endpoints;
        MaxDaysSinceEpoch = seg.SupportedDays.Endpoints.UpperValue;

        // Cache the computed property pre-validator.
        PreValidator = Schema.PreValidator;
    }

    public static MyGregorianDate MinDate => MyGregorianDate.MinValue;
    public static MyGregorianDate MaxDate => MyGregorianDate.MaxValue;

    internal static MyGregorianCalendar Instance { get; } = new();

    public int MinYear { get; }
    public int MaxYear { get; }

    // NB: MinDaysSinceEpoch = 0 (this calendar starts on 1/1/1)
    internal int MaxDaysSinceEpoch { get; }

    private ICalendricalPreValidator PreValidator { get; }

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

public partial class MyGregorianCalendar // Date helpers (ctors, factories & conversions)
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

    internal int CountDaysSinceEpochChecked(DayNumber dayNumber)
    {
        Scope.CheckOverflow(dayNumber);
        return dayNumber.DaysSinceZero - Epoch.DaysSinceZero;
    }

    internal int? TryCountDaysSinceEpoch(int year, int month, int day) =>
        Scope.CheckYearMonthDay(year, month, day)
        ? Schema.CountDaysSinceEpoch(year, month, day)
        : null;

    internal int? TryCountDaysSinceEpoch(int year, int dayOfYear) =>
        Scope.CheckOrdinal(year, dayOfYear)
        ? Schema.CountDaysSinceEpoch(year, dayOfYear)
        : null;
}

public partial class MyGregorianCalendar // Date helpers (no validation)
{
    // These methods do not validate their parameters because they don't need to.

    internal bool IsIntercalaryDay(int daysSinceEpoch)
    {
        Schema.GetDateParts(daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsIntercalaryDay(y, m, d);
    }

    internal void GetDateParts(int daysSinceEpoch, out int year, out int month, out int day) =>
        Schema.GetDateParts(daysSinceEpoch, out year, out month, out day);

    internal int GetYear(int daysSinceEpoch, out int dayofYear) =>
        Schema.GetYear(daysSinceEpoch, out dayofYear);

    internal int GetYear(int daysSinceEpoch) => Schema.GetYear(daysSinceEpoch);

    internal int CountDaysInYearAfter(int daysSinceEpoch) =>
        Schema.CountDaysInYearAfter(daysSinceEpoch);

    internal int CountDaysInMonthAfter(int daysSinceEpoch) =>
        Schema.CountDaysInMonthAfter(daysSinceEpoch);
}

public partial class MyGregorianCalendar // Date helpers
{
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
        PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newMonth, d);
        return new(daysSinceEpoch);
    }

    internal MyGregorianDate AdjustDayOfMonth(MyGregorianDate date, int newDayOfMonth)
    {
        var (y, m, _) = date;
        PreValidator.ValidateDayOfMonth(y, m, newDayOfMonth, nameof(newDayOfMonth));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, newDayOfMonth);
        return new(daysSinceEpoch);
    }

    internal MyGregorianDate AdjustDayOfYear(MyGregorianDate date, int newDayOfYear)
    {
        int y = date.Year;
        PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newDayOfYear);
        return new(daysSinceEpoch);
    }

    internal MyGregorianDate AddYears(MyGregorianDate date, int years) =>
        throw new NotImplementedException();

    internal MyGregorianDate AddMonths(MyGregorianDate date, int months) =>
        throw new NotImplementedException();

    internal int CountYearsBetween(MyGregorianDate start, MyGregorianDate end) =>
        throw new NotImplementedException();

    internal int CountMonthsBetween(MyGregorianDate start, MyGregorianDate end) =>
        throw new NotImplementedException();
}
