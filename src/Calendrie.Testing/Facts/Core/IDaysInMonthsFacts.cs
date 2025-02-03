// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.Facts.Core;

using Calendrie.Core;

internal static class IDaysInMonthsFacts
{
    public static void Test<TSchema>(TSchema schema, int commonYear, int leapYear)
        where TSchema : IDaysInMonths, ICalendricalCore
    {
        if (leapYear != commonYear)
        {
            TestCore(schema, leapYear, leapYear: true);
            TestCore(schema, commonYear, leapYear: false);
        }
        else
        {
            TestCore(schema, commonYear, leapYear: false);
        }
    }

    public static void TestCore<TSchema>(TSchema schema, int y, bool leapYear)
        where TSchema : IDaysInMonths, ICalendricalCore
    {
        ArgumentNullException.ThrowIfNull(schema);

        // Sanity check.
        Assert.Equal(leapYear, schema.IsLeapYear(y));

        var daysInMonths = TSchema.GetDaysInMonthsOfYear(leapYear);

        for (int m = 1; m <= daysInMonths.Length; m++)
        {
            Assert.Equal(
                (m, schema.CountDaysInMonth(y, m)),
                (m, daysInMonths[m - 1]));
        }
    }
}
