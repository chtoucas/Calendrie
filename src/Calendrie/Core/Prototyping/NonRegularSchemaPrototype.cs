// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Prototyping;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

// WARNING: only meant to be used for rapid prototyping.
//
// For explanations, see PrototypalSchemaSlim.

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
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
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
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
    public override void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
    {
        const int MinMonthsInYear = 12;

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
    /// method.</remarks>
    [Pure]
    public override int GetYear(int daysSinceEpoch, out int doy)
    {
        int y = 1 + MathZ.Divide(daysSinceEpoch, MinDaysInYear);
        int startOfYear = GetStartOfYear(y);

        if (daysSinceEpoch >= 0)
        {
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
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
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
    /// <remarks>For performance reasons, a derived class SHOULD override this
    /// method.</remarks>
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
