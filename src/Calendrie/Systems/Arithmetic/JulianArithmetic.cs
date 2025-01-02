// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems.Arithmetic;

using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

/// <summary>
/// Provides an implementation of <see cref="ICalendricalArithmetic"/> for the
/// Gregorian schema.
/// <para>This class cannot be inherited.</para>
/// <para>See also <see cref="MonthsCalculator"/>.</para>
/// </summary>
internal sealed class JulianArithmetic : ICalendricalArithmetic
{
    /// <summary>
    /// Represents the number of months in a year.
    /// <para>This field is a constant equal to 12.</para>
    /// </summary>
    private const int MonthsInYear = GJSchema.MonthsInYear;

    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to -999_998.</para>
    /// </summary>
    private const int MinYear = JulianScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para>
    /// </summary>
    private const int MaxYear = JulianScope.MaxYear;

    //
    // Operations on "Yemoda"
    //

    [Pure]
    public Yemoda AddYears(int y, int m, int d, int years)
    {
        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, JulianFormulae.CountDaysInMonth(newY, m));
        return new Yemoda(newY, m, newD);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda AddYears(int y, int m, int d, int years, out int roundoff)
    {
        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = JulianFormulae.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        return new Yemoda(newY, m, roundoff == 0 ? d : daysInMonth);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda AddMonths(int y, int m, int d, int months)
    {
        // NB: AddMonths(Yemo, months) is validating and exact.
        var (newY, newM) = AddMonths(y, m, months);

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, JulianFormulae.CountDaysInMonth(newY, newM));
        return new Yemoda(newY, newM, newD);
    }

    /// <inheritdoc />
    [Pure]
    public Yemoda AddMonths(int y, int m, int d, int months, out int roundoff)
    {
        // NB: AddMonths(Yemo, months) is validating and exact.
        var (newY, newM) = AddMonths(y, m, months);

        int daysInMonth = JulianFormulae.CountDaysInMonth(newY, newM);
        roundoff = Math.Max(0, d - daysInMonth);
        return new Yemoda(newY, newM, roundoff == 0 ? d : daysInMonth);
    }

    //
    // Operations on "Yemo"
    //

    /// <inheritdoc />
    [Pure]
    public Yemo AddMonths(int y, int m, int months)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowMonthOverflow();

        return new Yemo(newY, newM);
    }

    /// <inheritdoc />
    [Pure]
    public int CountMonthsBetween(Yemo start, Yemo end)
    {
        start.Unpack(out int y0, out int m0);
        end.Unpack(out int y1, out int m1);

        return checked((y1 - y0) * MonthsInYear + m1 - m0);
    }
}
