// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the standard scope of the Civil calendar.
/// <para>Supported dates are within the range [1..9999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class CivilScope : CalendarScope
{
    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    public const int MinYear = StandardScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 9999.</para>
    /// </summary>
    public const int MaxYear = StandardScope.MaxYear;

    // See comments in Armenian13Scope for instance.
    public static readonly CivilSchema SchemaT = new();
    public static readonly CivilScope Instance = new(SchemaT);

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public CivilScope(CivilSchema schema) :
        base(DayZero.NewStyle, CalendricalSegment.Create(schema, StandardScope.SupportedYears))
    {
        YearsValidator = StandardScope.YearsValidatorImpl;
    }

    /// <summary>
    /// Validates the specified month.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void ValidateYearMonthImpl(int year, int month, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        if (month < 1 || month > Solar12.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
    }

    /// <summary>
    /// Validates the specified date.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void ValidateYearMonthDayImpl(int year, int month, int day, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        if (month < 1 || month > Solar12.MonthsInYear)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
        if (day < 1
            || (day > Solar.MinDaysInMonth
                && day > GregorianFormulae.CountDaysInMonth(year, month)))
        {
            ThrowHelpers.ThrowDayOutOfRange(day, paramName);
        }
    }

    /// <summary>
    /// Validates the specified ordinal date.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void ValidateOrdinalImpl(int year, int dayOfYear, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        if (dayOfYear < 1
            || (dayOfYear > Solar.MinDaysInYear
                && dayOfYear > GregorianFormulae.CountDaysInYear(year)))
        {
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
        }
    }

    /// <inheritdoc />
    public sealed override void ValidateYearMonth(int year, int month, string? paramName = null) =>
        ValidateYearMonthImpl(year, month, paramName);

    /// <inheritdoc />
    public sealed override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null) =>
        ValidateYearMonthDayImpl(year, month, day, paramName);

    /// <inheritdoc />
    public sealed override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null) =>
        ValidateOrdinalImpl(year, dayOfYear, paramName);
}
