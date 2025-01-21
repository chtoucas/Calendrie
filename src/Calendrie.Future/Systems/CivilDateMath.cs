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
public sealed class CivilDateMath : DateMath<CivilDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CivilDateMath"/> class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rule"/>
    /// was not a known member of the enum <see cref="AdditionRule"/>.</exception>
    public CivilDateMath(AdditionRule rule) : base(rule) { }

    /// <inheritdoc />
    [Pure]
    protected sealed override CivilDate AddYearsCore(int y, int m, int d, int years, out int roundoff)
    {
        int newY = checked(y + years);
        if (newY < CivilScope.MinYear || newY > CivilScope.MaxYear) ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = GregorianFormulae.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        int newD = roundoff == 0 ? d : daysInMonth;

        int daysSinceEpoch = CivilFormulae.CountDaysSinceEpoch(newY, m, newD);
        return CivilDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override CivilDate AddMonthsCore(int y, int m, int d, int months, out int roundoff)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), GJSchema.MonthsPerYear, out int years);
        return AddYearsCore(y, newM, d, years, out roundoff);
    }

    /// <inheritdoc />
    [Pure]
    protected sealed override int CountMonthsBetween(int y0, int m0, int y1, int m1) =>
        checked((y1 - y0) * GJSchema.MonthsPerYear + m1 - m0);
}
