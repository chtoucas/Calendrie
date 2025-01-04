// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Systems;

using System.Numerics;

using Calendrie;
using Calendrie.Core.Intervals;

public interface IYear<TSelf> :
    // Comparison
    IEqualityOperators<TSelf, TSelf, bool>,
    IEquatable<TSelf>,
    IComparisonOperators<TSelf, TSelf, bool>,
    IComparable<TSelf>,
    IComparable,
    IMinMaxValue<TSelf>,
    // Arithmetic
    IAdditionOperators<TSelf, int, TSelf>,
    ISubtractionOperators<TSelf, int, TSelf>,
    //ISubtractionOperators<TSelf, TSelf, int>,
    IIncrementOperators<TSelf>,
    IDecrementOperators<TSelf>
    where TSelf : IYear<TSelf>
{
    /// <summary>
    /// Gets the century of the era.
    /// </summary>
    Ord CenturyOfEra { get; }

    /// <summary>
    /// Gets the century number.
    /// </summary>
    int Century { get; }

    /// <summary>
    /// Gets the year of the era.
    /// </summary>
    Ord YearOfEra { get; }

    /// <summary>
    /// Gets the year of the century.
    /// <para>The result is in the range from 1 to 100.</para>
    /// </summary>
    int YearOfCentury { get; }

    /// <summary>
    /// Gets the (algebraic) year number.
    /// </summary>
    int Year { get; }

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is a leap year;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    bool IsLeap { get; }

    /// <summary>
    /// Obtains the number of months in this year instance.
    /// </summary>
    int CountMonths();

    /// <summary>
    /// Obtains the number of days in this year instance.
    /// <para>See also <see cref="CalendarSystem{TDate}.CountDaysInYear(int)"/>.
    /// </para>
    /// </summary>
    int CountDays();

    //
    // Standard math ops
    //

    /// <summary>
    /// Counts the number of years elapsed since the specified year.
    /// </summary>
    int CountYearsSince(TSelf other);

    /// <summary>
    /// Adds a number of years to this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// range of supported years.</exception>
    TSelf PlusYears(int years);

    /// <summary>
    /// Obtains the year after this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// latest supported year.</exception>
    TSelf NextYear();

    /// <summary>
    /// Obtains the year before this year instance, yielding a new year.
    /// </summary>
    /// <exception cref="OverflowException">The operation would overflow the
    /// earliest supported year.</exception>
    TSelf PreviousYear();
}

public interface IDaysOfYearProvider<TDate>
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
    /// <para>See also <see cref="CalendarSystem{TDate}.GetDaysInYear(int)"/>.
    /// </para>
    /// </summary>
    Range<TDate> ToRangeOfDays();

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
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "<Pending>")]
    bool Contains(TDate date);
}

public interface IMonthsOfYearProvider<TMonth>
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
