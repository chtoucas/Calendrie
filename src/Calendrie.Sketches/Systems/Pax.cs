// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

public partial class PaxCalendar // Complements
{
    // REVIEW(code): PaxMonth.TryCreate() should use this.
    // Make it a constant via T4 and add it to the parameters to be set for a
    // non-regular calendar (CodeTemplates/README)?
#if false
    internal const int MinMonthsInYear = PaxSchema.MinMonthsInYear;
#endif

    /// <summary>
    /// Obtains the number of weeks in the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the
    /// range of supported years.</exception>
    [Pure]
    public int CountWeeksInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountWeeksInYear(year);
    }
}
