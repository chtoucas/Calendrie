// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Arithmetic;

using Calendrie.Core.Intervals;

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

    [Pure]
    public sealed override Yemoda AddYears(Yemoda ymd, int years)
    {
        ymd.Unpack(out int y, out int m, out int d);

        y = checked(y + years);
        YearsValidator.CheckOverflow(y);
        // NB: AdditionRule.Truncate.
        m = Math.Min(m, Schema.CountMonthsInYear(y));
        d = Math.Min(d, Schema.CountDaysInMonth(y, m));
        return new Yemoda(y, m, d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemoda AddYears(Yemoda ymd, int years, out int roundoff)
    {
        ymd.Unpack(out int y, out int m, out int d);

        int newY = checked(y + years);
        YearsValidator.CheckOverflow(newY);

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
            m = monthsInYear;
            int daysInMonth = sch.CountDaysInMonth(newY, m);
            roundoff += Math.Max(0, d - daysInMonth);
            return new Yemoda(newY, m, roundoff > 0 ? daysInMonth : d);
        }
        else
        {
            int daysInMonth = sch.CountDaysInMonth(newY, m);
            roundoff = Math.Max(0, d - daysInMonth);
            return new Yemoda(newY, m, roundoff > 0 ? daysInMonth : d);
        }
    }

    [Pure]
    public sealed override Yemoda AddMonths(Yemoda ymd, int months)
    {
        int d = ymd.Day;

        var (y, m) = AddMonths(ymd.Yemo, months);
        //YearsValidator.CheckOverflow(y);
        // NB: AdditionRule.Truncate.
        d = Math.Min(d, Schema.CountDaysInMonth(y, m));
        return new Yemoda(y, m, d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemoda AddMonths(Yemoda ymd, int months, out int roundoff)
    {
        int d = ymd.Day;

        var (y, m) = AddMonths(ymd.Yemo, months);
        //YearsValidator.CheckOverflow(y);

        int daysInMonth = Schema.CountDaysInMonth(y, m);
        roundoff = Math.Max(0, d - daysInMonth);
        return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemo AddMonths(Yemo ym, int months)
    {
        ym.Unpack(out int y, out int m);

        int monthsSinceEpoch = checked(Schema.CountMonthsSinceEpoch(y, m) + months);
        MonthsSinceEpochValidator.CheckOverflow(monthsSinceEpoch);

        return Schema.GetMonthParts(monthsSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsBetween(Yemo start, Yemo end)
    {
        start.Unpack(out int y0, out int m0);
        end.Unpack(out int y1, out int m1);

        return Schema.CountMonthsSinceEpoch(y1, m1) - Schema.CountMonthsSinceEpoch(y0, m0);
    }
}
