// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems.Arithmetic;

using Calendrie.Core;

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
internal abstract class CalendricalArithmetic : ICalendricalArithmetic
{
    /// <summary>
    /// Represents the earliest supported year.
    /// <para>This field is a constant equal to 1.</para>
    /// </summary>
    protected const int MinYear = StandardScope.MinYear;

    /// <summary>
    /// Represents the latest supported year.
    /// <para>This field is a constant equal to 9999.</para>
    /// </summary>
    protected const int MaxYear = StandardScope.MaxYear;

    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CalendricalArithmetic"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    protected CalendricalArithmetic(LimitSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);

        Schema = schema;
    }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    protected LimitSchema Schema { get; }

    /// <summary>
    /// Creates the default arithmetic object for the specified schema and range
    /// of supported years.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    [Pure]
    public static ICalendricalArithmetic CreateDefault(LimitSchema schema)
    {
        ArgumentNullException.ThrowIfNull(schema);

        return schema.IsRegular(out _) ? new RegularArithmetic(schema) : new PlainArithmetic(schema);
    }

    //
    // Operations on "Yemoda"
    //

    /// <inheritdoc />
    [Pure] public abstract Yemoda AddYears(int y, int m, int d, int years);

    /// <inheritdoc />
    [Pure] public abstract Yemoda AddYears(int y, int m, int d, int years, out int roundoff);

    /// <inheritdoc />
    [Pure]
    public Yemoda AddMonths(int y, int m, int d, int months)
    {
        // NB: AddMonths(Yemo, months) is validating and exact.
        var (newY, newM) = AddMonths(y, m, months);

        // NB: AdditionRule.Truncate.
        int newD = Math.Min(d, Schema.CountDaysInMonth(newY, newM));
        return new Yemoda(newY, newM, newD);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    [Pure] public abstract Yemo AddMonths(int y, int m, int months);

    /// <inheritdoc />
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "F# & VB.NET End statement.")]
    [Pure] public abstract int CountMonthsBetween(Yemo start, Yemo end);
}
