// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie;
using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Represents the proleptic scope of the Julian calendar.
/// <para>Supported dates are within the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class JulianScope : CalendarScope
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
    internal const int MinYear = -999_998;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para>
    /// </summary>
    internal const int MaxYear = 999_999;

    /// <summary>
    /// Represents the range of supported years.
    /// </summary>
    public static readonly Range<int> SupportedYears = Range.Create(MinYear, MaxYear);

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public JulianScope(JulianSchema schema)
        : base(CalendricalSegment.Create(schema, SupportedYears), DayZero.OldStyle)
    {
        // Check the constants Min/MaxYear.
        Debug.Assert(Segment != null);
        Debug.Assert(Segment.SupportedYears == Range.UnsafeCreate(MinYear, MaxYear));
        // Check that this scope uses the largest possible range of years.
        Debug.Assert(schema.SupportedYears == SupportedYears);
    }

    /// <summary>
    /// Checks the specified year.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CheckYearImpl(int year) => year >= MinYear && year <= MaxYear;

    /// <summary>
    /// Checks the specified month components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CheckYearMonthImpl(int year, int month) =>
        year >= MinYear && year <= MaxYear
        && month >= 1 && month <= Solar12.MonthsInYear;

    /// <summary>
    /// Checks the specified date components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CheckYearMonthDayImpl(int year, int month, int day) =>
        year >= MinYear && year <= MaxYear
        && month >= 1 && month <= Solar12.MonthsInYear
        && day >= 1
        && (day <= Solar.MinDaysInMonth || day <= JulianFormulae.CountDaysInMonth(year, month));

    /// <summary>
    /// Checks the specified ordinal components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CheckOrdinalImpl(int year, int dayOfYear) =>
        year >= MinYear && year <= MaxYear
        && dayOfYear >= 1
        && (dayOfYear <= Solar.MinDaysInYear || dayOfYear <= JulianFormulae.CountDaysInYear(year));

    /// <summary>
    /// Validates the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidateYearImpl(int year, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
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
                && day > JulianFormulae.CountDaysInMonth(year, month)))
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
                && dayOfYear > JulianFormulae.CountDaysInYear(year)))
        {
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
        }
    }

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
