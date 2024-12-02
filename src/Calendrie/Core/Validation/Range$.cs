// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;

/// <summary>
/// Provides extension methods for <see cref="Range{Int32}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class RangeExtensions
{
    /// <summary>
    /// Validates the specified <see cref="int"/> value.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void Validate(
        this Range<int> range, int value, string? paramName = null)
    {
        if (value < range.Min || value > range.Max)
            ThrowValueOutOfRange(value, paramName ?? nameof(value));
    }

    /// <summary>
    /// Determines whether the specified <see cref="int"/> is outside the
    /// range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> would
    /// overflow the range of supported values.</exception>
    public static void CheckOverflow(this Range<int> range, int value)
    {
        if (value < range.Min || value > range.Max) ThrowValueOverflow();
    }

    /// <summary>
    /// Determines whether the specified <see cref="DayNumber"/> is greater than
    /// the upper bound of the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException">The value is greater than the upper
    /// bound of the range of supported values.</exception>
    public static void CheckUpperBound(this Range<int> range, int value)
    {
        if (value > range.Max) ThrowValueOverflow();
    }

    /// <summary>
    /// Determines whether the specified <see cref="DayNumber"/> is less than
    /// the lower bound of the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException">The value is less than the lower
    /// bound of the range of supported values.</exception>
    public static void CheckLowerBound(this Range<int> range, int value)
    {
        if (value < range.Min) ThrowValueOverflow();
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
