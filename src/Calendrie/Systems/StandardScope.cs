﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Represents a scope for a calendar supporting <i>all</i> dates within the
/// range [1..9999] of years.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class StandardScope : CalendarScope
{
    // Even if this class becomes public, these constants MUST stay internal
    // in case we change their values in the future.

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
        YearsValidator = new StandardYearsValidator();
    }

    /// <summary>
    /// Gets the minimum value for the number of consecutive days from the epoch.
    /// </summary>
    public int MinDaysSinceEpoch => Segment.SupportedDays.Min;

    /// <summary>
    /// Gets the maximum value for the number of consecutive days from the epoch.
    /// </summary>
    public int MaxDaysSinceEpoch => Segment.SupportedDays.Max;

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
