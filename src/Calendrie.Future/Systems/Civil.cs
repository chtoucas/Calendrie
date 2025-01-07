// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

/// <summary>
/// Defines the non-standard mathematical operations suitable for use with the
/// <see cref="CivilDate"/> type.
/// <para>This class allows to customize the <see cref="AdditionRule"/> used
/// to resolve ambiguities.</para>
/// </summary>
public sealed class CivilDateMath : DateMathRegular<CivilDate, CivilCalendar>
{
    public CivilDateMath(AdditionRule rule) : base(rule, CivilCalendar.MonthsInYear) { }
}
