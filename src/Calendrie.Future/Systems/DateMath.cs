// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

[Obsolete("Better to inherit one the two classes directly")]
[ExcludeFromCodeCoverage]
public static class DateMath
{
    public static DateMath<TDate, TCalendar> Create<TDate, TCalendar>(AdditionRule rule)
        where TDate : struct, IDate<TDate>, ICalendarBound<TCalendar>, IUnsafeFactory<TDate>
        where TCalendar : Calendar
    {
        return TDate.Calendar.IsRegular(out int monthsInYear)
            ? new DateMathRegular<TDate, TCalendar>(rule, monthsInYear)
            : new DateMathPlain<TDate, TCalendar>(rule);
    }
}
