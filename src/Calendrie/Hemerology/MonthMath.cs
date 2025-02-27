﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core.Utilities;

/// <summary>
/// Provides non-standard mathematical operations for the
/// <see cref="IMonthBase{TSelf}"/> type.
/// <para>This class allows to customize the <see cref="Calendrie.AdditionRule"/>
/// strategy.</para>
/// <para>When the underlying calendar is regular, there is little to no reason
/// to use this class. Indeed, <i>all</i> operations are exact, therefore the
/// same can be achieved using only the methods already provided by the
/// <see cref="IMonthBase{TSelf}"/> type.</para>
/// </summary>
public class MonthMath
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MonthMath"/> class using the
    /// default strategy.
    /// </summary>
    public MonthMath() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MonthMath"/> class using the
    /// specified strategy.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="rule"/>
    /// is not a known member of the <see cref="AdditionRule"/> enum.</exception>
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
    /// Computes the exact difference (expressed in years and months) between
    /// the two specified months.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public MonthDifference Subtract<TMonth>(TMonth start, TMonth end)
        where TMonth : struct, IMonthBase<TMonth>
    {
        // Fast track.
        if (start == end) return MonthDifference.Zero;

        int years = CountYearsBetween(start, end, out var newStart);
        int months = end.CountMonthsSince(newStart);
        return MonthDifference.UnsafeCreate(years, months);
    }

    /// <summary>
    /// Adds a number of years to the year field of the specified month.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow either
    /// the capacity of <see cref="int"/> or the range of supported months.
    /// </exception>
    [Pure]
    public TMonth AddYears<TMonth>(TMonth month, int years)
        where TMonth : struct, IMonthBase<TMonth>
    {
        var newMonth = month.PlusYears(years, out int roundoff);
        return roundoff == 0 ? newMonth : Adjust(newMonth, roundoff, AdditionRule);
    }

    /// <summary>
    /// Counts the number of years between the two specified months.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// capacity of <see cref="int"/>.</exception>
    [Pure]
    public int CountYearsBetween<TMonth>(TMonth start, TMonth end)
        where TMonth : struct, IMonthBase<TMonth>
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
    public int CountYearsBetween<TMonth>(TMonth start, TMonth end, out TMonth newStart)
        where TMonth : struct, IMonthBase<TMonth>
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
    internal static TMonth Adjust<TMonth>(TMonth month, int roundoff, AdditionRule rule)
        where TMonth : struct, IMonthBase<TMonth>
    {
        // Si on ne filtrait pas roundoff > 0, il faudrait prendre en compte
        // le cas roundoff = 0 et retourner month (résultat exact).
        Debug.Assert(roundoff > 0);

        // NB: month is the last month of a year.
        return rule switch
        {
            AdditionRule.Truncate => month,
            AdditionRule.Overspill => month.NextMonth(),
            AdditionRule.Exact => month.PlusMonths(roundoff),

            _ => throw new NotSupportedException(),
        };
    }
}
