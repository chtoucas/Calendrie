// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Hemerology;

/// <summary>
/// Provides a plain implementation for <see cref="CalendarMath{TCalendar, TDate}"/>.
/// <para>This class uses the default <see cref="AdditionRuleset"/> to resolve
/// ambiguities.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class PlainMath<TCalendar, TDate> : CalendarMath<TCalendar, TDate>
    where TCalendar : CalendarSystem<TDate>
    where TDate :
        struct, IDateable, IAbsoluteDate<TDate>, IDateFactory<TDate>, ICalendarBound<TCalendar>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlainMath{TCalendar, TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is
    /// <see langword="null"/>.</exception>
    public PlainMath(CalendarSystem<TDate> calendar) : base(calendar, default) { }

    /// <inheritdoc />
    [Pure]
    public sealed override TDate AddYears(TDate date, int years)
    {
        var chr = TDate.Calendar;
        var scope = chr.Scope;
        var sch = scope.Schema;

        var (y, m, d) = date;
        y = checked(y + years);

        scope.YearsValidator.CheckOverflow(y);

        // NB: AdditionRule.Truncate.
        m = Math.Min(m, sch.CountMonthsInYear(y));
        d = Math.Min(d, sch.CountDaysInMonth(y, m));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(y, m, d);

        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override TDate AddMonths(TDate date, int months)
    {
        var chr = TDate.Calendar;
        var scope = chr.Scope;
        var sch = scope.Schema;

        var (y, m, d) = date;
        var yemo = new Yemo(y, m);

        var (newY, newM) = Arithmetic.AddMonths(yemo, months);

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, newM));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);

        return TDate.UnsafeCreate(daysSinceEpoch);
    }
}
