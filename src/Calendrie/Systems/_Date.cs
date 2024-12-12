// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct ArmenianDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct Armenian13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct CopticDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct Coptic13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct EthiopicDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct Ethiopic13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct TabularIslamicDate { }

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct WorldDate : IBlankDay
{
    /// <inheritdoc />
    public bool IsBlank
    {
        get
        {
            Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
            return Schema.IsBlankDay(y, m, d);
        }
    }
}

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct ZoroastrianDate : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct Zoroastrian13Date : IEpagomenalDay
{
    /// <inheritdoc />
    [Pure]
    public bool IsEpagomenal(out int epagomenalNumber)
    {
        Schema.GetDateParts(_daysSinceEpoch, out int y, out int m, out int d);
        return Schema.IsEpagomenalDay(y, m, d, out epagomenalNumber);
    }
}
