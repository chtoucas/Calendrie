// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

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

[ExcludeFromCodeCoverage]
public partial struct RevisedWorldMonth
{
    /// <summary>
    /// Obtains the genuine number of days in this month instance (excluding the
    /// blank days that are formally outside any month).
    /// </summary>
    [Pure]
    public int CountDaysInWorldMonth() => WorldSchema.CountDaysInWorldMonthImpl(Month);
}
