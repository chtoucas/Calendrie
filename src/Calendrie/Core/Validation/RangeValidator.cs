// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;

// FIXME(code): exceptions, custom validator for int >= 0.

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
    public void Validate(int value, string? paramName = null)
    {
        if (value < MinValue || value > MaxValue) ThrowHelpers.ThrowYearOutOfRange(value, paramName);
    }

    /// <inheritdoc/>
    public void CheckOverflow(int value)
    {
        if (value < MinValue || value > MaxValue) ThrowHelpers.ThrowDateOverflow();
    }

    /// <inheritdoc/>
    public void CheckUpperBound(int value)
    {
        if (value > MaxValue) ThrowHelpers.ThrowDateOverflow();
    }

    /// <inheritdoc/>
    public void CheckLowerBound(int value)
    {
        if (value < MinValue) ThrowHelpers.ThrowDateOverflow();
    }
}
