// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Linq;

using Calendrie.Core;
using Calendrie.Core.Validation;

public sealed class MinMaxYearDayNumberProvider : IDateProvider<DayNumber>
{
    private readonly MinMaxYearScope _scope;
    private readonly DayNumber _epoch;
    private readonly ICalendricalSchema _schema;
    private readonly IYearsValidator _yearsValidator;

    public MinMaxYearDayNumberProvider(MinMaxYearScope scope)
    {
        ArgumentNullException.ThrowIfNull(scope);

        _scope = scope;
        _epoch = scope.Epoch;
        _schema = scope.Schema;
        _yearsValidator = scope.YearsValidator;
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<DayNumber> GetDaysInYear(int year)
    {
        _yearsValidator.Validate(year);

        int startOfYear = _schema.GetStartOfYear(year);
        int daysInYear = _schema.CountDaysInYear(year);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select _epoch + daysSinceEpoch;
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);

        int startOfMonth = _schema.GetStartOfMonth(year, month);
        int daysInMonth = _schema.CountDaysInMonth(year, month);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select _epoch + daysSinceEpoch;
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetStartOfYear(int year)
    {
        _yearsValidator.Validate(year);
        return _epoch + _schema.GetStartOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetEndOfYear(int year)
    {
        _yearsValidator.Validate(year);
        return _epoch + _schema.GetEndOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetStartOfMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        return _epoch + _schema.GetStartOfMonth(year, month);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetEndOfMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        return _epoch + _schema.GetEndOfMonth(year, month);
    }
}
