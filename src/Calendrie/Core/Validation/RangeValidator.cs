// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;

// FIXME(code): exceptions.

/// <summary>
/// Represents a validator for a range of (algebraic) values.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class RangeValidator : IRangeValidator<int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RangeValidator"/> class.
    /// </summary>
    public RangeValidator(Range<int> range)
    {
        Range = range;
        (MinValue, MaxValue) = range.Endpoints;
    }

    /// <inheritdoc/>
    public Range<int> Range { get; }

    /// <summary>
    /// Gets the minimal supported value.
    /// </summary>
    public int MinValue { get; }

    /// <summary>
    /// Gets the maximal supported value.
    /// </summary>
    public int MaxValue { get; }

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public sealed override string ToString() => Range.ToString();

    /// <inheritdoc/>
    public void Validate(int year, string? paramName = null)
    {
        if (year < MinValue || year > MaxValue) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
    }

    /// <inheritdoc/>
    public void CheckOverflow(int year)
    {
        if (year < MinValue || year > MaxValue) ThrowHelpers.ThrowDateOverflow();
    }

    /// <inheritdoc/>
    public void CheckUpperBound(int year)
    {
        if (year > MaxValue) ThrowHelpers.ThrowDateOverflow();
    }

    /// <inheritdoc/>
    public void CheckLowerBound(int year)
    {
        if (year < MinValue) ThrowHelpers.ThrowDateOverflow();
    }
}
