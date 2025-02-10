// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

// Blank days
// ----------
//
// The use of blank-days can be traced back to Rev. Hugh Jones (1745) and
// was rediscovered later by Abbot Marco Mastrofini (1834).
// Also it seems that the "same idea had been thought of ~1650 years earlier
// c. 100 BCE and incorporated into the calendar used by the Qumran
// community"; see the wikipedia page
// https://en.wikipedia.org/wiki/Hugh_Jones_(professor)
//
// A blank-day schema is a solar schema that adds one extra blank day on
// common years and two on leap years. A blank day does not belong to any month
// and is kept outside the weekday cycle.
// For technical reasons, we pretend that a blank day is the last day of
// the preceding month.
// Blank-day calendars belong to the larger family of perennial calendars.

public partial struct InternationalFixedDate // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is a blank day;
    /// otherwise returns <see langword="false"/>.
    /// <para>A blank day does not belong to any month and is kept outside the
    /// weekday cycle.</para>
    /// </summary>
    public bool IsBlank => InternationalFixedSchema.IsBlankDayImpl(Day);
}

public partial struct PositivistDate // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is a blank day;
    /// otherwise returns <see langword="false"/>.
    /// <para>A blank day does not belong to any month and is kept outside the
    /// weekday cycle.</para>
    /// </summary>
    public bool IsBlank => PositivistSchema.IsBlankDayImpl(Day);
}

public partial struct WorldDate // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is a blank day;
    /// otherwise returns <see langword="false"/>.
    /// <para>A blank day does not belong to any month and is kept outside the
    /// weekday cycle.</para>
    /// </summary>
    public bool IsBlank
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
            return WorldSchema.IsBlankDayImpl(m, d);
        }
    }
}

// Temporaire
public static class EgyptianDateExtensions
{
    public static DayOfDecan GetDayOfDecan(this Egyptian12Date date)
    {
        int day = date.Day;
        return day > EgyptianSchema.DaysPerMonth ? DayOfDecan.Blank
        : (DayOfDecan)MathN.AdjustedModulo(day, 10);
    }

    public static int GetDecanOfMonth(this Egyptian12Date date)
    {
        int day = date.Day;
        return day > EgyptianSchema.DaysPerMonth ? 0 : MathN.AdjustedDivide(day, 10);
    }

    public static int GetDecanOfYear(this Egyptian12Date date)
    {
        int doy = date.DayOfYear;
        return doy > 360 ? 0 : MathN.AdjustedDivide(doy, 10);
    }

    public static DayOfDecan GetDayOfDecan(this Egyptian13Date date) =>
        date.IsEpagomenal(out _) ? DayOfDecan.Blank
        : (DayOfDecan)MathN.AdjustedModulo(date.Day, 10);

    public static int GetDecanOfMonth(this Egyptian13Date date) =>
        date.IsEpagomenal(out _) ? 0 : MathN.AdjustedDivide(date.Day, 10);

    public static int GetDecanOfYear(this Egyptian13Date date) =>
        date.IsEpagomenal(out _) ? 0 : MathN.AdjustedDivide(date.DayOfYear, 10);
}
