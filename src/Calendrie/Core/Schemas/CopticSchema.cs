// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents a Coptic schema and provides a base for derived classes.
/// <para>This class can ONLY be inherited from within friend assemblies.</para>
/// </summary>
public abstract partial class CopticSchema : PtolemaicSchema
{
    /// <summary>
    /// Represents the number of days per 4-year cycle.
    /// <para>This field is a constant equal to 1461.</para>
    /// <para>On average, a year is 365.25 days long.</para>
    /// </summary>
    public const int DaysPer4YearCycle = CalendricalConstants.DaysPerJulianCycle;

    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CopticSchema"/> class.
    /// </summary>
    private protected CopticSchema(int minDaysInMonth) : base(minDaysInMonth)
    {
        SupportedYearsCore = Segment.EndingAt(int.MaxValue - 1);
    }
}

public partial class CopticSchema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    // Julian rule: y mod. 4 == 3.
    public sealed override bool IsLeapYear(int y) => (checked(y + 1) & 3) == 0;
}

public partial class CopticSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysSinceEpoch(int y, int m, int d) =>
        GetStartOfYear(y) + DaysPerMonth * (m - 1) + d - 1;

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch) =>
        MathZ.Divide((daysSinceEpoch << 2) + 1463, DaysPer4YearCycle);
}

public partial class CopticSchema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y) => DaysPerCommonYear * (y - 1) + (y >> 2);
}
