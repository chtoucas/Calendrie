// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie.Core.Utilities;

using static Calendrie.Core.CalendricalConstants;

public static class DayOfWeekAdjusters
{
    [Pure]
    public static T Previous<T>(T date, DayOfWeek dayOfWeek)
        where T : struct, IAbsoluteDate, IAdditionOperators<T, int, T>
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - date.DayOfWeek;
        return date + (δ >= 0 ? δ - DaysInWeek : δ);
    }

    [Pure]
    public static T PreviousOrSame<T>(T date, DayOfWeek dayOfWeek)
        where T : struct, IAbsoluteDate, IAdditionOperators<T, int, T>
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - date.DayOfWeek;
        return δ == 0 ? date : date + (δ > 0 ? δ - DaysInWeek : δ);
    }

    [Pure]
    public static T NextOrSame<T>(T date, DayOfWeek dayOfWeek)
        where T : struct, IAbsoluteDate, IAdditionOperators<T, int, T>
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - date.DayOfWeek;
        return δ == 0 ? date : date + (δ < 0 ? δ + DaysInWeek : δ);
    }

    [Pure]
    public static T Next<T>(T date, DayOfWeek dayOfWeek)
        where T : struct, IAbsoluteDate, IAdditionOperators<T, int, T>
    {
        Requires.Defined(dayOfWeek);

        int δ = dayOfWeek - date.DayOfWeek;
        return date + (δ <= 0 ? δ + DaysInWeek : δ);
    }
}
