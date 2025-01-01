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
    where TDate : struct, IDateable, IAbsoluteDate<TDate>, IDateFactory<TDate>
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
        _arithmetic = scope.Schema is LimitSchema sch
            ? CalendricalArithmetic.CreateDefault(sch, scope.Segment.SupportedYears)
            : throw new NotSupportedException();
    }

    /// <summary>
    /// Gets the calendar.
    /// </summary>
    public CalendarSystem<TDate> Calendar { get; }

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
        var sch = Calendar.Scope.Schema;

        var (y, m, d) = date;
        // NB: Arithmetic.AddYears() is validating.
        var (newY, newM, newD) = _arithmetic.AddYears(y, m, d, years, out int roundoff);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        var newDate = TDate.UnsafeCreate(daysSinceEpoch);

        return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    [Pure]
    public int CountYearsBetween(TDate start, TDate end, out TDate newStart)
    {
        var sch = Calendar.Scope.Schema;
        var (y0, m0, d0) = start;

        // Exact difference between two years.
        int years = end.Year - start.Year;

        newStart = startPlusYears(years);
        if (start < end)
        {
            if (newStart > end)
            {
                years--;
                newStart = startPlusYears(years);
            }
        }
        else
        {
            if (newStart < end)
            {
                years++;
                newStart = startPlusYears(years);
            }
        }

        return years;

        // AddYears(start, years)
        [Pure]
        TDate startPlusYears(int years)
        {
            // NB: Arithmetic.AddYears() is validating.
            var (newY, newM, newD) = _arithmetic.AddYears(y0, m0, d0, years, out int roundoff);

            int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
            var newDate = TDate.UnsafeCreate(daysSinceEpoch);

            return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
        }
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public TDate AddMonths(TDate date, int months)
    {
        var sch = Calendar.Scope.Schema;

        var (y, m, d) = date;
        // NB: Arithmetic.AddMonths() is validating.
        var (newY, newM, newD) = _arithmetic.AddMonths(y, m, d, months, out int roundoff);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        var newDate = TDate.UnsafeCreate(daysSinceEpoch);

        return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// </summary>
    [Pure]
    public int CountMonthsBetween(TDate start, TDate end, out TDate newStart)
    {
        var sch = Calendar.Scope.Schema;
        var (y0, m0, d0) = start;
        var (y1, m1, _) = end;

        // Exact difference between two months.
        int months = _arithmetic.CountMonthsBetween(new Yemo(y0, m0), new Yemo(y1, m1));

        newStart = startPlusMonths(months);
        if (start < end)
        {
            if (newStart > end)
            {
                months--;
                newStart = startPlusMonths(months);
            }
        }
        else
        {
            if (newStart < end)
            {
                months++;
                newStart = startPlusMonths(months);
            }
        }

        return months;

        // AddMonths(start, months)
        [Pure]
        TDate startPlusMonths(int months)
        {
            // NB: Arithmetic.AddMonths() is validating.
            var (newY, newM, newD) = _arithmetic.AddMonths(y0, m0, d0, months, out int roundoff);

            int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
            var newDate = TDate.UnsafeCreate(daysSinceEpoch);

            return roundoff == 0 ? newDate : Adjust(newDate, roundoff);
        }
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
