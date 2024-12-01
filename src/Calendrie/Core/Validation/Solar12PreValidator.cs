// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Provides methods to check the well-formedness of data according to a schema
/// with profile <see cref="CalendricalProfile.Solar12"/>.
/// <para>For such schemas, we can mostly avoid to compute the number of days in
/// a year or in a month.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class Solar12PreValidator : ICalendricalPreValidator
{
    /// <summary>
    /// Represents the schema.
    /// </summary>
    private readonly CalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="Solar12PreValidator"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public Solar12PreValidator(CalendricalSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);
        Requires.Profile(schema, CalendricalProfile.Solar12);

        _schema = schema;
    }

    /// <inheritdoc />
    public void ValidateMonth(int y, int month, string? paramName = null)
    {
        if (month < 1 || month > Solar12.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
    }

    /// <inheritdoc />
    public void ValidateMonthDay(int y, int month, int day, string? paramName = null)
    {
        if (month < 1 || month > Solar12.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
        if (day < 1
            || (day > Solar.MinDaysInMonth
                && day > _schema.CountDaysInMonth(y, month)))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null)
    {
        if (dayOfYear < 1
            || (dayOfYear > Solar.MinDaysInYear
                && dayOfYear > _schema.CountDaysInYear(y)))
        {
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
        }
    }
}
