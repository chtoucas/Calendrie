// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;

using Calendrie;
using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

using Range_ = Calendrie.Core.Intervals.Range;

public sealed partial class MyJulianCalendar : UserCalendar
{
    internal const string DisplayName = "Julien";

    public MyJulianCalendar()
        : base(DisplayName,
            MinMaxYearScope.Create<JulianSchema>(DayZero.OldStyle, Range_.Create(1, Yemoda.MaxYear)))
    {
        UnderlyingSchema = (JulianSchema)Schema;

        (MinDateParts, MaxDateParts) = Scope.Segment.ExtractMinMaxDateParts();
    }

    internal JulianSchema UnderlyingSchema { get; }
}

public partial class MyJulianCalendar
{
    internal static MyJulianCalendar Instance { get; } = new();

    internal Yemoda MinDateParts { get; }
    internal Yemoda MaxDateParts { get; }

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

    internal int GetOrdinalParts(Yemoda ymd, out int dayOfYear)
    {
        var (y, m, d) = ymd;
        dayOfYear = Schema.GetDayOfYear(y, m, d);
        return y;
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

    internal int CountDaysInYearAfter(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return Schema.CountDaysInYear(y) - Schema.CountDaysInYearBeforeMonth(y, m) - d;
    }

    internal int CountDaysInMonthAfter(Yemoda ymd)
    {
        var (y, m, d) = ymd;
        return Schema.CountDaysInMonth(y, m) - d;
    }

    internal int CountDaysBetween(Yemoda left, Yemoda right) => throw new NotImplementedException();
    internal Yemoda AddDays(Yemoda ymd, int days) => throw new NotImplementedException();
    internal Yemoda NextDay(Yemoda ymd) => throw new NotImplementedException();
    internal Yemoda PreviousDay(Yemoda ymd) => throw new NotImplementedException();
}
