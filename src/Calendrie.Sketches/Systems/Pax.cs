// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Utilities;

public partial class PaxCalendar // Complements
{
    // REVIEW(code): MinMonthsInYear
    // Make it a constant via T4 and add it to the parameters to be set for a
    // non-regular calendar (CodeTemplates/README.txt)?
    // Mieux: utiliser un "scope" dédié à ce calendrier.
    internal const int MinMonthsInYear = 13;

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
