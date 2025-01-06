// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

// No IAbsoluteMonth or IAffineMonth. A month type is always "affine".
// Interconversion can only be achieved after converting a month to a range of
// days.

/// <summary>
/// Defines a calendar month.
/// </summary>
public interface ICalendarMonth
{
    /// <summary>
    /// Gets the count of months since the epoch of the calendar to which belongs
    /// the current instance.
    /// </summary>
    int MonthsSinceEpoch { get; }

    /// <summary>
    /// Gets the century of the era.
    /// </summary>
    /// <remarks>
    /// A default implementation should look like this:
    /// <code><![CDATA[
    ///   Ord CenturyOfEra => Ord.FromInt32(Century);
    /// ]]></code>
    /// </remarks>
    Ord CenturyOfEra { get; }

    /// <summary>
    /// Gets the century number.
    /// </summary>
    /// <remarks>
    /// A default implementation should look like this:
    /// <code><![CDATA[
    ///   int Century => YearNumbering.GetCentury(Year);
    /// ]]></code>
    /// </remarks>
    int Century { get; }

    /// <summary>
    /// Gets the year of the era.
    /// </summary>
    /// <remarks>
    /// A default implementation should look like this:
    /// <code><![CDATA[
    ///   Ord YearOfEra => Ord.FromInt32(Year);
    /// ]]></code>
    /// </remarks>
    Ord YearOfEra { get; }

    /// <summary>
    /// Gets the year of the century.
    /// <para>The result is in the range from 1 to 100.</para>
    /// </summary>
    /// <remarks>
    /// A default implementation should look like this:
    /// <code><![CDATA[
    ///   int YearOfCentury => YearNumbering.GetYearOfCentury(Year);
    /// ]]></code>
    /// </remarks>
    int YearOfCentury { get; }

    /// <summary>
    /// Gets the (algebraic) year number.
    /// </summary>
    int Year { get; }

    /// <summary>
    /// Gets the month of the year.
    /// </summary>
    int Month { get; }

    /// <summary>
    /// Returns <see langword="true"/> if the current instance is an intercalary
    /// month; otherwise returns <see langword="false"/>.
    /// </summary>
    bool IsIntercalary { get; }

    /// <summary>
    /// Deconstructs the current instance into its components.
    /// </summary>
    void Deconstruct(out int year, out int month);

    /// <summary>
    /// Obtains the number of whole months in the year elapsed since the start
    /// of the year and before this month instance.
    /// </summary>
    //
    // Trivial, only added for completeness.
    [Pure] int CountElapsedMonthsInYear() => Month - 1;

    /// <summary>
    /// Obtains the number of whole months remaining after this month instance
    /// and until the end of the year.
    /// </summary>
    [Pure] int CountRemainingMonthsInYear();

    // REVIEW(code): CountElapsedDaysInYear() and CountRemainingDaysInYear()?

    ///// <summary>
    ///// Obtains the number of whole days in the year elapsed since the start of
    ///// the year and before this month instance.
    ///// </summary>
    //[Pure] int CountElapsedDaysInYear();

    ///// <summary>
    ///// Obtains the number of whole days remaining after this month instance and
    ///// until the end of the year.
    ///// </summary>
    //[Pure] int CountRemainingDaysInYear();
}

/// <summary>
/// Defines a calendar month type.
/// <para>This interface SHOULD NOT be implemented by types participating in a
/// poly-calendar system; see <see cref="ICalendarMonthBase{TSelf}"/> for a more
/// suitable interface.</para>
/// </summary>
/// <typeparam name="TSelf">The month type that implements this interface.
/// </typeparam>
public interface ICalendarMonth<TSelf> :
    ICalendarMonthBase<TSelf>,
    IMinMaxValue<TSelf>
    where TSelf : ICalendarMonth<TSelf>
{ }
