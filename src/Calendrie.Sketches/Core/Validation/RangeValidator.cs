// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents a validator for a range of (algebraic) values of type
/// <see cref="int"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class RangeValidator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RangeValidator"/> class.
    /// </summary>
    public RangeValidator(Range<int> range)
    {
        Range = range;
        (MinValue, MaxValue) = range.Endpoints;
    }

    /// <summary>
    /// Gets the range of supported values.
    /// </summary>
    public Range<int> Range { get; }

    /// <summary>
    /// Gets the minimum number of consecutive days from the epoch.
    /// </summary>
    public int MinValue { get; }

    /// <summary>
    /// Gets the maximum number of consecutive days from the epoch.
    /// </summary>
    public int MaxValue { get; }

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public sealed override string ToString() => Range.ToString();

    /// <summary>
    /// Validates the specified value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    public void Validate(int value, string? paramName = null)
    {
        if (value < MinValue || value > MaxValue)
            throw new ArgumentOutOfRangeException(paramName ?? nameof(value));
    }

    /// <summary>
    /// Checks whether the specified value is outside the range of supported
    /// values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is
    /// outside the range of supported values.</exception>
    public void CheckOverflow(int value)
    {
        if (value < MinValue || value > MaxValue)
            ThrowHelpers.ThrowDateOverflow();
    }

    /// <summary>
    /// Checks whether the specified value is greater than the upper bound of
    /// the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is
    /// greater than the upper bound of the range of supported values.</exception>
    public void CheckUpperBound(int value)
    {
        if (value > MaxValue) ThrowHelpers.ThrowDateOverflow();
    }

    /// <summary>
    /// Checks whether the specified value is less than the lower bound of the
    /// range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is
    /// less than the lower bound of the range of supported values.</exception>
    public void CheckLowerBound(int value)
    {
        if (value < MinValue) ThrowHelpers.ThrowDateOverflow();
    }
}
