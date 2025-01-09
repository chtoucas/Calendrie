// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Utilities;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Provides methods to check the well-formedness of data according to a schema
/// with profile <see cref="CalendricalProfile.Lunar"/>.
/// <para>For such schemas, we can mostly avoid to compute the number of days in
/// a year or in a month.</para>
/// <para>This class cannot be inherited.</para></summary>
internal sealed class LunarPreValidator : ICalendricalPreValidator
{
    /// <summary>
    /// Represents the schema.
    /// </summary>
    private readonly CalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="LunarPreValidator"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public LunarPreValidator(CalendricalSchema schema)
    {
        Requires.Profile(schema, CalendricalProfile.Lunar);

        _schema = schema;
    }

    //
    // Soft validation
    //

    /// <inheritdoc />
    public bool CheckMonth(int y, int month) =>
        month >= 1 && month <= Lunar.MonthsInYear;

    /// <inheritdoc />
    public bool CheckMonthDay(int y, int month, int day) =>
        month >= 1 && month <= Lunar.MonthsInYear
        && day >= 1
        && (day <= Lunar.MinDaysInMonth || day <= _schema.CountDaysInMonth(y, month));

    /// <inheritdoc />
    public bool CheckDayOfYear(int y, int dayOfYear) =>
        dayOfYear >= 1
        && (dayOfYear <= Lunar.MinDaysInYear || dayOfYear <= _schema.CountDaysInYear(y));

    //
    // Hard validation
    //

    /// <inheritdoc />
    public void ValidateMonth(int y, int month, string? paramName = null)
    {
        if (month < 1 || month > Lunar.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
    }

    /// <inheritdoc />
    public void ValidateMonthDay(int y, int month, int day, string? paramName = null)
    {
        if (month < 1 || month > Lunar.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
        if (day < 1
            || (day > Lunar.MinDaysInMonth
                && day > _schema.CountDaysInMonth(y, month)))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null)
    {
        if (dayOfYear < 1
            || (dayOfYear > Lunar.MinDaysInYear
                && dayOfYear > _schema.CountDaysInYear(y)))
        {
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateDayOfMonth(int y, int m, int day, string? paramName = null)
    {
        if (day < 1
            || (day > Lunar.MinDaysInMonth
                && day > _schema.CountDaysInMonth(y, m)))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }
}
