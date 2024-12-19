// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;

using Calendrie;
using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

public sealed partial class MyJulianCalendar : UserCalendar
{
    internal const string DisplayName = "Julien";

    public MyJulianCalendar()
        : base(DisplayName,
            MinMaxYearScope.CreateMaximalOnOrAfterYear1<JulianSchema>(DayZero.OldStyle))
    {
        UnderlyingSchema = (JulianSchema)Schema;
    }

    internal JulianSchema UnderlyingSchema { get; }
}

public partial class MyJulianCalendar
{
    internal static MyJulianCalendar Instance { get; } = new();

    public Yemoda GetDate(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        throw new NotImplementedException();
    }

    public Yemoda GetDate(int year, int dayOfYear)
    {
        throw new NotImplementedException();
    }

    public Yemoda GetDate(DayNumber dayNumber)
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
