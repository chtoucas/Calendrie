// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Intervals;

// Main difference with CalendricalSchema: GetYear(int daysSinceEpoch) is
// abstract, whereas here it's implemented using GetYear(daysSinceEpoch, out doy).
//
// Main difference with PrototypalSchema: CountMonthsSinceEpoch() is implemented
// using GetStartOfYearInMonths(), whereas here it's abstract.

public abstract class RegularSchemaPrototype : CalendricalSchema
{
    protected RegularSchemaPrototype(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }

    public abstract int MonthsInYear { get; }

    [Pure]
    public override int CountDaysInYearBeforeMonth(int y, int m)
    {
        int count = 0;
        for (int i = 1; i < m; i++)
        {
            count += CountDaysInMonth(y, i);
        }
        return count;
    }

    [Pure]
    public override int CountMonthsSinceEpoch(int y, int m) =>
        new MonthsCalculator.Regular(this, MonthsInYear).GetStartOfYear(y) + m - 1;

    public override void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
    {
        if (monthsSinceEpoch < 0)
        {
            y = 0;
            int startOfYear = -MonthsInYear;

            while (monthsSinceEpoch < startOfYear)
            {
                startOfYear -= MonthsInYear;
            }

            // Notice that, as expected, m >= 1.
            m = 1 + monthsSinceEpoch - startOfYear;
        }
        else
        {
            y = 1;
            int startOfYear = 0;

            while (monthsSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + MonthsInYear;
                if (monthsSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(monthsSinceEpoch >= startOfYear);

            // Notice that, as expected, m >= 1.
            m = 1 + monthsSinceEpoch - startOfYear;
        }
    }

    [Pure]
    public override int GetYear(int daysSinceEpoch, out int doy)
    {
        // Find the year for which (daysSinceEpoch - startOfYear) = d0y
        // has the smallest value >= 0.
        if (daysSinceEpoch < 0)
        {
            int y = 0;
            int startOfYear = -CountDaysInYear(0);

            while (daysSinceEpoch < startOfYear)
            {
                startOfYear -= CountDaysInYear(--y);
            }

            // Notice that, as expected, doy >= 1.
            doy = 1 + daysSinceEpoch - startOfYear;
            return y;
        }
        else
        {
            int y = 1;
            int startOfYear = 0;

            while (daysSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + CountDaysInYear(y);
                if (daysSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(daysSinceEpoch >= startOfYear);

            // Notice that, as expected, doy >= 1.
            doy = 1 + daysSinceEpoch - startOfYear;
            return y;
        }
    }

    [Pure]
    public override int GetYear(int daysSinceEpoch) => GetYear(daysSinceEpoch, out _);

    [Pure]
    public override int GetMonth(int y, int doy, out int d)
    {
        int m = 1;
        int daysInYearBeforeMonth = 0;

        while (m < MonthsInYear)
        {
            int daysInYearBeforeNextMonth = CountDaysInYearBeforeMonth(y, m + 1);
            if (doy <= daysInYearBeforeNextMonth) { break; }

            daysInYearBeforeMonth = daysInYearBeforeNextMonth;
            m++;
        }

        // Notice that, as expected, d >= 1.
        d = doy - daysInYearBeforeMonth;
        return m;
    }

    [Pure]
    public override int GetStartOfYear(int y)
    {
        int daysSinceEpoch = 0;

        if (y < 1)
        {
            for (int i = y; i < 1; i++)
            {
                daysSinceEpoch -= CountDaysInYear(i);
            }
        }
        else
        {
            for (int i = 1; i < y; i++)
            {
                daysSinceEpoch += CountDaysInYear(i);
            }
        }

        return daysSinceEpoch;
    }
}
