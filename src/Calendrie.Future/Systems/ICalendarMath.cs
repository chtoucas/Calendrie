// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

internal interface ICalendarMath<TDate>
{
    [Pure] TDate AddYears(int y, int m, int d, int years);
    [Pure] TDate AddYears(int y, int m, int d, int years, out int roundoff);

    [Pure] TDate AddMonths(int y, int m, int d, int months);
    [Pure] TDate AddMonths(int y, int m, int d, int months, out int roundoff);
}
