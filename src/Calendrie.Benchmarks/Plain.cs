// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Benchmarks;

using Calendrie.Core.Schemas;

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class PlainCivilCalendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;
}

/// <remarks>This calendar supports <i>all</i> dates within the range [1..9999]
/// of years.</remarks>
public partial class PlainJulianCalendar
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;
}

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct PlainCivilDate { }

/// <remarks><i>All</i> dates within the range [1..9999] of years are supported.
/// </remarks>
public partial struct PlainJulianDate { }
