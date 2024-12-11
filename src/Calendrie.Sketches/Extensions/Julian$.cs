// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using Calendrie.Core;
using Calendrie.Specialized;

public static partial class JulianExtensions { }

public partial class JulianExtensions // JulianDate
{
    [Pure]
    public static DayOfWeek GetDayOfWeek(this JulianDate date)
    {
        // Should be faster than the base method which relies on CountDaysSinceEpoch().
        var (y, m, d) = date;

        int doomsday = DoomsdayRule.GetJulianDoomsday(y, m);
        return (DayOfWeek)MathZ.Modulo(doomsday + d, CalendricalConstants.DaysInWeek);
    }
}
