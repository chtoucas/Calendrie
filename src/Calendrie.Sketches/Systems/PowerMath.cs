// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Hemerology;

/// <summary>
/// Provides non-standard mathematical operations suitable for use with a given
/// calendar.
/// <para>This class allows to customize the <see cref="AdditionRuleset"/> used
/// to resolve ambiguities.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class PowerMath<TCalendar, TDate> : CalendarMath<TCalendar, TDate>
    where TCalendar : CalendarSystem<TDate>
    where TDate : struct, IDateable, IAbsoluteDate<TDate>, IDateFactory<TDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PowerMath{TCalendar, TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is
    /// <see langword="null"/>.</exception>
    public PowerMath(CalendarSystem<TDate> calendar, AdditionRuleset additionRuleset)
        : base(calendar, additionRuleset) { }

    /// <inheritdoc />
    [Pure]
    public sealed override TDate AddYears(TDate date, int years)
    {
        var sch = Calendar.Scope.Schema;

        var (y, m, d) = date;
        // NB: Arithmetic.AddYears() is validating.
        var (newY, newM, newD) = Arithmetic.AddYears(new Yemoda(y, m, d), years, out int roundoff);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        var newDate = TDate.UnsafeCreate(daysSinceEpoch);

        return roundoff > 0 ? Adjust(newDate, roundoff) : newDate;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override TDate AddMonths(TDate date, int months)
    {
        var sch = Calendar.Scope.Schema;

        var (y, m, d) = date;
        // NB: Arithmetic.AddMonths() is validating.
        var (newY, newM, newD) = Arithmetic.AddMonths(new Yemoda(y, m, d), months, out int roundoff);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        var newDate = TDate.UnsafeCreate(daysSinceEpoch);

        return roundoff > 0 ? Adjust(newDate, roundoff) : newDate;
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
            AdditionRule.Overspill => date + 1,
            AdditionRule.Exact => date + roundoff,
            AdditionRule.Overflow => throw new OverflowException(),

            _ => throw new InvalidOperationException(),
        };
    }
}
