// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

public partial struct PaxMonth // Complements
{
    public bool IsPaxMonth
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
            return sch.IsLastMonthOfYear(Year, Month);
        }
    }
}
