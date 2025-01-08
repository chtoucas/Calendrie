// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

using static Calendrie.Core.CalendricalConstants;

public static class Civil
{
    /// <summary>
    /// Attempts to create a new instance of the <see cref="CivilDate"/>
    /// struct from the specified date components.
    /// </summary>
    [Pure]
    public static CivilDate? TryCreate(int year, int month, int day)
    {
        bool ok = year >= CivilScope.MinYear && year <= CivilScope.MaxYear
            && month >= 1 && month <= CivilCalendar.MonthsInYear
            && day >= 1
            && (day <= Solar.MinDaysInMonth
                || day <= GregorianFormulae.CountDaysInMonth(year, month));

        if (ok)
        {
            int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(year, month, day);
            return new CivilDate(daysSinceZero);
        }

        return null;
    }
}
