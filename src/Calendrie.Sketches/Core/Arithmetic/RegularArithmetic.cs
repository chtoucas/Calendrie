// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Arithmetic;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Provides an implementation of <see cref="CalendricalArithmetic"/> for regular
/// schemas.
/// <para>This class cannot be inherited.</para>
/// <para>See also <see cref="MonthsCalculator"/>.</para>
/// </summary>
internal sealed class RegularArithmetic : CalendricalArithmetic
{
    /// <summary>
    /// Represents the number of months in a year.
    /// </summary>
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

    //
    // Operations on "Yemoda"
    //

    [Pure]
    public sealed override Yemoda AddYears(int y, int m, int d, int years)
    {
        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, Schema.CountDaysInMonth(newY, m));
        return new Yemoda(newY, m, newD);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemoda AddYears(int y, int m, int d, int years, out int roundoff)
    {
        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowDateOverflow();

        int daysInMonth = Schema.CountDaysInMonth(newY, m);
        roundoff = Math.Max(0, d - daysInMonth);
        // On retourne le dernier jour du mois si d > daysInMonth.
        return new Yemoda(newY, m, roundoff == 0 ? d : daysInMonth);
    }

    //
    // Operations on "Yemo"
    //

    /// <inheritdoc />
    [Pure]
    public sealed override Yemo AddMonths(int y, int m, int months)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), _monthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowMonthOverflow();

        return new Yemo(newY, newM);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsBetween(Yemo start, Yemo end)
    {
        start.Unpack(out int y0, out int m0);
        end.Unpack(out int y1, out int m1);

        return checked((y1 - y0) * _monthsInYear + m1 - m0);
    }
}
