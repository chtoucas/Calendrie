// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;

// REVIEW(code): optimisation (FastRangeValidator) mais cela nous obligerait à
// utiliser une interface (voir p.ex. CA1859 pour une explication sur les
// conséquences en termes de performance).

/// <summary>
/// Defines a validator for a range of (algebraic) values of type <see cref="int"/>.
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
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.</exception>
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

/// <summary>Defines a validator for a range of (algebraic) values.</summary>
/// <typeparam name="T">The type of the range elements.</typeparam>
[Obsolete("Use IRangeValidator instead")]
internal interface IRangeValidator<in T>
    where T : struct, IEquatable<T>, IComparable<T>
{
    /// <summary>Gets the raw range of values.</summary>
    Range<int> Range { get; }

    /// <summary>Validates the specified value.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.</exception>
    void Validate(T value, string? paramName = null);

    /// <summary>Checks whether the specified value is outside the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of
    /// supported values.</exception>
    void CheckOverflow(T value);

    /// <summary>Checks whether the specified value is greater than the upper bound of the range of
    /// supported values or not.</summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is greater than the upper bound
    /// of the range of supported values.</exception>
    void CheckUpperBound(T value);

    /// <summary>Checks whether the specified value is less than the lower bound of the range of
    /// supported values or not.</summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is less than the lower bound of
    /// the range of supported values.</exception>
    void CheckLowerBound(T value);
}
