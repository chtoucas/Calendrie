// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Represents a calendar with dates on or after a given date.
/// <para>The aforementioned date can NOT be the start of a year.</para>
/// </summary>
public class BoundedBelowCalendar : NakedCalendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BoundedBelowCalendar"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    public BoundedBelowCalendar(string name, BoundedBelowScope scope) : base(name, scope)
    {
        Debug.Assert(scope != null);

        MinDateParts = scope.MinDateParts;
        MinOrdinalParts = scope.MinOrdinalParts;

        DayNumberProvider = new BoundedBelowDayNumberProvider(scope);
    }

    // The following two properties should remain public, otherwise an outsider
    // won't have access to these infos. Indeed, BoundedBelowCalendar.Scope is
    // of type CalendarScope, not BoundedBelowScope.

    /// <summary>
    /// Gets the earliest supported date parts.
    /// </summary>
    public DateParts MinDateParts { get; }

    /// <summary>
    /// Gets the earliest supported ordinal date parts.
    /// </summary>
    public OrdinalParts MinOrdinalParts { get; }

    /// <summary>
    /// Gets the provider for day numbers.
    /// </summary>
    public BoundedBelowDayNumberProvider DayNumberProvider { get; }

    // NB : pour optimiser les choses on pourrait traiter d'abord le cas
    // limite (première année ou premier mois) puis le cas général.
    // Attention, il ne faudrait alors pas écrire
    // > if (new Yemo(year, month) == MinYemoda.Yemo) { ... }
    // mais plutôt
    // > if (year == MinYear && month == MinYemoda.Month) { ... }
    // car on n'a justement pas validé les paramètres.

    /// <inheritdoc />
    [Pure]
    public sealed override int CountMonthsInYear(int year)
    {
        YearsValidator.Validate(year);
        return year == MinDateParts.Year
            ? CountMonthsInFirstYear()
            : Schema.CountMonthsInYear(year);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        YearsValidator.Validate(year);
        return year == MinDateParts.Year
            ? CountDaysInFirstYear()
            : Schema.CountDaysInYear(year);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return new MonthParts(year, month) == MinDateParts.MonthParts
            ? CountDaysInFirstMonth()
            : Schema.CountDaysInMonth(year, month);
    }

    //
    // Custom methods
    //

    /// <summary>
    /// Obtains the number of months in the first supported year.
    /// </summary>
    [Pure]
    public int CountMonthsInFirstYear() =>
        Schema.CountMonthsInYear(MinDateParts.Year) - MinDateParts.Month + 1;

    /// <summary>
    /// Obtains the number of days in the first supported year.
    /// </summary>
    [Pure]
    public int CountDaysInFirstYear() =>
        Schema.CountDaysInYear(MinOrdinalParts.Year) - MinOrdinalParts.DayOfYear + 1;

    /// <summary>
    /// Obtains the number of days in the first supported month.
    /// </summary>
    [Pure]
    public int CountDaysInFirstMonth()
    {
        var (y, m, d) = MinDateParts;
        return Schema.CountDaysInMonth(y, m) - d + 1;
    }
}
