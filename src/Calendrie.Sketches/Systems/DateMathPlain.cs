// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Provides non-standard mathematical operations for the
/// <typeparamref name="TDate"/> type.
/// <para>This class allows to customize the <see cref="AdditionRule"/> strategy.
/// </para>
/// </summary>
public class DateMathPlain<TDate, TCalendar> : DateMath0<TDate>
    where TDate : struct, IDate<TDate>, IUnsafeFactory<TDate>
    where TCalendar : Calendar
{
    /// <summary>Represents the smallest possible value of the count of consecutive
    /// days since the epoch.</summary>
    private readonly int _minMonthsSinceEpoch;

    /// <summary>Represents the largest possible value of the count of consecutive
    /// days since the epoch.</summary>
    private readonly int _maxMonthsSinceEpoch;

    /// <summary>Represents the schema.</summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateMathPlain{TCalendar, TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rule"/>
    /// was not a known member of the enum <see cref="AdditionRule"/>.</exception>
    internal DateMathPlain(AdditionRule rule) : base(rule)
    {
        var scope = TDate.Calendar.Scope;
        Debug.Assert(scope is StandardScope);

        _schema = scope.Schema;

        (_minMonthsSinceEpoch, _maxMonthsSinceEpoch) = scope.Segment.SupportedMonths.Endpoints;
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override TDate AddYearsCore(int y, int m, int d, int years, out int roundoff)
    {
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int monthsInYear = _schema.CountMonthsInYear(newY);
        int newM;
        int newD;
        if (m > monthsInYear)
        {
            // The target year newY has less months than the year y, we
            // return the end of the target year.
            // roundoff =
            //   "days" after the end of (y, monthsInYear) until (y, m, d) included
            //   + diff between end of (y, monthsInYear) and (newY, monthsInYear)
            roundoff = d;
            for (int i = monthsInYear + 1; i < m; i++)
            {
                roundoff += _schema.CountDaysInMonth(y, i);
            }
            int daysInMonth = _schema.CountDaysInMonth(newY, monthsInYear);
            roundoff += Math.Max(0, d - daysInMonth);

            newM = monthsInYear;
            // On retourne le dernier jour du mois si d > daysInMonth.
            newD = roundoff == 0 ? d : daysInMonth;
        }
        else
        {
            int daysInMonth = _schema.CountDaysInMonth(newY, m);
            roundoff = Math.Max(0, d - daysInMonth);

            newM = m;
            // On retourne le dernier jour du mois si d > daysInMonth.
            newD = roundoff == 0 ? d : daysInMonth;
        }

        int daysSinceEpoch = _schema.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override TDate AddMonthsCore(int y, int m, int d, int months, out int roundoff)
    {
        int monthsSinceEpoch = checked(_schema.CountMonthsSinceEpoch(y, m) + months);
        if (monthsSinceEpoch < _minMonthsSinceEpoch || monthsSinceEpoch > _maxMonthsSinceEpoch)
            ThrowHelpers.ThrowDateOverflow();

        _schema.GetMonthParts(monthsSinceEpoch, out int newY, out int newM);

        int daysInMonth = _schema.CountDaysInMonth(newY, newM);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = _schema.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override int CountMonthsBetween(int y0, int m0, int y1, int m1) =>
        checked(_schema.CountMonthsSinceEpoch(y1, m1) - _schema.CountMonthsSinceEpoch(y0, m0));
}
