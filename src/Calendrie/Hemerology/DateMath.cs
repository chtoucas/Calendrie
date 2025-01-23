// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core.Utilities;

// AddYears() et CountYearsBetween() ne sont pas indépendantes car ce dernier
// utilise le premier pour fonctionner. Les deux méthodes doivent donc utiliser
// la même règle AdditionRule.

/// <summary>
/// Provides non-standard mathematical operations for the
/// <typeparamref name="TDate"/> type.
/// <para>This class allows to customize the <see cref="Calendrie.AdditionRule"/>
/// strategy.</para>
/// </summary>
public class DateMath<TDate> where TDate : struct, IDateBase<TDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateMath{TDate}"/> class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="rule"/>
    /// is not a known member of the <see cref="AdditionRule"/> enum.</exception>
    public DateMath(AdditionRule rule)
    {
        Requires.Defined(rule);

        AdditionRule = rule;
    }

    /// <summary>
    /// Gets the strategy employed to resolve ambiguities.
    /// </summary>
    public AdditionRule AdditionRule { get; }

    /// <summary>
    /// Calculates the exact difference (expressed in years, months and days)
    /// between the two specified dates.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public (int Years, int Months, int Days) Subtract(TDate start, TDate end)
    {
        int years = CountYearsBetween(start, end, out var newStart);
        int months = CountMonthsBetween(newStart, end, out newStart);
        int days = end.CountDaysSince(newStart);
        return (years, months, days);
    }

    /// <summary>
    /// Adds a number of years to the year field of the specified date, yielding
    /// a new date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public TDate AddYears(TDate date, int years)
    {
        var newDate = date.PlusYears(years, out int roundoff);
        return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public TDate AddMonths(TDate date, int months)
    {
        var newDate = date.PlusMonths(months, out int roundoff);
        return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountYearsBetween(TDate start, TDate end)
    {
        // Exact difference between two calendar years.
        int years = end.Year - start.Year;

        var newStart = AddYears(start, years);
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
    /// Counts the number of years between the two specified dates.
    /// <para><paramref name="newStart"/> is the result of adding the found
    /// number (of years) to <paramref name="start"/>.</para>
    /// <para>If <paramref name="start"/> &lt;= <paramref name="end"/>, then
    /// <paramref name="start"/> &lt;= <paramref name="newStart"/> &lt;= <paramref name="end"/></para>
    /// <para>If <paramref name="start"/> &gt;= <paramref name="end"/>, then
    /// <paramref name="start"/> &gt;= <paramref name="newStart"/> &gt;= <paramref name="end"/></para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountYearsBetween(TDate start, TDate end, out TDate newStart)
    {
        // Exact difference between two calendar years.
        int years = end.Year - start.Year;

        newStart = AddYears(start, years);
        if (start < end)
        {
            if (newStart > end) newStart = AddYears(start, --years);
            Debug.Assert(newStart <= end);
        }
        else
        {
            if (newStart < end) newStart = AddYears(start, ++years);
            Debug.Assert(newStart >= end);
        }

        return years;
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountMonthsBetween(TDate start, TDate end)
    {
        var (y0, m0, _) = start;
        var (y1, m1, _) = end;

        // Exact difference between two calendar months.
        int months = CountMonthsBetween(y0, m0, y1, m1);

        var newStart = AddMonths(start, months);
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
    /// Counts the number of months between the two specified dates.
    /// <para><paramref name="newStart"/> is the result of adding the found
    /// number (of months) to <paramref name="start"/>.</para>
    /// <para>If <paramref name="start"/> &lt;= <paramref name="end"/>, then
    /// <paramref name="start"/> &lt;= <paramref name="newStart"/> &lt;= <paramref name="end"/></para>
    /// <para>If <paramref name="start"/> &gt;= <paramref name="end"/>, then
    /// <paramref name="start"/> &gt;= <paramref name="newStart"/> &gt;= <paramref name="end"/></para>
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountMonthsBetween(TDate start, TDate end, out TDate newStart)
    {
        var (y0, m0, _) = start;
        var (y1, m1, _) = end;

        // Exact difference between two calendar months.
        int months = CountMonthsBetween(y0, m0, y1, m1);

        newStart = AddMonths(start, months);
        if (start < end)
        {
            if (newStart > end) newStart = AddMonths(start, --months);
            Debug.Assert(newStart <= end);
        }
        else
        {
            if (newStart < end) newStart = AddMonths(start, ++months);
            Debug.Assert(newStart >= end);
        }

        return months;
    }

    /// <summary>
    /// Counts the (exact) number of months between the two specified months.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    private static int CountMonthsBetween(int y0, int m0, int y1, int m1)
    {
        var sch = TDate.Calendar.Scope.Schema;
        return checked(sch.CountMonthsSinceEpoch(y1, m1) - sch.CountMonthsSinceEpoch(y0, m0));
    }

    /// <summary>
    /// Adjusts the result using the specified rule.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    private TDate Adjust(TDate date, int roundoff)
    {
        // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
        // le cas roundoff = 0 et retourner date (résultat exact).
        Debug.Assert(roundoff > 0);

        // NB: date is the last day of a month.
        return AdditionRule switch
        {
            AdditionRule.Truncate => date,
            AdditionRule.Overspill => date.NextDay(),
            AdditionRule.Exact => date.PlusDays(roundoff),

            _ => throw new NotSupportedException(),
        };
    }
}
