// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Validation;

/// <summary>
/// Defines the scope of application of a calendar, a range of days, and
/// provides a base for derived classes.
/// </summary>
public abstract partial class CalendarScope : ICalendricalValidator
{
    /// <summary>
    /// Represents the underlying schema.
    /// </summary>
    private readonly ICalendricalSchema _schema;

    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CalendarScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is
    /// <see langword="null"/>.</exception>
    protected CalendarScope(DayNumber epoch, CalendricalSegment segment)
    {
        ArgumentNullException.ThrowIfNull(segment);

        Segment = segment;
        _schema = segment.Schema;

        Epoch = epoch;

        YearsValidator = new YearsValidator(segment.SupportedYears);

        Domain = Range.FromEndpoints(
            segment.SupportedDays.Endpoints.Select(x => epoch + x));
    }

    /// <summary>
    /// Gets the epoch.
    /// </summary>
    public DayNumber Epoch { get; }

    /// <summary>
    /// Gets the range of supported values for a <see cref="DayNumber"/>.
    /// </summary>
    public Range<DayNumber> Domain { get; }

    /// <summary>
    /// Gets the segment of supported days.
    /// </summary>
    public CalendricalSegment Segment { get; }

    /// <summary>
    /// Gets the validator for the range of supported years.
    /// </summary>
    public IYearsValidator YearsValidator { get; internal init; }

    /// <summary>
    /// Gets the pre-validator.
    /// </summary>
    protected ICalendricalPreValidator PreValidator => _schema.PreValidator;

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    protected internal ICalendricalSchema Schema => _schema;

    /// <inheritdoc />
    public abstract void ValidateYearMonth(int year, int month, string? paramName = null);

    /// <inheritdoc />
    public abstract void ValidateYearMonthDay(int year, int month, int day, string? paramName = null);

    /// <inheritdoc />
    public abstract void ValidateOrdinal(int year, int dayOfYear, string? paramName = null);
}
