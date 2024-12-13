// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Validation;

public sealed class MinMaxYearOrdinalPartsProvider : IDateProvider<OrdinalParts>
{
    private readonly MinMaxYearScope _scope;
    private readonly ICalendricalSchema _schema;
    private readonly IYearsValidator _yearsValidator;

    private readonly PartsAdapter _adapter;

    public MinMaxYearOrdinalPartsProvider(MinMaxYearScope scope)
    {
        ArgumentNullException.ThrowIfNull(scope);

        _scope = scope;
        _schema = _scope.Schema;
        _yearsValidator = _scope.YearsValidator;

        _adapter = new PartsAdapter(_schema);
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<OrdinalParts> GetDaysInYear(int year)
    {
        // Check arg eagerly.
        _yearsValidator.Validate(year);

        return iterator();

        IEnumerable<OrdinalParts> iterator()
        {
            int daysInYear = _schema.CountDaysInYear(year);

            for (int doy = 1; doy <= daysInYear; doy++)
            {
                yield return new OrdinalParts(year, doy);
            }
        }
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<OrdinalParts> GetDaysInMonth(int year, int month)
    {
        // Check arg eagerly.
        _scope.ValidateYearMonth(year, month);

        return iterator();

        IEnumerable<OrdinalParts> iterator()
        {
            int startOfMonth = _schema.CountDaysInYearBeforeMonth(year, month);
            int daysInMonth = _schema.CountDaysInMonth(year, month);

            for (int d = 1; d <= daysInMonth; d++)
            {
                yield return new OrdinalParts(year, startOfMonth + d);
            }
        }
    }

    /// <inheritdoc/>
    [Pure]
    public OrdinalParts GetStartOfYear(int year)
    {
        _yearsValidator.Validate(year);
        return OrdinalParts.AtStartOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public OrdinalParts GetEndOfYear(int year)
    {
        _yearsValidator.Validate(year);
        return _adapter.GetOrdinalPartsAtEndOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public OrdinalParts GetStartOfMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        return _adapter.GetOrdinalPartsAtStartOfMonth(year, month);
    }

    /// <inheritdoc/>
    [Pure]
    public OrdinalParts GetEndOfMonth(int year, int month)
    {
        _scope.ValidateYearMonth(year, month);
        return _adapter.GetOrdinalPartsAtEndOfMonth(year, month);
    }
}
