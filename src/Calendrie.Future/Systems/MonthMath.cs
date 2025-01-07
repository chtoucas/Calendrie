// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

#if false
public static class MonthMath
{
    public static MonthMath<TMonth, TCalendar> Create<TMonth, TCalendar>(AdditionRule rule)
        where TMonth : struct, IMonth<TMonth>, ICalendarBound<TCalendar>, IUnsafeFactory<TMonth>
        where TCalendar : Calendar
    {
        return TMonth.Calendar.IsRegular(out _)
            ? new MonthMathRegular<TMonth, TCalendar>(rule)
            : new MonthMathPlain<TMonth, TCalendar>(rule);
    }
}
#endif

/// <summary>
/// Defines the non-standard mathematical operations for the
/// <typeparamref name="TMonth"/> type and provides a base for derived classes.
/// <para>This class allows to customize the <see cref="Calendrie.AdditionRule"/>
/// strategy.</para>
/// </summary>
public abstract class MonthMath<TMonth, TCalendar>
    where TMonth : struct, IMonth<TMonth>, ICalendarBound<TCalendar>
    where TCalendar : Calendar
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="MonthMath{TMonth, TCalendar}"/> class.
    /// </summary>
    private protected MonthMath(AdditionRule rule)
    {
        Requires.Defined(rule);

        AdditionRule = rule;

        var scope = TMonth.Calendar.Scope;
        Scope = scope;
        Schema = scope.Schema;

        (MinYear, MaxYear) = scope.Segment.SupportedYears.Endpoints;
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
    /// Gets the earliest supported year.
    /// </summary>
    protected int MinYear { get; }

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    protected int MaxYear { get; }

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
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "F# & VB.NET End statement.")]
    public abstract int CountYearsBetween(TMonth start, TMonth end, out TMonth newStart);

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
            AdditionRule.Overspill => month + 1,
            AdditionRule.Exact => month + roundoff,
            AdditionRule.Overflow => throw new OverflowException(),

            _ => throw new NotSupportedException(),
        };
    }
}
