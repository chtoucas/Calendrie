// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Hemerology;

// Struct constraint: no ArgumentNullException.ThrowIfNull(date).

/// <summary>
/// Provides static helpers for <see cref="DateAdjuster{TDate}"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class DateAdjuster
{
    /// <summary>
    /// Creates a new instance of the <see cref="DateAdjuster{TDate}"/> class
    /// from the specified calendar.
    /// </summary>
    [Pure]
    public static DateAdjuster<TDate> Create<TDate>(CalendarSystem<TDate> calendar)
        where TDate : struct, IDateable, IDateFactory<TDate>
    {
        ArgumentNullException.ThrowIfNull(calendar);

        return new(calendar.Scope);
    }
}

/// <summary>
/// Defines an adjuster for <typeparamref name="TDate"/> and provides a base for
/// derived classes.
/// <para>This class can ONLY be initialized from within friend assemblies.
/// </para>
/// </summary>
/// <typeparam name="TDate">The type of date object.</typeparam>
public sealed class DateAdjuster<TDate>
    where TDate : struct, IDateable, IDateFactory<TDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateAdjuster{TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="scope"/> is
    /// null.</exception>
    internal DateAdjuster(CalendarScope scope)
    {
        ArgumentNullException.ThrowIfNull(scope);

        Scope = scope;
        Schema = scope.Schema;
        // Cache the pre-validator which is a computed prop.
        PreValidator = Schema.PreValidator;
    }

    /// <summary>
    /// Gets the scope.
    /// </summary>
    internal CalendarScope Scope { get; }

    /// <summary>
    /// Gets the schema.
    /// </summary>
    private ICalendricalSchema Schema { get; }

    /// <summary>
    /// Gets the pre-validator.
    /// </summary>
    private ICalendricalPreValidator PreValidator { get; }

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
        int daysSinceEpoch = Schema.GetStartOfYear(date.Year);
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
        int daysSinceEpoch = Schema.GetEndOfYear(date.Year);
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
        int daysSinceEpoch = Schema.GetStartOfMonth(y, m);
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
        int daysSinceEpoch = Schema.GetEndOfMonth(y, m);
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
        Scope.ValidateYearMonthDay(newYear, m, d, nameof(newYear));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(newYear, m, d);
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
        PreValidator.ValidateMonthDay(y, newMonth, d, nameof(newMonth));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newMonth, d);
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
    public TDate AdjustDayOfMonth(TDate date, int newDayOfMonth)
    {
        var (y, m, _) = date;
        // We only need to validate "newDay".
        PreValidator.ValidateDayOfMonth(y, m, newDayOfMonth, nameof(newDayOfMonth));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, m, newDayOfMonth);
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
        PreValidator.ValidateDayOfYear(y, newDayOfYear, nameof(newDayOfYear));

        int daysSinceEpoch = Schema.CountDaysSinceEpoch(y, newDayOfYear);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }
}
