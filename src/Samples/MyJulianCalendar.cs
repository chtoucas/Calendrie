// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using Calendrie;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

public sealed partial class MyJulianCalendar : UserCalendar
{
    internal const string DisplayName = "Julien";

    public MyJulianCalendar()
        : base(DisplayName,
            MinMaxYearScope.CreateMaximalOnOrAfterYear1<JulianSchema>(DayZero.OldStyle))
    {
    }
}

public partial class MyJulianCalendar
{
    internal static MyJulianCalendar Instance { get; } = new();

    internal int CountDaysSinceEpoch(MyJulianDate date)
    {
        var (y, m, d) = date;
        return Schema.CountDaysSinceEpoch(y, m, d);
    }

    internal int GetDayOfYear(MyJulianDate date)
    {
        var (y, m, d) = date;
        return Schema.GetDayOfYear(y, m, d);
    }

    internal int GetOrdinalParts(MyJulianDate date, out int dayOfYear)
    {
        var (y, m, d) = date;
        dayOfYear = Schema.GetDayOfYear(y, m, d);
        return y;
    }

    internal bool IsIntercalaryDay(MyJulianDate date)
    {
        var (y, m, d) = date;
        return Schema.IsIntercalaryDay(y, m, d);
    }

    internal bool IsSupplementaryDay(MyJulianDate date)
    {
        var (y, m, d) = date;
        return Schema.IsSupplementaryDay(y, m, d);
    }

    internal int CountDaysInYearAfter(MyJulianDate date)
    {
        var (y, m, d) = date;
        return Schema.CountDaysInYear(y) - Schema.CountDaysInYearBeforeMonth(y, m) - d;
    }

    internal int CountDaysInMonthAfter(MyJulianDate date)
    {
        var (y, m, d) = date;
        return Schema.CountDaysInMonth(y, m) - d;
    }
}
