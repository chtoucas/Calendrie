// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

// TODO(code): create a PaxWeek type.

using Calendrie.Core.Utilities;

public partial class PaxCalendar // Complements
{
    [Pure]
    internal PaxDate AddYears(PaxDate date, int years, out int roundoff)
    {
        var sch = Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out int d);

        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

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

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return PaxDate.UnsafeCreate(daysSinceEpoch);
    }

    [Pure]
    internal PaxDate AddMonths(PaxDate date, int months, out int roundoff)
    {
#if RELEASE
        const int MaxMonthsSinceEpoch = 131_761;
#else
        Debug.Assert(MaxMonthsSinceEpoch == 131_761);
#endif

        var sch = Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out int d);

        int monthsSinceEpoch = checked(sch.CountMonthsSinceEpoch(y, m) + months);
        if (unchecked((uint)monthsSinceEpoch) > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();

        sch.GetMonthParts(monthsSinceEpoch, out int newY, out int newM);

        int daysInMonth = sch.CountDaysInMonth(newY, newM);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return PaxDate.UnsafeCreate(daysSinceEpoch);
    }
}

public partial struct PaxDate // Non-standard math ops
{
    [Pure]
    public PaxDate PlusYears(int years, out int roundoff) =>
        Calendar.AddYears(this, years, out roundoff);

    [Pure]
    public PaxDate PlusMonths(int months, out int roundoff) =>
        Calendar.AddMonths(this, months, out roundoff);
}

public partial struct PaxMonth // Complements
{
    public bool IsPaxMonthOfYear
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
            return sch.IsPaxMonth(y, m);
        }
    }

    public bool IsLastMonthOfYear
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
            return sch.IsLastMonthOfYear(y, m);
        }
    }
}

public partial struct PaxYear // Complements
{
    /// <summary>
    /// Obtains the number of weeks in the current instance.
    /// </summary>
    [Pure]
    public int CountWeeks() => Calendar.Schema.CountWeeksInYear(Year);
}
