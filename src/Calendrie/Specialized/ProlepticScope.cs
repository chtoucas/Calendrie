// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using Calendrie.Core.Intervals;
using Calendrie.Core.Validation;

/// <summary>
/// Provides static methods related to the (proleptic) scope of a calendar
/// supporting <i>all</i> dates within the range [-999_998..999_999] of years.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static class ProlepticScope
{
    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to -999_998.</para>
    /// </summary>
    public const int MinYear = -999_998;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para>
    /// </summary>
    public const int MaxYear = 999_999;

    /// <summary>
    /// Represents the range of supported years.
    /// </summary>
    public static readonly Range<int> SupportedYears = Range.Create(MinYear, MaxYear);

    /// <summary>
    /// Gets the validator for the range [-999_998..999_999] of years.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static IYearsValidator YearsValidatorImpl => new YearsValidator_();

    /// <summary>
    /// Represents a validator for the range [-999_998..999_999] of years.
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
