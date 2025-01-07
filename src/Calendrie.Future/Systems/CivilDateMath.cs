// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Provides non-standard mathematical operations for the <see cref="CivilDate"/>
/// type.
/// <para>This class allows to customize the <see cref="AdditionRule"/> strategy.
/// </para>
/// </summary>
public sealed class CivilDateMath : DateMath<CivilDate, CivilCalendar>
{
    public CivilDateMath(AdditionRule rule) : base(rule) { }

    /// <inheritdoc />
    [Pure]
    protected sealed override CivilDate AddYears(int y, int m, int d, int years, out int roundoff)
    {
        int newY = checked(y + years);
        if (newY < CivilScope.MinYear || newY > CivilScope.MaxYear) ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = GregorianFormulae.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = GregorianFormulae.CountDaysSinceEpoch(newY, m, newD);
        return new CivilDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override CivilDate AddMonths(int y, int m, int d, int months, out int roundoff)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), CivilCalendar.MonthsInYear, out int y0);
        return AddYears(y, newM, d, y0, out roundoff);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override int CountMonthsBetween(int y0, int m0, int y1, int m1) =>
        checked((y1 - y0) * CivilCalendar.MonthsInYear + m1 - m0);
}
