// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;

using Calendrie;
using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Core.Validation;
using Calendrie.Hemerology;

using Range_ = Calendrie.Core.Intervals.Range;

public sealed partial class MyJulianCalendar : Calendar
{
    internal const string DisplayName = "Julien";

    public const int MinYear = 1;
    public const int MaxYear = Yemoda.MaxYear;

    public MyJulianCalendar()
        : base(DisplayName,
            MinMaxYearScope.Create<JulianSchema>(DayZero.OldStyle, Range_.Create(MinYear, MaxYear)))
    {
        UnderlyingSchema = (JulianSchema)Schema;

        (MinDateParts, MaxDateParts) =
            Scope.Segment.MinMaxDateParts.Select(x => Yemoda.Create(x.Year, x.Month, x.Day));

        // Cache the pre-validator which is a computed prop.
        PreValidator = Schema.PreValidator;
    }

    internal static MyJulianCalendar Instance { get; } = new();

    internal JulianSchema UnderlyingSchema { get; }

    internal Yemoda MinDateParts { get; }
    internal Yemoda MaxDateParts { get; }

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

public partial class MyJulianCalendar // Date constructors helpers
{
    public Yemoda CreateDateParts(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return UnderlyingSchema.GetDateParts(year, month, day);
    }

    public Yemoda CreateDateParts(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);
        return UnderlyingSchema.GetDateParts(year, dayOfYear);
    }

    public Yemoda CreateDateParts(DayNumber dayNumber)
    {
        Scope.Validate(dayNumber);
        return UnderlyingSchema.GetDateParts(dayNumber - Epoch);
    }
}

public partial class MyJulianCalendar // Date helpers
{
    internal int CountDaysSinceEpoch(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return Schema.CountDaysSinceEpoch(y, m, d);
    }

    internal int GetDayOfYear(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return Schema.GetDayOfYear(y, m, d);
    }

    internal Yedoy GetOrdinalParts(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return UnderlyingSchema.GetOrdinalParts(y, m, d);
    }

    internal bool IsIntercalaryDay(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return Schema.IsIntercalaryDay(y, m, d);
    }

    internal bool IsSupplementaryDay(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return Schema.IsSupplementaryDay(y, m, d);
    }
}

public partial class MyJulianCalendar // Date helpers (counting)
{
    internal int CountDaysInYearAfter(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return UnderlyingSchema.CountDaysInYearAfter(y, m, d);
    }

    internal int CountDaysInMonthAfter(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return UnderlyingSchema.CountDaysInMonthAfter(y, m, d);
    }
}

public partial class MyJulianCalendar // Date helpers (adjustments)
{
    //
    // Adjustments for the core parts
    //

    internal MyJulianDate AdjustYear(MyJulianDate date, int newYear)
    {
        var (_, m, d) = date;
        Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        return new(UnderlyingSchema.GetDateParts(newYear, m, d));
    }

    internal MyJulianDate AdjustMonth(MyJulianDate date, int newMonth)
    {
        var (y, _, d) = date;
        PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        return new(UnderlyingSchema.GetDateParts(y, newMonth, d));
    }

    internal MyJulianDate AdjustDay(MyJulianDate date, int newDay)
    {
        var (y, m, _) = date;
        PreValidator.ValidateDayOfMonth(y, m, newDay, nameof(newDay));

        return new(UnderlyingSchema.GetDateParts(y, m, newDay));
    }

    internal MyJulianDate AdjustDayOfYear(MyJulianDate date, int newDayOfYear)
    {
        int y = date.Year;
        PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        return new(UnderlyingSchema.GetDateParts(y, newDayOfYear));
    }

    //
    // Adjusters for the day of the week
    //

    //internal MyJulianDate Previous(MyJulianDate date, DayOfWeek dayOfWeek)
    //{
    //    var dayNumber = date.DayNumber.Previous(dayOfWeek);
    //    Scope.CheckLowerBound(dayNumber);
    //    return MyJulianDate.FromDayNumber(dayNumber);
    //}

    //internal MyJulianDate PreviousOrSame(MyJulianDate date, DayOfWeek dayOfWeek)
    //{
    //    var dayNumber = date.DayNumber.PreviousOrSame(dayOfWeek);
    //    Scope.CheckLowerBound(dayNumber);
    //    return MyJulianDate.FromDayNumber(dayNumber);
    //}

    //internal MyJulianDate Nearest(MyJulianDate date, DayOfWeek dayOfWeek)
    //{
    //    var dayNumber = date.DayNumber.Nearest(dayOfWeek);
    //    Scope.CheckOverflow(dayNumber);
    //    return MyJulianDate.FromDayNumber(dayNumber);
    //}

    //internal MyJulianDate NextOrSame(MyJulianDate date, DayOfWeek dayOfWeek)
    //{
    //    var dayNumber = date.DayNumber.NextOrSame(dayOfWeek);
    //    Scope.CheckUpperBound(dayNumber);
    //    return MyJulianDate.FromDayNumber(dayNumber);
    //}

    //internal MyJulianDate Next(MyGregorianDate date, DayOfWeek dayOfWeek)
    //{
    //    var dayNumber = date.DayNumber.Next(dayOfWeek);
    //    Scope.CheckUpperBound(dayNumber);
    //    return MyJulianDate.FromDayNumber(dayNumber);
    //}
}

public partial class MyJulianCalendar // Date helpers (math)
{
    internal int CountDaysBetween(Yemoda left, Yemoda right) => throw new NotImplementedException();
    internal Yemoda AddDays(Yemoda ymd, int days) => throw new NotImplementedException();
    internal Yemoda NextDay(Yemoda ymd) => throw new NotImplementedException();
    internal Yemoda PreviousDay(Yemoda ymd) => throw new NotImplementedException();
}
