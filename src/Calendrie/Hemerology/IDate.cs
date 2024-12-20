// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

// A date type is expected to provide a constructor or a static factory method
// for the following parameters:
// - (y, m, d)
// - (y, doy)
// - (dayNumber)

/// <summary>
/// Defines a date type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IDate<TSelf> :
    IDateable,
    IAbsoluteDate<TSelf>
    where TSelf : IDate<TSelf>
{
    /// <summary>
    /// Adjusts the current instance using the specified adjuster.
    /// <para>If the adjuster throws, this method will propagate the exception.
    /// </para>
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="adjuster"/> is
    /// <see langword="null"/>.</exception>
    [Pure] TSelf Adjust(Func<TSelf, TSelf> adjuster);
}

// L'interface suivante est prévue pour les dates ne fonctionnant qu'avec un seul
// type de calendrier, d'où le fait d'avoir choisi des propriétés et méthodes
// __statiques__ :
// - IMinMaxValue<T>
// - Calendar
// - FromDayNumber()
//
// Pour des dates fonctionnant avec un calendrier "pluriel", on utilisera
// plutôt une propriété non-statique Calendar et on ajoutera une méthode
// WithCalendar(newCalendar) pour l'interconversion.

/// <summary>
/// Defines a date type with a fixed companion calendar type.
/// <para>This interface SHOULD NOT be implemented by date types participating
/// in a poly-calendar system.</para>
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="TCalendar">The companion calendar type.</typeparam>
public interface IDate<TSelf, out TCalendar> :
    IDate<TSelf>,
    IMinMaxValue<TSelf>
    where TCalendar : Calendar
    where TSelf : IDate<TSelf, TCalendar>
{
    /// <summary>
    /// Gets the calendar to which belongs the current date type.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    static abstract TCalendar Calendar { get; }

    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSelf"/> struct from
    /// the specified day number.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayNumber"/>
    /// is outside the range of supported values.</exception>
    static abstract TSelf FromDayNumber(DayNumber dayNumber);
}
