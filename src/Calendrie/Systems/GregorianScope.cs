﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie;
using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the proleptic scope of the Gregorian calendar.
/// <para>Supported dates are within the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class GregorianScope : CalendarScope
{
    // Even if this class becomes public, these constants MUST stay internal
    // in case we change their values in the future.
    //
    // WARNING: if you change these values, verify that it's compatible with
    // XXXDate.ToString() which expects years to be |year| < 1_000_000, ie
    // length <= 6.

    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to -999_998.</para>
    /// </summary>
    internal const int MinYear = ProlepticScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para>
    /// </summary>
    internal const int MaxYear = ProlepticScope.MaxYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public GregorianScope(GregorianSchema schema) :
        base(CalendricalSegment.Create(schema, ProlepticScope.SupportedYears), DayZero.NewStyle)
    {
        Debug.Assert(schema.SupportedYears == ProlepticScope.SupportedYears);

        YearsValidator = new ProlepticYearsValidator();
    }

    /// <summary>
    /// Validates the specified month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
