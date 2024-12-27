// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

/// <remarks>This calendar is <i>retropolated</i>. It supports <i>all</i> dates
/// within the range [1..9999] of years.</remarks>
public partial class PaxCalendar // Complements
{
    [Pure]
    public int CountWeeksInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountWeeksInYear(year);
    }
}
