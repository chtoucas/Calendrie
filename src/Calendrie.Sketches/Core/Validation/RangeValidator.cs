// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;

/// <summary>
/// Represents a validator for a range of (algebraic) values of type <see cref="int"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
[Obsolete("To be removed")]
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
    /// Gets the raw range of values.
    /// </summary>
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

    /// <summary>
    /// Validates the specified value.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public void Validate(int value, string? paramName = null)
    {
        if (value < MinValue || value > MaxValue)
            ThrowValueOutOfRange(value, paramName ?? nameof(value));
    }

    /// <summary>
    /// Checks whether the specified value is outside the range of supported
    /// values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is outside
    /// the range of supported values.</exception>
    public void CheckOverflow(int value)
    {
        if (value < MinValue || value > MaxValue) ThrowValueOverflow();
    }

    /// <summary>
    /// Checks whether the specified value is greater than the upper bound of
    /// the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is greater
    /// than the upper bound of the range of supported values.</exception>
    public void CheckUpperBound(int value)
    {
        if (value > MaxValue) ThrowValueOverflow();
    }

    /// <summary>
    /// Checks whether the specified value is less than the lower bound of the
    /// range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is less than
    /// the lower bound of the range of supported values.</exception>
    public void CheckLowerBound(int value)
    {
        if (value < MinValue) ThrowValueOverflow();
    }

    /// <summary>
    /// The value was out of range.
    /// </summary>
    /// <exception cref="AoorException"/>
    [DoesNotReturn]
    private static void ThrowValueOutOfRange(int value, string? paramName = null) =>
        throw new AoorException(
            paramName ?? nameof(value),
            value,
            $"The value was out of range; value = {value}.");

    /// <summary>
    /// The operation would overflow the range of supported values.
    /// </summary>
    /// <exception cref="OverflowException"/>
    [DoesNotReturn]
    private static void ThrowValueOverflow() =>
        throw new OverflowException("The computation would overflow the range of supported values.");

}
