// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Provides non-standard mathematical operations for the
/// <typeparamref name="TMonth"/> type.
/// <para>This class should only be used with month types for which the
/// underlying calendar isn't regular. Indeed, when the calendar is regular,
/// the operations defined here are always exact.</para>
/// <para>This class allows to customize the <see cref="AdditionRule"/> strategy.
/// </para>
/// </summary>
public class MonthMathPlain<TMonth, TCalendar> : MonthMath0<TMonth>
    where TMonth : struct, IMonth<TMonth>, IUnsafeFactory<TMonth>
    where TCalendar : Calendar
{
    /// <summary>Represents the schema.</summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonthMathPlain{TMonth, TCalendar}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rule"/>
    /// was not a known member of the enum <see cref="AdditionRule"/>.</exception>
    internal MonthMathPlain(AdditionRule rule) : base(rule)
    {
        var scope = TMonth.Calendar.Scope;
        Debug.Assert(scope is StandardScope);

        _schema = scope.Schema;
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override TMonth AddYearsCore(int y, int m, int years, out int roundoff)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < StandardScope.MinYear || newY > StandardScope.MaxYear)
            ThrowHelpers.ThrowMonthOverflow();

        int monthsInYear = _schema.CountMonthsInYear(newY);
        roundoff = Math.Max(0, m - monthsInYear);
        // On retourne le dernier mois de l'année si m > monthsInYear.
        int newM = roundoff == 0 ? m : monthsInYear;

        int monthsSinceEpoch = _schema.CountMonthsSinceEpoch(newY, newM);
        return TMonth.UnsafeCreate(monthsSinceEpoch);
    }
}
