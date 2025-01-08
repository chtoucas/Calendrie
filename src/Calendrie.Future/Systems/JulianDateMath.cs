// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Provides non-standard mathematical operations for the <see cref="JulianDate"/>
/// type.
/// <para>This class allows to customize the <see cref="AdditionRule"/> strategy.
/// </para>
/// </summary>
public sealed class JulianDateMath : DateMath<JulianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JulianDateMath"/> class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rule"/>
    /// was not a known member of the enum <see cref="AdditionRule"/>.</exception>
    public JulianDateMath(AdditionRule rule) : base(rule) { }

    /// <inheritdoc />
    [Pure]
    protected sealed override JulianDate AddYearsCore(
        int y, int m, int d, int years, out int roundoff)
    {
        int newY = checked(y + years);
        if (newY < JulianScope.MinYear || newY > JulianScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = JulianFormulae.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(newY, m, newD);
        return new JulianDate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override JulianDate AddMonthsCore(
        int y, int m, int d, int months, out int roundoff)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), JulianCalendar.MonthsInYear, out int years);
        return AddYearsCore(y, newM, d, years, out roundoff);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override int CountMonthsBetween(int y0, int m0, int y1, int m1) =>
        checked((y1 - y0) * JulianCalendar.MonthsInYear + m1 - m0);
}
