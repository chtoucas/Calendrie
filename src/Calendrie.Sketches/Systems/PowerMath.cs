// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Hemerology;

// TODO(code): months
// REVIEW(code): move these methods to CalendarSystem (create RegularSystem and
// NonRegularSystem) and offer the possibility to change the ruleset.

public static class PowerMath
{
    public static PowerMath<TDate> Create<TCalendar, TDate>(
        CalendarSystem<TDate> calendar, AdditionRuleset additionRuleset)
        where TCalendar : CalendarSystem<TDate>
        where TDate : struct, IDateable, IAbsoluteDate<TDate>, IUnsafeDateFactory<TDate>
    {
        ArgumentNullException.ThrowIfNull(calendar);

        var scope = calendar.Scope;
        var sch = scope.Schema;

        return sch.IsRegular(out int monthsInYear)
            ? new RegularPowerMath<TDate>(scope, additionRuleset, monthsInYear)
            : new PlainPowerMath<TDate>(scope, additionRuleset);
    }
}

/// <summary>
/// Defines the non-standard mathematical operations suitable for use with a
/// given calendar.
/// <para>This class allows to customize the <see cref="AdditionRuleset"/> used
/// to resolve ambiguities.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public abstract class PowerMath<TDate>
    where TDate : struct, IDateable, IAbsoluteDate<TDate>, IUnsafeDateFactory<TDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PowerMath{TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="scope"/> is
    /// <see langword="null"/>.</exception>
    private protected PowerMath(CalendarScope scope, AdditionRuleset additionRuleset)
    {
        Debug.Assert(scope != null);

        AdditionRuleset = additionRuleset;

        Schema = scope.Schema;

        var seg = scope.Segment;
        (MinYear, MaxYear) = seg.SupportedYears.Endpoints;
        (MinMonthsSinceEpoch, MaxMonthsSinceEpoch) = seg.SupportedMonths.Endpoints;
    }

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
    /// Gets the earliest supported month.
    /// </summary>
    protected int MinMonthsSinceEpoch { get; }

    /// <summary>
    /// Gets the latest supported month.
    /// </summary>
    protected int MaxMonthsSinceEpoch { get; }

    /// <summary>
    /// Gets the strategy employed to resolve ambiguities.
    /// </summary>
    public AdditionRuleset AdditionRuleset { get; }

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
    /// </summary>
    [Pure]
    public int CountYearsBetween(TDate start, TDate end, out TDate newStart)
    {
        var (y0, m0, d0) = start;

        // Exact difference between two years.
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
    /// </summary>
    [Pure]
    public int CountMonthsBetween(TDate start, TDate end, out TDate newStart)
    {
        var (y0, m0, d0) = start;
        var (y1, m1, _) = end;

        // Exact difference between two months.
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
        return AdditionRuleset.DateRule switch
        {
            AdditionRule.Truncate => date,
            // REVIEW(code): there is a slight inefficiency here. We know that
            // the addition won't overflow. We could write
            // > TDate.UnsafeCreate(date.DaysSinceEpoch + 1);
            AdditionRule.Overspill => date + 1,
            AdditionRule.Exact => date + roundoff,
            AdditionRule.Overflow => throw new OverflowException(),

            _ => throw new NotSupportedException(),
        };
    }
}
