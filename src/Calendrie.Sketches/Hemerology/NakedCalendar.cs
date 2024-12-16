// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Validation;

/// <summary>
/// Represents a calendar without a dedicated companion date type.
/// </summary>
public abstract class NakedCalendar : Calendar
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="NakedCalendar"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    protected NakedCalendar(string name, CalendarScope scope) : base(name, scope)
    {
        Debug.Assert(scope != null);

        Epoch = scope.Epoch;
        PartsAdapter = new PartsAdapter(Schema);
    }

    /// <summary>
    /// Gets the epoch.
    /// </summary>
    public DayNumber Epoch { get; }

    /// <summary>
    /// Gets the adapter for the calendrical parts.
    /// </summary>
    protected PartsAdapter PartsAdapter { get; }

    //
    // Conversions
    //

    /// <summary>
    /// Obtains the day number on the specified date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The date is not within the
    /// calendar boundaries.</exception>
    [Pure]
    public DayNumber GetDayNumber(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return Epoch + Schema.CountDaysSinceEpoch(year, month, day);
    }

    /// <summary>
    /// Obtains the day number on the specified ordinal date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The ordinal date is not
    /// within the calendar boundaries.</exception>
    [Pure]
    public DayNumber GetDayNumber(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);
        return Epoch + Schema.CountDaysSinceEpoch(year, dayOfYear);
    }

    /// <summary>
    /// Obtains the date parts for the specified day number.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayNumber"/>
    /// is outside the range of values supported by this calendar.</exception>
    [Pure]
    public DateParts GetDateParts(DayNumber dayNumber)
    {
        Scope.Domain.Validate(dayNumber);
        return PartsAdapter.GetDateParts(dayNumber - Epoch);
    }

    /// <summary>
    /// Obtains the date parts for the specified ordinal date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The ordinal date is either
    /// invalid or outside the range of supported dates.</exception>
    [Pure]
    public DateParts GetDateParts(int year, int dayOfYear)
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
    public OrdinalParts GetOrdinalParts(DayNumber dayNumber)
    {
        Scope.Domain.Validate(dayNumber);
        return PartsAdapter.GetOrdinalParts(dayNumber - Epoch);
    }

    /// <summary>
    /// Obtains the ordinal date parts for the specified date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The date is either invalid
    /// or outside the range of supported dates.</exception>
    [Pure]
    public OrdinalParts GetOrdinalParts(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return PartsAdapter.GetOrdinalParts(year, month, day);
    }
}
