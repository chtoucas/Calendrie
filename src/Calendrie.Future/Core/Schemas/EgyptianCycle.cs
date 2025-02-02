// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

public static class EgyptianCycle
{
    /// <summary>
    /// Represents the number of years in a cycle.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    public const int YearLength = 1;

    /// <summary>
    /// Represents the number of days in a year.
    /// <para>This field is constant equal to 365.</para>
    /// </summary>
    public const int DaysPerYear = CalendricalConstants.DaysPerWanderingYear;
}
