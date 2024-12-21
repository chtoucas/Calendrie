// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using Calendrie;
using Calendrie.Core;

public sealed class MyGregorianSchema : ICalendricalCore
{
    public const int MonthsInYear = 12;
    public const int DaysInCommonYear = CalendricalConstants.DaysInWanderingYear;
    public const int DaysInLeapYear = DaysInCommonYear + 1;

    public CalendricalAlgorithm Algorithm => CalendricalAlgorithm.Arithmetical;
    public CalendricalFamily Family => CalendricalFamily.Solar;
    public CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Days;

    public bool IsRegular(out int monthsInYear)
    {
        monthsInYear = MonthsInYear;
        return true;
    }

    public bool IsLeapYear(int y) => (y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);

    public bool IsIntercalaryMonth(int y, int m) => false;

    public bool IsIntercalaryDay(int y, int m, int d) => m == 2 && d == 29;

    public bool IsSupplementaryDay(int y, int m, int d) => false;

    public int CountMonthsInYear(int y) => MonthsInYear;

    public int CountDaysInYear(int y) => IsLeapYear(y) ? DaysInLeapYear : DaysInCommonYear;

    public int CountDaysInMonth(int y, int m) =>
        m != 2 ? 30 + ((m + (m >> 3)) & 1)
        : IsLeapYear(y) ? 29
        : 28;
}
