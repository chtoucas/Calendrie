// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Utilities;

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
    private BoundedBelowScope(CalendricalSegment segment, DayNumber epoch)
        : base(segment, epoch)
    {
        Debug.Assert(segment != null);

        if (segment.MinIsStartOfYear || !segment.MaxIsEndOfYear)
            throw new ArgumentException(null, nameof(segment));

        (MinYear, MaxYear) = segment.SupportedYears.Endpoints;
        MinDateParts = segment.MinMaxDateParts.LowerValue;
        MinOrdinalParts = segment.MinMaxOrdinalParts.LowerValue;
    }

    /// <summary>
    /// Gets the earliest supported year.
    /// </summary>
    public int MinYear { get; }

    /// <summary>
    /// Gets the latest supported year.
    /// </summary>
    public int MaxYear { get; }

    /// <summary>
    /// Gets the earliest supported date parts.
    /// </summary>
    public DateParts MinDateParts { get; }

    /// <summary>
    /// Gets the earliest supported ordinal date parts.
    /// </summary>
    public OrdinalParts MinOrdinalParts { get; }

    #region Factories

    /// <summary>
    /// Creates a new instance of the <see cref="BoundedBelowScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDateParts"/>
    /// is invalid or outside the range of dates supported by <typeparamref name="TSchema"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxYear"/>
    /// is outside the range of years supported by <typeparamref name="TSchema"/>.
    /// </exception>
    /// <exception cref="ArgumentException"><paramref name="minDateParts"/> is
    /// the first day of a year.</exception>
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
        var segment = builder.BuildSegment();

        return new BoundedBelowScope(segment, epoch);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="BoundedBelowScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="parts"/>
    /// is invalid or outside the range of dates supported by <typeparamref name="TSchema"/>.
    /// </exception>
    /// <exception cref="ArgumentException"><paramref name="parts"/> is the first
    /// day of a year.</exception>
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
        var segment = builder.BuildSegment();

        return new BoundedBelowScope(segment, epoch);
    }

    // Voir MinMaxYearScope.Create(CalendarScope)
    //
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
    public sealed override bool CheckYearMonthDay(int year, int month, int day)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public sealed override bool CheckOrdinal(int year, int dayOfYear)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public sealed override void ValidateYear(int year, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
    }

    /// <inheritdoc />
    public sealed override void ValidateYearMonth(int year, int month, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        PreValidator.ValidateMonth(year, month, paramName);

        // Tiny optimization: we first check "year".
        if (year == MinDateParts.Year && new MonthParts(year, month) < MinDateParts.MonthParts)
            ThrowHelpers.ThrowMonthOutOfRange(month, paramName);
    }

    /// <inheritdoc />
    public sealed override void ValidateYearMonthDay(
        int year, int month, int day, string? paramName = null)
    {
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        PreValidator.ValidateMonthDay(year, month, day, paramName);

        // Tiny optimization: we first check "year".
        if (year == MinDateParts.Year)
        {
            // We check the month parts first even if it is not necessary.
            // Reason: identify the guilty part.
            var parts = new DateParts(year, month, day);
            if (parts.MonthParts < MinDateParts.MonthParts)
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
        if (year < MinYear || year > MaxYear) ThrowHelpers.ThrowYearOutOfRange(year, paramName);
        PreValidator.ValidateDayOfYear(year, dayOfYear, paramName);

        // Tiny optimization: we first check "year".
        if (year == MinDateParts.Year && new OrdinalParts(year, dayOfYear) < MinOrdinalParts)
            ThrowHelpers.ThrowDayOfYearOutOfRange(dayOfYear, paramName);
    }
}
