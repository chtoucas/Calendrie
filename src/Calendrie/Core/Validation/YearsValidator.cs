﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Validation;

using Calendrie.Core.Intervals;

/// <summary>Represents a validator for a range of years.
/// <para>This class cannot be inherited.</para></summary>
public sealed class YearsValidator : IYearsValidator
{
    /// <summary>Initializes a new instance of the <see cref="YearsValidator"/> class.</summary>
    public YearsValidator(Range<int> range)
    {
        Range = range;
        (MinYear, MaxYear) = range.Endpoints;
    }

    /// <inheritdoc/>
    public Range<int> Range { get; }

    /// <summary>Gets the ealiest supported year.</summary>
    public int MinYear { get; }

    /// <summary>Gets the latest supported year.</summary>
    public int MaxYear { get; }

    /// <summary>Returns a culture-independent string representation of the current instance.</summary>
    [Pure]
    public sealed override string ToString() => Range.ToString();

    /// <inheritdoc/>
    public void Validate(int year, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
    }

    /// <inheritdoc/>
    public void CheckOverflow(int year)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowDateOverflow();
    }

    ///// <summary>Checks whether the specified year is outside the range of supported values or not.
    ///// </summary>
    ///// <exception cref="OverflowException"><paramref name="year"/> is outside the range of supported
    ///// values.</exception>
    //internal void CheckForMonth(int year)
    //{
    //    if (year < MinYear || year > MaxYear) ThrowHelpers.MonthOverflow();
    //}

    /// <inheritdoc/>
    public void CheckUpperBound(int year)
    {
        if (year > MaxYear) ThrowHelpers.ThrowDateOverflow();
    }

    /// <inheritdoc/>
    public void CheckLowerBound(int year)
    {
        if (year < MinYear) ThrowHelpers.ThrowDateOverflow();
    }
}
