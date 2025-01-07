// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

/// <summary>
/// Provides non-standard mathematical operations for the <see cref="CivilDate"/>
/// type.
/// <para>This class allows to customize the <see cref="AdditionRule"/> strategy.
/// </para>
/// </summary>
public sealed class CivilDateMath : DateMathRegular<CivilDate, CivilCalendar>
{
    public CivilDateMath(AdditionRule rule) : base(rule, CivilCalendar.MonthsInYear) { }
}
