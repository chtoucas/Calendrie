// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;

/// <summary>Represents a validator for a range of days, that is the range of supported numbers of
/// consecutive days from the epoch.
/// <para>This class cannot be inherited.</para></summary>
public sealed class DaysValidator : IRangeValidator<int>
{
    /// <summary>Initializes a new instance of the <see cref="DaysValidator"/> class.</summary>
    public DaysValidator(Range<int> range)
    {
        Range = range;
        (MinDaysSinceEpoch, MaxDaysSinceEpoch) = range.Endpoints;
    }

    /// <summary>Gets the range of supported numbers of consecutive days from the epoch.</summary>
    public Range<int> Range { get; }

    /// <summary>Gets the minimum number of consecutive days from the epoch.</summary>
    public int MinDaysSinceEpoch { get; }

    /// <summary>Gets the maximum number of consecutive days from the epoch.</summary>
    public int MaxDaysSinceEpoch { get; }

    /// <summary>Returns a culture-independent string representation of the current instance.</summary>
    [Pure]
    public sealed override string ToString() => Range.ToString();

    /// <summary>Validates the specified number of consecutive days from the epoch.</summary>
    /// <exception cref="AoorException">The validation failed.</exception>
    public void Validate(int daysSinceEpoch, string? paramName = null)
    {
        if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            throw new AoorException(paramName ?? nameof(daysSinceEpoch));
    }

    /// <summary>Checks whether the specified number of consecutive days from the epoch is outside
    /// the range of supported values or not.</summary>
    /// <exception cref="OverflowException"><paramref name="daysSinceEpoch"/> is outside the range
    /// of supported values.</exception>
    public void CheckOverflow(int daysSinceEpoch)
    {
        if (daysSinceEpoch < MinDaysSinceEpoch || daysSinceEpoch > MaxDaysSinceEpoch)
            ThrowHelpers.DateOverflow();
    }

    /// <summary>Checks whether the specified number of consecutive days from the epoch is greater
    /// than the upper bound of the range of supported values or not.</summary>
    /// <exception cref="OverflowException"><paramref name="daysSinceEpoch"/> is greater than the
    /// upper bound of the range of supported values.</exception>
    public void CheckUpperBound(int daysSinceEpoch)
    {
        if (daysSinceEpoch > MaxDaysSinceEpoch) ThrowHelpers.DateOverflow();
    }

    /// <summary>Checks whether the specified number of consecutive days from the epoch is less than
    /// the lower bound of the range of supported values or not.</summary>
    /// <exception cref="OverflowException"><paramref name="daysSinceEpoch"/> is less than the lower
    /// bound of the range of supported values.</exception>
    public void CheckLowerBound(int daysSinceEpoch)
    {
        if (daysSinceEpoch < MinDaysSinceEpoch) ThrowHelpers.DateOverflow();
    }
}
