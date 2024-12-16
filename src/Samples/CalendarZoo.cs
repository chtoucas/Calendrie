// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using Calendrie;
using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;

/// <summary>
/// Provides a compendium of calendars.
/// <para>These calendars don't come with a companion date type.</para>
/// <para>Unless specified otherwise, the calendars listed here do not allow
/// dates prior their epochal origin.</para>
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
                BoundedBelowScope.StartingAt<CivilSchema>(DayZero.NewStyle, new DateParts(1582, 10, 15)));

        internal static readonly MinMaxYearCalendar Julian =
            new("Julian",
                MinMaxYearScope.StartingAt<JulianSchema>(DayZero.OldStyle, 8));

        internal static readonly MinMaxYearCalendar FrenchRevolutionary =
            new("French Revolutionary",
                MinMaxYearScope.Create<Coptic12Schema>(DayZero.FrenchRepublican, Range.Create(1, 14)));
    }
}

// Long proleptic calendars:
// - LongGregorian
// - LongJulian
// - Tropicalia
public partial class CalendarZoo
{
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

    /// <summary>
    /// Gets the (long) proleptic Tropicália calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar Tropicalia => LongCalendars.Tropicalia;

    private static class LongCalendars
    {
        static LongCalendars() { }

        internal static readonly MinMaxYearCalendar Gregorian =
            new("Gregorian", MinMaxYearScope.CreateMaximal<GregorianSchema>(DayZero.NewStyle));

        internal static readonly MinMaxYearCalendar Julian =
            new("Julian", MinMaxYearScope.CreateMaximal<JulianSchema>(DayZero.OldStyle));

        internal static readonly MinMaxYearCalendar Tropicalia =
            new("Tropicalia", MinMaxYearScope.CreateMaximal<TropicaliaSchema>(DayZero.NewStyle));
    }
}

// Other retropolated calendars:
// - Egyptian
// - FrenchRepublican
// - Persian2820
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
    /// Gets the Persian calendar (proposed arithmetical form).
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar Persian2820 => RetropolatedCalendars.Persian2820;

    private static class RetropolatedCalendars
    {
        static RetropolatedCalendars() { }

        internal static readonly MinMaxYearCalendar Egyptian =
            new("Egyptian",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1<Egyptian12Schema>(DayZero.Egyptian));

        internal static readonly MinMaxYearCalendar FrenchRepublican =
            new("French Republican",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1<FrenchRepublican12Schema>(
                    DayZero.FrenchRepublican));

        internal static readonly MinMaxYearCalendar Persian2820 =
            new("Tabular Persian",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1<Persian2820Schema>(DayZero.Persian));
    }
}

// Perennial calendars:
// - InternationalFixed (blank-day)
// - Pax (leap-week)
// - Positivist (blank-day)
// - RevisedWorld (blank-day)
// - World (blank-day)
public partial class CalendarZoo
{
    /// <summary>
    /// Gets the International Fixed calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar InternationalFixed => PerennialCalendars.InternationalFixed;

    /// <summary>
    /// Gets the Pax calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar Pax => PerennialCalendars.Pax;

    /// <summary>
    /// Gets the Positivist calendar aka the Georgian calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar Positivist => PerennialCalendars.Positivist;

    /// <summary>
    /// Gets the revised World calendar aka the Universal calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar RevisedWorld => PerennialCalendars.RevisedWorld;

    /// <summary>
    /// Gets the World calendar.
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar World => PerennialCalendars.World;

    private static class PerennialCalendars
    {
        static PerennialCalendars() { }

        internal static readonly MinMaxYearCalendar InternationalFixed =
            // The International Fixed calendar re-uses the Gregorian epoch.
            new("International Fixed",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1<InternationalFixedSchema>(
                    DayZero.NewStyle));

        internal static readonly MinMaxYearCalendar Pax =
            new("Pax",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1<PaxSchema>(
                    DayZero.SundayBeforeGregorian));

        internal static readonly MinMaxYearCalendar Positivist =
            new("Positivist",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1<PositivistSchema>(DayZero.Positivist));

        internal static readonly MinMaxYearCalendar RevisedWorld =
            // The Revised World calendar re-uses the Gregorian epoch.
            new("Revised World",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1<WorldSchema>(DayZero.NewStyle));

        internal static readonly MinMaxYearCalendar World =
            new("World",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1<WorldSchema>(DayZero.SundayBeforeGregorian));
    }
}

// Offsetted calendars:
// - Holocene
// - Minguo
// - ThaiSolar
public partial class CalendarZoo
{
    /// <summary>
    /// Gets the Holocene calendar.
    /// <para>The Holocene calendar differs from the Gregorian calendar only in
    /// the way years are numbered.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar Holocene => OffsettedCalendars.Holocene;

    /// <summary>
    /// Gets the Minguo calendar.
    /// <para>The Minguo calendar differs from the Gregorian calendar only in
    /// the way years are numbered.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar Minguo => OffsettedCalendars.Minguo;

    /// <summary>
    /// Gets the Thai solar calendar.
    /// <para>The Thai solar calendar differs from the Gregorian calendar only in
    /// the way years are numbered.</para>
    /// <para>This static property is thread-safe.</para>
    /// </summary>
    public static MinMaxYearCalendar ThaiSolar => OffsettedCalendars.ThaiSolar;

    private static class OffsettedCalendars
    {
        static OffsettedCalendars() { }

        internal static readonly MinMaxYearCalendar Holocene =
            new("Holocene",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1(
                    OffsettedSchema.Create<CivilSchema>(10_000), DayZero.NewStyle));

        internal static readonly MinMaxYearCalendar Minguo =
            new("Holocene",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1(
                    OffsettedSchema.Create<CivilSchema>(-1911), DayZero.NewStyle));

        internal static readonly MinMaxYearCalendar ThaiSolar =
            new("Holocene",
                MinMaxYearScope.CreateMaximalOnOrAfterYear1(
                    OffsettedSchema.Create<CivilSchema>(543), DayZero.NewStyle));
    }
}
