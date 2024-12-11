// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Samples;

using System.Collections.Generic;

using Calendrie.Specialized;

[ExcludeFromCodeCoverage]
public static partial class Folklore { }

public partial class Folklore // Friday the 13th (Gregorian calendar)
{
    [Pure]
    public static bool IsUnluckyFriday(this CivilDate date) =>
        // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
        date.DayOfWeek == DayOfWeek.Friday
        && date.Day == 13;

    [Pure]
    public static bool IsUnluckyFriday(this GregorianDate date) =>
        // On vérifie d'abord le jour de la semaine (propriété la plus rapide à obtenir).
        date.DayOfWeek == DayOfWeek.Friday
        && date.Day == 13;

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

    [Pure]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static IEnumerable<GregorianDate> FindUnluckyFridays(this GregorianCalendar @this, int year)
    {
        // This method should be faster. By using the internals, we can validate
        // only once.

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
