// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;

/// <summary>
/// Represents a calendar without a dedicated companion date type and provides
/// a base for derived classes.
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
        Schema = scope.Schema;
        PartsAdapter = new PartsAdapter(Scope.Schema);
    }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    protected internal ICalendricalSchema Schema { get; }

    /// <summary>
    /// Gets the adapter for the calendrical parts.
    /// </summary>
    protected PartsAdapter PartsAdapter { get; }

    //
    // Characteristics
    //

    /// <summary>
    /// Returns <see langword="true"/> if this schema is regular; otherwise
    /// returns <see langword="false"/>.
    /// <para>The number of months is given in an output parameter; if this
    /// schema is not regular <paramref name="monthsInYear"/> is set to 0.
    /// </para>
    /// <para>See also <seealso cref="CountMonthsInYear(int)"/>.</para>
    /// </summary>
    [Pure]
    public bool IsRegular(out int monthsInYear) => Schema.IsRegular(out monthsInYear);

    /// <summary>
    /// Obtains the number of months in the specified year.
    /// <para>See also <seealso cref="IsRegular(out int)"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure] public abstract int CountMonthsInYear(int year);

    //
    // Day infos
    //

    /// <summary>
    /// Determines whether the specified date is an intercalary day or not.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The date is either invalid
    /// or outside the range of supported dates.</exception>
    [Pure]
    public bool IsIntercalaryDay(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return Schema.IsIntercalaryDay(year, month, day);
    }

    /// <summary>
    /// Determines whether the specified date is a supplementary day or not.
    /// <para>Supplementary days are days kept outside the intermediary cycles,
    /// those shorter than a year. For technical reasons, we usually attach them
    /// to the month before. Notice that a supplementary day may be intercalary
    /// too. An example of such days is given by the epagomenal days which are
    /// kept outside any regular month or decade.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The date is either invalid
    /// or outside the range of supported dates.</exception>
    [Pure]
    public bool IsSupplementaryDay(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return Schema.IsSupplementaryDay(year, month, day);
    }

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
        Scope.Validate(dayNumber);
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
        Scope.Validate(dayNumber);
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
