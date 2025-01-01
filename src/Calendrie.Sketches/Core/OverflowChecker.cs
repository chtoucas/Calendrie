// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents a validator for a range of (algebraic) values of type
/// <see cref="int"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class OverflowChecker
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OverflowChecker"/> class.
    /// </summary>
    public OverflowChecker(Range<int> range)
    {
        (MinValue, MaxValue) = range.Endpoints;
    }

    /// <summary>
    /// Gets the minimum number of consecutive days from the epoch.
    /// </summary>
    public int MinValue { get; }

    /// <summary>
    /// Gets the maximum number of consecutive days from the epoch.
    /// </summary>
    public int MaxValue { get; }

    /// <summary>
    /// Checks whether the specified value is outside the range of supported
    /// values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is
    /// outside the range of supported values.</exception>
    public void CheckOverflow(int value)
    {
        // TODO(code): throw with a generic message.
        if (value < MinValue || value > MaxValue) ThrowHelpers.ThrowDateOverflow();
    }

    ///// <summary>
    ///// Checks whether the specified value is greater than the upper bound of
    ///// the range of supported values or not.
    ///// </summary>
    ///// <exception cref="OverflowException"><paramref name="value"/> is
    ///// greater than the upper bound of the range of supported values.</exception>
    //public void CheckUpperBound(int value)
    //{
    //    if (value > MaxValue) ThrowHelpers.ThrowDateOverflow();
    //}

    ///// <summary>
    ///// Checks whether the specified value is less than the lower bound of the
    ///// range of supported values or not.
    ///// </summary>
    ///// <exception cref="OverflowException"><paramref name="value"/> is
    ///// less than the lower bound of the range of supported values.</exception>
    //public void CheckLowerBound(int value)
    //{
    //    if (value < MinValue) ThrowHelpers.ThrowDateOverflow();
    //}
}
