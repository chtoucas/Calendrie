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
public sealed class DateAdjuster<TDate> : IDateAdjuster<TDate>
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

    /// <inheritdoc />
    [Pure]
    public TDate GetStartOfYear(TDate date)
    {
        int daysSinceEpoch = _schema.GetStartOfYear(date.Year);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public TDate GetEndOfYear(TDate date)
    {
        int daysSinceEpoch = _schema.GetEndOfYear(date.Year);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public TDate GetStartOfMonth(TDate date)
    {
        var (y, m, _) = date;
        int daysSinceEpoch = _schema.GetStartOfMonth(y, m);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    [Pure]
    public TDate AdjustYear(TDate date, int newYear)
    {
        var (_, m, d) = date;
        // We MUST re-validate the entire date.
        _scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = _schema.CountDaysSinceEpoch(newYear, m, d);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <inheritdoc />
    [Pure]
    public TDate AdjustMonth(TDate date, int newMonth)
    {
        var (y, _, d) = date;
        // We only need to validate "newMonth" and "d".
        _schema.PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = _schema.CountDaysSinceEpoch(y, newMonth, d);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    [Pure]
    public TDate AdjustDayOfYear(TDate date, int newDayOfYear)
    {
        int y = date.Year;
        // We only need to validate "newDayOfYear".
        _schema.PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = _schema.CountDaysSinceEpoch(y, newDayOfYear);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    //
    // Adjusters for the core parts
    //
    // These adjusters are meant to be used by IAdjustable.Adjust().

    /// <summary>
    /// Obtains an adjuster for the year field of a date.
    /// </summary>
    [Pure]
    public Func<TDate, TDate> WithYear(int newYear) => x => AdjustYear(x, newYear);

    /// <summary>
    /// Obtains an adjuster for the month field of a date.
    /// </summary>
    [Pure]
    public Func<TDate, TDate> WithMonth(int newMonth) => x => AdjustMonth(x, newMonth);

    /// <summary>
    /// Obtains an adjuster for the day of the month field of a date.
    /// </summary>
    [Pure]
    public Func<TDate, TDate> WithDay(int newDay) => x => AdjustDay(x, newDay);

    /// <summary>
    /// Obtains an adjuster for the day of the year field of a date.
    /// </summary>
    [Pure]
    public Func<TDate, TDate> WithDayOfYear(int newDayOfYear) =>
        x => AdjustDayOfYear(x, newDayOfYear);
}
