// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Testing.CSharpTests;

using Calendrie.Hemerology;

[SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "<Pending>")]
public class CSharpOnlyTests
{
    [Fact]
    public static void DayNumber_FromDayNumber()
    {
        test<DayNumber>(DayNumber.MinValue);
        test<DayNumber>(DayNumber.Zero);
        test<DayNumber>(DayNumber.MaxValue);

        static void test<T>(DayNumber x) where T : IAbsoluteDate<DayNumber> =>
            Assert.Equal(x, T.FromDayNumber(x));
    }
}
