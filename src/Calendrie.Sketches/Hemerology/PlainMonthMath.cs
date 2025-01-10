// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Utilities;

/// <summary>
/// Provides non-standard mathematical operations for the
/// <typeparamref name="TMonth"/> type.
/// <para>This class should only be used with month types for which the
/// underlying calendar isn't regular. Indeed, when the calendar is regular,
/// the operations defined here are always exact.</para>
/// <para>This class allows to customize the <see cref="AdditionRule"/> strategy.
/// </para>
/// </summary>
public class PlainMonthMath<TMonth, TCalendar> : MonthMath<TMonth>
    where TMonth : struct, IMonth<TMonth>, ICalendarBound<TCalendar>
    where TCalendar : CalendarSystem
{
    /// <summary>Represents the schema.</summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>Represents the earliest supported year.</summary>
    private readonly int _minYear;

    /// <summary>Represents the latest supported year.</summary>
    private readonly int _maxYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainMonthMath{TMonth, TCalendar}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rule"/>
    /// was not a known member of the enum <see cref="AdditionRule"/>.</exception>
    public PlainMonthMath(AdditionRule rule) : base(rule)
    {
        var scope = TMonth.Calendar.Scope;

        _schema = scope.Schema;

        (_minYear, _maxYear) = scope.Segment.SupportedYears.Endpoints;
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override TMonth AddYearsCore(int y, int m, int years, out int roundoff)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < _minYear || newY > _maxYear) ThrowHelpers.ThrowMonthOverflow();

        int monthsInYear = _schema.CountMonthsInYear(newY);
        roundoff = Math.Max(0, m - monthsInYear);
        // On retourne le dernier mois de l'année si m > monthsInYear.
        int newM = roundoff == 0 ? m : monthsInYear;

        return TMonth.Create(newY, newM);
    }
}
