// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Linq;

using Calendrie.Core;
using Calendrie.Core.Validation;

/// <summary>
/// Represents a calendar with dates within a range of years.
/// </summary>
public partial class MinMaxYearCalendar : Calendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MinMaxYearCalendar"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    public MinMaxYearCalendar(string name, MinMaxYearScope scope) : base(name, scope)
    {
        PartsAdapter = new PartsAdapter(Schema);

        DayNumberProvider = new MinMaxYearDayNumberProvider(this);
        DatePartsProvider = new MinMaxYearDatePartsProvider(this);
        OrdinalPartsProvider = new MinMaxYearOrdinalPartsProvider(this);
    }

    /// <summary>
    /// Gets the provider for day numbers.
    /// </summary>
    public IDateProvider<DayNumber> DayNumberProvider { get; }

    /// <summary>
    /// Gets the provider for date parts.
    /// </summary>
    public IDateProvider<DateParts> DatePartsProvider { get; }

    /// <summary>
    /// Gets the provider for ordinal parts.
    /// </summary>
    public IDateProvider<OrdinalParts> OrdinalPartsProvider { get; }

    /// <summary>
    /// Gets the adapter for calendrical parts.
    /// </summary>
    protected internal PartsAdapter PartsAdapter { get; }

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountMonthsInYear(int year)
    {
        YearsValidator.Validate(year);
        return Schema.CountMonthsInYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        YearsValidator.Validate(year);
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

public partial class MinMaxYearCalendar // Conversions
{
    /// <inheritdoc />
    [Pure]
    public DateParts GetDateParts(DayNumber dayNumber)
    {
        var scope = Scope;
        scope.Domain.Validate(dayNumber);
        return PartsAdapter.GetDateParts(dayNumber - scope.Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public DateParts GetDateParts(int year, int dayOfYear)
    {
        Scope.ValidateOrdinal(year, dayOfYear);
        return PartsAdapter.GetDateParts(year, dayOfYear);
    }

    /// <inheritdoc />
    [Pure]
    public OrdinalParts GetOrdinalParts(DayNumber dayNumber)
    {
        var scope = Scope;
        scope.Domain.Validate(dayNumber);
        return PartsAdapter.GetOrdinalParts(dayNumber - scope.Epoch);
    }

    /// <inheritdoc />
    [Pure]
    public OrdinalParts GetOrdinalParts(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return PartsAdapter.GetOrdinalParts(year, month, day);
    }
}

public sealed class MinMaxYearDayNumberProvider : IDateProvider<DayNumber>
{
    private readonly MinMaxYearCalendar _inner;
    private readonly DayNumber _epoch;

    public MinMaxYearDayNumberProvider(MinMaxYearCalendar inner)
    {
        Debug.Assert(inner != null);

        _inner = inner;
        _epoch = inner.Scope.Epoch;
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<DayNumber> GetDaysInYear(int year)
    {
        _inner.YearsValidator.Validate(year);

        int startOfYear = _inner.Schema.GetStartOfYear(year);
        int daysInYear = _inner.Schema.CountDaysInYear(year);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select _epoch + daysSinceEpoch;
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
    {
        _inner.Scope.ValidateYearMonth(year, month);

        int startOfMonth = _inner.Schema.GetStartOfMonth(year, month);
        int daysInMonth = _inner.Schema.CountDaysInMonth(year, month);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select _epoch + daysSinceEpoch;
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetStartOfYear(int year)
    {
        _inner.YearsValidator.Validate(year);
        return _epoch + _inner.Schema.GetStartOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetEndOfYear(int year)
    {
        _inner.YearsValidator.Validate(year);
        return _epoch + _inner.Schema.GetEndOfYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetStartOfMonth(int year, int month)
    {
        _inner.Scope.ValidateYearMonth(year, month);
        return _epoch + _inner.Schema.GetStartOfMonth(year, month);
    }

    /// <inheritdoc/>
    [Pure]
    public DayNumber GetEndOfMonth(int year, int month)
    {
        _inner.Scope.ValidateYearMonth(year, month);
        return _epoch + _inner.Schema.GetEndOfMonth(year, month);
    }
}

public sealed class MinMaxYearDatePartsProvider : IDateProvider<DateParts>
{
    private readonly MinMaxYearCalendar _inner;

    public MinMaxYearDatePartsProvider(MinMaxYearCalendar inner)
    {
        Debug.Assert(inner != null);

        _inner = inner;
    }

    [Pure]
    public IEnumerable<DateParts> GetDaysInYear(int year)
    {
        // Check arg eagerly.
        _inner.YearsValidator.Validate(year);

        return iterator();

        IEnumerable<DateParts> iterator()
        {
            int monthsInYear = _inner.Schema.CountMonthsInYear(year);

            for (int m = 1; m <= monthsInYear; m++)
            {
                int daysInMonth = _inner.Schema.CountDaysInMonth(year, m);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new DateParts(year, m, d);
                }
            }
        }
    }

    [Pure]
    public IEnumerable<DateParts> GetDaysInMonth(int year, int month)
    {
        // Check arg eagerly.
        _inner.Scope.ValidateYearMonth(year, month);

        return iterator();

        IEnumerable<DateParts> iterator()
        {
            int daysInMonth = _inner.Schema.CountDaysInMonth(year, month);

            for (int d = 1; d <= daysInMonth; d++)
            {
                yield return new DateParts(year, month, d);
            }
        }
    }

    [Pure]
    public DateParts GetStartOfYear(int year)
    {
        _inner.YearsValidator.Validate(year);
        return DateParts.AtStartOfYear(year);
    }

    [Pure]
    public DateParts GetEndOfYear(int year)
    {
        _inner.YearsValidator.Validate(year);
        return _inner.PartsAdapter.GetDatePartsAtEndOfYear(year);
    }

    [Pure]
    public DateParts GetStartOfMonth(int year, int month)
    {
        _inner.Scope.ValidateYearMonth(year, month);
        return DateParts.AtStartOfMonth(year, month);
    }

    [Pure]
    public DateParts GetEndOfMonth(int year, int month)
    {
        _inner.Scope.ValidateYearMonth(year, month);
        return _inner.PartsAdapter.GetDatePartsAtEndOfMonth(year, month);
    }
}

public sealed class MinMaxYearOrdinalPartsProvider : IDateProvider<OrdinalParts>
{
    private readonly MinMaxYearCalendar _inner;

    public MinMaxYearOrdinalPartsProvider(MinMaxYearCalendar inner)
    {
        Debug.Assert(inner != null);

        _inner = inner;
    }

    [Pure]
    public IEnumerable<OrdinalParts> GetDaysInYear(int year)
    {
        // Check arg eagerly.
        _inner.YearsValidator.Validate(year);

        return iterator();

        IEnumerable<OrdinalParts> iterator()
        {
            int daysInYear = _inner.Schema.CountDaysInYear(year);

            for (int doy = 1; doy <= daysInYear; doy++)
            {
                yield return new OrdinalParts(year, doy);
            }
        }
    }

    [Pure]
    public IEnumerable<OrdinalParts> GetDaysInMonth(int year, int month)
    {
        // Check arg eagerly.
        _inner.Scope.ValidateYearMonth(year, month);

        return iterator();

        IEnumerable<OrdinalParts> iterator()
        {
            int startOfMonth = _inner.Schema.CountDaysInYearBeforeMonth(year, month);
            int daysInMonth = _inner.Schema.CountDaysInMonth(year, month);

            for (int d = 1; d <= daysInMonth; d++)
            {
                yield return new OrdinalParts(year, startOfMonth + d);
            }
        }
    }

    [Pure]
    public OrdinalParts GetStartOfYear(int year)
    {
        _inner.YearsValidator.Validate(year);
        return OrdinalParts.AtStartOfYear(year);
    }

    [Pure]
    public OrdinalParts GetEndOfYear(int year)
    {
        _inner.YearsValidator.Validate(year);
        return _inner.PartsAdapter.GetOrdinalPartsAtEndOfYear(year);
    }

    [Pure]
    public OrdinalParts GetStartOfMonth(int year, int month)
    {
        _inner.Scope.ValidateYearMonth(year, month);
        return _inner.PartsAdapter.GetOrdinalPartsAtStartOfMonth(year, month);
    }

    [Pure]
    public OrdinalParts GetEndOfMonth(int year, int month)
    {
        _inner.Scope.ValidateYearMonth(year, month);
        return _inner.PartsAdapter.GetOrdinalPartsAtEndOfMonth(year, month);
    }
}
