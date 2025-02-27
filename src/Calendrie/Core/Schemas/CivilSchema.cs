﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Schemas;

using Calendrie.Core.Utilities;
using Calendrie.Core.Validation;

// year > 0 but SupportedYearsCore is still equal to Maximal32.
// Do NOT add Debug.Assert() to check that y > 0 or daysSinceEpoch >= 0. It is
// currently not compatible with our automated tests at the limit (I have yet to
// check why).

/// <summary>
/// Represents the Gregorian schema (year > 0).
/// <para>This class cannot be inherited.</para>
/// <para>This class can ONLY be initialized from within friend assemblies.</para>
/// </summary>
public sealed partial class CivilSchema : GJSchema, ISchemaActivator<CivilSchema>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CivilSchema"/> class.
    /// </summary>
    internal CivilSchema() : base(supportedYears: DefaultSupportedYears.WithMin(1))
    {
        PreValidator = GregorianPreValidator.Instance;
    }

    /// <inheritdoc />
    public sealed override int MonthsInYear => MonthsPerYear;

    /// <inheritdoc />
    [Pure]
    static CivilSchema ISchemaActivator<CivilSchema>.CreateInstance() => new();
}

public partial class CivilSchema // Year, month or day infos
{
    /// <inheritdoc />
    [Pure]
    public sealed override bool IsLeapYear(int y) =>
        (y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);
}

public partial class CivilSchema // Conversions
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

        int C = MathN.Divide(y, 100, out int Y);

        return -DaysPerYearAfterFebruary
            + (GregorianSchema.DaysPer400YearCycle * C >> 2)
            + (GregorianSchema.DaysPer4YearSubcycle * Y >> 2)
            + (int)((uint)(153 * m + 2) / 5) + d - 1;
    }

    /// <inheritdoc />
    public sealed override void GetDateParts(int daysSinceEpoch, out int y, out int m, out int d)
    {
        daysSinceEpoch += DaysPerYearAfterFebruary;

        int C = (int)((uint)((daysSinceEpoch << 2) + 3) / GregorianSchema.DaysPer400YearCycle);
        int D = daysSinceEpoch - (GregorianSchema.DaysPer400YearCycle * C >> 2);

        int Y = (int)((uint)((D << 2) + 3) / GregorianSchema.DaysPer4YearSubcycle);
        int d0y = D - (GregorianSchema.DaysPer4YearSubcycle * Y >> 2);

        m = (int)((uint)(5 * d0y + 2) / 153);
        d = 1 + d0y - (int)((uint)(153 * m + 2) / 5);

        if (m > 9)
        {
            Y++;
            m -= 9;
        }
        else
        {
            m += 3;
        }

        y = 100 * C + Y;
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int GetYear(int daysSinceEpoch)
    {
        int y = (int)(400L * (daysSinceEpoch + 2) / GregorianSchema.DaysPer400YearCycle);
        int c = y / 100;
        int startOfYearAfter = DaysPerCommonYear * y + (y >> 2) - c + (c >> 2);

        return daysSinceEpoch < startOfYearAfter ? y : y + 1;
    }
}

public partial class CivilSchema // Counting months and days since the epoch
{
    /// <inheritdoc />
    [Pure]
    public sealed override int GetStartOfYear(int y)
    {
        y--;
        int c = y / 100;
        return DaysPerCommonYear * y + (y >> 2) - c + (c >> 2);
    }
}
