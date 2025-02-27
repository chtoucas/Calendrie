﻿// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

// FIXME(code): ENABLE_GENERIC_FACTORIES.

namespace Calendrie.Core;

using Calendrie.Core.Intervals;

/// <summary>
/// Provides informations on a range of days for a given schema.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class CalendricalSegment
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CalendricalSegment"/> class.
    /// <para>This constructor does NOT validate its parameters.</para>
    /// </summary>
    internal CalendricalSegment(ICalendricalSchema schema, Endpoint min, Endpoint max)
    {
        Debug.Assert(schema != null);
        Debug.Assert(min != null);
        Debug.Assert(max != null);

        Schema = schema;

        SupportedDays = Segment.Create(min.DaysSinceEpoch, max.DaysSinceEpoch);
        SupportedMonths = Segment.Create(min.MonthsSinceEpoch, max.MonthsSinceEpoch);
        SupportedYears = Segment.Create(min.Year, max.Year);

        MinMaxDateParts = OrderedPair.UnsafeCreate(min.DateParts, max.DateParts);
        MinMaxOrdinalParts = OrderedPair.UnsafeCreate(min.OrdinalParts, max.OrdinalParts);
        MinMaxMonthParts = OrderedPair.UnsafeCreate(min.MonthParts, max.MonthParts);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the minimum is the start of the minimal
    /// year; otherwise returns <see langword="false"/>.
    /// </summary>
    public bool MinIsStartOfYear { get; internal init; }

    /// <summary>
    /// Returns <see langword="true"/> if the maximum is the start of the maximal
    /// year; otherwise returns <see langword="false"/>
    /// .</summary>
    public bool MaxIsEndOfYear { get; internal init; }

    /// <summary>
    /// Returns <see langword="true"/> if this segment is complete; otherwise
    /// returns <see langword="false"/>.
    /// <para>A segment is said to be <i>complete</i> if it spans all days of a
    /// range of years.</para>
    /// </summary>
    public bool IsComplete => MinIsStartOfYear && MaxIsEndOfYear;

    /// <summary>
    /// Gets the range of supported days, that is the range of supported numbers
    /// of consecutive days from the epoch.
    /// </summary>
    /// <returns>The range from the first day of the first supported year to the
    /// last day of the last supported year.</returns>
    public Segment<int> SupportedDays { get; }

    /// <summary>
    /// Gets the range of supported months, that is the range of supported
    /// numbers of consecutive months from the epoch.
    /// </summary>
    /// <returns>The range from the first month of the first supported year to
    /// the last month of the last supported year.</returns>
    public Segment<int> SupportedMonths { get; }

    /// <summary>
    /// Gets the range of supported (algebraic) years.
    /// </summary>
    public Segment<int> SupportedYears { get; }

    /// <summary>
    /// Gets the pair of earliest and latest supported date parts.
    /// </summary>
    /// <returns>The pair of the first day of the first supported year and the
    /// last day of the last supported year.</returns>
    public OrderedPair<DateParts> MinMaxDateParts { get; }

    /// <summary>
    /// Gets the pair of earliest and latest supported ordinal date parts.
    /// </summary>
    /// <returns>The pair of the first day of the first supported year and the
    /// last day of the last supported year.</returns>
    public OrderedPair<OrdinalParts> MinMaxOrdinalParts { get; }

    /// <summary>
    /// Gets the pair of earliest and latest supported month parts.
    /// </summary>
    /// <returns>The pair of the first month of the first supported year and the
    /// last month of the last supported year.</returns>
    public OrderedPair<MonthParts> MinMaxMonthParts { get; }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    internal ICalendricalSchema Schema { get; }

    /// <summary>
    /// Returns a culture-independent string representation of the current
    /// instance.
    /// </summary>
    [Pure]
    public sealed override string ToString() => MinMaxDateParts.ToString();

    #region Factories

#if ENABLE_GENERIC_FACTORIES
    /// <summary>
    /// Creates a new instance of the <see cref="CalendricalSegment"/> class.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is
    /// NOT a subinterval of the range of supported years by
    /// <typeparamref name="TSchema"/>.</exception>
    [Pure]
    public static CalendricalSegment Create<TSchema>(Segment<int> supportedYears)
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return Create(TSchema.CreateInstance(), supportedYears);
    }
#endif

    /// <summary>
    /// Creates a new instance of the <see cref="CalendricalSegment"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is
    /// NOT a subinterval of the range of supported years by <paramref name="schema"/>.
    /// </exception>
    [Pure]
    public static CalendricalSegment Create(ICalendricalSchema schema, Segment<int> supportedYears)
    {
        var builder = new CalendricalSegmentBuilder(schema);
        builder.SetSupportedYears(supportedYears);
        return builder.BuildSegment();
    }

#if ENABLE_GENERIC_FACTORIES
    /// <summary>
    /// Creates the maximal segment for the <typeparamref name="TSchema"/> type.
    /// </summary>
    [Pure]
    public static CalendricalSegment CreateMaximal<TSchema>()
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return CreateMaximal(TSchema.CreateInstance());
    }
#endif

    /// <summary>
    /// Creates the maximal segment for <paramref name="schema"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    [Pure]
    public static CalendricalSegment CreateMaximal(ICalendricalSchema schema)
    {
        var builder = new CalendricalSegmentBuilder(schema);
        builder.SetMinToStartOfMinSupportedYear();
        builder.SetMaxToEndOfMaxSupportedYear();
        return builder.BuildSegment();
    }

#if ENABLE_GENERIC_FACTORIES
    /// <summary>
    /// Creates the maximal segment with years on after year 1 for the
    /// <typeparamref name="TSchema"/> type.
    /// </summary>
    /// <exception cref="ArgumentException">The range of supported years by
    /// <typeparamref name="TSchema"/> does not contain the years &gt;= 1.
    /// </exception>
    [Pure]
    public static CalendricalSegment CreateMaximalOnOrAfterYear1<TSchema>()
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return CreateMaximalOnOrAfterYear1(TSchema.CreateInstance());
    }
#endif

    /// <summary>
    /// Creates the maximal segment with years on after year 1 for
    /// <paramref name="schema"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The range of supported years by
    /// <paramref name="schema"/> does not contain any year &gt;= 1.
    /// </exception>
    [Pure]
    public static CalendricalSegment CreateMaximalOnOrAfterYear1(ICalendricalSchema schema)
    {
        var builder = new CalendricalSegmentBuilder(schema);
        if (!builder.TrySetMinToStartOfMinSupportedYearOnOrAfterYear1())
        {
            throw new ArgumentException(
                "The schema did not support any year greater than or equal to 1.",
                nameof(schema));
        }

        builder.SetMaxToEndOfMaxSupportedYear();
        return builder.BuildSegment();
    }

    #endregion

    internal sealed class Endpoint
    {
        public int MonthsSinceEpoch { get; internal set; }
        public int DaysSinceEpoch { get; init; }

        public DateParts DateParts { get; init; }
        public OrdinalParts OrdinalParts { get; init; }

        public MonthParts MonthParts => DateParts.MonthParts;
        public int Year => DateParts.Year;

        /// <summary>
        /// <c>left &gt; right</c>
        /// <para><c>(null &gt; null)</c> evaluates to false</para>
        /// </summary>
        public static bool IsGreaterThan(Endpoint? left, Endpoint? right) =>
            // WARNING: we do not follow the .NET convention here.
            // Comparison w/ null always returns false, even null >= null and
            // null <= null.
            left is not null
            && right is not null
            && left.DaysSinceEpoch > right.DaysSinceEpoch;
    }
}
