// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

/// <summary>
/// Defines the non-standard mathematical operations for the
/// <typeparamref name="TMonth"/> type when the <typeparamref name="TCalendar"/>
/// type is <i>regular</i>.
/// <para>This class allows to customize the <see cref="AdditionRule"/> strategy.
/// </para>
/// </summary>
public class MonthMathRegular<TMonth, TCalendar> : MonthMath<TMonth, TCalendar>
    where TMonth : struct, IMonth<TMonth>, ICalendarBound<TCalendar>, IUnsafeFactory<TMonth>
    where TCalendar : Calendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MonthMathRegular{TMonth, TCalendar}"/>
    /// class.
    /// </summary>
    internal MonthMathRegular(AdditionRule rule) : base(rule)
    {
        Debug.Assert(Schema != null);
        Debug.Assert(Schema.IsRegular(out _));
    }

    /// <inheritdoc />
    [Pure]
    public sealed override int CountYearsBetween(TMonth start, TMonth end, out TMonth newStart)
    {
        newStart = end;
        return end.Year - start.Year;
    }

    /// <inheritdoc />
    [Pure]
    protected override TMonth AddYears(int y, int m, int years, out int roundoff)
    {
        // Exact addition of years to a calendar year.
        int newY = checked(y + years);
        if (newY < MinYear || newY > MaxYear) ThrowHelpers.ThrowMonthOverflow();

        roundoff = 0;

        int monthsSinceEpoch = Schema.CountMonthsSinceEpoch(newY, m);
        return TMonth.UnsafeCreate(monthsSinceEpoch);
    }
}
