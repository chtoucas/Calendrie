// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

#if false
public static class MonthMath
{
    public static MonthMath<TMonth, TCalendar> Create<TMonth, TCalendar>(AdditionRule rule)
        where TMonth : struct, IMonth<TMonth>, ICalendarBound<TCalendar>, IUnsafeFactory<TMonth>
        where TCalendar : Calendar
    {
        return TMonth.Calendar.IsRegular(out _)
            ? new MonthMathRegular<TMonth, TCalendar>(rule)
            : new MonthMathPlain<TMonth, TCalendar>(rule);
    }
}
#endif
