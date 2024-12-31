// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Arithmetic;

/// <summary>
/// Defines a plain implementation for <see cref="CalendricalArithmetic"/> and
/// provides a base for derived classes.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class PlainArithmetic : CalendricalArithmetic
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlainArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is
    /// <see langword="null"/>.</exception>
    public PlainArithmetic(CalendricalSegment segment) : base(segment) { }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemo AddMonths(Yemo ym, int months)
    {
        ym.Unpack(out int y, out int m);

        int monthsSinceEpoch = checked(Schema.CountMonthsSinceEpoch(y, m) + months);
        //MonthsValidator.CheckOverflow(monthsSinceEpoch);

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

    /// <inheritdoc />
    [Pure]
    public sealed override Yemoda AddYears(Yemoda ymd, int years, out int roundoff)
    {
        ymd.Unpack(out int y0, out int m, out int d);

        int y = checked(y0 + years);
        YearsValidator.CheckOverflow(y);

        var sch = Schema;
        int monthsInYear = sch.CountMonthsInYear(y);
        if (m > monthsInYear)
        {
            // The target year y has less months than the year y0, we
            // return the end of the target year.
            // roundoff =
            //   "days" after the end of (y0, monthsInYear) until (y0, m, d) included
            //   + diff between end of (y0, monthsInYear) and (y, monthsInYear)
            roundoff = d;
            for (int i = monthsInYear + 1; i < m; i++)
            {
                roundoff += sch.CountDaysInMonth(y0, i);
            }
            m = monthsInYear;
            int daysInMonth = sch.CountDaysInMonth(y, m);
            roundoff += Math.Max(0, d - daysInMonth);
            return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
        }
        else
        {
            int daysInMonth = sch.CountDaysInMonth(y, m);
            roundoff = Math.Max(0, d - daysInMonth);
            return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
        }
    }

    /// <inheritdoc />
    [Pure]
    public sealed override Yemoda AddMonths(Yemoda ymd, int months, out int roundoff)
    {
        int d = ymd.Day;

        var (y, m) = AddMonths(ymd.Yemo, months);
        YearsValidator.CheckOverflow(y);

        int daysInMonth = Schema.CountDaysInMonth(y, m);
        roundoff = Math.Max(0, d - daysInMonth);
        return new Yemoda(y, m, roundoff > 0 ? daysInMonth : d);
    }
}
