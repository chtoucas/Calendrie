// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents a regular schema and provides a base for derived classes.
/// </summary>
public abstract class RegularSchema : CalendricalSchema
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CalendricalSchema"/> class.
    /// <para>All methods MUST work with years in
    /// <see cref="CalendricalSchema.DefaultSupportedYears"/>. In particular,
    /// methods must work with negative years.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInYear"/>
    /// or <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    protected RegularSchema(int minDaysInYear, int minDaysInMonth)
       : this(DefaultSupportedYears, minDaysInYear, minDaysInMonth) { }

    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="RegularSchema"/> class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDaysInYear"/>
    /// or <paramref name="minDaysInMonth"/> is a negative integer.</exception>
    protected RegularSchema(Range<int> supportedYears, int minDaysInYear, int minDaysInMonth)
        : base(supportedYears, minDaysInYear, minDaysInMonth) { }

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
