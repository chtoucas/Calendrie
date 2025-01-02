// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents the Civil calendar.
/// <para>This calendar is <i>retropolated</i>. It supports <i>all</i> dates
/// within the range [1..9999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class CivilCalendar : CalendarSystem<CivilDate>
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;

    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    private const int MinYear_ = CivilScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 9999.</para>
    /// </summary>
    private const int MaxYear_ = CivilScope.MaxYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="CivilCalendar"/> class.
    /// </summary>
    public CivilCalendar() : this(new CivilSchema()) { }

    private CivilCalendar(CivilSchema schema) : base("Civil", new CivilScope(schema))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="CivilCalendar"/> class.
    /// <para>See <see cref="CivilDate.Calendar"/>.</para>
    /// </summary>
    internal static CivilCalendar Instance { get; } = new();

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => MinYear_;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => MaxYear_;

    /// <summary>
    /// Gets the schema.
    /// </summary>
    internal CivilSchema Schema { get; }
}

public partial class CivilCalendar // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal static CivilDate AddYears(CivilDate date, int years)
    {
        CivilFormulae.GetDateParts(date.DaysSinceZero, out int y, out int m, out int d);

        int newY = checked(y + years);
        if (newY < MinYear_ || newY > MaxYear_) ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, GregorianFormulae.CountDaysInMonth(newY, m));

        int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(newY, m, newD);
        return new CivilDate(daysSinceZero);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    [Pure]
    internal static int CountYearsBetween(CivilDate start, CivilDate end)
    {
        // Exact difference between two calendar years.
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
    internal static CivilDate AddMonths(CivilDate date, int months)
    {
        CivilFormulae.GetDateParts(date.DaysSinceZero, out int y, out int m, out int d);

        // NB: this AddMonths() is validating.
        return AddMonths(y, m, d, months);
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// </summary>
    [Pure]
    internal static int CountMonthsBetween(CivilDate start, CivilDate end)
    {
        CivilFormulae.GetDateParts(start.DaysSinceZero, out int y0, out int m0, out int d0);
        CivilFormulae.GetDateParts(end.DaysSinceZero, out int y1, out int m1, out _);

        // Exact difference between two calendar months.
        int months = checked((y1 - y0) * MonthsInYear + m1 - m0);

        // To avoid extracting (y0, m0, d0) twice, we inline:
        // > var newStart = AddMonths(start, months);
        var newStart = AddMonths(y0, m0, d0, months);

        if (start < end)
        {
            if (newStart > end) months--;
        }
        else
        {
            if (newStart < end) months++;
        }

        return months;
    }

    [Pure]
    private static CivilDate AddMonths(int y, int m, int d, int months)
    {
        // Exact months addition to a calendar month.
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < MinYear_ || newY > MaxYear_) ThrowHelpers.ThrowMonthOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, GregorianFormulae.CountDaysInMonth(newY, newM));

        int daysSinceZero = CivilFormulae.CountDaysSinceEpoch(newY, newM, newD);
        return new CivilDate(daysSinceZero);
    }
}
