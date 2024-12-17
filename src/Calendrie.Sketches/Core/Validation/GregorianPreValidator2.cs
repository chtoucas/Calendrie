// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

using static Calendrie.Core.CalendricalConstants;

internal static class GregorianPreValidator2
{
    #region 64-bit versions

    /// <summary>
    /// Validates the well-formedness of the specified month of the year and day
    /// of the month.
    /// <para>This method does NOT validate <paramref name="y"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    public static void ValidateMonthDay(long y, int month, int day, string? paramName = null)
    {
        if (month < 1 || month > Solar12.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
        if (day < 1
            || (day > Solar.MinDaysInMonth
                && day > GregorianFormulae2.CountDaysInMonth(y, month)))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }

    /// <summary>
    /// Validates the well-formedness of the specified day of the year.
    /// <para>This method does NOT validate <paramref name="y"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    public static void ValidateDayOfYear(long y, int dayOfYear, string? paramName = null)
    {
        if (dayOfYear < 1
            || (dayOfYear > Solar.MinDaysInYear
                && dayOfYear > GregorianFormulae2.CountDaysInYear(y)))
        {
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
        }
    }

    #endregion
}
