﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Hemerology.Scopes;

/// <summary>Represents a basic calendar with dates within a range of years.</summary>
public class MinMaxYearBasicCalendar : BasicCalendar<MinMaxYearScope>
{
    /// <summary>Initializes a new instance of the <see cref="MinMaxYearBasicCalendar"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
    public MinMaxYearBasicCalendar(string name, MinMaxYearScope scope) : base(name, scope) { }

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
