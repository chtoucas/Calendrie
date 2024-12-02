// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;

/// <summary>
/// Represents a validator for a range of (algebraic) values of type <see cref="int"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal sealed class FastRangeValidator : IRangeValidator
{
    /// <summary>
    /// Represents the minimal supported value.
    /// <para>This field is a constant equal to 0.</para>
    /// </summary>
    public const int MinValue = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="RangeValidator"/> class.
    /// </summary>
    public FastRangeValidator(int maxValue)
    {
        // Le constructeur va valider maxValue >= 0.
        Range = new(0, maxValue);
        MaxValue = maxValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RangeValidator"/> class.
    /// </summary>
    public FastRangeValidator(Range<int> range)
    {
        Range = range;
        MaxValue = range.Max;
    }

    /// <inheritdoc/>
    public Range<int> Range { get; }

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
        if (unchecked((uint)value) > MaxValue) ThrowHelpers.ThrowYearOutOfRange(value, paramName);
    }

    /// <inheritdoc/>
    public void CheckOverflow(int value)
    {
        if ((uint)value > MaxValue) ThrowHelpers.ThrowDateOverflow();
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
