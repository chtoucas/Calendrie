﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents the default validator for a range of years.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class YearsValidator : IYearsValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="YearsValidator"/> class.
    /// </summary>
    public YearsValidator(Range<int> range)
    {
        Range = range;
        (MinYear, MaxYear) = range.Endpoints;
    }

    /// <inheritdoc/>
    public Range<int> Range { get; }

    /// <summary>
    /// Gets the ealiest supported year.
    /// </summary>
    public int MinYear { get; }

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public int MaxYear { get; }

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public sealed override string ToString() => Range.ToString();

    /// <inheritdoc/>
    public void Validate(int year, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year, paramName ?? nameof(year));
    }

    /// <inheritdoc/>
    public void CheckOverflow(int year)
    {
        if (year < MinYear || year > MaxYear) ThrowYearOverflow();
    }

    /// <inheritdoc/>
    public void CheckUpperBound(int year)
    {
        if (year > MaxYear) ThrowYearOverflow();
    }

    /// <inheritdoc/>
    public void CheckLowerBound(int year)
    {
        if (year < MinYear) ThrowYearOverflow();
    }

    /// <summary>
    /// The operation would overflow the range of supported years.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn]
    private static void ThrowYearOverflow() =>
        throw new OverflowException("The computation would overflow the range of supported years.");

}
