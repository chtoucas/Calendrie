// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core.Utilities;

// AddYears() et CountYearsBetween() ne sont pas indépendantes car ce dernier
// utilise le premier pour fonctionner. Les deux méthodes doivent donc utiliser
// la même règle AdditionRule. Pour simplifier, on utilise une règle commune
// définie à la construction de PowerMath. Une autre manière de procéder aurait
// été de définir des méthodes AddYears(TDate date, int years, AdditionRule rule)
// et CountYearsBetween(TDate start, TDate end, AdditionRule rule).

/// <summary>
/// Provides non-standard mathematical operations for the
/// <typeparamref name="TDate"/> type and provides a base for derived classes.
/// <para>This class allows to customize the <see cref="Calendrie.AdditionRule"/>
/// strategy.</para>
/// </summary>
public abstract class DateMath<TDate>
    where TDate : struct, IDateable, IDayFieldMath<TDate>, IComparisonOperators<TDate, TDate, bool>
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="DateMath{TDate}"/> class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rule"/>
    /// was not a known member of the enum <see cref="AdditionRule"/>.</exception>
    protected DateMath(AdditionRule rule)
    {
        Requires.Defined(rule);

        AdditionRule = rule;
    }

    /// <summary>
    /// Gets the strategy employed to resolve ambiguities.
    /// </summary>
    public AdditionRule AdditionRule { get; }

    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public TDate AddYears(TDate date, int years)
    {
        var (y, m, d) = date;
        var newDate = AddYearsCore(y, m, d, years, out int roundoff);
        return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
    }

    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure] protected abstract TDate AddYearsCore(int y, int m, int d, int years, out int roundoff);

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure]
    public TDate AddMonths(TDate date, int months)
    {
        var (y, m, d) = date;
        var newDate = AddMonthsCore(y, m, d, months, out int roundoff);
        return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported dates.
    /// </exception>
    [Pure] protected abstract TDate AddMonthsCore(int y, int m, int d, int months, out int roundoff);

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
        var (y0, m0, d0) = start;

        // Exact difference between two calendar years.
        int years = end.Year - y0;

        // To avoid extracting (y0, m0) more than once, we inline:
        // > var newStart = AddYears(start, years);
        newStart = addYears(y0, m0, d0, years);
        if (start < end)
        {
            if (newStart > end) newStart = addYears(y0, m0, d0, --years);
            Debug.Assert(newStart <= end);
        }
        else
        {
            if (newStart < end) newStart = addYears(y0, m0, d0, ++years);
            Debug.Assert(newStart >= end);
        }

        return years;

        [Pure]
        TDate addYears(int y, int m, int d, int years)
        {
            var newDate = AddYearsCore(y, m, d, years, out int roundoff);
            return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
        }
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
        var (y0, m0, d0) = start;
        var (y1, m1, _) = end;

        // Exact difference between two calendar months.
        int months = CountMonthsBetween(y0, m0, y1, m1);

        // To avoid extracting (y0, m0, d0) more than once, we inline:
        // > var newStart = AddMonths(start, months);
        newStart = addMonths(y0, m0, d0, months);
        if (start < end)
        {
            if (newStart > end) newStart = addMonths(y0, m0, d0, --months);
            Debug.Assert(newStart <= end);
        }
        else
        {
            if (newStart < end) newStart = addMonths(y0, m0, d0, ++months);
            Debug.Assert(newStart >= end);
        }

        return months;

        [Pure]
        TDate addMonths(int y, int m, int d, int months)
        {
            var newDate = AddMonthsCore(y, m, d, months, out int roundoff);
            return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
        }
    }

    /// <summary>
    /// Counts the (exact) number of months between the two specified months.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure] protected abstract int CountMonthsBetween(int y0, int m0, int y1, int m1);

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
