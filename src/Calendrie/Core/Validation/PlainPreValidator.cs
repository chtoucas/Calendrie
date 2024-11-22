// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

/// <summary>
/// Provides a plain implementation for <see cref="ICalendricalPreValidator"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class PlainPreValidator : ICalendricalPreValidator
{
    /// <summary>
    /// Represents the minimum total number of days there is at least in a year.
    /// </summary>
    private readonly int _minDaysInYear;

    /// <summary>
    /// Represents the schema.
    /// </summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainPreValidator"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is <see langword="null"/>.</exception>
    public PlainPreValidator(ICalendricalSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);

        _schema = schema;
        _minDaysInYear = schema.MinDaysInYear;
    }

    /// <inheritdoc />
    public void ValidateMonth(int y, int month, string? paramName = null)
    {
        if (month < 1 || month > _schema.CountMonthsInYear(y))
        {
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateMonthDay(int y, int month, int day, string? paramName = null)
    {
        if (month < 1 || month > _schema.CountMonthsInYear(y))
        {
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
        }
        if (day < 1 || day > _schema.CountDaysInMonth(y, month))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null)
    {
        if (dayOfYear < 1
            || (dayOfYear > _minDaysInYear && dayOfYear > _schema.CountDaysInYear(y)))
        {
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
        }
    }
}
