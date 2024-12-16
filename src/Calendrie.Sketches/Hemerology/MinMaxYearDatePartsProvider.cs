// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;

public sealed class MinMaxYearDatePartsProvider : IDateProvider<DateParts>
{
    private readonly MinMaxYearScope _scope;
    private readonly ICalendricalSchema _schema;

    private readonly PartsAdapter _adapter;

    public MinMaxYearDatePartsProvider(MinMaxYearScope scope)
    {
        ArgumentNullException.ThrowIfNull(scope);

        _scope = scope;
        _schema = scope.Schema;

        _adapter = new PartsAdapter(_schema);
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<DateParts> GetDaysInYear(int year)
    {
        // Check arg eagerly.
        _scope.ValidateYear(year);

        return iterator();

        IEnumerable<DateParts> iterator()
        {
            int monthsInYear = _schema.CountMonthsInYear(year);

            for (int m = 1; m <= monthsInYear; m++)
            {
                int daysInMonth = _schema.CountDaysInMonth(year, m);

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
        _scope.ValidateYearMonth(year, month);

        return iterator();

        IEnumerable<DateParts> iterator()
        {
            int daysInMonth = _schema.CountDaysInMonth(year, month);

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
        _scope.ValidateYear(year);
        return DateParts.AtStartOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public DateParts GetEndOfYear(int year)
    {
        _scope.ValidateYear(year);
        return _adapter.GetDatePartsAtEndOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public DateParts GetStartOfMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        return DateParts.AtStartOfMonth(year, month);
    }

    /// <inheritdoc/>
    [Pure]
    public DateParts GetEndOfMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        return _adapter.GetDatePartsAtEndOfMonth(year, month);
    }
}
