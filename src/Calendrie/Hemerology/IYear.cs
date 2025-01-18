// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) Tran Ngoc Bich. All rights reserved.

namespace Calendrie.Hemerology;

using System.Numerics;

// No universal year-unit, an IYear entirely depends on the underlying calendar.
// No IAbsoluteYear or IAffineYear. Interconversion can only be achieved after
// converting a year to a range of days.

/// <summary>
/// Defines a calendar year.
/// </summary>
public interface IYear
{
    /// <summary>
    /// Gets the companion calendar.
    /// </summary>
    static abstract Calendar Calendar { get; }

    /// <summary>
    /// Gets the count of consecutive years since the epoch of the calendar to
    /// which belongs the current instance.
    /// </summary>
    int YearsSinceEpoch { get; }

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
    /// Returns <see langword="true"/> if the current instance is a leap year;
    /// otherwise returns <see langword="false"/>.
    /// </summary>
    bool IsLeap { get; }
}

/// <summary>
/// Defines a calendar year type.
/// <para>This interface SHOULD NOT be implemented by types participating in a
/// poly-calendar system; see <see cref="IYearBase{TSelf}"/> for a more
/// suitable interface.</para>
/// </summary>
/// <typeparam name="TSelf">The year type that implements this interface.
/// </typeparam>
public interface IYear<TSelf> :
    IYearBase<TSelf>,
    IMinMaxValue<TSelf>
    where TSelf : IYear<TSelf>
{
    /// <summary>
    /// Creates a new instance of the <typeparamref name="TSelf"/> type from
    /// the specified year.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is
    /// outside the range of years supported values.</exception>
    [Pure] static abstract TSelf Create(int year);

    /// <summary>
    /// Attempts to create a new instance of the <typeparamref name="TSelf"/>
    /// type from the specified year.
    /// </summary>
    /// <returns><see langword="true"/> if the method succeeded; otherwise,
    /// <see langword="false"/>.</returns>
    [Pure] static abstract bool TryCreate(int year, [NotNullWhen(true)] out TSelf? result);
}
