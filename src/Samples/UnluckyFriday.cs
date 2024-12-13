// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Calendrie.Systems;

public static class CivilDateExtras
{
    [Pure]
    public static bool IsUnluckyFriday(this CivilDate date) =>
        // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
        date.DayOfWeek == DayOfWeek.Friday
        && date.Day == 13;

    [Pure]
    public static IEnumerable<CivilDate> FindUnluckyFridays(int year)
    {
        for (int m = 1; m <= 12; m++)
        {
            var date = new CivilDate(year, m, 13);
            if (date.DayOfWeek == DayOfWeek.Friday)
            {
                yield return date;
            }
        }
    }
}
