// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;

using Calendrie;
using Calendrie.Core;

using Range_ = Calendrie.Core.Intervals.Range;

public sealed partial class GregorianPrototype : RegularSchemaPrototype
{
    public const int DaysInCommonYear = 365;
    public const int DaysInLeapYear = 366;
    public const int DaysPer400YearCycle = 400 * DaysInCommonYear + 97;

    private static ReadOnlySpan<byte> DaysInMonthOfCommonYear =>
        [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    private static ReadOnlySpan<byte> DaysInMonthOfLeapYear =>
        [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    private static ReadOnlySpan<int> DaysInYearBeforeMonthOfCommonYear =>
        [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334];
    private static ReadOnlySpan<int> DaysInYearBeforeMonthOfLeapYear =>
        [0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335];

    internal GregorianPrototype()
        : base(Range_.Create(-9998, 9999), minDaysInYear: 365, minDaysInMonth: 28) { }

    public sealed override int MonthsInYear => 12;

    public sealed override int CountDaysInYearBeforeMonth(int y, int m)
    {
        // This method throws an IndexOutOfRangeException if m < 1 or m > 12.
        return (IsLeapYear(y)
            ? DaysInYearBeforeMonthOfLeapYear
            : DaysInYearBeforeMonthOfCommonYear
        )[m - 1];
    }

    public sealed override int GetYear(int daysSinceEpoch)
    {
        int y = (int)Divide(400L * (daysSinceEpoch + 2), DaysPer400YearCycle);
        int c = Divide(y, 100);
        int startOfYearAfter = DaysInCommonYear * y + (y >> 2) - c + (c >> 2);

        return daysSinceEpoch < startOfYearAfter ? y : y + 1;
    }

    public sealed override int GetStartOfYear(int y)
    {
        y--;
        int c = Divide(y, 100);
        return DaysInCommonYear * y + (y >> 2) - c + (c >> 2);
    }

    //
    // Helpers
    //

    private static int Divide(int m, int n)
    {
        int q = m / n;
        int r = m - q * n;
        return m >= 0 || r == 0 ? q : q - 1;
    }

    private static long Divide(long m, long n)
    {
        long q = m / n;
        long r = m - q * n;
        return m >= 0L || r == 0L ? q : q - 1L;
    }
}

public partial class GregorianPrototype // ICalendricalCore
{
    public sealed override CalendricalFamily Family => CalendricalFamily.Solar;
    public sealed override CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Days;

    public sealed override bool IsRegular(out int monthsInYear)
    {
        monthsInYear = MonthsInYear;
        return true;
    }

    public sealed override int CountMonthsInYear(int y) => MonthsInYear;

    public sealed override int CountDaysInYear(int y) =>
        IsLeapYear(y) ? DaysInLeapYear : DaysInCommonYear;

    // CountDaysInMonth() is not safe to use. Three solutions:
    // 1. Let .NET throw an IndexOutOfRangeException.
    // 2. Throw an ArgumentOutOfRangeException.
    // 3. Use a purely computational formula.
    public sealed override int CountDaysInMonth(int y, int m) =>
        // This method throws an IndexOutOfRangeException if m < 1 or m > 12.
        (IsLeapYear(y) ? DaysInMonthOfLeapYear : DaysInMonthOfCommonYear)[m - 1];

    public sealed override bool IsLeapYear(int y) => (y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);

    public sealed override bool IsIntercalaryMonth(int y, int m) => false;

    public sealed override bool IsIntercalaryDay(int y, int m, int d) => m == 2 && d == 29;

    public sealed override bool IsSupplementaryDay(int y, int m, int d) => false;
}
