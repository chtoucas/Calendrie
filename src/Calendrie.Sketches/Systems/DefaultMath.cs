// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Hemerology;

/// <summary>
/// Defines the non-standard mathematical operations suitable for use with a
/// given calendar.
/// <para>This is strictly equivalent to what we do in <see cref="CalendarSystem{TDate}"/>.
/// </para>
/// <para>This class uses the default <see cref="AdditionRuleset"/> to resolve
/// ambiguities.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class DefaultMath<TCalendar, TDate>
    where TCalendar : CalendarSystem<TDate>
    where TDate : struct, IDateable, IAbsoluteDate<TDate>, IDateFactory<TDate>
{
    /// <summary>
    /// Represents the calendrical arithmetic.
    /// </summary>
    private readonly ICalendricalArithmetic _arithmetic;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultMath{TCalendar, TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is
    /// <see langword="null"/>.</exception>
    public DefaultMath(CalendarSystem<TDate> calendar)
    {
        ArgumentNullException.ThrowIfNull(calendar);

        Calendar = calendar;

        var scope = calendar.Scope;
        _arithmetic = scope.Schema is LimitSchema sch
            ? Arithmetic.CalendricalArithmetic.CreateDefault(sch)
            : throw new NotSupportedException();
    }

    /// <summary>
    /// Gets the calendar.
    /// </summary>
    public CalendarSystem<TDate> Calendar { get; }

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
        var (newY, newM, newD) = _arithmetic.AddYears(y, m, d, years);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
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
        var (newY, newM, newD) = _arithmetic.AddMonths(y, m, d, months);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    [Pure]
    public int CountYearsBetween(TDate start, TDate end)
    {
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
    /// Counts the number of months between the two specified dates.
    /// </summary>
    [Pure]
    public int CountMonthsBetween(TDate start, TDate end)
    {
        var (y0, m0, _) = start;
        var (y1, m1, _) = end;
        int months = _arithmetic.CountMonthsBetween(new Yemo(y0, m0), new Yemo(y1, m1));

        var newStart = AddMonths(start, months);
        if (start < end)
        {
            if (newStart > end) months--;
        }
        else
        {
            if (newStart < end) months++;
        }

        return months;
    }
}
