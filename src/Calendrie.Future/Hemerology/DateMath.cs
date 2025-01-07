// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core;
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
public abstract class DateMath<TDate, TCalendar>
    where TDate : struct, IDateable, IDayFieldMath<TDate>, ICalendarBound<TCalendar>,
        IComparisonOperators<TDate, TDate, bool>
    where TCalendar : Calendar
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="DateMath{TDate, TCalendar}"/> class.
    /// </summary>
    protected DateMath(AdditionRule rule)
    {
        Requires.Defined(rule);

        AdditionRule = rule;

        var scope = TDate.Calendar.Scope;
        Scope = scope;
        Schema = scope.Schema;
    }

    /// <summary>
    /// Gets the strategy employed to resolve ambiguities.
    /// </summary>
    public AdditionRule AdditionRule { get; }

    /// <summary>
    /// Gets the scope.
    /// </summary>
    protected CalendarScope Scope { get; }

    /// <summary>
    /// Gets the schema.
    /// </summary>
    protected ICalendricalSchema Schema { get; }

    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public TDate AddYears(TDate date, int years)
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
    public TDate AddMonths(TDate date, int months)
    {
        var (y, m, d) = date;
        return AddMonths(y, m, d, months);
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
    [Pure]
    public int CountYearsBetween(TDate start, TDate end, out TDate newStart)
    {
        var (y0, m0, d0) = start;

        // Exact difference between two calendar years.
        int years = end.Year - y0;

        // To avoid extracting y0 more than once, we inline:
        // > var newStart = AddYears(start, years);
        newStart = AddYears(y0, m0, d0, years);
        if (start < end)
        {
            if (newStart > end) newStart = AddYears(y0, m0, d0, --years);
        }
        else
        {
            if (newStart < end) newStart = AddYears(y0, m0, d0, ++years);
        }

        return years;
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
    [Pure]
    public int CountMonthsBetween(TDate start, TDate end, out TDate newStart)
    {
        var (y0, m0, d0) = start;
        var (y1, m1, _) = end;

        // Exact difference between two calendar months.
        // REVIEW(code): would be easier if we had a property TDate.CalendarMonth
        // then we could write: end.CalendarMonth - start.CalendarMonth
        int months = CountMonthsBetween(y0, m0, y1, m1);

        // To avoid extracting (y0, m0, d0) more than once, we inline:
        // > var newStart = AddMonths(start, months);
        newStart = AddMonths(y0, m0, d0, months);
        if (start < end)
        {
            if (newStart > end) newStart = AddMonths(y0, m0, d0, --months);
        }
        else
        {
            if (newStart < end) newStart = AddMonths(y0, m0, d0, ++months);
        }

        return months;
    }

    [Pure] protected abstract TDate AddYears(int y, int m, int d, int years, out int roundoff);

    [Pure] protected abstract TDate AddMonths(int y, int m, int d, int months, out int roundoff);

    [Pure] protected abstract int CountMonthsBetween(int y0, int m0, int y1, int m1);

    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private TDate AddYears(int y, int m, int d, int years)
    {
        var newDate = AddYears(y, m, d, years, out int roundoff);
        return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private TDate AddMonths(int y, int m, int d, int months)
    {
        var newDate = AddMonths(y, m, d, months, out int roundoff);
        return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
    }

    [Pure]
    private TDate Adjust(TDate date, int roundoff)
    {
        // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
        // le cas roundoff = 0 et retourner date (résultat exact).
        Debug.Assert(roundoff > 0);

        // NB: according to CalendricalArithmetic, date is the last day of the month.
        return AdditionRule switch
        {
            AdditionRule.Truncate => date,
            // REVIEW(code): there is a slight inefficiency here. We know that
            // the addition won't overflow, do we?
            AdditionRule.Overspill => date.PlusDays(1),
            AdditionRule.Exact => date.PlusDays(roundoff),
            AdditionRule.Overflow => throw new OverflowException(),

            _ => throw new NotSupportedException(),
        };
    }
}
