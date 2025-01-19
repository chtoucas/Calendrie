// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

// TODO(code): blank-days should be kept outside the week cycle, review all
// methods that compute the days of the week.

// Epagomenal days
// ---------------
//
// Epagomenal days are usually found in descendants of the Egyptian calendar.
// Un jour épagomène est un des 5 ou 6 jours ajoutés en fin d'année
// d'un calendrier composé de 12 mois de 30 jours pour synchroniser les
// années avec le cycle solaire.
// Un jour épagomène ne fait partie d'aucun mois, cependant pour des
// questions d'ordre technique on le rattache au douzième mois.
// Ex. : le jour de la révolution du calendrier républicain.
//
// Blank days
// ----------
//
// The use of blank-days can be traced back to Rev. Hugh Jones (1745) and
// was rediscovered later by Abbot Marco Mastrofini (1834).
// Also it seems that the "same idea had been thought of ~1650 years earlier
// c. 100 BCE and incorporated into the calendar used by the Qumran
// community"; see the wikipedia page
// https://en.wikipedia.org/wiki/Hugh_Jones_(professor)
//
// A blank-day schema is a solar schema that adds one extra blank day on
// common years and two on leap years. A blank day does not belong to any month
// and is kept outside the weekday cycle.
// For technical reasons, we pretend that a blank day is the last day of
// the preceding month.
// Blank-day calendars belong to the larger family of perennial calendars.

public partial struct ArmenianDate // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
        return Egyptian12Schema.IsEpagomenalDayImpl(d, out epagomenalNumber);
    }
}

public partial struct Armenian13Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
        return Egyptian13Schema.IsEpagomenalDayImpl(m, d, out epagomenalNumber);
    }
}

public partial struct CopticDate // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
        return Coptic12Schema.IsEpagomenalDayImpl(d, out epagomenalNumber);
    }
}

public partial struct Coptic13Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
        return Coptic13Schema.IsEpagomenalDayImpl(m, d, out epagomenalNumber);
    }
}

public partial struct EgyptianDate // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
        return Egyptian12Schema.IsEpagomenalDayImpl(d, out epagomenalNumber);
    }
}

public partial struct Egyptian13Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
        return Egyptian13Schema.IsEpagomenalDayImpl(m, d, out epagomenalNumber);
    }
}

public partial struct EthiopicDate // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
        return Coptic12Schema.IsEpagomenalDayImpl(d, out epagomenalNumber);
    }
}

public partial struct Ethiopic13Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
        return Coptic13Schema.IsEpagomenalDayImpl(m, d, out epagomenalNumber);
    }
}

public partial struct FrenchRepublicanDate // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
        return FrenchRepublican12Schema.IsEpagomenalDayImpl(d, out epagomenalNumber);
    }
}

public partial struct FrenchRepublican13Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
        return FrenchRepublican13Schema.IsEpagomenalDayImpl(m, d, out epagomenalNumber);
    }
}

public partial struct InternationalFixedDate // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is a blank day;
    /// otherwise returns <see langword="false"/>.
    /// <para>A blank day does not belong to any month and is kept outside the
    /// weekday cycle.</para>
    /// </summary>
    public bool IsBlank
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
            return InternationalFixedSchema.IsBlankDayImpl(d);
        }
    }
}

public partial struct PositivistDate // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is a blank day;
    /// otherwise returns <see langword="false"/>.
    /// <para>A blank day does not belong to any month and is kept outside the
    /// weekday cycle.</para>
    /// </summary>
    public bool IsBlank
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
            return PositivistSchema.IsBlankDayImpl(d);
        }
    }
}

public partial struct WorldDate // Complements
{
    /// <summary>
    /// Returns <see langword="true"/> if the current instance is a blank day;
    /// otherwise returns <see langword="false"/>.
    /// <para>A blank day does not belong to any month and is kept outside the
    /// weekday cycle.</para>
    /// </summary>
    public bool IsBlank
    {
        get
        {
            Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
            return WorldSchema.IsBlankDayImpl(m, d);
        }
    }
}

public partial struct ZoroastrianDate // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out _, out int d);
        return Egyptian12Schema.IsEpagomenalDayImpl(d, out epagomenalNumber);
    }
}

public partial struct Zoroastrian13Date // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Calendar.Schema.GetDateParts(_daysSinceEpoch, out _, out int m, out int d);
        return Egyptian13Schema.IsEpagomenalDayImpl(m, d, out epagomenalNumber);
    }
}
