// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Utilities;

public partial class PaxCalendar // Complements
{
    /// <summary>
    /// Obtains the number of months in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure]
    public int CountMonthsInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountMonthsInYear(year);
    }

    /// <summary>
    /// Obtains the number of weeks in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure]
    public int CountWeeksInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountWeeksInYear(year);
    }
}

public partial struct PaxDate // Complements
{
}

public partial struct PaxDate // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of this date instance, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public PaxDate PlusYears(int years)
    {
        var (y, m, d) = this;
        return AddYears(y, m, d, years);
    }

    /// <summary>
    /// Adds a number of months to the month field of this date instance,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public PaxDate PlusMonths(int months)
    {
        var (y, m, d) = this;
        return AddMonths(y, m, d, months);
    }

    /// <summary>
    /// Counts the number of years elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountYearsSince(PaxDate other)
    {
        var (y0, m0, d0) = other;

        // Exact difference between two calendar years.
        int years = Year - y0;

        // To avoid extracting y0 twice, we inline:
        // > var newStart = other.PlusYears(years);
        var newStart = AddYears(y0, m0, d0, years);
        if (other < this)
        {
            if (newStart > this) years--;
        }
        else
        {
            if (newStart < this) years++;
        }

        return years;
    }

    /// <summary>
    /// Counts the number of months elapsed since the specified date.
    /// </summary>
    [Pure]
    public int CountMonthsSince(PaxDate other)
    {
        var sch = Calendar.Schema;

        var (y, m, _) = this;
        var (y0, m0, d0) = other;

        // Exact difference between two calendar months.
        int months = checked(sch.CountMonthsSinceEpoch(y, m) - sch.CountMonthsSinceEpoch(y0, m0));

        // To avoid extracting (y0, m0, d0) twice, we inline:
        // > var newStart = other.PlusMonths(months);
        var newStart = AddMonths(y0, m0, d0, months);

        if (other < this)
        {
            if (newStart > this) months--;
        }
        else
        {
            if (newStart < this) months++;
        }

        return months;
    }

    /// <summary>
    /// Adds a number of years to the year field of the specified date, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static PaxDate AddYears(int y, int m, int d, int years)
    {
        var sch = Calendar.Schema;

        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newM = Math.Min(m, sch.CountMonthsInYear(newY));
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, m));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return new PaxDate(daysSinceEpoch);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date,
    /// yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private static PaxDate AddMonths(int y, int m, int d, int months)
    {
        var sch = Calendar.Schema;

        // Exact addition of months to a calendar month.
        int monthsSinceEpoch = checked(sch.CountMonthsSinceEpoch(y, m) + months);
        // FIXME(code): validation of monthsSinceEpoch.
        var (minMonthsSinceEpoch, maxMonthsSinceEpoch) =
            Calendar.Scope.Segment.SupportedMonths.Endpoints;
        if (monthsSinceEpoch < minMonthsSinceEpoch || monthsSinceEpoch > maxMonthsSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();

        sch.GetMonthParts(monthsSinceEpoch, out int newY, out int newM);

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, newM));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return new PaxDate(daysSinceEpoch);
    }
}
