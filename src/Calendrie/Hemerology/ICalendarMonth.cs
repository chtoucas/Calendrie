// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

using Calendrie;

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
}

/// <summary>
/// Defines a calendar month type.
/// </summary>
/// <typeparam name="TSelf">The month type that implements this interface.
/// </typeparam>
public interface ICalendarMonth<TSelf> :
    ICalendarMonth,
    // Comparison
    IEqualityOperators<TSelf, TSelf, bool>,
    IEquatable<TSelf>,
    IComparisonOperators<TSelf, TSelf, bool>,
    IComparable<TSelf>,
    IComparable,
    IMinMaxValue<TSelf>,
    // Arithmetic
    ICalendarMonthArithmetic<TSelf>
    where TSelf : ICalendarMonth<TSelf>
{
    /// <summary>
    /// Obtains the earliest month between the two specified months.
    /// </summary>
    [Pure] static abstract TSelf Min(TSelf x, TSelf y);

    /// <summary>
    /// Obtains the latest month between the two specified months.
    /// </summary>
    [Pure] static abstract TSelf Max(TSelf x, TSelf y);
}
