// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core.Utilities;

/// <summary>
/// Provides non-standard mathematical operations for the
/// <typeparamref name="TMonth"/> type and provides a base for derived classes.
/// <para>This class allows to customize the <see cref="Calendrie.AdditionRule"/>
/// strategy.</para>
/// </summary>
public class MonthMath<TMonth>
    where TMonth : struct, IMonthBase<TMonth>
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="MonthMath{TMonth}"/> class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rule"/>
    /// was not a known member of the enum <see cref="AdditionRule"/>.</exception>
    public MonthMath(AdditionRule rule)
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
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [Pure]
    public TMonth AddYears(TMonth month, int years)
    {
        var newMonth = month.PlusYears(years, out int roundoff);
        return roundoff == 0 ? newMonth : Adjust(newMonth, roundoff);
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
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountYearsBetween(TMonth start, TMonth end, out TMonth newStart)
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
    /// Adjusts the result using the specified rule.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [Pure]
    private TMonth Adjust(TMonth month, int roundoff)
    {
        // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
        // le cas roundoff = 0 et retourner month (résultat exact).
        Debug.Assert(roundoff > 0);

        // NB: month is the last month of a year.
        return AdditionRule switch
        {
            AdditionRule.Truncate => month,
            AdditionRule.Overspill => month.NextMonth(),
            AdditionRule.Exact => month.PlusMonths(roundoff),

            _ => throw new NotSupportedException(),
        };
    }
}
