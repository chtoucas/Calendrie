// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core;
using Calendrie.Core.Utilities;
using Calendrie.Hemerology;
using Calendrie.Systems.Arithmetic;

public partial class RegularSystem<TDate> : CalendarSystem<TDate>
    where TDate : struct, IDateable, IAbsoluteDate<TDate>, IDateFactory<TDate>
{
    /// <summary>
    /// Represents the number of months in a year.
    /// </summary>
    private readonly int _monthsInYear;

    /// <summary>
    /// Represents the calendrical arithmetic.
    /// </summary>
    private readonly RegularArithmetic _arithmetic;

    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to -999_998.</para>
    /// </summary>
    private const int MinYear_ = StandardScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para>
    /// </summary>
    private const int MaxYear_ = StandardScope.MaxYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarSystem{TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    internal RegularSystem(string name, StandardScope scope) : base(name, scope)
    {
        Debug.Assert(scope != null);

        var schema = scope.Schema;

        if (!schema.IsRegular(out _monthsInYear))
            throw new ArgumentException(null, nameof(scope));

        _arithmetic = schema is LimitSchema sch ? new RegularArithmetic(sch)
            : throw new ArgumentException(null, nameof(scope));
    }
}

public partial class RegularSystem<TDate> // Non-standard math ops
{
    // TODO(code): overflows? document them if any?

    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal TDate AddYears(TDate date, int years)
    {
        var sch = Scope.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out int d);

        int newY = checked(y + years);
        if (newY < MinYear_ || newY > MaxYear_) ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, sch.CountDaysInMonth(newY, m));

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, m, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    [Pure]
    internal int CountYearsBetween(TDate start, TDate end)
    {
        // Exact difference between two years.
        int years = end.Year - start.Year;

        var newStart = AddYears(start, years);
        if (start < end)
        {
            if (newStart > end) years--;
        }
        else
        {
            if (newStart < end) years++;
        }

        return years;
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal TDate AddMonths(TDate date, int months)
    {
        var sch = Scope.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out int d);

        // NB: _arithmetic.AddMonths() is validating.
        var (newY, newM, newD) = _arithmetic.AddMonths(y, m, d, months);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// </summary>
    [Pure]
    internal int CountMonthsBetween(TDate start, TDate end)
    {
        var sch = Scope.Schema;
        sch.GetDateParts(start.DaysSinceEpoch, out int y0, out int m0, out int d0);
        sch.GetDateParts(end.DaysSinceEpoch, out int y1, out int m1, out _);

        // Exact difference between two months.
        int months = _arithmetic.CountMonthsBetween(new Yemo(y0, m0), new Yemo(y1, m1));

        // To avoid extracting (y0, m0, d0) twice, we inline:
        // > var newStart = AddMonths(start, months);
        var newStart = startPlusMonths(months);

        if (start < end)
        {
            if (newStart > end) months--;
        }
        else
        {
            if (newStart < end) months++;
        }

        return months;

        [Pure]
        TDate startPlusMonths(int months)
        {
            var (newY, newM, newD) = _arithmetic.AddMonths(y0, m0, d0, months);
            int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
            return TDate.UnsafeCreate(daysSinceEpoch);
        }
    }

    [Pure]
    private Yemo AddMonths(int y, int m, int months)
    {
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), _monthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < MinYear_ || newY > MaxYear_) ThrowHelpers.ThrowMonthOverflow();

        return new Yemo(newY, newM);
    }
}
