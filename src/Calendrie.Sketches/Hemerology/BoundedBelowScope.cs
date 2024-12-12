// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie;
using Calendrie.Core;

/// <summary>
/// Represents a scope for a calendar supporting <i>all</i> dates on or after a
/// given date, <i>but not the first day of a year</i>.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class BoundedBelowScope : CalendarScope
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BoundedBelowScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The start of <paramref name="segment"/>
    /// is the first day of a year -or- the end of <paramref name="segment"/> is
    /// not the end of a year.</exception>
    public BoundedBelowScope(DayNumber epoch, CalendricalSegment segment)
        : base(epoch, segment)
    {
        var seg = Segment;
        if (seg.MinIsStartOfYear || !seg.MaxIsEndOfYear)
            throw new ArgumentException(null, nameof(segment));

        MinDateParts = seg.MinMaxDateParts.LowerValue;
        MinOrdinalParts = seg.MinMaxOrdinalParts.LowerValue;
        MinMonthParts = MinDateParts.MonthParts;

        MinYear = MinDateParts.Year;
    }

    /// <summary>
    /// Gets the earliest supported month parts.
    /// </summary>
    public MonthParts MinMonthParts { get; }

    /// <summary>
    /// Gets the earliest supported date parts.
    /// </summary>
    public DateParts MinDateParts { get; }

    /// <summary>
    /// Gets the earliest supported ordinal date parts.
    /// </summary>
    public OrdinalParts MinOrdinalParts { get; }

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    private int MinYear { get; }

    #region Factories

    public static BoundedBelowScope Create<TSchema>(
        DayNumber epoch, DateParts minDateParts, int maxYear)
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return Create(TSchema.CreateInstance(), epoch, minDateParts, maxYear);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="BoundedBelowScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDateParts"/>
    /// is invalid or outside the range of dates supported by <paramref name="schema"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxYear"/>
    /// is outside the range of years supported by <paramref name="schema"/>.
    /// </exception>
    /// <exception cref="ArgumentException"><paramref name="minDateParts"/> is
    /// the first day of a year.</exception>
    [Pure]
    public static BoundedBelowScope Create(
        ICalendricalSchema schema, DayNumber epoch, DateParts minDateParts, int maxYear)
    {
        var builder = new CalendricalSegmentBuilder(schema) { MinDateParts = minDateParts };
        builder.SetMaxToEndOfYear(maxYear);
        var seg = builder.BuildSegment();

        return new BoundedBelowScope(epoch, seg);
    }

    public static BoundedBelowScope StartingAt<TSchema>(DayNumber epoch, DateParts parts)
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return StartingAt(TSchema.CreateInstance(), epoch, parts);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="BoundedBelowScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="parts"/>
    /// is invalid or outside the range of dates supported by <paramref name="schema"/>.
    /// </exception>
    /// <exception cref="ArgumentException"><paramref name="parts"/> is the first
    /// day of a year.</exception>
    [Pure]
    public static BoundedBelowScope StartingAt(
        ICalendricalSchema schema, DayNumber epoch, DateParts parts)
    {
        var builder = new CalendricalSegmentBuilder(schema) { MinDateParts = parts };
        builder.SetMaxToEndOfMaxSupportedYear();
        var seg = builder.BuildSegment();

        return new BoundedBelowScope(epoch, seg);
    }

    ///// <summary>
    ///// Creates a new instance of the <see cref="BoundedBelowScope"/> class.
    ///// </summary>
    ///// <exception cref="ArgumentNullException"><paramref name="scope"/> is
    ///// <see langword="null"/>.</exception>
    ///// <exception cref="ArgumentException">The minimum date of <paramref name="scope"/>
    ///// is the start of the minimal year -or- the maximum date of <paramref name="scope"/>
    ///// is not the end of the maximal year.</exception>
    //[Pure]
    //public static BoundedBelowScope Create(CalendarScope scope)
    //{
    //    ArgumentNullException.ThrowIfNull(scope);

    //    return scope is BoundedBelowScope scope_ ? scope_
    //        : new BoundedBelowScope(scope.Epoch, scope.Segment);
    //}

    #endregion

    /// <inheritdoc />
    public sealed override void ValidateYearMonth(int year, int month, string? paramName = null)
    {
        YearsValidator.Validate(year, paramName);
        PreValidator.ValidateMonth(year, month, paramName);

        // Tiny optimization: we first check "year".
        if (year == MinYear && new MonthParts(year, month) < MinMonthParts)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
    }

    /// <inheritdoc />
    public sealed override void ValidateYearMonthDay(
        int year, int month, int day, string? paramName = null)
    {
        YearsValidator.Validate(year, paramName);
        PreValidator.ValidateMonthDay(year, month, day, paramName);

        // Tiny optimization: we first check "year".
        if (year == MinYear)
        {
            // We check the month parts first even if it is not necessary.
            // Reason: identify the guilty part.
            var parts = new DateParts(year, month, day);
            if (parts.MonthParts < MinMonthParts)
            {
                ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
            }
            else if (parts < MinDateParts)
            {
                ThrowHelpers.ThrowDayOutOfRange(day, paramName);
            }
        }
    }

    /// <inheritdoc />
    public sealed override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
    {
        YearsValidator.Validate(year, paramName);
        PreValidator.ValidateDayOfYear(year, dayOfYear, paramName);

        // Tiny optimization: we first check "year".
        if (year == MinYear && new OrdinalParts(year, dayOfYear) < MinOrdinalParts)
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
    }
}
