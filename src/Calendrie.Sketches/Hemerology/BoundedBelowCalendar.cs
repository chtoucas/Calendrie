// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Collections.Generic;
using System.Linq;

using Calendrie.Core;
using Calendrie.Core.Validation;

/// <summary>
/// Represents a calendar with dates on or after a given date.
/// <para>The aforementioned date can NOT be the start of a year.</para>
/// </summary>
public partial class BoundedBelowCalendar : Calendar
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

        PartsAdapter = new PartsAdapter(Schema);

        DayNumberProvider = new DayNumberProvider_(this);

        MinDateParts = scope.MinDateParts;
        MinOrdinalParts = scope.MinOrdinalParts;
        MinMonthParts = scope.MinMonthParts;
        MinYear = MinDateParts.Year;
    }

    /// <summary>
    /// Gets the provider for day numbers.
    /// </summary>
    public IDateProvider<DayNumber> DayNumberProvider { get; }

    /// <summary>
    /// Gets the earliest supported month parts.
    /// </summary>
    public MonthParts MinMonthParts { get; }

    /// <summary>
    /// Gets the earliest supported date parts.
    /// </summary>
    public DateParts MinDateParts { get; }

    /// <summary>
    /// Gets the earliest supported ordinal date parts.
    /// </summary>
    public OrdinalParts MinOrdinalParts { get; }

    /// <summary>
    /// Gets the adapter for calendrical parts.
    /// </summary>
    protected PartsAdapter PartsAdapter { get; }

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    private int MinYear { get; }

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
        return year == MinYear
            ? CountMonthsInFirstYear()
            : Schema.CountMonthsInYear(year);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        YearsValidator.Validate(year);
        return year == MinYear
            ? CountDaysInFirstYear()
            : Schema.CountDaysInYear(year);
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return new MonthParts(year, month) == MinMonthParts
            ? CountDaysInFirstMonth()
            : Schema.CountDaysInMonth(year, month);
    }

    /// <summary>
    /// Obtains the number of months in the first supported year.
    /// </summary>
    [Pure]
    public int CountMonthsInFirstYear() =>
        Schema.CountMonthsInYear(MinYear) - MinDateParts.Month + 1;

    /// <summary>
    /// Obtains the number of days in the first supported year.
    /// </summary>
    [Pure]
    public int CountDaysInFirstYear() =>
        Schema.CountDaysInYear(MinYear) - MinOrdinalParts.DayOfYear + 1;

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

public partial class BoundedBelowCalendar // Conversions
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

public partial class BoundedBelowCalendar
{
    private sealed class DayNumberProvider_ : IDateProvider<DayNumber>
    {
        private readonly BoundedBelowCalendar _this;
        private readonly DayNumber _epoch;

        public DayNumberProvider_(BoundedBelowCalendar @this)
        {
            Debug.Assert(@this != null);

            _this = @this;
            _epoch = @this.Scope.Epoch;
        }

        /// <inheritdoc />
        [Pure]
        public IEnumerable<DayNumber> GetDaysInYear(int year)
        {
            _this.YearsValidator.Validate(year);
            int startOfYear, daysInYear;
            if (year == _this.MinYear)
            {
                startOfYear = _this.Scope.Domain.Min - _epoch;
                daysInYear = _this.CountDaysInFirstYear();
            }
            else
            {
                startOfYear = _this.Schema.GetStartOfYear(year);
                daysInYear = _this.Schema.CountDaysInYear(year);
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
            _this.Scope.ValidateYearMonth(year, month);
            int startOfMonth, daysInMonth;
            if (new MonthParts(year, month) == _this.MinMonthParts)
            {
                startOfMonth = _this.Scope.Domain.Min - _epoch;
                daysInMonth = _this.CountDaysInFirstMonth();
            }
            else
            {
                startOfMonth = _this.Schema.GetStartOfMonth(year, month);
                daysInMonth = _this.Schema.CountDaysInMonth(year, month);
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
            _this.YearsValidator.Validate(year);
            return year == _this.MinYear
                ? throw new ArgumentOutOfRangeException(nameof(year))
                : _epoch + _this.Schema.GetStartOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetEndOfYear(int year)
        {
            _this.YearsValidator.Validate(year);
            return _epoch + _this.Schema.GetEndOfYear(year);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetStartOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return new MonthParts(year, month) == _this.MinMonthParts
                ? throw new ArgumentOutOfRangeException(nameof(month))
                : _epoch + _this.Schema.GetStartOfMonth(year, month);
        }

        /// <inheritdoc />
        [Pure]
        public DayNumber GetEndOfMonth(int year, int month)
        {
            _this.Scope.ValidateYearMonth(year, month);
            return _epoch + _this.Schema.GetEndOfMonth(year, month);
        }
    }
}
