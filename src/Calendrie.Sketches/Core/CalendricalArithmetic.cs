﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Core;

using Calendrie.Core.Arithmetic;
using Calendrie.Core.Intervals;

// TODO(code): CalendricalArithmetic
// - Move ICalendricalArithmetic to sketches (before that we need to rework
//   the calendars)
// - I don't like the ctor (its use of Segment)
// - We could add more math ops
//   - CountYearsBetween(Yemoda, Yemoda)
//   - CountMonthsBetween(Yemoda, Yemoda)
//   - Ops with Yedoy
// - Date type in Calendrie.Systems:
//   - NextMonth(), PreviousMonth() & co?
//   - PlusYears() & other math ops should be part of an interface
//   - Move adjustments methods Calendar.GetStartOfYear & co to the date type
// - Testing:
//   - I'm pretty sure that PlainArithmetic.AddYears(Yemoda, years, roundoff) is
//     wrong
// - Optimisations here:
//   - MinYear or MinMonthsSinceEpoch = 0 -> uint for range checks

/// <summary>
/// Defines the core mathematical operations on dates and months, and provides
/// a base for derived classes.
/// <para>Operations are <i>lenient</i>, they assume that their parameters are
/// valid from a calendrical point of view. They MUST ensure that all returned
/// values are valid when the previous condition is met.</para>
/// </summary>
public abstract class CalendricalArithmetic
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CalendricalArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is
    /// NOT a subinterval of the range of supported years by <paramref name="schema"/>.
    /// </exception>
    protected CalendricalArithmetic(LimitSchema schema, Range<int> supportedYears)
    {
        ArgumentNullException.ThrowIfNull(schema);

        Schema = schema;

        (MinYear, MaxYear) = supportedYears.Endpoints;

        var seg = CalendricalSegment.Create(schema, supportedYears);
        (MinMonthsSinceEpoch, MaxMonthsSinceEpoch) = seg.SupportedMonths.Endpoints;
    }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    protected LimitSchema Schema { get; }

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    protected int MinYear { get; }

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    protected int MaxYear { get; }

    /// <summary>
    /// Gets the earliest supported month.
    /// </summary>
    protected int MinMonthsSinceEpoch { get; }

    /// <summary>
    /// Gets the latest supported month.
    /// </summary>
    protected int MaxMonthsSinceEpoch { get; }

    /// <summary>
    /// Creates the default arithmetic object for the specified schema and range
    /// of supported years.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="supportedYears"/>
    /// is NOT a subinterval of the range of supported years by <paramref name="schema"/>.
    /// </exception>
    [Pure]
    public static CalendricalArithmetic CreateDefault(LimitSchema schema, Range<int> supportedYears)
    {
        ArgumentNullException.ThrowIfNull(schema);

        return schema.IsRegular(out _)
            ? new RegularArithmetic(schema, supportedYears)
            : new PlainArithmetic(schema, supportedYears);
    }

    //
    // Operations on "Yemoda"
    //
    // Non-standard ops, those using the year or month units:
    // - AddYears(Yemoda, years)
    // - AddYears(Yemoda, years, out roundoff)
    // - AddMonths(Yemoda, months)
    // - AddMonths(Yemoda, months, out roundoff)
    // Of course, one can also implement the standard ops:
    // - AddDays(Yemoda, days)
    // - CountDaysBetween(Yemoda, Yemoda)
    // but we don't do it here because all our date types are based on the count
    // of days since the epoch (daysSinceEpoch) for which these ops are trivial.

    /// <summary>
    /// Adds a number of years to the year field of the specified date, yielding
    /// a new date.
    /// </summary>
    /// <returns>The end of the target month (resp. year) when the naive result
    /// is not a valid day (resp. month).</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure] public abstract Yemoda AddYears(int y, int m, int d, int years);

    /// <summary>
    /// Adds a number of years to the year field of the specified date, yielding
    /// a new date.
    /// </summary>
    /// <returns>The end of the target month (resp. year) when the naive result
    /// is not a valid day (resp. month).</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure] public abstract Yemoda AddYears(int y, int m, int d, int years, out int roundoff);

    /// <summary>
    /// Adds a number of months to the specified date, yielding a new date.
    /// </summary>
    /// <returns>The last day of the month when the naive result is not a valid
    /// day (roundoff > 0).</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public Yemoda AddMonths(int y, int m, int d, int months)
    {
        // NB: AddMonths(Yemo, months) is validating and exact.
        var (newY, newM) = AddMonths(y, m, months);

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, Schema.CountDaysInMonth(newY, newM));
        return new Yemoda(newY, newM, newD);
    }

    /// <summary>
    /// Adds a number of months to the specified date, yielding a new date.
    /// </summary>
    /// <returns>The last day of the month when the naive result is not a valid
    /// day (roundoff > 0).</returns>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    public Yemoda AddMonths(int y, int m, int d, int months, out int roundoff)
    {
        // NB: AddMonths(Yemo, months) is validating and exact.
        var (newY, newM) = AddMonths(y, m, months);

        int daysInMonth = Schema.CountDaysInMonth(newY, newM);
        roundoff = Math.Max(0, d - daysInMonth);
        return new Yemoda(newY, newM, roundoff == 0 ? d : daysInMonth);
    }

    //
    // Operations on "Yemo"
    //
    // The standard ops, those based on the month unit:
    // - AddMonths(Yemo, months)
    // - CountMonthsBetween(Yemo, Yemo)

    /// <summary>
    /// Adds a number of months to the specified month, yielding a new month.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported calendar months.</exception>
    [Pure] public abstract Yemo AddMonths(int y, int m, int months);

    /// <summary>
    /// Counts the number of months between the two specified months.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported calendar months.</exception>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "F# & VB.NET End statement.")]
    [Pure] public abstract int CountMonthsBetween(Yemo start, Yemo end);
}
