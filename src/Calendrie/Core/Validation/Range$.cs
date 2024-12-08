// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;

/// <summary>
/// Provides extension methods for <see cref="Range{Int32}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
internal static partial class RangeExtensions
{
    /// <summary>
    /// Validates the specified <see cref="int"/> value.
    /// </summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public static void Validate(this Range<int> range, int value, string? paramName = null)
    {
        if (value < range.Min || value > range.Max)
            throwValueOutOfRange(value, paramName ?? nameof(value));

        [DoesNotReturn]
        static void throwValueOutOfRange(int value, string? paramName = null) =>
            throw new AoorException(
                paramName ?? nameof(value),
                value,
                $"The value was out of range; value = {value}.");
    }
}
