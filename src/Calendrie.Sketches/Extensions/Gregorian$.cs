// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using System.Collections.Generic;

using Calendrie.Core;
using Calendrie.Specialized;

public static partial class GregorianExtensions { }

public partial class GregorianExtensions // GregorianDate
{
    [Pure]
    public static DayOfWeek GetDayOfWeek(this GregorianDate date)
    {
        // Should be faster than the base method which relies on CountDaysSinceEpoch().
        var (y, m, d) = date;

        int doomsday = DoomsdayRule.GetGregorianDoomsday(y, m);
        return (DayOfWeek)MathZ.Modulo(doomsday + d, CalendricalConstants.DaysInWeek);
    }

    [Pure]
    public static bool IsUnluckyFriday(this GregorianDate date) =>
        // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
        date.DayOfWeek == DayOfWeek.Friday
        && date.Day == 13;
}

public partial class GregorianExtensions // GregorianCalendar
{
    [Pure]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static IEnumerable<GregorianDate> FindUnluckyFridays(this GregorianCalendar @this, int year)
    {
        // This method should be faster than the one found in CivilExtensions.
        // Indeed, by using the internals, validation occurs only once.

        ProlepticScope.YearsValidatorImpl.Validate(year);

        return iterator();

        IEnumerable<GregorianDate> iterator()
        {
            var sch = GregorianCalendar.SchemaT;

            for (int m = 1; m <= 12; m++)
            {
                int daysSinceZero = sch.CountDaysSinceEpoch(year, m, 13);
                if (new DayNumber(daysSinceZero).DayOfWeek == DayOfWeek.Friday)
                {
                    yield return new GregorianDate(daysSinceZero);
                }
            }
        }
    }
}
