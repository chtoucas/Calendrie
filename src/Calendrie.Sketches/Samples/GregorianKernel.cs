﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Samples;

using Calendrie.Core;
using Calendrie.Core.Intervals;

public sealed class GregorianKernel : ICalendricalKernel
{
    public const int MonthsInYear = 12;
    public const int DaysInCommonYear = 365;
    public const int DaysInLeapYear = 366;

    // Index 0 is fake in order for the span index to match the month index.
    internal static ReadOnlySpan<byte> DaysInMonthOfCommonYear =>
        [0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    internal static ReadOnlySpan<byte> DaysInMonthOfLeapYear =>
        [0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // CountDaysInMonth() is not safe to use. Three solutions:
    // 1. Let .NET throw an IndexOutOfRangeException.
    // 2. Throw an ArgumentOutOfRangeException.
    // 3. Use a purely computational formula.

    internal GregorianKernel() { }

    public CalendricalAlgorithm Algorithm => CalendricalAlgorithm.Arithmetical;
    public CalendricalFamily Family => CalendricalFamily.Solar;
    public CalendricalAdjustments PeriodicAdjustments => CalendricalAdjustments.Days;

    public static Range<int> SupportedYears => Range.Maximal32;

    [Pure]
    public bool IsRegular(out int monthsInYear)
    {
        monthsInYear = MonthsInYear;
        return true;
    }

    [Pure]
    public int CountMonthsInYear(int y) => MonthsInYear;

    [Pure]
    public int CountDaysInYear(int y) => IsLeapYear(y) ? DaysInLeapYear : DaysInCommonYear;

    [Pure]
    public int CountDaysInMonth(int y, int m)
    {
        // This method throws an IndexOutOfRangeException if m < 0 or m > 12.
        // Index 0 is fake.
        Debug.Assert(m != 0);
        return (IsLeapYear(y) ? DaysInMonthOfLeapYear : DaysInMonthOfCommonYear)[m];
    }

    [Pure]
    public bool IsLeapYear(int y) => (y & 3) == 0 && (y % 100 != 0 || y % 400 == 0);

    [Pure]
    public bool IsIntercalaryMonth(int y, int m) => false;

    [Pure]
    public bool IsIntercalaryDay(int y, int m, int d) => m == 2 && d == 29;

    [Pure]
    public bool IsSupplementaryDay(int y, int m, int d) => false;
}