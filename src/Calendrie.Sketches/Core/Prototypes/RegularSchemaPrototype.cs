// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core.Prototypes;

using Calendrie.Core.Intervals;

// WARNING: only meant to be used for rapid prototyping.
//
// Main differences with PrototypalSchema:
// - the schema is regular
// - GetYear(int daysSinceEpoch) and GetYear(daysSinceEpoch, out _).

public abstract partial class RegularSchemaPrototype : RegularSchema
{
    protected RegularSchemaPrototype(bool proleptic, int minDaysInYear, int minDaysInMonth)
        : base(
            proleptic ? ProlepticSupportedYears : StandardSupportedYears,
            minDaysInYear,
            minDaysInMonth)
    {
        IsProleptic = proleptic;
    }

    // Comme pour PrototypalSchema, on limite la plage des années supportées.
    // Voir les commentaires au niveau de PrototypalSchema.SupportedYears.
    internal static Range<int> StandardSupportedYears => Range.Create(1, 9999);
    internal static Range<int> ProlepticSupportedYears => Range.Create(-9998, 9999);

    public bool IsProleptic { get; }
}

public partial class RegularSchemaPrototype // Prototypal methods
{
    /// <inheritdoc />
    [Pure]
    public override int CountDaysInYearBeforeMonth(int y, int m)
    {
        int count = 0;
        for (int i = 1; i < m; i++)
        {
            count += CountDaysInMonth(y, i);
        }
        return count;
    }

    /// <inheritdoc />
    [Pure]
    public override int GetYear(int daysSinceEpoch)
    {
        // Find the year for which (daysSinceEpoch - startOfYear) = d0y
        // has the smallest value >= 0.
        if (daysSinceEpoch < 0)
        {
            int y = 0;
            int startOfYear = -CountDaysInYear(0);

            while (daysSinceEpoch < startOfYear)
            {
                startOfYear -= CountDaysInYear(--y);
            }

            return y;
        }
        else
        {
            int y = 1;
            int startOfYear = 0;

            while (daysSinceEpoch >= startOfYear)
            {
                int startOfNextYear = startOfYear + CountDaysInYear(y);
                if (daysSinceEpoch < startOfNextYear) { break; }
                y++;
                startOfYear = startOfNextYear;
            }
            Debug.Assert(daysSinceEpoch >= startOfYear);

            return y;
        }
    }

    /// <inheritdoc />
    [Pure]
    public override int GetMonth(int y, int doy, out int d)
    {
        int m = 1;
        int daysInYearBeforeMonth = 0;

        while (m < MonthsInYear)
        {
            int daysInYearBeforeNextMonth = CountDaysInYearBeforeMonth(y, m + 1);
            if (doy <= daysInYearBeforeNextMonth) { break; }

            daysInYearBeforeMonth = daysInYearBeforeNextMonth;
            m++;
        }

        // Notice that, as expected, d >= 1.
        d = doy - daysInYearBeforeMonth;
        return m;
    }

    /// <inheritdoc />
    [Pure]
    public override int GetStartOfYear(int y)
    {
        int daysSinceEpoch = 0;

        if (y < 1)
        {
            for (int i = y; i < 1; i++)
            {
                daysSinceEpoch -= CountDaysInYear(i);
            }
        }
        else
        {
            for (int i = 1; i < y; i++)
            {
                daysSinceEpoch += CountDaysInYear(i);
            }
        }

        return daysSinceEpoch;
    }
}
