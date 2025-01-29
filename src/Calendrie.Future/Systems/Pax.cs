// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie;
using Calendrie.Core.Utilities;

// FIXME(code): T4 for AddYears/Months() for non-regular calendars.
// Rework Min/MaxMonthsSinceEpoch.
// TODO(code): create a PaxWeek type.

public partial class PaxCalendar // Math
{
#if RELEASE
    /// <summary>
    /// Represents the maximum value for the number of consecutive months from
    /// the epoch.
    /// <para>This field is a constant equal to 131_761.</para>
    /// </summary>
    private const int MaxMonthsSinceEpoch = 131_761;
#endif

    //
    // PaxDate
    //

    [Pure]
    internal PaxDate AddYears(int y, int m, int d, int years)
    {
        var sch = Schema;

        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newM;
        int newD;
        int monthsInYear = sch.CountMonthsInYear(newY);
        if (m > monthsInYear)
        {
            // Pour le calendrier Pax, cela signifie que "y" est une année
            // bissextile, mais pas "newY", et que m = 14.
            //
            // On retourne le dernier jour valide de l'année (ordinaire) newY
            // autrement dit le 28/13.
            // > newM = monthsInYear;
            // > newD = sch.CountDaysInMonth(newY, monthsInYear);
            newM = 13;
            newD = 28;
        }
        else
        {
            newM = m;
            newD = Math.Min(d, sch.CountDaysInMonth(newY, m));
        }

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return PaxDate.UnsafeCreate(daysSinceEpoch);
    }

    [Pure]
    internal PaxDate AddYears(int y, int m, int d, int years, out int roundoff)
    {
        var sch = Schema;

        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int newM;
        int newD;
        int monthsInYear = sch.CountMonthsInYear(newY);
        if (m > monthsInYear)
        {
            // Pour le calendrier Pax, cela signifie que "y" est une année
            // bissextile, mais pas "newY", et que m = 14.
            roundoff = d;

            // On retourne le dernier jour valide de l'année (ordinaire) newY
            // autrement dit le 28/13.
            // > newM = monthsInYear;
            // > newD = sch.CountDaysInMonth(newY, monthsInYear);
            newM = 13;
            newD = 28;
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
    internal PaxDate AddMonths(int y, int m, int d, int months)
    {
#if DEBUG
        Debug.Assert(MinMonthsSinceEpoch == 0);
        Debug.Assert(MaxMonthsSinceEpoch == 131_761);
#endif
        var sch = Schema;

        // Exact addition of months to a calendar month.
        int monthsSinceEpoch = checked(sch.CountMonthsSinceEpoch(y, m) + months);
        if (unchecked((uint)monthsSinceEpoch) > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();

        sch.GetMonthParts(monthsSinceEpoch, out int newY, out int newM);

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, newM));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return PaxDate.UnsafeCreate(daysSinceEpoch);
    }

    [Pure]
    internal PaxDate AddMonths(int y, int m, int d, int months, out int roundoff)
    {
#if DEBUG
        Debug.Assert(MinMonthsSinceEpoch == 0);
        Debug.Assert(MaxMonthsSinceEpoch == 131_761);
#endif

        var sch = Schema;

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

    //
    // PaxMonth
    //

    /// <summary>
    /// Adds a number of years to the year part of the specified month, yielding
    /// a new date.
    /// <para>This method may truncate the result to ensure that it returns a
    /// valid month; see <see cref="AdditionRule.Truncate"/>.</para>
    /// </summary>
    /// <returns>The last month of the target year when truncation happens.
    /// </returns>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal PaxMonth AddYears(int y, int m, int years)
    {
        var sch = Schema;

        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowMonthOverflow();

        // NB: AdditionRule.Truncate.
        int newM = Math.Min(m, sch.CountMonthsInYear(newY));

        int monthsSinceEpoch = sch.CountMonthsSinceEpoch(newY, newM);
        return PaxMonth.UnsafeCreate(monthsSinceEpoch);
    }

    [Pure]
    internal PaxMonth AddYears(int y, int m, int years, out int roundoff)
    {
        var sch = Schema;

        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowMonthOverflow();

        int monthsInYear = sch.CountMonthsInYear(newY);
        roundoff = Math.Max(0, m - monthsInYear);
        // On retourne le dernier mois de l'année si m > monthsInYear.
        int newM = roundoff == 0 ? m : monthsInYear;

        int monthsSinceEpoch = sch.CountMonthsSinceEpoch(newY, newM);
        return PaxMonth.UnsafeCreate(monthsSinceEpoch);
    }
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
