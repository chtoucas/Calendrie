// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Samples;

using Calendrie;
using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;
using Calendrie.Hemerology.Scopes;

/// <summary>
/// Provides a compendium of calendars.
/// <para>Unless specified otherwise, the calendars listed here do not allow
/// dates prior their epochal origin.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class CalendarZoo { }

// More historically accurate calendars of type NakedCalendar.
// - GenuineGregorian
// - GenuineJulian
// - FrenchRevolutionary
public partial class CalendarZoo
{
    /// <summary>
    /// Gets the Gregorian calendar with dates on or after the 15th of October
    /// 1582 CE.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static BoundedBelowCalendar GenuineGregorian => GenuineCalendars.Gregorian;

    /// <summary>
    /// Gets the Julian calendar with dates on or after the year 8.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar GenuineJulian => GenuineCalendars.Julian;

    /// <summary>
    /// Gets the French Revolutionary calendar with dates in the range from year
    /// I to year XIV.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar FrenchRevolutionary =>
        // Il s'agit d'une arithmétisation du calendrier révolutionnaire :
        // correspond à la réalité pour les années I à XIV, mais peut
        // diverger ensuite car on n'utilise pas la règle astronomique pour
        // déterminer le jour de l'an.
        //
        // Formellement, l'an XIV est incomplet, mais, pour simplifier, on
        // le fait aller jusqu'au bout, de même pour l'an I.
        // Entrée en vigueur le 15 vendémiaire de l'an II, c-à-d le 6
        // octobre 1793 (grégorien).
        // Abrogation le 10 nivôse de l'an XIV (10/4/XIV), c-à-d le 31
        // décembre 1805 (grégorien).
        GenuineCalendars.FrenchRevolutionary;

    private static class GenuineCalendars
    {
        static GenuineCalendars() { }

        internal static readonly BoundedBelowCalendar Gregorian =
            new("Genuine Gregorian",
                BoundedBelowScope.StartingAt(
                    new CivilSchema(), DayZero.NewStyle, new DateParts(1582, 10, 15)));

        internal static readonly MinMaxYearCalendar Julian =
            new("Genuine Julian",
                MinMaxYearScope.StartingAt(
                    new JulianSchema(), DayZero.OldStyle, 8));

        internal static readonly MinMaxYearCalendar FrenchRevolutionary =
            new("French Revolutionary",
                MinMaxYearScope.Create(
                    new Coptic12Schema(), DayZero.FrenchRepublican, Range.Create(1, 14)));
    }
}
