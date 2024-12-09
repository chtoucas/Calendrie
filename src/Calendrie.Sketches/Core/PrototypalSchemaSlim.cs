﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

/// <summary>
/// Represents a prototypal implementation of the <see cref="ICalendricalSchemaPlus"/>
/// interface.
/// </summary>
public class PrototypalSchemaSlim : PrototypalSchema
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrototypalSchemaSlim"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.
    /// </exception>
    internal PrototypalSchemaSlim(ICalendricalSchema schema) : base(schema)
    {
        // See GetMonth() for an explanation of the formula.
        ApproxMonthsInYear = 1 + (MinDaysInYear - 1) / MinDaysInMonth;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PrototypalSchemaSlim"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="kernel"/> is null.
    /// </exception>
    internal PrototypalSchemaSlim(ICalendricalKernel kernel, int minDaysInYear, int minDaysInMonth)
        : base(kernel, minDaysInYear, minDaysInMonth)
    {
        // See GetMonth() for an explanation of the formula.
        ApproxMonthsInYear = 1 + (minDaysInYear - 1) / minDaysInMonth;
    }

    /// <inheritdoc />
    public int MinDaysInYear => _minDaysInYear;

    /// <inheritdoc />
    public int MinDaysInMonth => _minDaysInMonth;

    protected int ApproxMonthsInYear { get; }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch, out int doy)
    {
        // It's very similar to what we do in PrototypalSchema, but when we
        // start the loop we are much closer to the actual value of the year.

        // To get our first approximation of the value of the year, we pretend
        // that the years have a constant length equal to MinDaysInYear.
        // > y = 1 + MathZ.Divide(daysSinceEpoch, MinDaysInYear, out int d0y);
        // Notice that the division gives us a zero-based year.
        int y = 1 + MathZ.Divide(daysSinceEpoch, MinDaysInYear);
        int startOfYear = GetStartOfYear(y);

        // TODO(code): explain the algorithm, idem with PrototypalSchema.

        if (daysSinceEpoch >= 0)
        {
            // Notice that the first approximation for the value of the year
            // is greater than or equal to the actual value.
            while (daysSinceEpoch < startOfYear)
            {
                startOfYear -= CountDaysInYear(--y);
            }
        }
        else
        {
            while (daysSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + CountDaysInYear(y);
                if (daysSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(daysSinceEpoch >= startOfYear);
        }

        // Notice that, as expected, doy >= 1.
        doy = 1 + daysSinceEpoch - startOfYear;
        return y;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        // Algorithm:
        // > int m = GetMonth(y, doy);
        // > d = doy - CountDaysInYearBeforeMonth(y, m);
        // > return m;

        int monthsInYear = CountMonthsInYear(y);

        // Base method: at most (monthsInYear - 1) iteration steps.
        // Local method: at most
        //   Math.Min(ApproxMonthsInYear, monthsInYear) - 1
        // iteration steps if doy = MinDaysInYear.
        if (ApproxMonthsInYear > 2 * monthsInYear) { return base.GetMonth(y, doy, out d); }

        // To get our first approximation of the value of the month, we pretend
        // that the months have a constant length equal to MinDaysInMonth.
        // > int m = MathN.AugmentedDivide(doy - 1, MinDaysInMonth, out int d);
        // Notice that the division gives us a zero-based month.
        // We can ignore the remainder of the division which gives a theoretical
        // but wrong (except if the months do actually have a constant length)
        // value for the day of the month.
        int m = Math.Min(1 + (doy - 1) / MinDaysInMonth, monthsInYear);
        int daysInYearBeforeMonth = CountDaysInYearBeforeMonth(y, m);

        while (doy < 1 + daysInYearBeforeMonth)
        {
            //if (m == 1) { daysInYearBeforeMonth = 0; break; }
            daysInYearBeforeMonth -= CountDaysInMonth(y, --m);
        }

        // Notice that, as expected, d >= 1.
        d = doy - daysInYearBeforeMonth;
        return m;
    }
}
