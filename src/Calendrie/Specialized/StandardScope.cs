// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Validation;
using Calendrie.Hemerology;

/// <summary>
/// Represents a scope for a calendar supporting <i>all</i> dates within the
/// range [1..9999] of years.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class StandardScope : CalendarScope
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
    /// Initializes a new instance of the <see cref="StandardScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    public StandardScope(ICalendricalSchema schema, DayNumber epoch)
        : base(epoch, CalendricalSegment.Create(schema, SupportedYears))
    {
        YearsValidator = new YearsValidator_();
    }

    /// <summary>
    /// Gets the validator for the range [1..9999] of years.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static IYearsValidator YearsValidatorImpl => new YearsValidator_();

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

    /// <summary>
    /// Represents a validator for the range [1..9999] of years.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    private sealed class YearsValidator_ : IYearsValidator
    {
        /// <inheritdoc />
        public Range<int> Range => SupportedYears;

        /// <inheritdoc />
        public void Validate(int year, string? paramName = null)
        {
            if (year < MinYear || year > MaxYear)
                ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        }

        /// <inheritdoc />
        public void CheckOverflow(int year)
        {
            if (year < MinYear || year > MaxYear)
                ThrowHelpers.ThrowDateOverflow();
        }

        /// <inheritdoc />
        public void CheckUpperBound(int year)
        {
            if (year > MaxYear) ThrowHelpers.ThrowDateOverflow();
        }

        /// <inheritdoc />
        public void CheckLowerBound(int year)
        {
            if (year < MinYear) ThrowHelpers.ThrowDateOverflow();
        }
    }
}
