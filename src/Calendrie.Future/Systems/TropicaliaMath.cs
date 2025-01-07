// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

public sealed class TropicaliaDateMath : DateMathRegular<TropicaliaDate, TropicaliaCalendar>
{
    public TropicaliaDateMath(AdditionRule rule) : base(rule, TropicaliaCalendar.MonthsInYear) { }
}

public sealed class TropicaliaMonthMath : MonthMathRegular<TropicaliaMonth, TropicaliaCalendar>
{
    public TropicaliaMonthMath(AdditionRule rule) : base(rule) { }
}
