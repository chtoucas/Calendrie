// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Utilities;
using Calendrie.Hemerology;

internal sealed class MonthMathRegular<TMonth, TCalendar> : MonthMath<TMonth, TCalendar>
    where TMonth : struct, ICalendarMonth<TMonth>, ICalendarBound<TCalendar>, IUnsafeFactory<TMonth>
    where TCalendar : Calendar
{
    /// <summary>
    /// Represents the number of months in a year.
    /// </summary>
    private readonly int _monthsInYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonthMathRegular{TMonth, TCalendar}"/>
    /// class.
    /// </summary>
    public MonthMathRegular(AdditionRule additionRule, int monthsInYear) : base(additionRule)
    {
        Debug.Assert(Schema != null);
        Debug.Assert(Schema.IsRegular(out _));

        _monthsInYear = monthsInYear;
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

    /// <inheritdoc />
    [Pure]
    public sealed override int CountYearsBetween(TMonth start, TMonth end, out TMonth newStart)
    {
        newStart = end;
        return end.Year - start.Year;
    }
}
