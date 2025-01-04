// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using Calendrie.Core.Intervals;

public interface IYearDaysView<TDate> : ICalendarYear
    where TDate : struct, IEquatable<TDate>, IComparable<TDate>
{
    /// <summary>
    /// Gets the first day of this year instance.
    /// </summary>
    TDate FirstDay { get; }

    /// <summary>
    /// Gets the last day of this year instance.
    /// </summary>
    TDate LastDay { get; }

    /// <summary>
    /// Converts the current instance to a range of days.
    /// </summary>
    Range<TDate> ToRangeOfDays();

    /// <summary>
    /// Obtains the number of days in this year instance.
    /// </summary>
    int CountDays();

    /// <summary>
    /// Obtains the ordinal date corresponding to the specified day of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="dayOfYear"/>
    /// is outside the range of valid values.</exception>
    TDate GetDayOfYear(int dayOfYear);

    /// <summary>
    /// Obtains the sequence of all days in this year instance.
    /// </summary>
    IEnumerable<TDate> GetAllDays();

    /// <summary>
    /// Determines whether the current instance contains the specified date or
    /// not.
    /// </summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords.", Justification = "VB.NET Date.")]
    bool Contains(TDate date);
}

public interface IYearMonthsView<TMonth> : ICalendarYear
    where TMonth : struct, IEquatable<TMonth>, IComparable<TMonth>
{
    /// <summary>
    /// Gets the first month of this year instance.
    /// </summary>
    TMonth FirstMonth { get; }

    /// <summary>
    /// Gets the last month of this year instance.
    /// </summary>
    TMonth LastMonth { get; }

    /// <summary>
    /// Converts the current instance to a range of months.
    /// </summary>
    Range<TMonth> ToRangeOfMonths();

    /// <summary>
    /// Obtains the number of months in this year instance.
    /// </summary>
    int CountMonths();

    /// <summary>
    /// Obtains the month corresponding to the specified month of this year
    /// instance.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="month"/>
    /// is outside the range of valid values.</exception>
    TMonth GetMonthOfYear(int month);

    /// <summary>
    /// Obtains the sequence of all months in this year instance.
    /// </summary>
    IEnumerable<TMonth> GetAllMonths();

    /// <summary>
    /// Determines whether the current instance contains the specified month or
    /// not.
    /// </summary>
    bool Contains(TMonth month);
}
