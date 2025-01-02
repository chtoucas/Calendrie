// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Core.Schemas;
using Calendrie.Core.Utilities;

/// <summary>
/// Represents the Julian calendar.
/// <para>This calendar is <i>proleptic</i>. It supports <i>all</i> dates within
/// the range [-999_998..999_999] of years.</para>
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed partial class JulianCalendar : CalendarSystem<JulianDate>
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
    private const int MinYear_ = JulianScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 999_999.</para>
    /// </summary>
    private const int MaxYear_ = JulianScope.MaxYear;

    /// <summary>
    /// Initializes a new instance of the <see cref="JulianCalendar"/> class.
    /// </summary>
    public JulianCalendar() : this(new JulianSchema()) { }

    private JulianCalendar(JulianSchema schema) : base("Julian", new JulianScope(schema))
    {
        Debug.Assert(schema != null);
        Schema = schema;
    }

    /// <summary>
    /// Gets a singleton instance of the <see cref="JulianCalendar"/> class.
    /// <para>See <see cref="JulianDate.Calendar"/>.</para>
    /// </summary>
    internal static JulianCalendar Instance { get; } = new();

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
    internal JulianSchema Schema { get; }
}

public partial class JulianCalendar // Non-standard math ops
{
    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal static JulianDate AddYears(JulianDate date, int years)
    {
        JulianFormulae.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out int d);

        int newY = checked(y + years);
        if (newY < MinYear_ || newY > MaxYear_) ThrowHelpers.ThrowDateOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, JulianFormulae.CountDaysInMonth(newY, m));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(newY, m, newD);
        return new JulianDate(daysSinceEpoch);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    [Pure]
    internal static int CountYearsBetween(JulianDate start, JulianDate end)
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
    internal static JulianDate AddMonths(JulianDate date, int months)
    {
        JulianFormulae.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out int d);

        // NB: this AddMonths() is validating.
        return AddMonths(y, m, d, months);
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// </summary>
    [Pure]
    internal static int CountMonthsBetween(JulianDate start, JulianDate end)
    {
        JulianFormulae.GetDateParts(start.DaysSinceEpoch, out int y0, out int m0, out int d0);
        JulianFormulae.GetDateParts(end.DaysSinceEpoch, out int y1, out int m1, out _);

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
    private static JulianDate AddMonths(int y, int m, int d, int months)
    {
        // Exact months addition to a calendar month.
        int newM = 1 + MathZ.Modulo(checked(m - 1 + months), MonthsInYear, out int y0);
        int newY = checked(y + y0);
        if (newY < MinYear_ || newY > MaxYear_) ThrowHelpers.ThrowMonthOverflow();

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, JulianFormulae.CountDaysInMonth(newY, newM));

        int daysSinceEpoch = JulianFormulae.CountDaysSinceEpoch(newY, newM, newD);
        return new JulianDate(daysSinceEpoch);
    }
}
