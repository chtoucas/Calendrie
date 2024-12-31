// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Arithmetic;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Provides an implementation of <see cref="CalendricalArithmetic"/> for regular
/// schemas.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class RegularArithmetic : CalendricalArithmetic
{
    private readonly int _monthsInYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegularArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The underlying schema is not regular.
    /// </exception>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is
    /// NOT a subinterval of the range of supported years by <paramref name="schema"/>.
    /// </exception>
    public RegularArithmetic(LimitSchema schema, Range<int> supportedYears)
        : base(schema, supportedYears)
    {
        Debug.Assert(schema != null);

        if (!schema.IsRegular(out int monthsInYear))
            throw new ArgumentException(null, nameof(schema));

        _monthsInYear = monthsInYear;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemoda AddYears(Yemoda ymd, int years, out int roundoff)
    {
        ymd.Unpack(out int y, out int m, out int d);

        y = checked(y + years);
        YearsValidator.CheckOverflow(y);

        int daysInMonth = Schema.CountDaysInMonth(y, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemoda AddMonths(Yemoda ymd, int months, out int roundoff)
    {
        ymd.Unpack(out int y, out int m, out int d);

        // On retranche 1 à "m" pour le rendre algébrique.
        m = 1 + MathZ.Modulo(checked(m - 1 + months), _monthsInYear, out int y0);
        y += y0;
        YearsValidator.CheckOverflow(y);

        int daysInMonth = Schema.CountDaysInMonth(y, m);
        roundoff = Math.Max(0, d - daysInMonth);
        return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemo AddMonths(Yemo ym, int months)
    {
        ym.Unpack(out int y, out int m);

        m = 1 + MathZ.Modulo(checked(m - 1 + months), _monthsInYear, out int y0);
        y += y0;
        YearsValidator.CheckOverflow(y);

        return new Yemo(y, m);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsBetween(Yemo start, Yemo end)
    {
        start.Unpack(out int y0, out int m0);
        end.Unpack(out int y1, out int m1);

        return (y1 - y0) * _monthsInYear + m1 - m0;
    }
}
