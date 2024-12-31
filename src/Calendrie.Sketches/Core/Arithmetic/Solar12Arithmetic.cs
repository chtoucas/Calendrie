// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Arithmetic;

using Calendrie.Core.Utilities;

/// <summary>
/// Provides the core mathematical operations on dates for schemas with profile
/// <see cref="CalendricalProfile.Solar12"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class Solar12Arithmetic : SolarArithmetic
{
    private const int MonthsInYear = CalendricalConstants.Solar12.MonthsInYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="Solar12Arithmetic"/> class
    /// with the specified schema.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The underlying schema does not have
    /// the expected profile <see cref="CalendricalProfile.Solar12"/>.</exception>
    public Solar12Arithmetic(CalendricalSegment segment) : base(segment)
    {
        Requires.Profile(Schema, CalendricalProfile.Solar12, nameof(segment));
    }

    //
    // Operations on Yemo
    //

    /// <inheritdoc />
    [Pure]
    public sealed override Yemo AddMonths(Yemo ym, int months)
    {
        ym.Unpack(out int y, out int m);

        m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
        y += y0;
        //YearsValidator.CheckForMonth(y);

        return new Yemo(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsBetween(Yemo start, Yemo end)
    {
        start.Unpack(out int y0, out int m0);
        end.Unpack(out int y1, out int m1);

        return (y1 - y0) * MonthsInYear + m1 - m0;
    }

    //
    // Non-standard operations
    //

    /// <inheritdoc />
    [Pure]
    public sealed override Yemoda AddMonths(Yemoda ymd, int months, out int roundoff)
    {
        ymd.Unpack(out int y, out int m, out int d);

        // On retranche 1 à "m" pour le rendre algébrique.
        m = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
        y += y0;
        YearsValidator.CheckOverflow(y);

        int daysInMonth = Schema.CountDaysInMonth(y, m);
        roundoff = Math.Max(0, d - daysInMonth);
        return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
    }
}
