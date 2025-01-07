// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Provides non-standard mathematical operations for the
/// <typeparamref name="TDate"/> type when the <typeparamref name="TCalendar"/>
/// type is <i>regular</i>.
/// <para>This class allows to customize the <see cref="AdditionRule"/> strategy.
/// </para>
/// </summary>
public class DateMathRegular<TDate, TCalendar> : DateMath<TDate, TCalendar>
    where TDate : struct, IDate<TDate>, ICalendarBound<TCalendar>, IUnsafeFactory<TDate>
    where TCalendar : Calendar
{
    /// <summary>
    /// Represents the schema.
    /// </summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Represents the number of months in a year.
    /// </summary>
    private readonly int _monthsInYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateMathRegular{TDate, TCalendar}"/>
    /// class.
    /// </summary>
    internal DateMathRegular(AdditionRule rule) : base(rule)
    {
        var scope = TDate.Calendar.Scope;
        Debug.Assert(scope is StandardScope);

        var schema = scope.Schema;
        if (!schema.IsRegular(out int monthsInYear)) throw new ArgumentException(null);

        _schema = schema;
        _monthsInYear = monthsInYear;
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override TDate AddYears(int y, int m, int d, int years, out int roundoff)
    {
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = _schema.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = _schema.CountDaysSinceEpoch(newY, m, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override TDate AddMonths(int y, int m, int d, int months, out int roundoff)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), _monthsInYear, out int y0);
#if true
        return AddYears(y, newM, d, y0, out roundoff);
#else
        int newY = checked(y + y0);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = Schema.CountDaysInMonth(newY, newM);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
#endif
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override int CountMonthsBetween(int y0, int m0, int y1, int m1) =>
        checked((y1 - y0) * _monthsInYear + m1 - m0);
}
