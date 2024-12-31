// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Hemerology;

/// <summary>
/// Defines the non-standard mathematical operations suitable for use with a
/// given calendar and provides a base for derived classes.
/// </summary>
public abstract class CalendarMath<TCalendar, TDate>
    where TCalendar : CalendarSystem<TDate>
    where TDate :
        struct, IDateable, IAbsoluteDate<TDate>, IDateFactory<TDate>, ICalendarBound<TCalendar>
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CalendarMath{TCalendar, TDate}"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is
    /// <see langword="null"/>.</exception>
    protected CalendarMath(CalendarSystem<TDate> calendar, AdditionRuleset additionRuleset)
    {
        ArgumentNullException.ThrowIfNull(calendar);

        Calendar = calendar;
        AdditionRuleset = additionRuleset;

        Arithmetic = CalendricalArithmetic.CreateDefault(calendar.Scope.Segment);
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
    /// Gets the calendrical arithmetic.
    /// </summary>
    protected CalendricalArithmetic Arithmetic { get; }

    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
    public abstract TDate AddYears(TDate date, int years);

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
    public abstract TDate AddMonths(TDate date, int months);

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    [Pure]
    public int CountYearsBetween(TDate start, TDate end, out TDate newStart)
    {
        int years = end.Year - start.Year;
        newStart = AddYears(start, years);

        if (start < end)
        {
            if (newStart > end)
            {
                years--;
                newStart = AddYears(start, years);
            }
        }
        else
        {
            if (newStart < end)
            {
                years++;
                newStart = AddYears(start, years);
            }
        }

        return years;
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// </summary>
    [Pure]
    public int CountMonthsBetween(TDate start, TDate end, out TDate newStart)
    {
        int months = Arithmetic.CountMonthsBetween(convertToYemo(start), convertToYemo(end));
        newStart = AddMonths(start, months);

        if (start < end)
        {
            if (newStart > end)
            {
                months--;
                newStart = AddMonths(start, months);
            }
        }
        else
        {
            if (newStart < end)
            {
                months++;
                newStart = AddMonths(start, months);
            }
        }

        return months;

        [Pure]
        static Yemo convertToYemo(TDate date)
        {
            var (y, m, _) = date;
            return new Yemo(y, m);
        }
    }
}
