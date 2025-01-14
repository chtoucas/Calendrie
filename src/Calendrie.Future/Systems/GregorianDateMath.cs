// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Provides non-standard mathematical operations for the <see cref="GregorianDate"/>
/// type.
/// <para>This class allows to customize the <see cref="AdditionRule"/> strategy.
/// </para>
/// </summary>
public sealed class GregorianDateMath : DateMath<GregorianDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianDateMath"/> class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rule"/>
    /// was not a known member of the enum <see cref="AdditionRule"/>.</exception>
    public GregorianDateMath(AdditionRule rule) : base(rule) { }

    /// <inheritdoc />
    [Pure]
    protected sealed override GregorianDate AddYearsCore(
        int y, int m, int d, int years, out int roundoff)
    {
        int newY = checked(y + years);
        if (newY < GregorianScope.MinYear || newY > GregorianScope.MaxYear)
            ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = GregorianFormulae.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = GregorianFormulae.CountDaysSinceEpoch(newY, m, newD);
        return GregorianDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override GregorianDate AddMonthsCore(
        int y, int m, int d, int months, out int roundoff)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), GJSchema.MonthsInYear, out int years);
        return AddYearsCore(y, newM, d, years, out roundoff);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override int CountMonthsBetween(int y0, int m0, int y1, int m1) =>
        checked((y1 - y0) * GJSchema.MonthsInYear + m1 - m0);
}
