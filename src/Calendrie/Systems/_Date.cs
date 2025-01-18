// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

public partial struct ArmenianDate : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct Armenian13Date : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct CopticDate : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct Coptic13Date : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct EgyptianDate : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct Egyptian13Date : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct EthiopicDate : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct Ethiopic13Date : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct FrenchRepublicanDate : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct FrenchRepublican13Date : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct WorldDate : IBlankDay // Complements
{
    /// <inheritdoc />
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

public partial struct ZoroastrianDate : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

public partial struct Zoroastrian13Date : IEpagomenalDay // Complements
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        var sch = Calendar.Schema;
        sch.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return sch.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}
