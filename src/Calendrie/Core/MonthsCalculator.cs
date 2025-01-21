// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Utilities;

internal static class MonthsCalculator
{
    internal static class Regular12
    {
        private const int MonthsPerYear = 12;

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountMonthsSinceEpoch(int y, int m) => MonthsPerYear * (y - 1) + m - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
        {
            y = 1 + MathZ.Divide(monthsSinceEpoch, MonthsPerYear, out int m0);
            m = 1 + m0;
        }

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetStartOfYear(int y) => MonthsPerYear * (y - 1);

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetEndOfYear(int y) => MonthsPerYear * y - 1;
    }

    internal static class Regular13
    {
        private const int MonthsPerYear = 13;

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountMonthsSinceEpoch(int y, int m) => MonthsPerYear * (y - 1) + m - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetMonthParts(int monthsSinceEpoch, out int y, out int m)
        {
            y = 1 + MathZ.Divide(monthsSinceEpoch, MonthsPerYear, out int m0);
            m = 1 + m0;
        }

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetStartOfYear(int y) => MonthsPerYear * (y - 1);

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetEndOfYear(int y) => MonthsPerYear * y - 1;
    }
}
