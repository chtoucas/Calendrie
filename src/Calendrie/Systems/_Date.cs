// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

public partial struct ArmenianDate // Complements
{
    /// <summary>
    /// Determines whether the current instance is an epagomenal day or not, and
    /// also returns the epagomenal number of the day in an output parameter,
    /// zero if the date is not an epagomenal day.
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
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
    /// <para>Epagomenal days are usually found in descendants of the Egyptian
    /// calendar.</para>
    /// </summary>
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}
