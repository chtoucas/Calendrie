// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System.Diagnostics.Contracts;

using Calendrie.Systems;

using static Calendrie.Core.CalendricalConstants;

/// <summary>
/// Provides extension methods for <see cref="CivilDate"/>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public static class CivilDateExtensions
{
    [Pure]
    public static CivilDate AddWeeks(this CivilDate date, int weeks) => date.AddDays(DaysInWeek * weeks);

    [Pure]
    public static CivilDate NextWeek(this CivilDate date) => date.AddDays(DaysInWeek);

    [Pure]
    public static CivilDate PreviousWeek(this CivilDate date) => date.AddDays(-DaysInWeek);
}
