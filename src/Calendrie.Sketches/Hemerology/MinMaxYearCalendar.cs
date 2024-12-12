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

        DayNumberProvider = new DayNumberProvider_(this);
        DatePartsProvider = new DatePartsProvider_(this);
        OrdinalPartsProvider = new OrdinalPartsProvider_(this);
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
    protected PartsAdapter PartsAdapter { get; }

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

public partial class MinMaxYearCalendar // Parts providers
{
    private sealed class DayNumberProvider_ : IDateProvider<DayNumber>
    {
        private readonly MinMaxYearCalendar _this;
        private readonly DayNumber _epoch;

        public DayNumberProvider_(MinMaxYearCalendar @this)
        {
            Debug.Assert(@this != null);

            _this = @this;
            _epoch = @this.Scope.Epoch;
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<DayNumber> GetDaysInYear(int year)
        {
            _this.YearsValidator.Validate(year);

            int startOfYear = _this.Schema.GetStartOfYear(year);
            int daysInYear = _this.Schema.CountDaysInYear(year);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfYear, daysInYear)
                   select _epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<DayNumber> GetDaysInMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);

            int startOfMonth = _this.Schema.GetStartOfMonth(year, month);
            int daysInMonth = _this.Schema.CountDaysInMonth(year, month);

            return from daysSinceEpoch
                   in Enumerable.Range(startOfMonth, daysInMonth)
                   select _epoch + daysSinceEpoch;
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetStartOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return _epoch + _this.Schema.GetStartOfYear(year);
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetEndOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return _epoch + _this.Schema.GetEndOfYear(year);
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetStartOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return _epoch + _this.Schema.GetStartOfMonth(year, month);
        }

        /// <inheritdoc/>
        [Pure]
        public DayNumber GetEndOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return _epoch + _this.Schema.GetEndOfMonth(year, month);
        }
    }

    private sealed class DatePartsProvider_ : IDateProvider<DateParts>
    {
        private readonly MinMaxYearCalendar _this;

        public DatePartsProvider_(MinMaxYearCalendar @this)
        {
            Debug.Assert(@this != null);

            _this = @this;
        }

        [Pure]
        public IEnumerable<DateParts> GetDaysInYear(int year)
        {
            // Check arg eagerly.
            _this.YearsValidator.Validate(year);

            return iterator();

            IEnumerable<DateParts> iterator()
            {
                int monthsInYear = _this.Schema.CountMonthsInYear(year);

                for (int m = 1; m <= monthsInYear; m++)
                {
                    int daysInMonth = _this.Schema.CountDaysInMonth(year, m);

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
            _this.Scope.ValidateYearMonth(year, month);

            return iterator();

            IEnumerable<DateParts> iterator()
            {
                int daysInMonth = _this.Schema.CountDaysInMonth(year, month);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new DateParts(year, month, d);
                }
            }
        }

        [Pure]
        public DateParts GetStartOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return DateParts.AtStartOfYear(year);
        }

        [Pure]
        public DateParts GetEndOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return _this.PartsAdapter.GetDatePartsAtEndOfYear(year);
        }

        [Pure]
        public DateParts GetStartOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return DateParts.AtStartOfMonth(year, month);
        }

        [Pure]
        public DateParts GetEndOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return _this.PartsAdapter.GetDatePartsAtEndOfMonth(year, month);
        }
    }

    private sealed class OrdinalPartsProvider_ : IDateProvider<OrdinalParts>
    {
        private readonly MinMaxYearCalendar _this;

        public OrdinalPartsProvider_(MinMaxYearCalendar @this)
        {
            Debug.Assert(@this != null);

            _this = @this;
        }

        [Pure]
        public IEnumerable<OrdinalParts> GetDaysInYear(int year)
        {
            // Check arg eagerly.
            _this.YearsValidator.Validate(year);

            return iterator();

            IEnumerable<OrdinalParts> iterator()
            {
                int daysInYear = _this.Schema.CountDaysInYear(year);

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
            _this.Scope.ValidateYearMonth(year, month);

            return iterator();

            IEnumerable<OrdinalParts> iterator()
            {
                int startOfMonth = _this.Schema.CountDaysInYearBeforeMonth(year, month);
                int daysInMonth = _this.Schema.CountDaysInMonth(year, month);

                for (int d = 1; d <= daysInMonth; d++)
                {
                    yield return new OrdinalParts(year, startOfMonth + d);
                }
            }
        }

        [Pure]
        public OrdinalParts GetStartOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return OrdinalParts.AtStartOfYear(year);
        }

        [Pure]
        public OrdinalParts GetEndOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return _this.PartsAdapter.GetOrdinalPartsAtEndOfYear(year);
        }

        [Pure]
        public OrdinalParts GetStartOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return _this.PartsAdapter.GetOrdinalPartsAtStartOfMonth(year, month);
        }

        [Pure]
        public OrdinalParts GetEndOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return _this.PartsAdapter.GetOrdinalPartsAtEndOfMonth(year, month);
        }
    }
}
