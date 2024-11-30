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
/// <para>These calendars are pretty useless on their own as they don't come
/// with a date type.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class CalendarZoo { }

// More historically accurate calendars:
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
            new("Gregorian",
                BoundedBelowScope.StartingAt(
                    new CivilSchema(), DayZero.NewStyle, new DateParts(1582, 10, 15)));

        internal static readonly MinMaxYearCalendar Julian =
            new("Julian",
                MinMaxYearScope.StartingAt(
                    new JulianSchema(), DayZero.OldStyle, 8));

        internal static readonly MinMaxYearCalendar FrenchRevolutionary =
            new("French Revolutionary",
                MinMaxYearScope.Create(
                    new Coptic12Schema(), DayZero.FrenchRepublican, Range.Create(1, 14)));
    }
}

// Long proleptic calendars:
// - Tropicalia
// - LongGregorian
// - LongJulian
public partial class CalendarZoo
{
    /// <summary>
    /// Gets the proleptic Tropicália calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar Tropicalia => LongCalendars.Tropicalia;

    /// <summary>
    /// Gets the (long) proleptic Gregorian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar LongGregorian => LongCalendars.Gregorian;

    /// <summary>
    /// Gets the (long) proleptic Julian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar LongJulian => LongCalendars.Julian;

    private static class LongCalendars
    {
        static LongCalendars() { }

        internal static readonly MinMaxYearCalendar Gregorian =
            new("Gregorian",
                MinMaxYearScope.CreateMaximal(new GregorianSchema(), DayZero.NewStyle));

        internal static readonly MinMaxYearCalendar Julian =
            new("Julian",
                MinMaxYearScope.CreateMaximal(new JulianSchema(), DayZero.NewStyle));

        internal static readonly MinMaxYearCalendar Tropicalia =
            new("Tropicalia",
                MinMaxYearScope.CreateMaximal(new TropicaliaSchema(), DayZero.NewStyle));
    }
}

// Retropolated calendars:
// - Egyptian
// - FrenchRepublican
// - InternationalFixed
// - Pax
// - Persian2820
// - Positivist
// - RevisedWorld
// - World
public partial class CalendarZoo
{
    /// <summary>
    /// Gets the Egyptian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar Egyptian => RetropolatedCalendars.Egyptian;

    /// <summary>
    /// Gets the French Republican calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar FrenchRepublican => RetropolatedCalendars.FrenchRepublican;

    /// <summary>
    /// Gets the International Fixed calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar InternationalFixed => RetropolatedCalendars.InternationalFixed;

    ///// <summary>
    ///// Gets the Pax calendar.
    ///// <para>This static property is thread-safe.</para>
    ///// </summary>
    //public static MinMaxYearCalendar Pax => RetropolatedCalendars.Pax;

    /// <summary>
    /// Gets the Persian calendar (proposed arithmetical form).
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar Persian2820 => RetropolatedCalendars.Persian2820;

    /// <summary>
    /// Gets the Positivist calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar Positivist => RetropolatedCalendars.Positivist;

    /// <summary>
    /// Gets the revised World calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar RevisedWorld => RetropolatedCalendars.RevisedWorld;

    /// <summary>
    /// Gets the World calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar World => RetropolatedCalendars.World;

    private static class RetropolatedCalendars
    {
        static RetropolatedCalendars() { }

        internal static readonly MinMaxYearCalendar Egyptian =
            new("Egyptian",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1(new Egyptian12Schema(), DayZero.Egyptian));

        internal static readonly MinMaxYearCalendar FrenchRepublican =
            new("French Republican",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1(
                    new FrenchRepublican12Schema(), DayZero.FrenchRepublican));

        internal static readonly MinMaxYearCalendar InternationalFixed =
            // The International Fixed calendar re-uses the Gregorian epoch.
            new("International Fixed",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1(
                    new InternationalFixedSchema(), DayZero.NewStyle));

        //internal static readonly MinMaxYearCalendar Pax =
        //    new("Pax",
        //        MinMaxYearScope.CreateMaximalOnOrAfterYear1(
        //            new PaxSchema(), DayZero.SundayBeforeGregorian));

        internal static readonly MinMaxYearCalendar Persian2820 =
            new("Tabular Persian",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1(
                    new Persian2820Schema(), DayZero.Persian));

        internal static readonly MinMaxYearCalendar Positivist =
            new("Positivist",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1(
                    new PositivistSchema(), DayZero.Positivist));

        internal static readonly MinMaxYearCalendar RevisedWorld =
            // The Revised World calendar re-uses the Gregorian epoch.
            new("Revised World",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1(
                    new WorldSchema(), DayZero.NewStyle));

        internal static readonly MinMaxYearCalendar World =
            new("World",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1(
                    new WorldSchema(), DayZero.SundayBeforeGregorian));
    }
}

// Offsetted calendars:
// - Holocene
// - Minguo
// - ThaiSolar
public partial class CalendarZoo
{
}
