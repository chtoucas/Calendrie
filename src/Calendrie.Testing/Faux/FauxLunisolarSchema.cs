// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Faux;

using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

using static Calendrie.Core.CalendricalConstants;

// NB: waiting for HebrewSchema.

/// <summary>
/// Represents a <b>fake</b> lunisolar schema.
/// </summary>
public sealed class FauxLunisolarSchema : CalendricalSchema, IDaysInMonths
{
    public const int MonthsPer4YearCycle = 49;
    public const int DaysPer4YearCycle = 1446;

    public const int MonthsInCommonYear = 12;
    public const int MonthsInLeapYear = 13;

    public const int DaysInCommonYear = 354;
    public const int DaysInLeapYear = 384;

    public FauxLunisolarSchema() : base(Lunisolar.MinDaysInYear, Lunisolar.MinDaysInMonth) { }

    private static ReadOnlySpan<byte> DaysInMonthsOfCommonYear =>
        [30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29];

    private static ReadOnlySpan<byte> DaysInMonthsOfLeapYear =>
        [30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30];

    [Pure]
    static ReadOnlySpan<byte> IDaysInMonths.GetDaysInMonthsOfYear(bool leapYear) =>
        leapYear ? DaysInMonthsOfLeapYear : DaysInMonthsOfCommonYear;

    public sealed override CalendricalFamily Family => CalendricalFamily.Lunisolar;
    public sealed override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Months;

    /// <inheritdoc />
    [Pure]
    public sealed override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = 0;
        return false;
    }

    [Pure] public sealed override bool IsLeapYear(int y) => (y & 3) == 0;
    [Pure] public sealed override bool IsIntercalaryMonth(int y, int m) => m == 13;
    [Pure] public sealed override bool IsIntercalaryDay(int y, int m, int d) => false;
    [Pure] public sealed override bool IsSupplementaryDay(int y, int m, int d) => false;

    [Pure] public sealed override int CountMonthsInYear(int y) => IsLeapYear(y) ? MonthsInLeapYear : MonthsInCommonYear;
    [Pure] public sealed override int CountDaysInYear(int y) => IsLeapYear(y) ? DaysInLeapYear : DaysInCommonYear;
    [Pure] public sealed override int CountDaysInYearBeforeMonth(int y, int m) => 29 * (m - 1) + (m >> 1);
    [Pure] public sealed override int CountDaysInMonth(int y, int m) => 29 + (m & 1);

    [Pure]
    public sealed override int GetStartOfYearInMonths(int y)
    {
        y--;
        return MonthsInCommonYear * y + (y >> 2);
    }

    public sealed override void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
    {
        y = MathZ.Divide((monthsSinceEpoch << 2) + 52, 49);
        m = 1 + monthsSinceEpoch - (49 * y - 49 >> 2);
    }

    [Pure]
    public sealed override int GetMonth(int y, int doy, out int d)
    {
        int d0y = doy - 1;
        int m = ((d0y << 1) + 59) / 59;
        d = 1 + d0y - 29 * (m - 1) - (m >> 1);
        return m;
    }

    [Pure]
    public sealed override int GetYear(int daysSinceEpoch)
    {
        int C = MathZ.Divide(daysSinceEpoch, DaysPer4YearCycle, out int D);
        return (C << 2) + (D >= 1416 ? 4 : 1 + D / 354);
    }

    [Pure]
    public sealed override int GetStartOfYear(int y) => DaysInCommonYear * --y + 30 * (y >> 2);
}
