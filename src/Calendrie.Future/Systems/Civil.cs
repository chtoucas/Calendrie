// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;

public static class Civil
{
    /// <summary>
    /// Attempts to create a new instance of the <see cref="CivilDate"/>
    /// struct from the specified date components.
    /// </summary>
    [Pure]
    public static CivilDate? TryCreate(int year, int month, int day)
    {
        // TO BE REMOVED
        var chr = CivilCalendar.Instance;

        bool ok = year >= CivilScope.MinYear && year <= CivilScope.MaxYear
            && chr.Scope.PreValidator.CheckMonthDay(year, month, day);

        if (ok)
        {
            int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(year, month, day);
            return new CivilDate(daysSinceZero);
        }

        return null;
    }
    /// <summary>
    /// Attempts to create a new instance of the <see cref="CivilDate"/>
    /// struct from the specified ordinal components.
    /// </summary>
    [Pure]
    public static CivilDate? TryCreate(int year, int dayOfYear)
    {
        // TO BE REMOVED
        var chr = CivilCalendar.Instance;

        bool ok = year >= CivilScope.MinYear && year <= CivilScope.MaxYear
            && chr.Scope.PreValidator.CheckDayOfYear(year, dayOfYear);

        if (ok)
        {
            int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(year, dayOfYear);
            return new CivilDate(daysSinceZero);
        }

        return null;
    }
}
