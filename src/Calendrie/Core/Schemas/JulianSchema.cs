// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;
using Calendrie.Core.Validation;

/// <summary>
/// Represents the Julian schema.
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
public sealed partial class JulianSchema : GJSchema, ISchemaActivator<JulianSchema>
{
    /// <summary>
    /// Represents the number of days per 4-year cycle.
    /// <para>This field is a constant equal to 1461.</para>
    /// <para>On average, a year is 365.25 days long.</para>
    /// </summary>
    public const int DaysPer4YearCycle = CalendricalConstants.DaysPerJulianCycle;

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianSchema"/> class.
    /// </summary>
    internal JulianSchema() : base(DefaultSupportedYears)
    {
        PreValidator = JulianPreValidator.Instance;
    }

    /// <inheritdoc />
    public sealed override int MonthsInYear => MonthsPerYear;

    /// <inheritdoc />
    [Pure]
    static JulianSchema ISchemaActivator<JulianSchema>.CreateInstance() => new();
}

public partial class JulianSchema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y) => (y & 3) == 0;
}

public partial class JulianSchema // Conversions
{
    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysSinceEpoch(int y, int m, int d)
    {
        if (m < 3)
        {
            y--;
            m += 9;
        }
        else
        {
            m -= 3;
        }

        return -DaysPerYearAfterFebruary
            + (DaysPer4YearCycle * y >> 2) + (int)((uint)(153 * m + 2) / 5) + d - 1;
    }

    /// <inheritdoc />
    public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        daysSinceEpoch += DaysPerYearAfterFebruary;

        y = MathZ.Divide((daysSinceEpoch << 2) + 3, DaysPer4YearCycle);
        int d0y = daysSinceEpoch - (DaysPer4YearCycle * y >> 2);

        m = (int)((uint)(5 * d0y + 2) / 153);
        d = 1 + d0y - (int)((uint)(153 * m + 2) / 5);

        if (m > 9)
        {
            y++;
            m -= 9;
        }
        else
        {
            m += 3;
        }
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch) =>
        MathZ.Divide((daysSinceEpoch << 2) + 1464, DaysPer4YearCycle);
}

public partial class JulianSchema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y)
    {
        y--;
        return DaysPerCommonYear * y + (y >> 2);
    }
}
