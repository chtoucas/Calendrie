// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;

public sealed class BoundedBelowDayNumberProvider : IDateProvider<DayNumber>
{
    private readonly CalendarScope _scope;
    private readonly DayNumber _epoch;
    private readonly ICalendricalSchema _schema;

    private readonly DateParts _minDateParts;
    private readonly OrdinalParts _minOrdinalParts;

    public BoundedBelowDayNumberProvider(BoundedBelowScope scope)
    {
        ArgumentNullException.ThrowIfNull(scope);

        _scope = scope;
        _epoch = scope.Epoch;
        _schema = scope.Schema;

        _minDateParts = scope.MinDateParts;
        _minOrdinalParts = scope.MinOrdinalParts;
    }

    /// <summary>
    /// Obtains the number of days in the first supported year.
    /// </summary>
    [Pure]
    private int CountDaysInFirstYear() =>
        _schema.CountDaysInYear(_minOrdinalParts.Year) - _minOrdinalParts.DayOfYear + 1;

    /// <summary>
    /// Obtains the number of days in the first supported month.
    /// </summary>
    [Pure]
    private int CountDaysInFirstMonth()
    {
        var (y, m, d) = _minDateParts;
        return _schema.CountDaysInMonth(y, m) - d + 1;
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerable<DayNumber> GetDaysInYear(int year)
    {
        _scope.ValidateYear(year);
        int startOfYear, daysInYear;
        if (year == _minDateParts.Year)
        {
            startOfYear = _scope.Domain.Min - _epoch;
            daysInYear = CountDaysInFirstYear();
        }
        else
        {
            startOfYear = _schema.GetStartOfYear(year);
            daysInYear = _schema.CountDaysInYear(year);
        }

        return iterator();

        IEnumerable<DayNumber> iterator()
        {
            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select _epoch + daysSinceEpoch;
        }
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        int startOfMonth, daysInMonth;
        if (new MonthParts(year, month) == _minDateParts.MonthParts)
        {
            startOfMonth = _scope.Domain.Min - _epoch;
            daysInMonth = CountDaysInFirstMonth();
        }
        else
        {
            startOfMonth = _schema.GetStartOfMonth(year, month);
            daysInMonth = _schema.CountDaysInMonth(year, month);
        }

        return iterator();

        IEnumerable<DayNumber> iterator()
        {
            return from daysSinceEpoch
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select _epoch + daysSinceEpoch;
        }
    }

    /// <inheritdoc />
    [Pure]
    public DayNumber GetStartOfYear(int year)
    {
        _scope.ValidateYear(year);
        return year == _minDateParts.Year
            ? throw new ArgumentOutOfRangeException(nameof(year))
            : _epoch + _schema.GetStartOfYear(year);
    }

    /// <inheritdoc />
    [Pure]
    public DayNumber GetEndOfYear(int year)
    {
        _scope.ValidateYear(year);
        return _epoch + _schema.GetEndOfYear(year);
    }

    /// <inheritdoc />
    [Pure]
    public DayNumber GetStartOfMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        return new MonthParts(year, month) == _minDateParts.MonthParts
            ? throw new ArgumentOutOfRangeException(nameof(month))
            : _epoch + _schema.GetStartOfMonth(year, month);
    }

    /// <inheritdoc />
    [Pure]
    public DayNumber GetEndOfMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        return _epoch + _schema.GetEndOfMonth(year, month);
    }
}
