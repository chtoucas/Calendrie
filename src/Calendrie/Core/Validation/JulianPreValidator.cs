// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Provides methods to check the well-formedness of data in the Julian case.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class JulianPreValidator : ICalendricalPreValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JulianPreValidator"/> class.
    /// </summary>
    private JulianPreValidator() { }

    /// <summary>
    /// Gets a singleton instance of the <see cref="JulianPreValidator"/> class.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static JulianPreValidator Instance { get; } = new();

    //
    // Soft validation
    //

    /// <inheritdoc />
    public bool CheckMonth(int y, int month) =>
        month >= 1 && month <= Solar12.MonthsPerYear;

    /// <inheritdoc />
    public bool CheckMonthDay(int y, int month, int day) =>
        month >= 1 && month <= Solar12.MonthsPerYear
        && day >= 1
        && (day <= Solar.MinDaysPerMonth || day <= JulianFormulae.CountDaysInMonth(y, month));

    /// <inheritdoc />
    public bool CheckDayOfYear(int y, int dayOfYear) =>
        dayOfYear >= 1
        && (dayOfYear <= Solar.MinDaysPerYear || dayOfYear <= JulianFormulae.CountDaysInYear(y));

    //
    // Hard validation
    //

    /// <inheritdoc />
    public void ValidateMonth(int y, int month, string? paramName = null)
    {
        if (month < 1 || month > Solar12.MonthsPerYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
    }

    /// <inheritdoc />
    public void ValidateMonthDay(int y, int month, int day, string? paramName = null)
    {
        if (month < 1 || month > Solar12.MonthsPerYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
        if (day < 1
            || (day > Solar.MinDaysPerMonth
                && day > JulianFormulae.CountDaysInMonth(y, month)))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null)
    {
        if (dayOfYear < 1
            || (dayOfYear > Solar.MinDaysPerYear
                && dayOfYear > JulianFormulae.CountDaysInYear(y)))
        {
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateDayOfMonth(int y, int m, int day, string? paramName = null)
    {
        if (day < 1
            || (day > Solar.MinDaysPerMonth
                && day > JulianFormulae.CountDaysInMonth(y, m)))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }
}
