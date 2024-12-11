// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized.Extensions;

public static class CivilDateExtensions
{
    [Pure]
    public static bool IsUnluckyFriday(this CivilDate date) =>
        // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
        date.DayOfWeek == DayOfWeek.Friday
        && date.Day == 13;
}
