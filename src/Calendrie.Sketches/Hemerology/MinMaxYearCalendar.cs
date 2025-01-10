// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Represents a calendar with dates within a range of years.
/// </summary>
public class MinMaxYearCalendar : NakedCalendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MinMaxYearCalendar"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    public MinMaxYearCalendar(string name, MinMaxYearScope scope) : base(name, scope)
    {
        Debug.Assert(scope != null);

        (MinYear, MaxYear) = scope.Segment.SupportedYears.Endpoints;

        DayNumberProvider = new MinMaxYearDayNumberProvider(scope);
        DatePartsProvider = new MinMaxYearDatePartsProvider(scope);
        OrdinalPartsProvider = new MinMaxYearOrdinalPartsProvider(scope);
    }

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public int MinYear { get; }

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public int MaxYear { get; }

    /// <summary>
    /// Gets the provider for day numbers.
    /// </summary>
    public MinMaxYearDayNumberProvider DayNumberProvider { get; }

    /// <summary>
    /// Gets the provider for date parts.
    /// </summary>
    public MinMaxYearDatePartsProvider DatePartsProvider { get; }

    /// <summary>
    /// Gets the provider for ordinal parts.
    /// </summary>
    public MinMaxYearOrdinalPartsProvider OrdinalPartsProvider { get; }

    /// <summary>
    /// Obtains the number of months in the specified year.
    /// <para>See also <seealso cref="CalendarSystem2.IsRegular(out int)"/>.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure]
    public int CountMonthsInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountMonthsInYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        Scope.ValidateYear(year);
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
