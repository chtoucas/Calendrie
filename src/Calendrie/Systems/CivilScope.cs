﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

// See comments in StandardScope.

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
    internal const int MinYear = StandardScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 9999.</para>
    /// </summary>
    internal const int MaxYear = StandardScope.MaxYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public CivilScope(CivilSchema schema)
        : base(CalendricalSegment.Create(schema, StandardScope.SupportedYears), DayZero.NewStyle)
    {
        // Check the constants Min/MaxYear.
        Debug.Assert(Segment != null);
        Debug.Assert(Segment.SupportedYears == Range.UnsafeCreate(MinYear, MaxYear));
    }

    //
    // Soft validation
    //

    /// <summary>
    /// Checks whether the specified year is valid or not.
    /// <para>The range of supported years being fixed, this method should only
    /// be used by <see cref="CheckYear(int)"/>.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool CheckYearImpl(int year) => year >= MinYear && year <= MaxYear;

    /// <summary>
    /// Checks whether the specified month components are valid or not.
    /// <para>The calendar being regular, this method should only be used by
    /// <see cref="CheckYearMonth(int, int)"/>.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool CheckYearMonthImpl(int year, int month) =>
        year >= MinYear && year <= MaxYear
        && month >= 1 && month <= Solar12.MonthsInYear;

    /// <summary>
    /// Checks whether the specified date components are valid or not.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CheckYearMonthDayImpl(int year, int month, int day) =>
        year >= MinYear && year <= MaxYear
        && month >= 1 && month <= Solar12.MonthsInYear
        && day >= 1
        && (day <= Solar.MinDaysInMonth || day <= GregorianFormulae.CountDaysInMonth(year, month));

    /// <summary>
    /// Checks whether the specified ordinal components are valid or not.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CheckOrdinalImpl(int year, int dayOfYear) =>
        year >= MinYear && year <= MaxYear
        && dayOfYear >= 1
        && (dayOfYear <= Solar.MinDaysInYear || dayOfYear <= GregorianFormulae.CountDaysInYear(year));

    //
    // Hard validation
    //

    /// <summary>
    /// Validates the specified year.
    /// <para>The range of supported years being fixed, this method should only
    /// be used by <see cref="ValidateYear(int, string?)"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ValidateYearImpl(int year, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
    }

    /// <summary>
    /// Validates the specified month.
    /// <para>The calendar being regular, this method should only be used by
    /// <see cref="ValidateYearMonth(int, int, string?)"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ValidateYearMonthImpl(int year, int month, string? paramName = null)
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

    //
    // Soft validation
    //

    /// <inheritdoc />
    public sealed override bool CheckYear(int year) => CheckYearImpl(year);

    /// <inheritdoc />
    public sealed override bool CheckYearMonth(int year, int month) =>
        CheckYearMonthImpl(year, month);

    /// <inheritdoc />
    public sealed override bool CheckYearMonthDay(int year, int month, int day) =>
        CheckYearMonthDayImpl(year, month, day);

    /// <inheritdoc />
    public sealed override bool CheckOrdinal(int year, int dayOfYear) =>
        CheckOrdinalImpl(year, dayOfYear);

    //
    // Hard validation
    //

    /// <inheritdoc />
    public sealed override void ValidateYear(int year, string? paramName = null) =>
        ValidateYearImpl(year, paramName);

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
