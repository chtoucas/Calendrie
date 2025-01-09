// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Utilities;

public partial class PaxCalendar // Complements
{
    /// <summary>
    /// Obtains the number of weeks in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure]
    public int CountWeeksInYear(int year)
    {
        if (year < StandardScope.MinYear || year > StandardScope.MaxYear)
            ThrowHelpers.ThrowYearOutOfRange(year);

        return Schema.CountWeeksInYear(year);
    }
}
