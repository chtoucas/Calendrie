// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;

/// <summary>
/// Defines a validator for a range of (algebraic) values of integer type.
/// </summary>
public interface IRangeValidator
{
    /// <summary>
    /// Gets the raw range of values.
    /// </summary>
    Range<int> Range { get; }

    /// <summary>
    /// Validates the specified value.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    void Validate(int value, string? paramName = null);

    /// <summary>
    /// Checks whether the specified value is outside the range of supported
    /// values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is outside
    /// the range of supported values.</exception>
    void CheckOverflow(int value);

    /// <summary>
    /// Checks whether the specified value is greater than the upper bound of
    /// the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is greater
    /// than the upper bound of the range of supported values.</exception>
    void CheckUpperBound(int value);

    /// <summary>
    /// Checks whether the specified value is less than the lower bound of the
    /// range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is less than
    /// the lower bound of the range of supported values.</exception>
    void CheckLowerBound(int value);
}
