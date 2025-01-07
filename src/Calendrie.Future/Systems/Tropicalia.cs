// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

/// <summary>
/// Defines the non-standard mathematical operations suitable for use with the
/// <see cref="TropicaliaDate"/> type.
/// <para>This class allows to customize the <see cref="AdditionRule"/> used
/// to resolve ambiguities.</para>
/// </summary>
public sealed class TropicaliaDateMath : DateMathRegular<TropicaliaDate, TropicaliaCalendar>
{
    public TropicaliaDateMath(AdditionRule rule) : base(rule, TropicaliaCalendar.MonthsInYear) { }
}
