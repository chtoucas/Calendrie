// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core.Utilities;

/// <summary>
/// Provides non-standard mathematical operations for the
/// <typeparamref name="TMonth"/> type and provides a base for derived classes.
/// <para>This class allows to customize the <see cref="Calendrie.AdditionRule"/>
/// strategy.</para>
/// </summary>
public abstract class MonthMath<TMonth, TCalendar>
    where TMonth : struct, IMonth, IMonthFieldMath<TMonth>, IComparisonOperators<TMonth, TMonth, bool>
    where TCalendar : Calendar
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="MonthMath{TMonth, TCalendar}"/> class.
    /// </summary>
    protected MonthMath(AdditionRule rule)
    {
        Requires.Defined(rule);

        AdditionRule = rule;
    }

    /// <summary>
    /// Gets the strategy employed to resolve ambiguities.
    /// </summary>
    public AdditionRule AdditionRule { get; }

    /// <summary>
    /// Adds a number of years to the year field of the specified month.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported months.</exception>
    [Pure]
    public TMonth AddYears(TMonth month, int years)
    {
        var (y, m) = month;
        return AddYears(y, m, years);
    }

    /// <summary>
    /// Counts the number of years between the two specified months.
    /// <para><paramref name="newStart"/> is the result of adding the found
    /// number (of years) to <paramref name="start"/>.</para>
    /// <para>If <paramref name="start"/> &lt;= <paramref name="end"/>, then
    /// <paramref name="start"/> &lt;= <paramref name="newStart"/> &lt;= <paramref name="end"/></para>
    /// <para>If <paramref name="start"/> &gt;= <paramref name="end"/>, then
    /// <paramref name="start"/> &gt;= <paramref name="newStart"/> &gt;= <paramref name="end"/></para>
    /// </summary>
    [Pure]
    public int CountYearsBetween(TMonth start, TMonth end, out TMonth newStart)
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

    [Pure] protected abstract TMonth AddYears(int y, int m, int years, out int roundoff);

    /// <summary>
    /// Adds a number of years to the year field of the specified month.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported months.</exception>
    [Pure]
    protected TMonth AddYears(int y, int m, int years)
    {
        var newMonth = AddYears(y, m, years, out int roundoff);
        return roundoff == 0 ? newMonth : Adjust(newMonth, roundoff);
    }

    [Pure]
    private TMonth Adjust(TMonth month, int roundoff)
    {
        // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
        // le cas roundoff = 0 et retourner month (résultat exact).
        Debug.Assert(roundoff > 0);

        // NB: according to CalendricalArithmetic, date is the last month of the year.
        return AdditionRule switch
        {
            AdditionRule.Truncate => month,
            AdditionRule.Overspill => month.PlusMonths(1),
            AdditionRule.Exact => month.PlusMonths(roundoff),
            AdditionRule.Overflow => throw new OverflowException(),

            _ => throw new NotSupportedException(),
        };
    }
}
