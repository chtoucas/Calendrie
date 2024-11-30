// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Hemerology.Scopes;

/// <summary>
/// Represents a calendar with dates within a range of years.
/// </summary>
public class MinMaxYearCalendar : Calendar<MinMaxYearScope>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MinMaxYearCalendar"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    public MinMaxYearCalendar(string name, MinMaxYearScope scope) : base(name, scope) { }

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountMonthsInYear(int year)
    {
        YearsValidator.Validate(year);
        return Schema.CountMonthsInYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        YearsValidator.Validate(year);
        return Schema.CountDaysInYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Schema.CountDaysInMonth(year, month);
    }
}
