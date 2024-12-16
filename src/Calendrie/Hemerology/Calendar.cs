﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Validation;

#region Developer Notes

// Standard calendar: year/month/day subdivision of time, a single era,
// that is an interval of days.
//
// Types implementing ICalendar or ICalendar<T>
// --------------------------------------------
//
// Calendar
//   CalendarSystem
//     CivilCalendar                    CivilDate
//     GregorianCalendar                GregorianDate
//     JulianCalendar                   JulianDate
//     etc
//   NakedCalendar
//     BoundedBelowCalendar
//     MinMaxYearCalendar
//
// Calendar vs Date
// ----------------
//
// Calendar pros
// - Pluggable calendars means that we define only once all kind of
//   calendrical objects (CalendarDate, CalendarDay, OrdinalDate,
//   CalendarYear, CalendarMonth, etc.).
// Date pros
// - We can be very specific. With calendars, we have to take the "plus
//   petit dénominateur commun".
//
// See https://blog.joda.org/2009/11/why-jsr-310-isn-joda-time_4941.html
//
// Remarks
// -------
//
// Interval of days: this is the simplest and most natural choice. Beware,
// without this assumption, arithmetic becomes much more complicated.
//
// Do NOT use DayNumber in parameters. Since the calendars we are dealing
// with are modeled around a year, month, day partition, most of the time a
// DayNumber in input will be converted before doing anything, so better
// left this (conversion) outside the method.
//
// Even for conversion purposes, we do not add methods to convert from a
// DayNumber to a y/m/d or y/doy representation. See comments in ICalendar<>.
//
// To be complete, we should have methods GetStart(End)OfYear(Month) with a
// DayNumber return type. We do not include them here but in ICalendar<>,
// which means that we expect that calendar developpers to implement
// ICalendar<DayNumber> (unless they have a custom day type like CalendarDay,
// in which they surely implement ICalendar<the date type>).
//
// Since week numbering has no universal definition, this interface has
// nothing to say about weeks.
//
// We have three different ways to model a date:
//  1. year/month/day repr. (human)
//  2. year/dayOfYear repr. (ordinal)
//  3. DaysSinceEpoch repr. (universal)
// Notes:
// - Common calendar types come w/ a companion date of type 1 and use
//   DayNumber for type 3.
// - DayNumber being always available, we offer conversions from
//   representation of type 1 and 2 to DayNumber.

// We have three different ways to model a date:
//  1. Year/Month/Day repr. (human)
//  2. Year/DayOfYear repr. (ordinal)
//  3. DayNumber repr.      (universal)
// Classes implementing this interface should also provide:
// - A type constructor, a factory,
//     GetXXX(repr) -> TDate
// - Conversion ops from the other date repr. to TDate
//     GetXXX(other repr) -> TDate
// where XXX is the name of the date type.
// With conversion ops, not only do we have to validate the input but we
// must also transform it before we can create the target object.
// We did not include them in the interface because GetDate() feel too
// generic to me. For instance,
// ZCalendar has
// - GetDate(y, m, d)   -> ZDate
// - GetDate(y, doy)    -> ZDate
// - GetDate(dayNumber) -> ZDate
// See also Calendar which offers several methods of this kind.
//
// Regarding min/max values, for exactly the same reason, we do not include
// them here but end calendars should have them, e.g. Min/MaxDate or a
// single MinMaxDate. For instance, I prefer Min/MaxDateParts to Min/MaxDate
// with NakedCalendar, but it could be also Min/MaxDay or Min/MaxOrdinalDate.
// Furthermore, for mono-system of calendars, we expect TDate to implement
// IMinMaxValue<TDate>.
//
// API
// ---
//
// Missing methods/props?
// - factories, today, conversions, interconversion
// - adjusters, providers
// - min/max values
//
// ## Mono-calendar system with a single companion date type.
// Ex. CivilCalendar+CivilDate
// * Calendar:
//   - impl ICalendar<CivilDate>, ie date providers
// * Date:
//   - impl IDate<TSelf, TCalendar>
//     - prop MinMaxValue <- IMinMaxValue<TSelf>
//     - static prop Calendar
//   - static prop Adjuster
//   - Today() <- ITodayProvider
// * Supporting types:
//   - date adjuster
//
// Special case of a mono-calendar system with TDate = DayNumber.
// - min/max values are provided by Calendar.Domain
// - today is the only method missing
// - factories are provided by BasicCalendar
// - conversions, none
// - adjusters, none
//
// ## Poly-calendar system with a single companion date type
// Ex. ZCalendar+ZDate
// * Calendar:
//   - impl ICalendar<ZDate>, ie date providers
//   - prop MinMaxDate
//   - impl IDateFactory<ZDate>
//      - GetDate(dayNumber)
//      - GetDate(y, m, d)
//      - GetDate(y, doy)
//   - Today()
// * Date:
//   - impl IDate<TSelf>
//   - inst prop Calendar
//   - WithCalendar(newCalendar) <- interconversion
//   - Today() is using the __default calendar__
//   - FromDayNumber() is using the __default calendar__
// * Supporting types:
//   - date adjuster
//
// ## Poly-calendar system with more than one companion date type
// Ex. SimpleCalendar+CalendarDate/CalendarDay/OrdinalDate.
// * Calendar:
//   - impl ICalendar
//   - prop MinMaxXXX
//   - GetXXXDate(dayNumber)
//   - GetXXXDate(y, m, d)
//   - GetXXXDate(y, doy)
//   - GetCurrentXXX()
// * Date:
//   - impl IDate<TSelf>
//   - inst prop Calendar
//   - WithCalendar(newCalendar) <- interconversion
//   - Today() is using the __default calendar__
//   - FromDayNumber() is using the __default calendar__
// * Supporting types:
//   - date provider
//   - date adjuster
//
// ## The date type is the calendar type too
// Ex. XCivilDate or MyDate.
//
// ## Mono-calendar system without a companion date type.
// Ex. NakedCalendar, MinMaxYearCalendar<TDate>?

