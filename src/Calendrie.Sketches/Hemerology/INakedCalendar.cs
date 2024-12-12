// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Validation;

/// <summary>
/// Defines a calendar without a dedicated companion date type.
/// </summary>
public interface INakedCalendar
{
    /// <summary>
    /// Gets the calendar scope.
    /// </summary>
    CalendarScope Scope { get; }

    /// <summary>
    /// Gets the provider for day numbers.
    /// </summary>
    IDateProvider<DayNumber> DayNumberProvider { get; }

    /// <summary>
    /// Gets the provider for date parts.
    /// </summary>
    IDateProvider<DateParts> DatePartsProvider { get; }

    /// <summary>
    /// Gets the provider for ordinal parts.
    /// </summary>
    IDateProvider<OrdinalParts> OrdinalPartsProvider { get; }

    /// <summary>
    /// Gets the adapter for calendrical parts.
    /// </summary>
    protected PartsAdapter PartsAdapter { get; }

    /// <summary>
    /// Obtains the date parts for the specified day number.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayNumber"/>
    /// is outside the range of values supported by this calendar.</exception>
    [Pure]
    DateParts GetDateParts(DayNumber dayNumber)
    {
        var scope = Scope;
        scope.Domain.Validate(dayNumber);
        return PartsAdapter.GetDateParts(dayNumber - scope.Epoch);
    }

    /// <summary>
    /// Obtains the date parts for the specified ordinal date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The ordinal date is either
    /// invalid or outside the range of supported dates.</exception>
    [Pure]
    DateParts GetDateParts(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);
        return PartsAdapter.GetDateParts(year, dayOfYear);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the specified day number.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayNumber"/>
    /// is outside the range of values supported by this calendar.</exception>
    [Pure]
    OrdinalParts GetOrdinalParts(DayNumber dayNumber)
    {
        var scope = Scope;
        scope.Domain.Validate(dayNumber);
        return PartsAdapter.GetOrdinalParts(dayNumber - scope.Epoch);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the specified date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The date is either invalid
    /// or outside the range of supported dates.</exception>
    [Pure]
    OrdinalParts GetOrdinalParts(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return PartsAdapter.GetOrdinalParts(year, month, day);
    }
}
