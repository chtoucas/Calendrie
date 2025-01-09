// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Represents a scope for a calendar supporting <i>all</i> dates within the
/// range [1..9999] of years.
/// </summary>
internal class StandardScope : CalendarScope
{
    // Even if this class becomes public, these constants MUST stay internal
    // in case we change their values in the future.
    //
    // WARNING: if you change these values, verify that they are compatible with
    // XXXDate.ToString() which expects years to be > 0 and < 10_000, ie
    // length <= 4 and no negative values.
    // More importantly, all year types for non-proleptic calendars expect the
    // year value to fit into an unsigned short.

    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    internal const int MinYear = 1;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 9999.</para>
    /// </summary>
    internal const int MaxYear = 9999;

    /// <summary>
    /// Represents the range of supported years.
    /// </summary>
    public static readonly Range<int> SupportedYears = Range.Create(MinYear, MaxYear);

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public StandardScope(ICalendricalSchema schema, DayNumber epoch)
        : base(CalendricalSegment.Create(schema, SupportedYears), epoch)
    {
        // Check the constants Min/MaxYear.
        Debug.Assert(Segment != null);
        Debug.Assert(Segment.SupportedYears == Range.UnsafeCreate(MinYear, MaxYear));
    }

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

    /// <inheritdoc />
    public sealed override void ValidateYear(int year, string? paramName = null) =>
        ValidateYearImpl(year, paramName);

    /// <inheritdoc />
    public sealed override void ValidateYearMonth(int year, int month, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        PreValidator.ValidateMonth(year, month, paramName);
    }

    /// <inheritdoc />
    public sealed override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        PreValidator.ValidateMonthDay(year, month, day, paramName);
    }

    /// <inheritdoc />
    public sealed override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        PreValidator.ValidateDayOfYear(year, dayOfYear, paramName);
    }
}
