// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Arithmetic;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Provides a plain implementation of <see cref="CalendricalArithmetic"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class PlainArithmetic : CalendricalArithmetic
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlainArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is
    /// NOT a subinterval of the range of supported years by <paramref name="schema"/>.
    /// </exception>
    public PlainArithmetic(LimitSchema schema, Range<int> supportedYears)
        : base(schema, supportedYears) { }

    //
    // Operations on "Yemoda"
    //

    [Pure]
    public sealed override Yemoda AddYears(Yemoda ymd, int years)
    {
        ymd.Unpack(out int y, out int m, out int d);

        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newM = Math.Min(m, Schema.CountMonthsInYear(newY));
        int newD = Math.Min(d, Schema.CountDaysInMonth(newY, newM));
        return new Yemoda(newY, newM, newD);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemoda AddYears(Yemoda ymd, int years, out int roundoff)
    {
        ymd.Unpack(out int y, out int m, out int d);

        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowDateOverflow();

        var sch = Schema;
        int monthsInYear = sch.CountMonthsInYear(newY);
        if (m > monthsInYear)
        {
            // The target year newY has less months than the year y, we
            // return the end of the target year.
            // roundoff =
            //   "days" after the end of (y, monthsInYear) until (y, m, d) included
            //   + diff between end of (y, monthsInYear) and (newY, monthsInYear)
            roundoff = d;
            for (int i = monthsInYear + 1; i < m; i++)
            {
                roundoff += sch.CountDaysInMonth(y, i);
            }
            int daysInMonth = sch.CountDaysInMonth(newY, monthsInYear);
            roundoff += Math.Max(0, d - daysInMonth);
            return new Yemoda(newY, monthsInYear, roundoff == 0 ? d : daysInMonth);
        }
        else
        {
            int daysInMonth = sch.CountDaysInMonth(newY, m);
            roundoff = Math.Max(0, d - daysInMonth);
            return new Yemoda(newY, m, roundoff == 0 ? d : daysInMonth);
        }
    }

    //
    // Operations on "Yemo"
    //

    /// <inheritdoc />
    [Pure]
    public sealed override Yemo AddMonths(Yemo ym, int months)
    {
        ym.Unpack(out int y, out int m);

        int monthsSinceEpoch = checked(Schema.CountMonthsSinceEpoch(y, m) + months);
        if (monthsSinceEpoch < MinMonthsSinceEpoch || monthsSinceEpoch > MaxMonthsSinceEpoch)
            ThrowHelpers.ThrowMonthOverflow();

        return Schema.GetMonthParts(monthsSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsBetween(Yemo start, Yemo end)
    {
        start.Unpack(out int y0, out int m0);
        end.Unpack(out int y1, out int m1);

        return checked(Schema.CountMonthsSinceEpoch(y1, m1) - Schema.CountMonthsSinceEpoch(y0, m0));
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemo AddYears(Yemo ym, int years)
    {
        ym.Unpack(out int y, out int m);

        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowMonthOverflow();

        int newM = Math.Min(m, Schema.CountMonthsInYear(newY));
        return new Yemo(newY, newM);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemo AddYears(Yemo ym, int years, out int roundoff)
    {
        ym.Unpack(out int y, out int m);

        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowMonthOverflow();

        int monthsInYear = Schema.CountMonthsInYear(newY);
        roundoff = Math.Max(0, m - monthsInYear);
        return new Yemo(newY, roundoff > 0 ? monthsInYear : m);
    }
}
