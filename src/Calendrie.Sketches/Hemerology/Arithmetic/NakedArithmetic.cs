// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology.Arithmetic;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Validation;

// Works even if the boundary years are not complete.

/// <summary>
/// Defines the standard mathematical operations on calendrical parts and
/// provides a base for derived classes.
/// <para>This class cannot be inherited.</para>
/// </summary>
public abstract partial class NakedArithmetic
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="NakedArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is
    /// null.</exception>
    protected NakedArithmetic(CalendricalSegment segment)
    {
        Segment = segment ?? throw new ArgumentNullException(nameof(segment));

        Schema = segment.Schema;
        PartsAdapter = new PartsAdapter(Schema);

        DaysValidator = new RangeValidator(segment.SupportedDays);
        MonthsValidator = new RangeValidator(segment.SupportedMonths);
        YearsValidator = new YearsValidator(segment.SupportedYears);
    }

    /// <summary>
    /// Gets the segment of supported days.
    /// </summary>
    public CalendricalSegment Segment { get; }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    protected ICalendricalSchema Schema { get; }

    /// <summary>
    /// Gets the adapter for calendrical parts.
    /// </summary>
    protected PartsAdapter PartsAdapter { get; }

    /// <summary>
    /// Gets the validator for the range of supported days.
    /// </summary>
    protected RangeValidator DaysValidator { get; }

    /// <summary>
    /// Gets the validator for the range of supported months.
    /// </summary>
    protected RangeValidator MonthsValidator { get; }

    /// <summary>
    /// Gets the validator for the range of supported years.
    /// </summary>
    protected IYearsValidator YearsValidator { get; init; }

    /// <summary>
    /// Creates the default arithmetic object for the specified schema and range
    /// of supported years.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="supportedYears"/>
    /// is NOT a subinterval of the range of supported years by <paramref name="schema"/>.
    /// </exception>
    public static NakedArithmetic CreateDefault(ICalendricalSchema schema, Range<int> supportedYears)
    {
        ArgumentNullException.ThrowIfNull(schema);

        var seg = CalendricalSegment.Create(schema, supportedYears);

        return schema.MinDaysInMonth >= RegularArithmetic.MinMinDaysInMonth && schema.IsRegular(out _)
            ? new RegularArithmetic(seg)
            : new PlainArithmetic(seg);
    }
}

public partial class NakedArithmetic // Operations on DateParts
{
    /// <summary>
    /// Adds a number of days to the specified date, yielding a new date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract DateParts AddDays(DateParts parts, int days);

    /// <summary>
    /// Obtains the day after the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract DateParts NextDay(DateParts parts);

    /// <summary>
    /// Obtains the day before the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract DateParts PreviousDay(DateParts parts);

    /// <summary>
    /// Counts the number of days between the two specified dates.
    /// </summary>
    [Pure]
    public int CountDaysBetween(DateParts start, DateParts end)
    {
        if (end.MonthParts == start.MonthParts) { return end.Day - start.Day; }

        var (y0, m0, d0) = start;
        var (y1, m1, d1) = end;

        return Schema.CountDaysSinceEpoch(y1, m1, d1) - Schema.CountDaysSinceEpoch(y0, m0, d0);
    }
}

public partial class NakedArithmetic // Operations on OrdinalParts
{
    /// <summary>
    /// Adds a number of days to the specified ordinal date, yielding a new
    /// ordinal date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure]
    public abstract OrdinalParts AddDays(OrdinalParts parts, int days);

    /// <summary>
    /// Obtains the day after the specified ordinal date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract OrdinalParts NextDay(OrdinalParts parts);

    /// <summary>
    /// Obtains the day before the specified ordinal date.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract OrdinalParts PreviousDay(OrdinalParts parts);

    /// <summary>
    /// Counts the number of days between the two specified ordinal dates.
    /// </summary>
    [Pure]
    public int CountDaysBetween(OrdinalParts start, OrdinalParts end)
    {
        if (end.Year == start.Year) { return end.DayOfYear - start.DayOfYear; }

        var (y0, doy0) = start;
        var (y1, doy1) = end;

        return Schema.CountDaysSinceEpoch(y1, doy1) - Schema.CountDaysSinceEpoch(y0, doy0);
    }
}

public partial class NakedArithmetic // Operations on MonthParts
{
    /// <summary>
    /// Adds a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract MonthParts AddMonths(MonthParts parts, int months);

    /// <summary>
    /// Obtains the month after the specified month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract MonthParts NextMonth(MonthParts parts);

    /// <summary>
    /// Obtains the month before the specified month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported values.</exception>
    [Pure] public abstract MonthParts PreviousMonth(MonthParts parts);

    /// <summary>
    /// Counts the number of months between the two specified months.
    /// </summary>
    [Pure]
    public int CountMonthsBetween(MonthParts start, MonthParts end)
    {
        if (end.Year == start.Year) { return end.Month - start.Month; }

        var (y0, m0) = start;
        var (y1, m1) = end;

        return Schema.CountMonthsSinceEpoch(y1, m1) - Schema.CountMonthsSinceEpoch(y0, m0);
    }
}
