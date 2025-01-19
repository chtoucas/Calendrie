// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

// TODO(code): create a PaxWeek type.

public partial struct PaxMonth // Complements
{
    public bool IsPaxMonthOfYear
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
            return sch.IsPaxMonth(y, m);
        }
    }

    public bool IsLastMonthOfYear
    {
        get
        {
            var sch = Calendar.Schema;
            sch.GetMonthParts(_monthsSinceEpoch, out int y, out int m);
            return sch.IsLastMonthOfYear(y, m);
        }
    }
}

public partial struct PaxYear // Complements
{
    /// <summary>
    /// Obtains the number of weeks in the current instance.
    /// </summary>
    [Pure]
    public int CountWeeks() => Calendar.Schema.CountWeeksInYear(Year);
}
