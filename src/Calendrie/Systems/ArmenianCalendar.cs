// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Utilities;

public partial class ArmenianCalendar // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal ArmenianDate AddYears(ArmenianDate date, int years)
    {
        var (y, m, d) = date;
        return AddYears(y, m, d, years);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal ArmenianDate AddMonths(ArmenianDate date, int months)
    {
        var (y, m, d) = date;
        return AddMonths(y, m, d, months);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    [Pure]
    internal int CountYearsBetween(ArmenianDate start, ArmenianDate end)
    {
        var (y0, m0, d0) = start;

        // Exact difference between two calendar years.
        int years = end.Year - y0;

        // To avoid extracting y0 twice, we inline:
        // > var newStart = AddYears(start, years);
        var newStart = AddYears(y0, m0, d0, years);
        if (start < end)
        {
            if (newStart > end) years--;
        }
        else
        {
            if (newStart < end) years++;
        }

        return years;
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// </summary>
    [Pure]
    internal int CountMonthsBetween(ArmenianDate start, ArmenianDate end)
    {
        var (y, m, _) = end;
        var (y0, m0, d0) = start;

        // Exact difference between two calendar months.
        int months = checked(MonthsInYear * (y - y0) + m - m0);

        // To avoid extracting (y0, m0, d0) twice, we inline:
        // > var newStart = AddMonths(start, months);
        var newStart = AddMonths(y0, m0, d0, months);

        if (start < end)
        {
            if (newStart > end) months--;
        }
        else
        {
            if (newStart < end) months++;
        }

        return months;
    }

    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private ArmenianDate AddYears(int y, int m, int d, int years)
    {
        var sch = Schema;

        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, m));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, m, newD);
        return new ArmenianDate(daysSinceEpoch);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private ArmenianDate AddMonths(int y, int m, int d, int months)
    {
        var sch = Schema;

        // Exact addition of months to a calendar month.
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, newM));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return new ArmenianDate(daysSinceEpoch);
    }
}
