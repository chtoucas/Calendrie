// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Provides non-standard mathematical operations for the <typeparamref name="TDate"/>
/// type when the underlying calendar is <i>regular</i>.
/// <para>This class allows to customize the <see cref="AdditionRule"/> strategy.
/// </para>
/// </summary>
public class DateMathRegular<TDate, TCalendar> : DateMath<TDate>
    where TDate : struct, IDate<TDate>, ICalendarBound<TCalendar>, IUnsafeFactory<TDate>
    where TCalendar : Calendar
{
    /// <summary>Represents the number of months in a year.</summary>
    private readonly int _monthsInYear;

    /// <summary>Represents the schema.</summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateMathRegular{TDate, TCalendar}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rule"/>
    /// was not a known member of the enum <see cref="AdditionRule"/>.</exception>
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
    protected sealed override TDate AddYearsCore(int y, int m, int d, int years, out int roundoff)
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
    protected sealed override TDate AddMonthsCore(int y, int m, int d, int months, out int roundoff)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), _monthsInYear, out int years);
        return AddYearsCore(y, newM, d, years, out roundoff);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override int CountMonthsBetween(int y0, int m0, int y1, int m1) =>
        checked((y1 - y0) * _monthsInYear + m1 - m0);
}
