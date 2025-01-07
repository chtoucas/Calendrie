﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

internal sealed class MonthMathPlain<TMonth, TCalendar> : MonthMath<TMonth, TCalendar>
    where TMonth : struct, ICalendarMonth<TMonth>, ICalendarBound<TCalendar>, IUnsafeFactory<TMonth>
    where TCalendar : Calendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MonthMathPlain{TMonth, TCalendar}"/>
    /// class.
    /// </summary>
    public MonthMathPlain(AdditionRule additionRule) : base(additionRule) { }

    /// <inheritdoc />
    [Pure]
    protected sealed override TMonth AddYears(int y, int m, int years, out int roundoff)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowMonthOverflow();

        //int newM = Math.Min(m, Schema.CountMonthsInYear(newY));
        int monthsInYear = Schema.CountMonthsInYear(newY);
        roundoff = Math.Max(0, m - monthsInYear);
        // On retourne le dernier du mois de l'année si m > monthsInYear.
        int newM = roundoff == 0 ? m : monthsInYear;

        int monthsSinceEpoch = Schema.CountMonthsSinceEpoch(newY, newM);
        return TMonth.UnsafeCreate(monthsSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountYearsBetween(TMonth start, TMonth end, out TMonth newStart)
    {
        var (y0, m0) = start;

        // Exact difference between two calendar years.
        int years = end.Year - y0;

        // To avoid extracting y0 more than once, we inline:
        // > var newStart = AddYears(start, years);
        newStart = AddYears(y0, m0, years);
        if (start < end)
        {
            if (newStart > end) newStart = AddYears(y0, m0, --years);
        }
        else
        {
            if (newStart < end) newStart = AddYears(y0, m0, ++years);
        }

        return years;
    }
}
