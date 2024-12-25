﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Prototyping;

using Calendrie.Core.Intervals;

// WARNING: only meant to be used for rapid prototyping.
//
// For explanations, see PrototypalSchema.

/// <summary>
/// Represents a prototype for a non-regular schema and provides a base for
/// derived classes.
/// </summary>
public abstract partial class NonRegularSchemaPrototype : CalendricalSchema
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonRegularSchemaPrototype"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInYear"/>
    /// or <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    protected NonRegularSchemaPrototype(bool proleptic, int minDaysInYear, int minDaysInMonth)
        : this(PrototypeHelpers.GetSupportedYears(proleptic), minDaysInYear, minDaysInMonth) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NonRegularSchemaPrototype"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInYear"/>
    /// or <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    protected NonRegularSchemaPrototype(
        Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth)
    {
        IsProleptic = supportedYears.Min < 1;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is proleptic;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsProleptic { get; }

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = 0;
        return false;
    }
}

public partial class NonRegularSchemaPrototype // Prototypal methods
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
    public override void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
    {
        if (monthsSinceEpoch < 0)
        {
            y = 0;
            int startOfYear = -CountMonthsInYear(0);

            while (monthsSinceEpoch < startOfYear)
            {
                startOfYear -= CountMonthsInYear(--y);
            }

            m = 1 + monthsSinceEpoch - startOfYear;
        }
        else
        {
            y = 1;
            int startOfYear = 0;

            while (monthsSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + CountMonthsInYear(y);
                if (monthsSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(monthsSinceEpoch >= startOfYear);

            m = 1 + monthsSinceEpoch - startOfYear;
        }
    }

    /// <inheritdoc />
    [Pure]
    public override int GetYear(int daysSinceEpoch, out int doy)
    {
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

    /// <inheritdoc />
    [Pure]
    public override int GetYear(int daysSinceEpoch) => GetYear(daysSinceEpoch, out _);

    /// <inheritdoc />
    [Pure]
    public override int GetMonth(int y, int doy, out int d)
    {
        int m = 1;
        int daysInYearBeforeMonth = 0;

        int monthsInYear = CountMonthsInYear(y);
        while (m < monthsInYear)
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
    public override int GetStartOfYearInMonths(int y)
    {
        int monthsSinceEpoch = 0;

        if (y < 1)
        {
            for (int i = y; i < 1; i++)
            {
                monthsSinceEpoch -= CountMonthsInYear(i);
            }
        }
        else
        {
            for (int i = 1; i < y; i++)
            {
                monthsSinceEpoch += CountMonthsInYear(i);
            }
        }

        return monthsSinceEpoch;
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