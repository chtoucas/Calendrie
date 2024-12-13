// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using Calendrie.Core;
using Calendrie.Core.Utilities;
using Calendrie.Systems;

/// <summary>
/// Provides extension methods for <see cref="CivilDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class CivilDateExtensions
{
    [Pure]
    public static DayOfWeek GetDayOfWeek(this CivilDate date)
    {
        var (y, m, d) = date;
        int doomsday = DoomsdayRule.GetGregorianDoomsday(y, m);
        return (DayOfWeek)MathZ.Modulo(doomsday + d, CalendricalConstants.DaysInWeek);
    }
}
