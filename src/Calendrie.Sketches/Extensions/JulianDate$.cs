// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using Calendrie.Core;
using Calendrie.Core.Utilities;
using Calendrie.Systems;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Provides extension methods for <see cref="JulianDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class JulianDateExtensions
{
    [Pure]
    public static DayOfWeek GetDayOfWeek(this JulianDate date)
    {
        var (y, m, d) = date;
        int doomsday = DoomsdayRule.GetJulianDoomsday(y, m);
        return (DayOfWeek)MathZ.Modulo(doomsday + d, DaysInWeek);
    }
}
