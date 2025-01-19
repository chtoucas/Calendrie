// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

// Epagomenal days are usually found in descendants of the Egyptian calendar.
// Un jour épagomène est un des 5 ou 6 jours ajoutés en fin d'année
// d'un calendrier composé de 12 mois de 30 jours pour synchroniser les
// années avec le cycle solaire.
// Un jour épagomène ne fait partie d'aucun mois, cependant pour des
// questions d'ordre technique on le rattache au douzième mois.
// Ex. : le jour de la révolution du calendrier républicain.

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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
            var sch = Calendar.Schema;
            sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return sch.IsBlankDay(y, m, d);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
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
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}
