// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Intervals;

/// <summary>
/// Represents a scope for a calendar supporting <i>all</i> dates within a range
/// of years.
/// <para>This class cannot be inherited.</para>
/// </summary>
public sealed class MinMaxYearScope : CalendarScope
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MinMaxYearScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is
    /// <see langword="null"/>.</exception>
    private MinMaxYearScope(DayNumber epoch, CalendricalSegment segment)
        : base(epoch, segment)
    {
        Debug.Assert(segment != null);
        Debug.Assert(segment.IsComplete);
    }

    #region Factories


    /// <summary>
    /// Creates the default maximal scope for the specified schema type and epoch.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is
    /// NOT a subinterval of the range of supported years by <typeparamref name="TSchema"/>.
    /// </exception>
    public static MinMaxYearScope Create<TSchema>(DayNumber epoch, Range<int> supportedYears)
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return Create(TSchema.CreateInstance(), epoch, supportedYears);
    }

    /// <summary>
    /// Creates the default maximal scope for the specified schema and epoch.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="supportedYears"/> is
    /// NOT a subinterval of the range of supported years by <paramref name="schema"/>.
    /// </exception>
    [Pure]
    public static MinMaxYearScope Create(ICalendricalSchema schema, DayNumber epoch, Range<int> supportedYears)
    {
        var seg = CalendricalSegment.Create(schema, supportedYears);

        return new MinMaxYearScope(epoch, seg);
    }

    /// <summary>
    /// Creates the default maximal scope for the specified schema type and epoch.
    /// </summary>
    public static MinMaxYearScope CreateMaximal<TSchema>(DayNumber epoch)
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return CreateMaximal(TSchema.CreateInstance(), epoch);
    }

    /// <summary>
    /// Creates the default maximal scope for the specified schema and epoch.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    [Pure]
    public static MinMaxYearScope CreateMaximal(ICalendricalSchema schema, DayNumber epoch)
    {
        var seg = CalendricalSegment.CreateMaximal(schema);

        return new MinMaxYearScope(epoch, seg);
    }

    /// <summary>
    /// Creates the default maximal scope for the specified schema and epoch.
    /// </summary>
    /// <exception cref="ArgumentException">The range of supported years by
    /// <typeparamref name="TSchema"/> does not contain the year 1.</exception>
    public static MinMaxYearScope CreateMaximalOnOrAfterYear1<TSchema>(DayNumber epoch)
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return CreateMaximalOnOrAfterYear1(TSchema.CreateInstance(), epoch);
    }

    /// <summary>
    /// Creates the default maximal scope for the specified schema and epoch.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="schema"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The range of supported years by
    /// <paramref name="schema"/> does not contain the year 1.</exception>
    [Pure]
    public static MinMaxYearScope CreateMaximalOnOrAfterYear1(ICalendricalSchema schema, DayNumber epoch)
    {
        var seg = CalendricalSegment.CreateMaximalOnOrAfterYear1(schema);

        return new MinMaxYearScope(epoch, seg);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with
    /// dates on or after the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of supported values by the schema.</exception>
    public static MinMaxYearScope StartingAt<TSchema>(DayNumber epoch, int year)
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return StartingAt(TSchema.CreateInstance(), epoch, year);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with
    /// dates on or after the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of supported values by the schema.</exception>
    [Pure]
    public static MinMaxYearScope StartingAt(ICalendricalSchema schema, DayNumber epoch, int year)
    {
        var builder = new CalendricalSegmentBuilder(schema);
        builder.SetMinToStartOfYear(year);
        builder.SetMaxToEndOfMaxSupportedYear();
        var segment = builder.BuildSegment();

        return new MinMaxYearScope(epoch, segment);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with
    /// dates on or before the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of supported values by the schema.</exception>
    public static MinMaxYearScope EndingAt<TSchema>(DayNumber epoch, int year)
        where TSchema : ICalendricalSchema, ISchemaActivator<TSchema>
    {
        return EndingAt(TSchema.CreateInstance(), epoch, year);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="MinMaxYearScope"/> class with
    /// dates on or before the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of supported values by the schema.</exception>
    [Pure]
    public static MinMaxYearScope EndingAt(ICalendricalSchema schema, DayNumber epoch, int year)
    {
        var builder = new CalendricalSegmentBuilder(schema);
        builder.SetMinToStartOfMinSupportedYear();
        builder.SetMaxToEndOfYear(year);
        var segment = builder.BuildSegment();

        return new MinMaxYearScope(epoch, segment);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="MinMaxYearScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="scope"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="scope"/> is NOT
    /// complete.</exception>
    [Pure]
    public static MinMaxYearScope Create(CalendarScope scope)
    {
        ArgumentNullException.ThrowIfNull(scope);

        return scope is MinMaxYearScope scope_ ? scope_
            : !scope.Segment.IsComplete ? throw new ArgumentException(null, nameof(scope))
            : new MinMaxYearScope(scope.Epoch, scope.Segment);
    }

    #endregion

    /// <inheritdoc />
    public sealed override void ValidateYearMonth(int year, int month, string? paramName = null)
    {
        YearsValidator.Validate(year, paramName);
        PreValidator.ValidateMonth(year, month, paramName);
    }

    /// <inheritdoc />
    public sealed override void ValidateYearMonthDay(int year, int month, int day, string? paramName = null)
    {
        YearsValidator.Validate(year, paramName);
        PreValidator.ValidateMonthDay(year, month, day, paramName);
    }

    /// <inheritdoc />
    public sealed override void ValidateOrdinal(int year, int dayOfYear, string? paramName = null)
    {
        YearsValidator.Validate(year, paramName);
        PreValidator.ValidateDayOfYear(year, dayOfYear, paramName);
    }
}
