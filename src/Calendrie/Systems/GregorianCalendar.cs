// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents the Gregorian calendar.
/// <para>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within
/// the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class GregorianCalendar : CalendarSystem<GregorianDate>
{
    /// <summary>
    /// Represents the total number of months in a year.
    /// <para>This field is constant equal to 12.</para>
    /// </summary>
    public const int MonthsInYear = GJSchema.MonthsInYear;

    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to -999_998.</para>
    /// </summary>
    private const int MinYear_ = ProlepticScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para>
    /// </summary>
    private const int MaxYear_ = ProlepticScope.MaxYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="GregorianCalendar"/> class.
    /// </summary>
    public GregorianCalendar() : this(new GregorianSchema()) { }

    private GregorianCalendar(GregorianSchema schema) : base("Gregorian", new GregorianScope(schema))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="GregorianCalendar"/> class.
    /// <para>See <see cref="GregorianDate.Calendar"/>.</para>
    /// </summary>
    internal static GregorianCalendar Instance { get; } = new();

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public static int MinYear => MinYear_;

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public static int MaxYear => MaxYear_;

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    internal GregorianSchema Schema { get; }
}

public partial class GregorianCalendar // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal static GregorianDate AddYears(GregorianDate date, int years)
    {
        GregorianFormulae.GetDateParts(date.DaysSinceZero, out int y, out int m, out int d);

        int newY = checked(y + years);
        if (newY < MinYear_ || newY > MaxYear_) ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, GregorianFormulae.CountDaysInMonth(newY, m));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(newY, m, newD);
        return new GregorianDate(daysSinceZero);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    [Pure]
    internal static int CountYearsBetween(GregorianDate start, GregorianDate end)
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
    internal static GregorianDate AddMonths(GregorianDate date, int months)
    {
        GregorianFormulae.GetDateParts(date.DaysSinceZero, out int y, out int m, out int d);

        // NB: this AddMonths() is validating.
        return AddMonths(y, m, d, months);
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// </summary>
    [Pure]
    internal static int CountMonthsBetween(GregorianDate start, GregorianDate end)
    {
        GregorianFormulae.GetDateParts(start.DaysSinceZero, out int y0, out int m0, out int d0);
        GregorianFormulae.GetDateParts(end.DaysSinceZero, out int y1, out int m1, out _);

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
    private static GregorianDate AddMonths(int y, int m, int d, int months)
    {
        // Exact months addition to a calendar month.
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < MinYear_ || newY > MaxYear_) ThrowHelpers.ThrowMonthOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, GregorianFormulae.CountDaysInMonth(newY, newM));

        int daysSinceZero = GregorianFormulae.CountDaysSinceEpoch(newY, newM, newD);
        return new GregorianDate(daysSinceZero);
    }
}
