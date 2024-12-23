// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie;
using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

// Main differences with PrototypalSchema:
// - the schema is regular
// - GetYear(int daysSinceEpoch) and GetYear(daysSinceEpoch, out _).

public abstract partial class RegularSchema : CalendricalSchema
{
    protected RegularSchema(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }
}

public partial class RegularSchema // Regular schema
{
    /// <summary>
    /// Gets the number of months in a year.
    /// </summary>
    public abstract int MonthsInYear { get; }

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = MonthsInYear;
        return true;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsIntercalaryMonth(int y, int m) => false;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsInYear(int y) => MonthsInYear;

    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsSinceEpoch(int y, int m) => MonthsInYear * (y - 1) + m - 1;

    /// <inheritdoc />
    public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
    {
        y = 1 + MathZ.Divide(monthsSinceEpoch, MonthsInYear, out int m0);
        m = 1 + m0;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYearInMonths(int y) => MonthsInYear * (y - 1);

    /// <inheritdoc />
    [Pure]
    public sealed override int GetEndOfYearInMonths(int y) => MonthsInYear * y - 1;
}

public partial class RegularSchema // Prototypal methods
{
    /// <inheritdoc />
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

    /// <inheritdoc />
    [Pure]
    public override int GetYear(int daysSinceEpoch)
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

            return y;
        }
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

public partial class RegularSchema
{
    // Only for testing.
    internal static RegularSchema Create(ICalendricalCore kernel, bool proleptic = true)
    {
        ArgumentNullException.ThrowIfNull(kernel);

        var supportedYears = Range.Create(proleptic ? -9998 : 1, 9999);

        return kernel.IsRegular(out int monthsInYear)
            ? new Impl(kernel, supportedYears, monthsInYear)
            : throw new ArgumentException(null, nameof(kernel));
    }

    private sealed class Impl : RegularSchema
    {
        private readonly ICalendricalCore _kernel;

        public Impl(ICalendricalCore kernel, Range<int> supportedYears, int monthsInYear)
            : base(supportedYears, minDaysInYear: 1, minDaysInMonth: 1)
        {
            Debug.Assert(kernel != null);

            _kernel = kernel;
            MonthsInYear = monthsInYear;
        }

        public sealed override int MonthsInYear { get; }

        public sealed override CalendricalFamily Family => _kernel.Family;

        public sealed override CalendricalAdjustments PeriodicAdjustments =>
            _kernel.PeriodicAdjustments;

        public sealed override bool IsLeapYear(int y) => _kernel.IsLeapYear(y);

        public sealed override bool IsIntercalaryDay(int y, int m, int d) =>
            _kernel.IsIntercalaryDay(y, m, d);

        public sealed override bool IsSupplementaryDay(int y, int m, int d) =>
            _kernel.IsSupplementaryDay(y, m, d);

        public sealed override int CountDaysInYear(int y) => _kernel.CountDaysInYear(y);

        public sealed override int CountDaysInMonth(int y, int m) => _kernel.CountDaysInMonth(y, m);
    }
}
