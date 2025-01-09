// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

/// <summary>
/// Provides an implementation of <see cref="ICalendricalPreValidator"/> for
/// the <see cref="PaxSchema"/> type.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class PaxPrevalidator : ICalendricalPreValidator
{
    /// <summary>
    /// Represents the minimal total number of days there is at least in a year.
    /// <para>This field is a constant equal to 364.</para>
    /// </summary>
    private const int MinDaysInYear = 364;

    /// <summary>
    /// Represents the minimal total number of months there is at least in a year.
    /// <para>This field is a constant equal to 13.</para>
    /// </summary>
    private const int MinMonthsInYear = 13;

    /// <summary>
    /// Represents the schema.
    /// </summary>
    private readonly PaxSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaxPrevalidator"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public PaxPrevalidator(PaxSchema schema)
    {
        Debug.Assert(MinDaysInYear == schema.MinDaysInYear);
        Debug.Assert(MinMonthsInYear == schema.MinMonthsInYear);

        ArgumentNullException.ThrowIfNull(schema);

        _schema = schema;
    }

    /// <inheritdoc />
    public void ValidateMonth(int y, int month, string? paramName = null)
    {
        if (month < 1
            || (month > MinMonthsInYear
                && month > _schema.CountMonthsInYear(y)))
        {
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateMonthDay(int y, int month, int day, string? paramName = null)
    {
        if (month < 1
            || (month > MinMonthsInYear
                && month > _schema.CountMonthsInYear(y)))
        {
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
        }
        // No fast track with MinDaysInMonth as it's too small.
        if (day < 1 || day > _schema.CountDaysInMonth(y, month))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateDayOfYear(int y, int dayOfYear, string? paramName = null)
    {
        if (dayOfYear < 1
            || (dayOfYear > MinDaysInYear
                && dayOfYear > _schema.CountDaysInYear(y)))
        {
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
        }
    }

    /// <inheritdoc />
    public void ValidateDayOfMonth(int y, int m, int day, string? paramName = null)
    {
        // No fast track with MinDaysInMonth as it's too small.
        if (day < 1 || day > _schema.CountDaysInMonth(y, m))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }
}
