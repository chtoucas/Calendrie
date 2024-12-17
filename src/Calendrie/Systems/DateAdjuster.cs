// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Hemerology;

// See comments in CalendarSystem<>.

/// <summary>
/// Defines an adjuster for <typeparamref name="TDate"/> and provides a base for
/// derived classes.
/// <para>This class can ONLY be initialized from within friend assemblies.
/// </para>
/// </summary>
/// <typeparam name="TDate">The type of date object.</typeparam>
public sealed class DateAdjuster<TDate>
    where TDate : struct, IDate<TDate>, IFixedDateFactory<TDate>
{
    /// <summary>
    /// Represents the calendar scope.
    /// </summary>
    private readonly CalendarScope _scope;

    /// <summary>
    /// Represents the schema.
    /// </summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateAdjuster{TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="calendar"/> is
    /// null.</exception>
    internal DateAdjuster(CalendarSystem<TDate> calendar)
    {
        ArgumentNullException.ThrowIfNull(calendar);

        Calendar = calendar;

        _scope = calendar.Scope;
        _schema = calendar.Scope.Schema;
    }

    /// <summary>
    /// Gets the associated calendar.
    /// </summary>
    public CalendarSystem<TDate> Calendar { get; }

    /// <summary>
    /// Obtains the first day of the year to which belongs the specified date.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="date"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The result would overflow
    /// the range of supported dates.</exception>
    [Pure]
    public TDate GetStartOfYear(TDate date)
    {
        int daysSinceEpoch = _schema.GetStartOfYear(date.Year);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <summary>
    /// Obtains the last day of the year to which belongs the specified date.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="date"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The result would overflow
    /// the range of supported dates.</exception>
    [Pure]
    public TDate GetEndOfYear(TDate date)
    {
        int daysSinceEpoch = _schema.GetEndOfYear(date.Year);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <summary>
    /// Obtains the first day of the month to which belongs the specified date.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="date"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The result would overflow
    /// the range of supported dates.</exception>
    [Pure]
    public TDate GetStartOfMonth(TDate date)
    {
        var (y, m, _) = date;
        int daysSinceEpoch = _schema.GetStartOfMonth(y, m);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <summary>
    /// Obtains the last day of the month to which belongs the specified date.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="date"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The result would overflow
    /// the range of supported dates.</exception>
    [Pure]
    public TDate GetEndOfMonth(TDate date)
    {
        var (y, m, _) = date;
        int daysSinceEpoch = _schema.GetEndOfMonth(y, m);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    //
    // Adjustments for the core parts
    //

    /// <summary>
    /// Adjusts the year field to the specified value, yielding a new date.
    /// <para>See also TDate.WithYear().</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure]
    public TDate AdjustYear(TDate date, int newYear)
    {
        var (_, m, d) = date;
        // We MUST re-validate the entire date.
        _scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = _schema.CountDaysSinceEpoch(newYear, m, d);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <summary>
    /// Adjusts the month field to the specified value, yielding a new date.
    /// <para>See also TDate.WithMonth().</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure]
    public TDate AdjustMonth(TDate date, int newMonth)
    {
        var (y, _, d) = date;
        // We only need to validate "newMonth" and "d".
        _schema.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = _schema.CountDaysSinceEpoch(y, newMonth, d);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <summary>
    /// Adjusts the day of the month field to the specified value, yielding a new
    /// date.
    /// <para>See also TDate.WithDay().</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure]
    public TDate AdjustDay(TDate date, int newDay)
    {
        var (y, m, _) = date;
        // We only need to validate "newDay".
        if (newDay < 1
            || (newDay > _schema.MinDaysInMonth
                && newDay > _schema.CountDaysInMonth(y, m)))
        {
            throw new ArgumentOutOfRangeException(nameof(newDay));
        }

        int daysSinceEpoch = _schema.CountDaysSinceEpoch(y, m, newDay);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <summary>
    /// Adjusts the day of the year field to the specified value, yielding a new
    /// date.
    /// <para>See also TDate.WithDayOfYear().</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The resulting date would
    /// be invalid.</exception>
    [Pure]
    public TDate AdjustDayOfYear(TDate date, int newDayOfYear)
    {
        int y = date.Year;
        // We only need to validate "newDayOfYear".
        _schema.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = _schema.CountDaysSinceEpoch(y, newDayOfYear);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }
}
