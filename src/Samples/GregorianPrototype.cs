// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Samples;

using System;
using System.Diagnostics.Contracts;

using Calendrie.Core;
using Calendrie.Hemerology;

public sealed class GregorianPrototype : PrototypalSchema
{
    private static ReadOnlySpan<int> DaysInYearBeforeMonthOfCommonYear =>
        [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334];

    private static ReadOnlySpan<int> DaysInYearBeforeMonthOfLeapYear =>
        [0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335];

    internal GregorianPrototype()
        : base(new GregorianKernel(), minDaysInYear: 365, minDaysInMonth: 28) { }

    //
    // Overriden methods
    //

    /// <inheritdoc />
    [Pure]
    public sealed override int CountDaysInYearBeforeMonth(int y, int m)
    {
        var kernel = this as ICalendar;

        // This method throws an IndexOutOfRangeException if m < 1 or m > 12.
        return (kernel.IsLeapYear(y)
            ? DaysInYearBeforeMonthOfLeapYear
            : DaysInYearBeforeMonthOfCommonYear
        )[m - 1];
    }
}
