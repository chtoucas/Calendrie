// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

public partial struct WorldMonth // Complements
{
    /// <summary>
    /// Obtains the genuine number of days in this month instance (excluding the
    /// blank days that are formally outside any month).
    /// </summary>
    [Pure]
    public int CountDaysInWorldMonth() => WorldSchema.CountDaysInWorldMonthImpl(Month);
}
