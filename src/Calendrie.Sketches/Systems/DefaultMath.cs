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
[Obsolete("Use the date methods PlusYears() and co.")]
[ExcludeFromCodeCoverage]
public sealed class DefaultMath<TCalendar, TDate>
    where TCalendar : CalendarSystem<TDate>
    where TDate : struct, IDateable, IAbsoluteDate<TDate>, IUnsafeDateFactory<TDate>
{
    /// <summary>
    /// Represents the calendrical arithmetic.
    /// </summary>
    private readonly CalendricalArithmetic _arithmetic;

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
    public int CountYearsBetween(TDate start, TDate end)
    {
        var (y0, m0, d0) = start;

        // Exact difference between two years.
        int years = end.Year - y0;

        // To avoid extracting y0 twice, we inline:
        // > var newStart = AddYears(start, years);
        var newStart = AddYears(y0, m0, d0, years);
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
        var (y0, m0, d0) = start;
        var (y1, m1, _) = end;

        // Exact difference between two months.
        int months = _arithmetic.CountMonthsBetween(new Yemo(y0, m0), new Yemo(y1, m1));

        // To avoid extracting (y0, m0, d0) twice, we inline:
        // > var newStart = other.PlusMonths(months);
        var newStart = AddMonths(y0, m0, d0, months);
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

    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    private TDate AddYears(int y, int m, int d, int years)
    {
        // NB: Arithmetic.AddYears() is validating.
        var (newY, newM, newD) = _arithmetic.AddYears(new Yemoda(y, m, d), years);

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
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
        var (newY, newM, newD) = _arithmetic.AddMonths(new Yemoda(y, m, d), months);

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }
}
