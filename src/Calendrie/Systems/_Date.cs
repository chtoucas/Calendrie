// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
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

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
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

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
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

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
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

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
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

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
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

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct TabularIslamicDate // Complements
{ }

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
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

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
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

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
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
