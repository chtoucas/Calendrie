// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Represents a calendar with dates on or after a given date.
/// <para>The aforementioned date can NOT be the start of a year.</para>
/// </summary>
public partial class BoundedBelowCalendar : CalendarSans, IDateProvider<DayNumber>
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
        MaxYear = scope.Segment.SupportedYears.Max;
    }

    // The following properties should remain public, otherwise an outsider
    // may have difficulty to find them.

    /// <summary>
    /// Gets the earliest supported date parts.
    /// </summary>
    public DateParts MinDateParts { get; }

    /// <summary>
    /// Gets the earliest supported ordinal date parts.
    /// </summary>
    public OrdinalParts MinOrdinalParts { get; }

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public int MaxYear { get; }

    // NB : pour optimiser les choses on pourrait traiter d'abord le cas
    // limite (première année ou premier mois) puis le cas général.
    // Attention, il ne faudrait alors pas écrire
    // > if (new Yemo(year, month) == MinYemoda.Yemo) { ... }
    // mais plutôt
    // > if (year == MinYear && month == MinYemoda.Month) { ... }
    // car on n'a justement pas validé les paramètres.

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountMonthsInYear(int year)
    {
        Scope.ValidateYear(year);
        return year == MinDateParts.Year
            ? CountMonthsInFirstYear()
            : Schema.CountMonthsInYear(year);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        Scope.ValidateYear(year);
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

public partial class BoundedBelowCalendar // IDateProvider
{
    /// <inheritdoc />
    [Pure]
    public IEnumerable<DayNumber> GetDaysInYear(int year)
    {
        Scope.ValidateYear(year);
        int startOfYear, daysInYear;
        if (year == MinDateParts.Year)
        {
            startOfYear = Scope.Domain.Min - Epoch;
            daysInYear = CountDaysInFirstYear();
        }
        else
        {
            startOfYear = Schema.GetStartOfYear(year);
            daysInYear = Schema.CountDaysInYear(year);
        }

        return iterator();

        IEnumerable<DayNumber> iterator()
        {
            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select Epoch + daysSinceEpoch;
        }
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int startOfMonth, daysInMonth;
        if (new MonthParts(year, month) == MinDateParts.MonthParts)
        {
            startOfMonth = Scope.Domain.Min - Epoch;
            daysInMonth = CountDaysInFirstMonth();
        }
        else
        {
            startOfMonth = Schema.GetStartOfMonth(year, month);
            daysInMonth = Schema.CountDaysInMonth(year, month);
        }

        return iterator();

        IEnumerable<DayNumber> iterator()
        {
            return from daysSinceEpoch
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select Epoch + daysSinceEpoch;
        }
    }

    /// <inheritdoc />
    [Pure]
    public DayNumber GetStartOfYear(int year)
    {
        Scope.ValidateYear(year);
        return year == MinDateParts.Year
            ? throw new ArgumentOutOfRangeException(nameof(year))
            : Epoch + Schema.GetStartOfYear(year);
    }

    /// <inheritdoc />
    [Pure]
    public DayNumber GetEndOfYear(int year)
    {
        Scope.ValidateYear(year);
        return Epoch + Schema.GetEndOfYear(year);
    }

    /// <inheritdoc />
    [Pure]
    public DayNumber GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return new MonthParts(year, month) == MinDateParts.MonthParts
            ? throw new ArgumentOutOfRangeException(nameof(month))
            : Epoch + Schema.GetStartOfMonth(year, month);
    }

    /// <inheritdoc />
    [Pure]
    public DayNumber GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Epoch + Schema.GetEndOfMonth(year, month);
    }
}
