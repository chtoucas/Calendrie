﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Validation;
using Calendrie.Hemerology;

// TODO(code): extend CalendarScope along the line of MinMaxYearScope but without
// using YearsValidator? Use it instead of MinMaxYearScope in Specialized.

/// <summary>
/// Provides static methods related to the standard scope of a calendar.
/// <para>Supported dates are within the range [1..9999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class StandardScope
{
    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    public const int MinYear = 1;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 9999.</para>
    /// </summary>
    public const int MaxYear = 9999;

    /// <summary>
    /// Represents the range of supported years.
    /// </summary>
    public static readonly Range<int> SupportedYears = Range.Create(MinYear, MaxYear);

    /// <summary>
    /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with
    /// the specified schema and epoch.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="AoorException">The range of supported years by
    /// <paramref name="schema"/> does not contain the interval [1..9999].
    /// </exception>
    public static MinMaxYearScope Create(ICalendricalSchema schema, DayNumber epoch) =>
        new(epoch, CalendricalSegment.Create(schema, SupportedYears))
        {
            YearsValidator = YearsValidatorImpl
        };

    /// <summary>
    /// Gets the validator for the range of supported years.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static IYearsValidator YearsValidatorImpl { get; } = new YearsValidator_();

    private sealed class YearsValidator_ : IYearsValidator
    {
        public Range<int> Range => SupportedYears;

        public void Validate(int year, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        }

        public void CheckOverflow(int year)
        {
            if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowDateOverflow();
        }

        public void CheckUpperBound(int year)
        {
            if (year > MaxYear) ThrowHelpers.ThrowDateOverflow();
        }

        public void CheckLowerBound(int year)
        {
            if (year < MinYear) ThrowHelpers.ThrowDateOverflow();
        }
    }
}