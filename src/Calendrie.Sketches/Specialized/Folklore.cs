// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Specialized;

using System.Collections.Generic;

public static class Folklore // Friday the 13th (Gregorian calendar)
{
    [Pure]
    public static IEnumerable<GregorianDate> FindUnluckyFridays(int year)
    {
        for (int m = 1; m <= 12; m++)
        {
            var date = new GregorianDate(year, m, 13);
            if (date.DayOfWeek == DayOfWeek.Friday)
            {
                yield return date;
            }
        }
    }

    //[Pure]
    //public static IEnumerable<GregorianDate> FindUnluckyFridays(int year)
    //{
    //    for (int m = 1; m <= 12; m++)
    //    {
    //        var date = new GregorianDate(year, m, 13);
    //        if (date.DayOfWeek == DayOfWeek.Friday)
    //        {
    //            yield return date;
    //        }
    //    }
    //}
}
