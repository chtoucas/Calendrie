// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

public partial class PaxCalendar // Complements
{
    [Pure]
    public int CountWeeksInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountWeeksInYear(year);
    }
}

public partial struct PaxDate // Complements
{
}
