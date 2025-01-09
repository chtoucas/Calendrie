// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core;
using Calendrie.Core.Intervals;
using Calendrie.Core.Utilities;

// TODO(code): add CheckXXX() variants. Optimize the Civil, Julian and Gregorian cases.
// Optimize construction of TropicaliaMonth and others like with TryCreate()?
// See all refs to ValidateYear() and ValidateYearMonth() where this can be done.

/// <summary>
/// Defines the scope of application of a calendar, a range of days, and
/// provides a base for derived classes.
/// </summary>
public abstract partial class CalendarScope
{
    /// <summary>
    /// Called from constructors in derived classes to initialize the
    /// <see cref="CalendarScope"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="segment"/> is
    /// <see langword="null"/>.</exception>
    protected CalendarScope(CalendricalSegment segment, DayNumber epoch)
    {
        ArgumentNullException.ThrowIfNull(segment);

        Segment = segment;
        Schema = segment.Schema;

        // Cache the computed property pre-validator.
        PreValidator = Schema.PreValidator;

        Epoch = epoch;
        Domain = Range.FromEndpoints(segment.SupportedDays.Endpoints.Select(x => epoch + x));
    }

    /// <summary>
    /// Gets the epoch.
    /// </summary>
    public DayNumber Epoch { get; }

    /// <summary>
    /// Gets the range of supported <see cref="DayNumber"/> values.
    /// </summary>
    public Range<DayNumber> Domain { get; }

    /// <summary>
    /// Gets the segment of supported days.
    /// </summary>
    public CalendricalSegment Segment { get; }

    /// <summary>
    /// Gets the (cached) pre-validator.
    /// </summary>
    protected internal ICalendricalPreValidator PreValidator { get; }

    /// <summary>
    /// Gets the underlying schema.
    /// </summary>
    protected internal ICalendricalSchema Schema { get; }

    /// <summary>
    /// Validates the specified <see cref="DayNumber"/> value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    public void Validate(DayNumber dayNumber)
    {
        if (dayNumber < Domain.Min || dayNumber > Domain.Max)
            fail(dayNumber, nameof(dayNumber));

        [DoesNotReturn]
        static void fail(DayNumber dayNumber, string paramName) =>
            throw new ArgumentOutOfRangeException(
                paramName,
                dayNumber,
                $"The value of the day number was out of range; value = {dayNumber}.");
    }

    /// <summary>
    /// Checks whether the specified <see cref="DayNumber"/> is outside the range
    /// of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="dayNumber"/> would
    /// overflow the range of supported values.</exception>
    public void CheckOverflow(DayNumber dayNumber)
    {
        if (dayNumber < Domain.Min || dayNumber > Domain.Max) ThrowHelpers.ThrowDateOverflow();
    }

    /// <summary>
    /// Checks whether the specified <see cref="DayNumber"/> is greater than the
    /// upper bound of the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException">The value is greater than the upper
    /// bound of the range of supported values.</exception>
    public void CheckUpperBound(DayNumber dayNumber)
    {
        if (dayNumber > Domain.Max) ThrowHelpers.ThrowDateOverflow();
    }

    /// <summary>
    /// Checks whether the specified <see cref="DayNumber"/> is less than the
    /// lower bound of the range of supported values or not.
    /// </summary>
    /// <exception cref="OverflowException">The value is less than the lower
    /// bound of the range of supported values.</exception>
    public void CheckLowerBound(DayNumber dayNumber)
    {
        if (dayNumber < Domain.Min) ThrowHelpers.ThrowDateOverflow();
    }

    /// <summary>
    /// Checks the specified date components.
    /// </summary>
    public abstract bool CheckYearMonthDay(int year, int month, int day);

    /// <summary>
    /// Checks the specified ordinal components.
    /// </summary>
    public abstract bool CheckOrdinal(int year, int dayOfYear);

    /// <summary>
    /// Validates the specified year.
    /// <para>In general the minimal and maximal years being constants, it's
    /// advisable to write the validation in situ.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    public abstract void ValidateYear(int year, string? paramName = null);

    /// <summary>
    /// Validates the specified month.
    /// <para>For regular calendars, it's advisable to write the validation in
    /// situ.</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    public abstract void ValidateYearMonth(int year, int month, string? paramName = null);

    /// <summary>
    /// Validates the specified date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    public abstract void ValidateYearMonthDay(int year, int month, int day, string? paramName = null);

    /// <summary>
    /// Validates the specified ordinal date.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The validation failed.
    /// </exception>
    public abstract void ValidateOrdinal(int year, int dayOfYear, string? paramName = null);
}
