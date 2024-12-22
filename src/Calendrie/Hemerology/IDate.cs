// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

// A date type is expected to provide a constructor or a static factory method
// for the following parameters:
// - (y, m, d)
// - (y, doy)
// - (dayNumber)

// Commentaires en partie obsolètes.
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
/// Defines a date type.
/// </summary>
/// <typeparam name="TSelf">The date type that implements this interface.
/// </typeparam>
public interface IDate<TSelf> :
    IDateable,
    IAbsoluteDate<TSelf>
    where TSelf : IDate<TSelf>
{ }
