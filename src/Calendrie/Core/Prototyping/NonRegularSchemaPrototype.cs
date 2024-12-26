// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Prototyping;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

// WARNING: only meant to be used for rapid prototyping.

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
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minMonthsInYear"/>,
    /// <paramref name="minDaysInYear"/> or <paramref name="minDaysInMonth"/> is
    /// a negative integer.</exception>
    protected NonRegularSchemaPrototype(
        bool proleptic, int minMonthsInYear, int minDaysInYear, int minDaysInMonth)
        : this(
              PrototypeHelpers.GetSupportedYears(proleptic),
              minMonthsInYear,
              minDaysInYear,
              minDaysInMonth)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NonRegularSchemaPrototype"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minMonthsInYear"/>,
    /// <paramref name="minDaysInYear"/> or <paramref name="minDaysInMonth"/> is
    /// a negative integer.</exception>
    protected NonRegularSchemaPrototype(
        Range<int> supportedYears, int minMonthsInYear, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(minMonthsInYear, 0);

        IsProleptic = supportedYears.Min < 1;

        MinMonthsInYear = minMonthsInYear;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is proleptic;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    public bool IsProleptic { get; }

    /// <summary>
    /// Gets the minimal total number of months there is at least in a year.
    /// </summary>
    public int MinMonthsInYear { get; }

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
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    [Pure]
    public override int CountDaysInYearBeforeMonth(int y, int m)
    {
        // Max nb of iterations = MaxMonthsInYear - 1, ie 11 or 12, at most 13.
        int count = 0;
        for (int i = 1; i < m; i++)
        {
            count += CountDaysInMonth(y, i);
        }
        return count;
    }

    /// <inheritdoc />
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    public override void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
    {
        y = 1 + MathZ.Divide(monthsSinceEpoch, MinMonthsInYear);
        int startOfYear = GetStartOfYearInMonths(y);

        if (monthsSinceEpoch >= 0)
        {
            while (monthsSinceEpoch < startOfYear)
            {
                startOfYear -= CountMonthsInYear(--y);
            }
        }
        else
        {
            while (monthsSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + CountMonthsInYear(y);
                if (monthsSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(monthsSinceEpoch >= startOfYear);
        }

        m = 1 + monthsSinceEpoch - startOfYear;
    }

    /// <inheritdoc />
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method. In fact, one should override <see cref="GetYear(int)"/> using a
    /// computational formulae, and use it to implement this method the way it's
    /// done in <see cref="CalendricalSchema"/>.</remarks>
    [Pure]
    public override int GetYear(int daysSinceEpoch, out int doy)
    {
        // It's very similar to what we do in PrototypalSchema, but when we
        // start the loop we are much closer to the actual value of the year.

        // To get our first approximation of the value of the year, we pretend
        // that the years have a constant length equal to MinDaysInYear.
        // > y = 1 + MathZ.Divide(daysSinceEpoch, MinDaysInYear, out int d0y);
        // Notice that the division gives us a zero-based year.
        int y = 1 + MathZ.Divide(daysSinceEpoch, MinDaysInYear);
        int startOfYear = GetStartOfYear(y);

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
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    [Pure]
    public override int GetYear(int daysSinceEpoch) => GetYear(daysSinceEpoch, out _);

    /// <inheritdoc />
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    [Pure]
    public override int GetMonth(int y, int doy, out int d)
    {
        int m = 1;
        int daysInYearBeforeMonth = 0;

        // Max nb of iterations = MaxMonthsInYear - 1, ie 11 or 12, at most 13.
        int monthsInYear = CountMonthsInYear(y);
        while (m < monthsInYear)
        {
            int daysInYearBeforeNextMonth = CountDaysInYearBeforeMonth(y, m + 1);
            if (doy <= daysInYearBeforeNextMonth) { break; }

            daysInYearBeforeMonth = daysInYearBeforeNextMonth;
            m++;
        }

        d = doy - daysInYearBeforeMonth;
        return m;
    }

    /// <inheritdoc />
    /// <remarks>For performance reasons, a derived class <b>OUGHT TO</b>
    /// override this method.</remarks>
    [Pure]
    public override int GetStartOfYearInMonths(int y)
    {
        int monthsSinceEpoch = 0;

        // Number of iterations = y - 1.
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
    /// <remarks>For performance reasons, a derived class <b>OUGHT TO</b>
    /// override this method.</remarks>
    [Pure]
    public override int GetStartOfYear(int y)
    {
        int daysSinceEpoch = 0;

        // Number of iterations = y - 1.
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
