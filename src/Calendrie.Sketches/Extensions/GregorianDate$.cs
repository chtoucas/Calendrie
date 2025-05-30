﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Extensions;

using Calendrie.Core;
using Calendrie.Core.Utilities;
using Calendrie.Systems;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Provides extension methods for <see cref="GregorianDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class GregorianDateExtensions
{
    [Pure]
    public static DayOfWeek GetDayOfWeek(this GregorianDate date)
    {
        var (y, m, d) = date;
        int doomsday = DoomsdayRule.GetGregorianDoomsday(y, m);
        return (DayOfWeek)MathZ.Modulo(doomsday + d, DaysPerWeek);
    }
}
