// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Hemerology;

/// <summary>
/// Defines the non-standard mathematical operations suitable for use with a
/// given calendar.
/// <para>This class allows to customize the <see cref="AdditionRuleset"/> used
/// to resolve ambiguities.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class PowerMath<TCalendar, TDate>
    where TCalendar : CalendarSystem<TDate>
    where TDate : struct, IDateable, IAbsoluteDate<TDate>, IUnsafeDateFactory<TDate>
{
    /// <summary>
    /// Represents the calendrical arithmetic.
    /// </summary>
    private readonly CalendricalArithmetic _arithmetic;

    /// <summary>
    /// Initializes a new instance of the <see cref="PowerMath{TCalendar, TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is
    /// <see langword="null"/>.</exception>
    public PowerMath(CalendarSystem<TDate> calendar, AdditionRuleset additionRuleset)
    {
        ArgumentNullException.ThrowIfNull(calendar);

        Calendar = calendar;
        AdditionRuleset = additionRuleset;

        var scope = calendar.Scope;
        if (scope.Schema is LimitSchema sch)
        {
            _arithmetic = CalendricalArithmetic.CreateDefault(sch, scope.Segment.SupportedYears);
            Schema = sch;
        }
        else
        {
            throw new ArgumentException(null, nameof(calendar));
        }
    }

    /// <summary>
    /// Gets the calendar.
    /// </summary>
    public CalendarSystem<TDate> Calendar { get; }

    private LimitSchema Schema { get; }

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
        int months = _arithmetic.CountMonthsBetween(new Yemo(y0, m0), new Yemo(y1, m1));

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

    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private TDate AddYears(int y, int m, int d, int years)
    {
        // NB: Arithmetic.AddYears() is validating.
        var (newY, newM, newD) = _arithmetic.AddYears(y, m, d, years, out int roundoff);

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, newM, newD);
        var newDate = TDate.UnsafeCreate(daysSinceEpoch);
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
        // NB: Arithmetic.AddMonths() is validating.
        var (newY, newM, newD) = _arithmetic.AddMonths(y, m, d, months, out int roundoff);

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, newM, newD);
        var newDate = TDate.UnsafeCreate(daysSinceEpoch);
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
