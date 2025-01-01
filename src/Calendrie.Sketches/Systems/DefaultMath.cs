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
public sealed class DefaultMath<TCalendar, TDate> : CalendarMath<TCalendar, TDate>
    where TCalendar : CalendarSystem<TDate>
    where TDate : struct, IDateable, IAbsoluteDate<TDate>, IDateFactory<TDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultMath{TCalendar, TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is
    /// <see langword="null"/>.</exception>
    public DefaultMath(CalendarSystem<TDate> calendar) : base(calendar, default)
    {
        Debug.Assert(calendar != null);
        if (!calendar.Scope.Schema.IsRegular(out _))
            throw new ArgumentException(null, nameof(calendar));
    }

    /// <inheritdoc />
    [Pure]
    public sealed override TDate AddYears(TDate date, int years)
    {
        var sch = Calendar.Scope.Schema;

        var (y, m, d) = date;
        // NB: Arithmetic.AddYears() is validating.
        var (newY, newM, newD) = Arithmetic.AddYears(new Yemoda(y, m, d), years);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override TDate AddMonths(TDate date, int months)
    {
        var sch = Calendar.Scope.Schema;

        var (y, m, d) = date;
        // NB: Arithmetic.AddMonths() is validating.
        var (newY, newM, newD) = Arithmetic.AddMonths(new Yemoda(y, m, d), months);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }
}