#endregion

/// <summary>
/// Represents a calendar and provides a base for derived classes.
/// <para>We do NOT assume the existence of a dedicated companion date type.
/// </para>
/// </summary>
public abstract partial class Calendar : ICalendricalCore
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="Calendar"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    protected Calendar(string name, CalendarScope scope)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(scope);

        Name = name;
        Scope = scope;

        Epoch = scope.Epoch;
        Schema = scope.Schema;
        YearsValidator = scope.YearsValidator;
    }

    /// <summary>
    /// Gets the culture-independent name of the calendar.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the calendar scope.
    /// </summary>
    public CalendarScope Scope { get; }

    /// <summary>
    /// Gets the epoch.
    /// </summary>
    public DayNumber Epoch { get; }

    /// <inheritdoc />
    public CalendricalAlgorithm Algorithm => Schema.Algorithm;

    /// <inheritdoc />
    public CalendricalFamily Family => Schema.Family;

    /// <inheritdoc />
    public CalendricalAdjustments PeriodicAdjustments => Schema.PeriodicAdjustments;

    /// <summary>
    /// Gets a validator for the range of supported years.
    /// </summary>
    protected IYearsValidator YearsValidator { get; }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    protected ICalendricalSchema Schema { get; }

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public override string ToString() => Name;

    /// <inheritdoc />
    [Pure]
    public bool IsRegular(out int monthsInYear) => Schema.IsRegular(out monthsInYear);
}

public partial class Calendar // Year, month, day infos
{
#pragma warning disable CA1725 // Parameter names should match base declaration (Naming) ✓
    // Base parameter names (y, m, d) are not explicit enough.

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of supported years.</exception>
    [Pure]
    public bool IsLeapYear(int year)
    {
        YearsValidator.Validate(year);
        return Schema.IsLeapYear(year);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException">The month is either invalid
    /// or outside the range of supported months.</exception>
    [Pure]
    public bool IsIntercalaryMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Schema.IsIntercalaryMonth(year, month);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException">The date is either invalid
    /// or outside the range of supported dates.</exception>
    [Pure]
    public bool IsIntercalaryDay(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return Schema.IsIntercalaryDay(year, month, day);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException">The date is either invalid
    /// or outside the range of supported dates.</exception>
    [Pure]
    public bool IsSupplementaryDay(int year, int month, int day)
    {
        Scope.ValidateYearMonthDay(year, month, day);
        return Schema.IsSupplementaryDay(year, month, day);
    }

    // Les méthodes suivantes sont abstraites car une année ou un mois peut être
    // incomplet.

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure] public abstract int CountMonthsInYear(int year);

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure] public abstract int CountDaysInYear(int year);

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException">The month is either invalid
    /// or outside the range of supported months.</exception>
    [Pure] public abstract int CountDaysInMonth(int year, int month);

#pragma warning restore CA1725
}
