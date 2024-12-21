// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using Calendrie.Hemerology;

// Reasons to keep the constructor internal (system calendars and adjusters):
// - the scope must be of type "MinMaxYearScope" but we don't enforce this
// - we don't validate the input. Only for TDate developed whitin this
//   project do we know that it's not possible to create an invalid date.
//   In this project, we don't have an example based on IDateable but on
//   IAbsoluteDate. Indeed, a DayNumber exists beyond the scope of a calendar
//   and therefore could be used as a type argument.
// - This impl is only interesting if NewDate() is non-validating, otherwise
//   we should simply use the methods provided by a calendar.
// - this class works best for date types based on the count of days since
//   the epoch which is the case for all date types in Calendrie.Systems. For
//   types using a y/m/d/doy repr. there is a better way of implementing
//   IDateAdjuster<TDate>; see e.g. MyDate in Samples.
// We could remove the constraint on TDate but it would make things a
// bit harder than necessary. Without IDateable, we would have to obtain the
// date parts (y, m, d, doy) by other means, e.g. using the underlying schema.

/// <summary>
/// Represents a calendar with dates within a range of years and provides a base
/// for derived classes.
/// <para>This class can ONLY be initialized from within friend assemblies.
/// </para>
/// <para>This class works best when <typeparamref name="TDate"/> is based on
/// the count of consecutive days since the epoch.</para>
/// </summary>
/// <typeparam name="TDate">The type of date object.</typeparam>
public partial class CalendarSystem<TDate> : Calendar, IDateProvider<TDate>
    where TDate : struct, IDate<TDate>, IDateFactory<TDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarSystem{TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    internal CalendarSystem(string name, CalendarScope scope) : base(name, scope)
    {
        Debug.Assert(scope.Segment.IsComplete);

        Adjuster = DateAdjuster.Create(this);
    }

    // The next internal properties are only meant to be used by the date type.

    /// <summary>
    /// Gets the date adjuster.
    /// </summary>
    internal DateAdjuster<TDate> Adjuster { get; }

    /// <summary>
    /// Gets the minimum value for the number of consecutive days from the epoch.
    /// </summary>
    internal int MinDaysSinceEpoch => Scope.Segment.SupportedDays.Min;

    /// <summary>
    /// Gets the maximum value for the number of consecutive days from the epoch.
    /// </summary>
    internal int MaxDaysSinceEpoch => Scope.Segment.SupportedDays.Max;

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        Scope.ValidateYear(year);
        return Schema.CountDaysInYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        return Schema.CountDaysInMonth(year, month);
    }
}

public partial class CalendarSystem<TDate> // IDateProvider<TDate>
{
    /// <inheritdoc/>
    [Pure]
    public IEnumerable<TDate> GetDaysInYear(int year)
    {
        Scope.ValidateYear(year);

        int startOfYear = Schema.GetStartOfYear(year);
        int daysInYear = Schema.CountDaysInYear(year);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<TDate> GetDaysInMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);

        int startOfMonth = Schema.GetStartOfMonth(year, month);
        int daysInMonth = Schema.CountDaysInMonth(year, month);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetStartOfYear(int year)
    {
        Scope.ValidateYear(year);
        int daysSinceEpoch = Schema.GetStartOfYear(year);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetEndOfYear(int year)
    {
        Scope.ValidateYear(year);
        int daysSinceEpoch = Schema.GetEndOfYear(year);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetStartOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = Schema.GetStartOfMonth(year, month);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetEndOfMonth(int year, int month)
    {
        Scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = Schema.GetEndOfMonth(year, month);
        return TDate.FromDaysSinceEpochUnchecked(daysSinceEpoch);
    }
}
