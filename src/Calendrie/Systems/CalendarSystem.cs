// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

//#define ENABLE_MATH_OPS

namespace Calendrie.Systems;

#if ENABLE_MATH_OPS
using Calendrie.Core;
using Calendrie.Core.Schemas;
using Calendrie.Hemerology;
using Calendrie.Systems.Arithmetic;
#else
using Calendrie.Hemerology;
#endif

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
    where TDate : struct, IDateable, IAbsoluteDate<TDate>, IDateFactory<TDate>
{
#if ENABLE_MATH_OPS
    /// <summary>
    /// Represents the calendrical arithmetic.
    /// </summary>
    private readonly ICalendricalArithmetic _arithmetic;
#endif

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

#if ENABLE_MATH_OPS
        var schema = scope.Schema;

        _arithmetic =
            schema is GregorianSchema ? new GregorianArithmetic()
            : schema is JulianSchema ? new JulianArithmetic()
            : schema is LimitSchema sch ? CalendricalArithmetic.CreateDefault(sch)
            : throw new ArgumentException(null, nameof(scope));
#endif
    }

#if DEBUG
    // While creating a new type, these properties prove to be useful in
    // determining the actual value of MaxDaysSinceEpoch to be used by the T4
    // template.
    // For "standard" calendars, MinDaysSinceEpoch = 0.

    /// <summary>
    /// Gets the minimum value for the number of consecutive days from the epoch.
    /// </summary>
    internal int MinDaysSinceEpoch => Scope.Segment.SupportedDays.Min;

    /// <summary>
    /// Gets the maximum value for the number of consecutive days from the epoch.
    /// </summary>
    internal int MaxDaysSinceEpoch => Scope.Segment.SupportedDays.Max;
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

public partial class CalendarSystem<TDate> // Non-standard math ops
{
    // TODO(code): overflows? document them if any?

#if ENABLE_MATH_OPS
    /// <summary>
    /// Adds a number of years to the year field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal TDate AddYears(TDate date, int years)
    {
        var sch = Scope.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out int d);

        // NB: _arithmetic.AddYears() is validating.
        var (newY, newM, newD) = _arithmetic.AddYears(y, m, d, years);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <summary>
    /// Counts the number of years between the two specified dates.
    /// </summary>
    [Pure]
    internal int CountYearsBetween(TDate start, TDate end)
    {
        // Exact difference between two years.
        int years = end.Year - start.Year;

        var newStart = AddYears(start, years);
        if (start < end)
        {
            if (newStart > end) years--;
        }
        else
        {
            if (newStart < end) years++;
        }

        return years;
    }

    /// <summary>
    /// Adds a number of months to the month field of the specified date.
    /// </summary>
    /// <exception cref="OverflowException">The calculation would overflow the
    /// range of supported dates.</exception>
    [Pure]
    internal TDate AddMonths(TDate date, int months)
    {
        var sch = Scope.Schema;
        sch.GetDateParts(date.DaysSinceEpoch, out int y, out int m, out int d);

        // NB: _arithmetic.AddMonths() is validating.
        var (newY, newM, newD) = _arithmetic.AddMonths(y, m, d, months);

        int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
        return TDate.UnsafeCreate(daysSinceEpoch);
    }

    /// <summary>
    /// Counts the number of months between the two specified dates.
    /// </summary>
    [Pure]
    internal int CountMonthsBetween(TDate start, TDate end)
    {
        var sch = Scope.Schema;
        sch.GetDateParts(start.DaysSinceEpoch, out int y0, out int m0, out int d0);
        sch.GetDateParts(end.DaysSinceEpoch, out int y1, out int m1, out _);

        // Exact difference between two months.
        int months = _arithmetic.CountMonthsBetween(new Yemo(y0, m0), new Yemo(y1, m1));

        // To avoid extracting (y0, m0, d0) twice, we inline:
        // > var newStart = AddMonths(start, months);
        var newStart = startPlusMonths(months);

        if (start < end)
        {
            if (newStart > end) months--;
        }
        else
        {
            if (newStart < end) months++;
        }

        return months;

        [Pure]
        TDate startPlusMonths(int months)
        {
            var (newY, newM, newD) = _arithmetic.AddMonths(y0, m0, d0, months);
            int daysSinceEpoch = sch.CountDaysSinceEpoch(newY, newM, newD);
            return TDate.UnsafeCreate(daysSinceEpoch);
        }
    }
#endif
}
