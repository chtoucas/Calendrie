// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;

using Calendrie;
using Calendrie.Core;
using Calendrie.Core.Prototyping;

public sealed class GregorianKernel : ICalendricalCore
{
    public const int MonthsInYear = 12;
    public const int DaysInCommonYear = 365;
    public const int DaysInLeapYear = 366;

    private static ReadOnlySpan<byte> DaysInMonthsOfCommonYear =>
        [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    private static ReadOnlySpan<byte> DaysInMonthsOfLeapYear =>
        [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    public static ICalendricalSchema CreatePrototypalSchema() =>
        new PrototypalSchema(
            new GregorianKernel(), proleptic: true, minDaysInYear: 365, minDaysInMonth: 28);

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

    // CountDaysInMonth() is not safe to use. Three solutions:
    // 1. Let .NET throw an IndexOutOfRangeException.
    // 2. Throw an ArgumentOutOfRangeException.
    // 3. Use a purely computational formula.
    public int CountDaysInMonth(int y, int m) =>
        // This method throws an IndexOutOfRangeException if m < 1 or m > 12.
        (IsLeapYear(y) ? DaysInMonthsOfLeapYear : DaysInMonthsOfCommonYear)[m - 1];
}
