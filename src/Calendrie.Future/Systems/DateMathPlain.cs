// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

internal sealed class DateMathPlain<TDate, TCalendar> : DateMath<TDate, TCalendar>
    where TDate : struct, ICalendarDate<TDate>, ICalendarBound<TCalendar>, IUnsafeFactory<TDate>
    where TCalendar : Calendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateMathPlain{TCalendar, TDate}"/>
    /// class.
    /// </summary>
    public DateMathPlain(AdditionRule rule) : base(rule) { }

    /// <inheritdoc />
    [Pure]
    protected sealed override TDate AddYears(int y, int m, int d, int years, out int roundoff)
    {
        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowDateOverflow();

        var sch = Schema;
        int monthsInYear = sch.CountMonthsInYear(newY);
        int newM;
        int newD;
        if (m > monthsInYear)
        {
            // The target year newY has less months than the year y, we
            // return the end of the target year.
            // roundoff =
            //   "days" after the end of (y, monthsInYear) until (y, m, d) included
            //   + diff between end of (y, monthsInYear) and (newY, monthsInYear)
            roundoff = d;
            for (int i = monthsInYear + 1; i < m; i++)
            {
                roundoff += sch.CountDaysInMonth(y, i);
            }
            int daysInMonth = sch.CountDaysInMonth(newY, monthsInYear);
            roundoff += Math.Max(0, d - daysInMonth);

            newM = monthsInYear;
            // On retourne le dernier jour du mois si d > daysInMonth.
            newD = roundoff == 0 ? d : daysInMonth;
        }
        else
        {
            int daysInMonth = sch.CountDaysInMonth(newY, m);
            roundoff = Math.Max(0, d - daysInMonth);

            newM = m;
            // On retourne le dernier jour du mois si d > daysInMonth.
            newD = roundoff == 0 ? d : daysInMonth;
        }

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override TDate AddMonths(int y, int m, int d, int months, out int roundoff)
    {
        int monthsSinceEpoch = checked(Schema.CountMonthsSinceEpoch(y, m) + months);
        if (monthsSinceEpoch < MinMonthsSinceEpoch || monthsSinceEpoch > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();

        Schema.GetMonthParts(monthsSinceEpoch, out int newY, out int newM);

        int daysInMonth = Schema.CountDaysInMonth(newY, newM);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override int CountMonthsBetween(int y0, int m0, int y1, int m1) =>
        checked(Schema.CountMonthsSinceEpoch(y1, m1) - Schema.CountMonthsSinceEpoch(y0, m0));
}
