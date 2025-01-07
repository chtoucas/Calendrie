// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

public sealed class CivilDateMath : DateMathRegular<CivilDate, CivilCalendar>
{
    public CivilDateMath(AdditionRule rule) : base(rule, CivilCalendar.MonthsInYear) { }
}

public sealed class CivilMonthMath : MonthMathRegular<CivilMonth, CivilCalendar>
{
    public CivilMonthMath(AdditionRule rule) : base(rule) { }
}
