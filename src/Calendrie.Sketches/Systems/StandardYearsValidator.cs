// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents a validator for the range [1..9999] of years.
/// <para>This class cannot be inherited.</para>
/// </summary>
[Obsolete("To be removed")]
internal sealed class StandardYearsValidator : IYearsValidator
{
    // Even if this class becomes public, these constants MUST stay private
    // in case we change their values in the future.

    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    private const int MinYear = StandardScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 9999.</para>
    /// </summary>
    private const int MaxYear = StandardScope.MaxYear;

    /// <inheritdoc />
    public Range<int> Range => StandardScope.SupportedYears;

    /// <inheritdoc />
    public void Validate(int year, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
    }

    /// <inheritdoc />
    public void CheckOverflow(int year)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowDateOverflow();
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
