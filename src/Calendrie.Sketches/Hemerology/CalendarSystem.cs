// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;

/// <summary>
/// Represents a calendar system and provides a base for derived classes.
/// </summary>
public abstract class CalendarSystem : Calendar
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CalendarSystem"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    protected CalendarSystem(string name, CalendarScope scope) : base(name, scope)
    {
        Debug.Assert(scope != null);
        Schema = scope.Schema;
    }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    protected internal ICalendricalSchema Schema { get; }

    //
    // Characteristics
    //

    /// <summary>
    /// Returns <see langword="true"/> if this calendar is regular; otherwise
    /// returns <see langword="false"/>.
    /// <para>The number of months is given in an output parameter; if this
    /// calendar is not regular <paramref name="monthsInYear"/> is set to 0.
    /// </para>
    /// </summary>
    [Pure]
    public bool IsRegular(out int monthsInYear) => Scope.Schema.IsRegular(out monthsInYear);

    //
    // Year infos
    //

    /// <summary>
    /// Determines whether the specified year is leap or not.
    /// <para>A leap year is a year with at least one intercalary day, week or
    /// month.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of supported years.</exception>
    [Pure]
    public bool IsLeapYear(int year)
    {
        var scope = Scope;
        scope.ValidateYear(year);
        return scope.Schema.IsLeapYear(year);
    }

    /// <summary>
    /// Obtains the number of months in the specified year.
    /// <para>See also <seealso cref="IsRegular(out int)"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure] public abstract int CountMonthsInYear(int year);

    // La méthode suivante est abstraite car une année peut être incomplète.

    /// <summary>
    /// Obtains the number of days in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure] public abstract int CountDaysInYear(int year);

    //
    // Month infos
    //

    /// <summary>
    /// Determines whether the specified month is intercalary or not.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The month is either invalid
    /// or outside the range of supported months.</exception>
    [Pure]
    public bool IsIntercalaryMonth(int year, int month)
    {
        var scope = Scope;
        scope.ValidateYearMonth(year, month);
        return scope.Schema.IsIntercalaryMonth(year, month);
    }

    // La méthode suivante est abstraite car une année peut être incomplète.

    /// <summary>
    /// Obtains the number of days in the specified month.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The month is either invalid
    /// or outside the range of supported months.</exception>
    [Pure] public abstract int CountDaysInMonth(int year, int month);

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
}
