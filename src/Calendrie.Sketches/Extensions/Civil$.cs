// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using System.Collections.Generic;

using Calendrie.Core;
using Calendrie.Specialized;

/// <summary>
/// Provides extension methods for <see cref="CivilDate"/> and
/// <see cref="CivilCalendar"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static partial class CivilExtensions { }

public partial class CivilExtensions // CivilDate
{
    [Pure]
    public static DayOfWeek GetDayOfWeek(this CivilDate date)
    {
        var (y, m, d) = date;
        int doomsday = DoomsdayRule.GetGregorianDoomsday(y, m);
        return (DayOfWeek)MathZ.Modulo(doomsday + d, CalendricalConstants.DaysInWeek);
    }

    [Pure]
    public static bool IsUnluckyFriday(this CivilDate date) =>
        // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
        date.DayOfWeek == DayOfWeek.Friday
        && date.Day == 13;
}

public partial class CivilExtensions // CivilCalendar
{
    [Pure]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static IEnumerable<CivilDate> FindUnluckyFridays(this CivilCalendar @this, int year)
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
