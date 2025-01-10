// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

// See also Calendrie.Extensions.Interconversion.

public partial struct TropicaliaDate
{
    /// <summary>
    /// Creates a new instance of the <see cref="TropicaliaDate"/> struct from
    /// the specified absolute date.
    /// </summary>
    [Pure]
    public static TropicaliaDate FromAbsoluteDate(IAbsoluteDate date)
    {
        ArgumentNullException.ThrowIfNull(date);
        return FromDayNumber(date.DayNumber);
    }
}
