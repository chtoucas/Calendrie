// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

// A date type is expected to provide a constructor or a factory with the
// following parameters:
// - (y, m, d)
// - (y, doy)
// - (dayNumber)

/// <summary>
/// Defines a date type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IDate<TSelf> :
    IDateable,
    IFixedDate<TSelf>,
    // Comparison
    IComparisonOperators<TSelf, TSelf>,
    IMinMaxFunction<TSelf>,
    // Arithmetic
    IDayArithmetic<TSelf>,
    IAdditionOperators<TSelf, int, TSelf>,
    ISubtractionOperators<TSelf, int, TSelf>,
    IIncrementOperators<TSelf>,
    IDecrementOperators<TSelf>
    where TSelf : IDate<TSelf>
{
    /// <summary>
    /// Subtracts the two specified dates and returns the number of days between
    /// them.
    /// </summary>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "See IStandardArithmetic<TSelf>.CountDaysSince()")]
    static abstract int operator -(TSelf left, TSelf right);
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
/// Defines a date type with a companion calendar of fixed type.
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
