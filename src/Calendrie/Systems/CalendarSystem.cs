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
// - this impl is only interesting if NewDate() is non-validating, otherwise
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
    where TDate : struct, IDateable, IAbsoluteDate<TDate>, IUnsafeFactory<TDate>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarSystem{TDate}"/>
    /// class.
    /// </summary>
    /// <exception cref="ArgumentNullException">One of the parameters is
    /// <see langword="null"/>.</exception>
    internal CalendarSystem(string name, CalendarScope scope) : base(name, scope)
    {
        Debug.Assert(scope != null);
        Debug.Assert(scope.Segment.IsComplete);
    }

#if DEBUG
    // While creating a new type, these properties prove to be useful in
    // determining the actual value of MaxDaysSinceEpoch and MaxMonthsSinceEpoch
    // to be used by the T4 template.
    // For non-proleptic calendars, MinDaysSinceEpoch and MinMonthsSinceEpoch = 0.

    /// <summary>
    /// Gets the minimum value for the number of consecutive days from the epoch.
    /// </summary>
    internal int MinDaysSinceEpoch => Scope.Segment.SupportedDays.Min;

    /// <summary>
    /// Gets the maximum value for the number of consecutive days from the epoch.
    /// </summary>
    internal int MaxDaysSinceEpoch => Scope.Segment.SupportedDays.Max;

    /// <summary>
    /// Gets the minimum value for the number of consecutive months from the epoch.
    /// </summary>
    internal int MinMonthsSinceEpoch => Scope.Segment.SupportedMonths.Min;

    /// <summary>
    /// Gets the maximum value for the number of consecutive months from the epoch.
    /// </summary>
    internal int MaxMonthsSinceEpoch => Scope.Segment.SupportedMonths.Max;
#endif

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountDaysInYear(int year)
    {
        var scope = Scope;
        scope.ValidateYear(year);
        return scope.Schema.CountDaysInYear(year);
    }

    /// <inheritdoc/>
    [Pure]
    public sealed override int CountDaysInMonth(int year, int month)
    {
        var scope = Scope;
        scope.ValidateYearMonth(year, month);
        return scope.Schema.CountDaysInMonth(year, month);
    }
}

// TODO(code): if we enable the year and month types, we should remove these methods

public partial class CalendarSystem<TDate> // IDateProvider<TDate>
{
    /// <inheritdoc/>
    [Pure]
    public IEnumerable<TDate> GetDaysInYear(int year)
    {
        var scope = Scope;
        var sch = Scope.Schema;

        scope.ValidateYear(year);

        int startOfYear = sch.GetStartOfYear(year);
        int daysInYear = sch.CountDaysInYear(year);

        return from daysSinceEpoch
               in Enumerable.Range(startOfYear, daysInYear)
               select TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public IEnumerable<TDate> GetDaysInMonth(int year, int month)
    {
        var scope = Scope;
        var sch = Scope.Schema;

        scope.ValidateYearMonth(year, month);

        int startOfMonth = sch.GetStartOfMonth(year, month);
        int daysInMonth = sch.CountDaysInMonth(year, month);

        return from daysSinceEpoch
               in Enumerable.Range(startOfMonth, daysInMonth)
               select TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetStartOfYear(int year)
    {
        var scope = Scope;
        scope.ValidateYear(year);
        int daysSinceEpoch = scope.Schema.GetStartOfYear(year);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetEndOfYear(int year)
    {
        var scope = Scope;
        scope.ValidateYear(year);
        int daysSinceEpoch = Scope.Schema.GetEndOfYear(year);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetStartOfMonth(int year, int month)
    {
        var scope = Scope;
        scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = scope.Schema.GetStartOfMonth(year, month);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <inheritdoc/>
    [Pure]
    public TDate GetEndOfMonth(int year, int month)
    {
        var scope = Scope;
        scope.ValidateYearMonth(year, month);
        int daysSinceEpoch = scope.Schema.GetEndOfMonth(year, month);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }
}

public partial class CalendarSystem<TDate> // Transformers
{
    // Compare to IDateProvider<>, we can bypass the validation.
    //
    // REVIEW(code): move transformers to the date type?

    /// <summary>
    /// Obtains the first day of the year to which belongs the specified date.
    /// </summary>
    [Pure]
    public TDate GetStartOfYear(TDate date)
    {
        int daysSinceEpoch = Scope.Schema.GetStartOfYear(date.Year);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <summary>
    /// Obtains the last day of the year to which belongs the specified date.
    /// </summary>
    [Pure]
    public TDate GetEndOfYear(TDate date)
    {
        int daysSinceEpoch = Scope.Schema.GetEndOfYear(date.Year);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <summary>
    /// Obtains the first day of the month to which belongs the specified date.
    /// </summary>
    [Pure]
    public TDate GetStartOfMonth(TDate date)
    {
        var sch = Scope.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
        int daysSinceEpoch = sch.GetStartOfMonth(y, m);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <summary>
    /// Obtains the last day of the month to which belongs the specified date.
    /// </summary>
    [Pure]
    public TDate GetEndOfMonth(TDate date)
    {
        var sch = Scope.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out _);
        int daysSinceEpoch = sch.GetEndOfMonth(y, m);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }
}
