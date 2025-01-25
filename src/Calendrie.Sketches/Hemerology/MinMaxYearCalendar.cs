// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

/// <summary>
/// Represents a calendar with dates within a range of years.
/// </summary>
public partial class MinMaxYearCalendar : CalendarSans, IDateProvider<DateParts>
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
    }

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public int MinYear { get; }

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public int MaxYear { get; }

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountMonthsInYear(int year)
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

public partial class MinMaxYearCalendar // IDateProvider
{
    /// <inheritdoc/>
    [Pure]
    public IEnumerable<DateParts> GetDaysInYear(int year)
    {
        // Check arg eagerly.
        Scope.ValidateYear(year);

        return iterator();

        IEnumerable<DateParts> iterator()
        {
            int monthsInYear = Schema.CountMonthsInYear(year);

            for (int m = 1; m <= monthsInYear; m++)
            {
                int daysInMonth = Schema.CountDaysInMonth(year, m);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new DateParts(year, m, d);
                }
            }
        }
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<DateParts> GetDaysInMonth(int year, int month)
    {
        // Check arg eagerly.
        Scope.ValidateYearMonth(year, month);

        return iterator();

        IEnumerable<DateParts> iterator()
        {
            int daysInMonth = Schema.CountDaysInMonth(year, month);

            for (int d = 1; d <= daysInMonth; d++)
            {
                yield return new DateParts(year, month, d);
            }
        }
    }

    /// <inheritdoc/>
    [Pure]
    public DateParts GetStartOfYear(int year)
    {
        Scope.ValidateYear(year);
        return DateParts.AtStartOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public DateParts GetEndOfYear(int year)
    {
        Scope.ValidateYear(year);
        return PartsAdapter.GetDatePartsAtEndOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public DateParts GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return DateParts.AtStartOfMonth(year, month);
    }

    /// <inheritdoc/>
    [Pure]
    public DateParts GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return PartsAdapter.GetDatePartsAtEndOfMonth(year, month);
    }
}
