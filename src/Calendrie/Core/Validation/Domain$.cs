// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Provides extension methods for <see cref="Range{DayNumber}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class DomainExtensions
{
    /// <summary>
    /// Validates the specified <see cref="DayNumber"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Validate(this Range<DayNumber> domain, DayNumber dayNumber)
    {
        if (dayNumber < domain.Min || dayNumber > domain.Max)
            throwDateOutOfRange(dayNumber, nameof(dayNumber));

        [DoesNotReturn]
        static void throwDateOutOfRange(DayNumber dayNumber, string paramName) =>
            throw new ArgumentOutOfRangeException(
                paramName,
                dayNumber,
                $"The value of the day number was out of range; value = {dayNumber}.");
    }

    /// <summary>
    /// Determines whether the specified <see cref="DayNumber"/> is outside the
    /// range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="dayNumber"/> would
    /// overflow the range of supported values.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckOverflow(this Range<DayNumber> domain, DayNumber dayNumber)
    {
        if (dayNumber < domain.Min || dayNumber > domain.Max) ThrowHelpers.ThrowDateOverflow();
    }

    /// <summary>
    /// Determines whether the specified <see cref="DayNumber"/> is greater than
    /// the upper bound of the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException">The value is greater than the upper
    /// bound of the range of supported values.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckUpperBound(this Range<DayNumber> domain, DayNumber dayNumber)
    {
        if (dayNumber > domain.Max) ThrowHelpers.ThrowDateOverflow();
    }

    /// <summary>
    /// Determines whether the specified <see cref="DayNumber"/> is less than
    /// the lower bound of the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException">The value is less than the lower
    /// bound of the range of supported values.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckLowerBound(this Range<DayNumber> domain, DayNumber dayNumber)
    {
        if (dayNumber < domain.Min) ThrowHelpers.ThrowDateOverflow();
    }
}
